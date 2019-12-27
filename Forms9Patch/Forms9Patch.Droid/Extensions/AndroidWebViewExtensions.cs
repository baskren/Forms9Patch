using System;
using System.Reflection;
using System.Threading.Tasks;
using Android.Webkit;

namespace Forms9Patch.Droid
{
    static class AndroidWebViewExtensions
    {
        public static int ContentWidth(this Android.Webkit.WebView webView)
        {
            MethodInfo method = webView.GetType().GetMethod("ComputeHorizontalScrollRange", BindingFlags.NonPublic | BindingFlags.Instance);
            var width = (int)method.Invoke(webView, new object[] { });
            return width;
        }

        public static int ContentHeight(this Android.Webkit.WebView webView)
        {
            MethodInfo method = webView.GetType().GetMethod("ComputeVerticalScrollRange", BindingFlags.NonPublic | BindingFlags.Instance);
            var height = (int)method.Invoke(webView, new object[] { });

            return (int)(height / Display.Scale) + webView.MeasuredHeight;
        }

        public static Task<Java.Lang.Object> EvaluateJavaScriptAsync(this Android.Webkit.WebView webView, string script)
        {
            var evaluator = new JavaScriptEvaluator(webView, script);
            return evaluator.TaskCompletionSource.Task;
        }

    }

    class JavaScriptEvaluator : Java.Lang.Object, IValueCallback
    {
        public TaskCompletionSource<Java.Lang.Object> TaskCompletionSource = new TaskCompletionSource<Java.Lang.Object>();

        public JavaScriptEvaluator(Android.Webkit.WebView webView, string script)
        {
            webView.EvaluateJavascript(script, this);
        }
        public void OnReceiveValue(Java.Lang.Object value)
            => TaskCompletionSource.SetResult(value);

    }
}
