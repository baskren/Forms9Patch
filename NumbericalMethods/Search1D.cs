using System;

namespace NumericalMethods
{
	public static class Search1D
	{
		const double GOLD = 1.61803399;
		const double GLIMIT = 100.0;
		const double TINY = 1.0e-20;

		static void SHFT(ref double a, ref double b, ref double c, double d) {
			a = b;
			b = c;
			c = d;
		}

		static double SIGN(double a, double b) {
			return (b > 0.0 ? Math.Abs (a) : -Math.Abs (a));
		}

		public static void BracketMin(ref double ax, ref double bx, out double cx, out double fa, out double fb, out double fc, Func<double,double> func) {
			double ulim, u, r, q, fu, dum=0;

			fa = func (ax);
			fb = func (bx);
			if (fb > fa) {
				SHFT (ref dum, ref ax, ref bx, dum);
				SHFT (ref dum, ref fb, ref fa, dum);
			}
			cx = bx + GOLD * (bx - ax);
			fc = func (cx);
			while (fb > fc) {
				r = (bx - ax) * (fb - fc);
				q = (bx - cx) * (fb - fa);
				u = bx - ((bx-cx)*q - (bx-ax)*r)/(2.0*SIGN(Math.Max(Math.Abs(q-r),TINY),q-r));
				ulim = bx + GLIMIT * (cx - bx);
				if ((bx - u) * (u - cx) > 0.0) {
					fu = func (u);
					if (fu < fc) {
						ax = bx;
						bx = u;
						fa = fb;
						fb = fu;
						return;
					} else if (fu > fb) {
						cx = u;
						fc = fu;
						return;
					}
					u = cx + GOLD * (cx - bx);
					fu = func (u);
				} else if ((cx - u) * (u - ulim) > 0.0) {
					fu = func (u);
					if (fu < fc) {
						SHFT (ref bx, ref cx, ref u, cx + GOLD * (cx - bx));
						SHFT (ref fb, ref fc, ref fu, func (u));
					}
				} else if ((u - ulim) * (ulim - cx) >= 0.0) {
					u = ulim;
					fu = func (u);
				} else {
					u = cx + GOLD * (cx - bx);
					fu = func (u);
				}
				SHFT (ref ax, ref bx, ref cx, u);
				SHFT (ref fa, ref fb, ref fc, fu);
			}
		}

		const double R =  GOLD - 1.0;
		const double C = (1.0-R);

		public static double GoldenMin(double ax, double bx, double cx, Func<double,double> func, double tol, out double xmin, int maxIter=-1) {
			double f0=0, f1, f2, f3=0, x0, x1, x2, x3;
			x0 = ax;
			x3 = cx;
			if (Math.Abs (cx - bx) > Math.Abs (bx - ax)) {
				x1 = bx;
				x2 = bx + C * (cx - bx);
			} else {
				x2 = bx;
				x1 = bx - C * (bx - ax);
			}
			f1 = func (x1);
			f2 = func (x2);
			int iter=0;
			while (Math.Abs (x3 - x0) > tol * (Math.Abs (x1) + Math.Abs (x2))) {
			//while (Math.Abs (x3 - x0) > tol ) {
				if (f2 < f1) {
					SHFT (ref x0, ref x1, ref x2, R * x1 + C * x3);
					SHFT (ref f0, ref f1, ref f2, func (x2));
				} else {
					SHFT (ref x3, ref x2, ref x1, R * x2 + C * x0);
					SHFT (ref f3, ref f2, ref f1, func (x1));
				}
				if (maxIter > 0) {
					if (maxIter < iter++)
						break;
				}
			}
			if (f1 < f2) {
				xmin = x1;
				return f1;
			} else {
				xmin = x2;
				return f2;
			}
		}

		const int ITMAX = 1000;
		const double ZEPS = 1e-10;

		public static double BrentMin(double ax, double bx, double cx, Func<double,double> func, double tol, out double xmin) {
			int iter;
			double a, b, d=0, etemp, fu, fv, fw, fx, p, q, r, tol1, tol2, u, v, w, x, xm;
			double e = 0.0;

			a = ((ax < cx) ? ax : cx);
			b = ((ax > cx) ? ax : cx);
			w = v = x = bx;
			fw = fv = fx = func (x);
			for (iter = 0; iter < ITMAX; iter++) {
				xm = 0.5 * (a + b);
				tol2 = 2.0 * (tol1 = tol * Math.Abs (x) + ZEPS);
				if (Math.Abs (x - xm) <= (tol2 - 0.5 * (b - a))) {
					xmin = x;
					return fx;
				}
				if (Math.Abs (e) > tol1) {
					r = (x - w) * (fx - fv);
					q = (x - v) * (fx - fw);
					p = (x - v) * q - (x - w) * r;
					q = 2.0 * (q - r);
					if (q > 0.0)
						p = -p;
					q = Math.Abs (q);
					etemp = e;
					e = d;
					if (Math.Abs (p) >= Math.Abs (0.5 * q * etemp) || p <= q * (a - x) || p >= q * (b - x))
						d = C * (e = (x >= xm ? a - x : b - x));
					else {
						d = p / q;
						u = x + d;
						if (u - a < tol2 || b - u < tol2)
							d = SIGN (tol1, xm - x);
					}
				} else {
					d = C * (e = (x >= xm ? a - x : b - x));
				}
				u = (Math.Abs (d) >= tol1 ? x + d : x + SIGN (tol1, d));
				fu = func (u);
				if (fu <= fx) {
					if (u >= x)
						a = x;
					else
						b = x;
					SHFT (ref v, ref w, ref x, u);
					SHFT (ref fv, ref fw, ref fx, fu);
				} else {
					if (u < x)
						a = u;
					else
						b = u;
					if (fu <= fx || w == x) {
						v = w;
						w = u;
						fv = fw;
						fw = fu;
					} else if (fu <= fv || v == x || v == w) {
						v = u;
						fv = fu;
					}
				}
			}
			xmin = x;
			return fx;
		}
	}
}

