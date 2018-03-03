using System;
using System.Collections.Generic;
using Foundation;
using UIKit;
using ObjCRuntime;
using Xamarin.Forms;

[assembly: ExportRenderer(typeof(Forms9Patch.RootPage), typeof(Forms9Patch.iOS.PageRenderer))]
namespace Forms9Patch.iOS
{
    public class PageRenderer : Xamarin.Forms.Platform.iOS.PageRenderer
    {
        public PageRenderer()
        {
        }

        public override void DidUpdateFocus(UIFocusUpdateContext context, UIFocusAnimationCoordinator coordinator)
        {
            base.DidUpdateFocus(context, coordinator);
        }

        public override bool ShouldUpdateFocus(UIFocusUpdateContext context)
        {
            return base.ShouldUpdateFocus(context);
        }

        public override void UpdateFocusIfNeeded()
        {
            base.UpdateFocusIfNeeded();
        }

        public override void SetNeedsFocusUpdate()
        {
            base.SetNeedsFocusUpdate();
        }


        [Export("OnKeyPress:")]
        void OnKeyPress(UIKeyCommand cmd)
        {
            System.Diagnostics.Debug.WriteLine("cmd.Input=[" + cmd.Input + "] cmd.ModifierFlags[" + cmd.ModifierFlags + "]");
        }

        public override UIKeyCommand[] KeyCommands
        {
            get
            {
                var baseCommands = base.KeyCommands;
                var sel = new Selector("OnKeyPress:");

                var c1 = UIKeyCommand.Create(new NSString("1"), (UIKeyModifierFlags)0, sel, new NSString("1 one"));
                var c2 = UIKeyCommand.Create(new NSString("2"), (UIKeyModifierFlags)0, sel, new NSString("2 one"));
                var c3 = UIKeyCommand.Create(new NSString("3"), (UIKeyModifierFlags)0, sel, new NSString("3 one"));
                var c4 = UIKeyCommand.Create(new NSString("4"), (UIKeyModifierFlags)0, sel, new NSString("4 one"));
                var c5 = UIKeyCommand.Create(new NSString("5"), (UIKeyModifierFlags)0, sel);
                var c6 = UIKeyCommand.Create(new NSString("6"), (UIKeyModifierFlags)0, sel);
                var c7 = UIKeyCommand.Create(new NSString("7"), (UIKeyModifierFlags)0, sel);
                var c8 = UIKeyCommand.Create(new NSString("8"), (UIKeyModifierFlags)0, sel);
                var c9 = UIKeyCommand.Create(new NSString("9"), (UIKeyModifierFlags)0, sel);
                var c0 = UIKeyCommand.Create(new NSString("0"), (UIKeyModifierFlags)0, sel);
                var cPlus = UIKeyCommand.Create(new NSString("+"), (UIKeyModifierFlags)0, sel);
                var cMinus = UIKeyCommand.Create(new NSString("-"), (UIKeyModifierFlags)0, sel);
                var cMult = UIKeyCommand.Create(new NSString("*"), (UIKeyModifierFlags)0, sel);
                var cDiv = UIKeyCommand.Create(new NSString("/"), (UIKeyModifierFlags)0, sel);
                var cEql = UIKeyCommand.Create(new NSString("="), (UIKeyModifierFlags)0, sel);

                var commandList = new List<UIKeyCommand>();
                if (baseCommands != null && baseCommands.Length > 0)
                    commandList.AddRange(baseCommands);
                commandList.Add(c1);
                commandList.Add(c2);
                commandList.Add(c3);
                commandList.Add(c4);
                commandList.Add(c5);
                commandList.Add(c6);
                commandList.Add(c7);
                commandList.Add(c8);
                commandList.Add(c9);
                commandList.Add(c0);
                commandList.Add(cPlus);
                commandList.Add(cMinus);
                commandList.Add(cMult);
                commandList.Add(cDiv);
                commandList.Add(cEql);

                return commandList.ToArray();
            }
        }

    }
}
