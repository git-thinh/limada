/*
 * Limaki 
 * Version 0.071
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */


using System.Drawing;
using Limaki.Common;
using Limaki.Winform.Displays;
using Limaki.Drawing;
using Limaki.Widgets;
using Limaki.Actions;
using Limaki.Drawing.Shapes;
using NUnit.Framework;
using System.Windows.Forms;
using System;

namespace Limaki.Tests.Widget {
    public class BenchmarkOneTests:WidgetDisplayTest {
        public BenchmarkOneTests():base() {}
        public BenchmarkOneTests(WidgetDisplay display):base(display) {}

        bool editorEnabled = false;
        bool dragDropEnabled = false;
        ILayout<Scene, IWidget> oldlayout = null;
        public override void Setup() {
            if (Display != null) {
                oldlayout = ( (SceneControler<Scene, IWidget>) Display.LayoutControler ).Layout;
                ( (SceneControler<Scene, IWidget>) Display.LayoutControler ).Layout = null;
            }
            base.Setup();
            // this is neccessary as the mouse cursor returns after a long time
            // back to its position and activates WidgetTextEditor
            ((SceneControler<Scene, IWidget>)Display.LayoutControler).Layout =
                new BenchmarkOneSceneFactory.LongtermPerformanceLayout(
                    Display.displayKit.dataHandler,factory.styleSheet);
            ( (WidgetLayer) Display.DataLayer ).Layout =
                ( (SceneControler<Scene, IWidget>) Display.LayoutControler ).Layout;
            factory.Arrange (Display.Data);
            Display.CommandsInvoke ();
            editorEnabled = Display.WidgetTextEditor.Enabled;
            dragDropEnabled = Display.WidgetDragDrop.Enabled;
            Display.WidgetChanger.Enabled = true;
            Display.EdgeWidgetChanger.Enabled = true;
            Display.WidgetTextEditor.Enabled = false;
            Display.WidgetDragDrop.Enabled = false;
            Display.AddWidgetAction.Enabled = false;
            Display.AddEdgeAction.Enabled = false;


            ( (WidgetLayer) Display.DataLayer ).sceneRenderer.iWidgets = 0;
        }

        public override void TearDown() {
            base.TearDown();
            Display.WidgetTextEditor.Enabled = editorEnabled;
            Display.WidgetDragDrop.Enabled = dragDropEnabled;
            if (oldlayout != null)
            ( (SceneControler<Scene, IWidget>) Display.LayoutControler ).Layout = oldlayout;
        }

        BenchmarkOneSceneFactory factory = null;
        public override Scene Scene {
            get {
                if (_scene == null) {
                    factory = new BenchmarkOneSceneFactory ();
                    base.Scene = factory.Scene;
                }
                return base.Scene;
            }
            set {
                base.Scene = value;
            }
        }

        public void MoveLinks(Rectangle bounds) {
            MoveLink(factory.Link[4],factory.Link[1]);
            MoveLink(factory.Link[5], factory.Link[3]);
        }


        

        public void MoveNode1(Rectangle bounds) {
            NeutralPosition ();
            Point startposition = factory.Node[1].Shape[Anchor.LeftTop]+new Size(10,0);
            Point position = camera.FromSource(startposition);

            MouseEventArgs e = new MouseEventArgs(MouseButtons.Left, 0, position.X, position.Y, 0);
            Display.EventControler.OnMouseDown (e);

            Assert.AreSame (Scene.Focused, factory.Node[1]);

            
            Vector v = new Vector ();
            // diagonal movement:
            v.Start = startposition;
            v.End = new Point(bounds.Right, bounds.Bottom);
            MoveAlongLine(v);

            // horizontal movement:
            v.Start = v.End;
            v.End = new Point (bounds.Right, startposition.Y);
            MoveAlongLine(v);

            // vertical movement:
            v.Start = v.End;
            v.End = new Point(startposition.X, bounds.Bottom);
            MoveAlongLine (v);

            v.Start = v.End;
            v.End = new Point(bounds.Width/2, bounds.Bottom);
            MoveAlongLine(v);

            v.Start = v.End;
            v.End = new Point(bounds.Width / 2, factory.distance.Height);
            MoveAlongLine(v);

            v.Start = v.End;
            v.End = startposition;
            MoveAlongLine(v);

            position = camera.FromSource (v.End);
            e = new MouseEventArgs(MouseButtons.Left, 0, position.X, position.Y, 0);
            Display.EventControler.OnMouseUp(e);
        }

        [Test]
        public void MoveAlongSceneBoundsTest() {
            string testName = "MoveAlongSceneBoundsTest";
            this.ReportDetail (testName);
            
            ticker.Start();
            Rectangle bounds = Scene.Shape.BoundsRect;
            this.ReportDetail ("bounds:\t" + bounds);
            MoveNode1 (bounds);
            
            // this test does not work with zoom!
            Display.ZoomState = Limaki.Actions.ZoomState.Custom;
            float inc = 0.05f;
            while (Display.ZoomFactor < 1.5f) {
                Display.ZoomFactor = Display.ZoomFactor+inc;
                Display.UpdateZoom ();
                Application.DoEvents();
            }

            MoveLinks(bounds);

            while (Display.ZoomFactor < 2f) {
                Display.ZoomFactor = Display.ZoomFactor + inc;
                Display.UpdateZoom();
                Application.DoEvents();
            }

            MoveNode1(bounds);

            NeutralPosition();

            while (Display.ZoomFactor > 1f) {
                Display.ZoomFactor = Display.ZoomFactor - inc;
                Display.UpdateZoom();
                Application.DoEvents();
            }

            ticker.Stop ();
            this.ReportDetail (
                testName + " \t" +
                ticker.ElapsedInSec () + " sec \t" +
                ticker.FramePerSecond () + " fps \t"+
                ( (WidgetLayer) Display.DataLayer ).sceneRenderer.iWidgets +" widgets \t"
                );
        }
    }
}
