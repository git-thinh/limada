using Limaki.Common;
using Limaki.Visuals;
using Limaki.View.UI;
using Limaki.Drawing;
using Xwt;
using System;

namespace Limaki.Tests.View.Display {
    [Obsolete]
    public class VisualsDisplayTest008_1:VisualsDisplayTest008 {
        public void SelectorVersusMulitSelectTest() {
            Display.SelectAction.Enabled = true;

            var factory = Registry.Pool.TryGetCreate<IVisualFactory>();
            var w = factory.CreateItem ("SelectorVersusMulitSelectTest");
            Scene.Add (w);
            Display.Layout.Reset ();
            Display.Perform();

            var p = w.Shape[Anchor.LeftBottom];
            p = Display.Viewport.Camera.FromSource (p);
            var e = new MouseActionEventArgs (MouseActionButtons.Left, ModifierKeys.None, 1, p.X+5, p.Y+1, 0);

            Display.EventControler.OnMouseDown(e);
            //Display.EventControler.OnMouseUp(e);
            DoEvents();
            e = new MouseActionEventArgs(MouseActionButtons.None, ModifierKeys.None, 1, p.X + 5, p.Y + 2, 0);
            Display.EventControler.OnMouseMove(e);
            DoEvents();

            e = new MouseActionEventArgs(MouseActionButtons.Left, ModifierKeys.None, 1, p.X + 5, p.Y + 3, 0);
            Display.EventControler.OnMouseDown (e);
            Display.EventControler.OnMouseMove(e);

            for (int i = 0; i<100;i++) {
                e = new MouseActionEventArgs(MouseActionButtons.Left, ModifierKeys.None, 1, e.Location.X + 1, e.Location.Y + 1,0);
                Display.EventControler.OnMouseMove(e);
            }


            //Display.EventControler.OnMouseUp(e);
        }
    }
}