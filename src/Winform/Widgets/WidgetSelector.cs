/*
 * Limaki 
 * Version 0.064
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

using System;
using System.Drawing;
using System.Windows.Forms;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Widgets;

namespace Limaki.Winform.Widgets {

    /// <summary>
    /// Selects a widget on mouse down or mouse move
    /// Sets scene.focused and scene.selected or scene.hovered
    /// </summary>
    public class WidgetSelector:MouseActionBase, IDragDropAction {
        public WidgetSelector():base() {
            this.Priority = ActionPriorities.SelectionPriority - 10;
        }
        ///<directed>True</directed>
        ITransformer transformer = null;
        IWinControl control = null;
        public WidgetSelector(Handler<Scene> sceneHandler, IWinControl control, ITransformer transformer):this() {
            this.control = control;
            this.transformer = transformer;
            this.SceneHandler = sceneHandler;
        }

        ///<directed>True</directed>
        Handler<Scene> SceneHandler;
        public Scene Scene {
            get { return SceneHandler(); }
        }

        private int _hitSize = 5;
        /// <summary>
        /// has to be the same as in WidgetResizer
        /// </summary>
        public int HitSize {
            get { return _hitSize; }
            set { _hitSize = value; }
        }

        private IWidget _current = null;
        public IWidget Current {
            get { return _current; }
            set { _current = value; }
        }

        IWidget HitTest(Point p) {
            IWidget result = null;
            Point sp = transformer.ToSource(p);

            result = Scene.Hit(sp, HitSize);

            return result;
        }

        bool _exclusive = false;
        public override bool Exclusive {
            get { return _exclusive; }
            protected set { _exclusive = value; }
        }

        void ClearSelection() {
            if ((Form.ModifierKeys & Keys.Control) == 0) {
                foreach (IWidget w in Scene.Selected.Elements) {
                    if (w != Current)
                        Scene.Commands.Add (new Command<IWidget> (w));
                }
                Scene.Selected.Clear ();
                if (Scene.Focused != null) {
                    Scene.Selected.Add(Scene.Focused);
                }
            }
        }
        public override void OnMouseDown(MouseEventArgs e) {
            if (Scene == null) return;
            base.OnMouseDown(e);
            if (e.Button == MouseButtons.Left) {
                IWidget last = Scene.Focused;
                Current = HitTest (e.Location);

                Resolved = (Current != null)&&(Scene.Focused != Current);
                Scene.Focused = Current;
                if (Current != last && last != null) {
                    Scene.Commands.Add(new Command<IWidget>(last));
                }
                if (Scene.Focused != null) {
                    Point sp = transformer.ToSource(e.Location);
                    if (!Scene.Focused.Shape.IsBorderHit(sp, HitSize)) {
                        ClearSelection();
                        Scene.Commands.Add(new Command<IWidget>(Scene.Focused));
                    }
                }
                if (Resolved && Current != null) {
                    if (!Scene.Selected.Contains(Current)) {
                        ClearSelection();
                        Scene.Selected.Add(Current);
                    }
                    Scene.Commands.Add(new Command<IWidget>(Current));
                }
                control.CommandsExecute();
            }
        }


        public override void OnMouseMove(MouseEventArgs e) {
            if (Scene == null) return;
            Exclusive = false;
            IWidget before = Scene.Hovered;
            IWidget hit = HitTest(e.Location);
            if(hit != Scene.Focused || Scene.Focused == null) {
                Current = hit;
            }
            if (Current != null && Scene.Selected.Contains(Current)&& e.Button== MouseButtons.None) {
                if (Scene.Focused != null && Scene.Focused != Current) {
                    Scene.Commands.Add (new Command<IWidget> (Scene.Focused));
                }
                Scene.Focused = Current;
                Scene.Hovered = null;
            } else {
                Scene.Hovered = Current;
            }
            if (before != Current) {
                if (before != null)
                    Scene.Commands.Add(new Command<IWidget>(before));
                if (Current != null)
                    Scene.Commands.Add(new Command<IWidget>(Current));
                control.CommandsExecute ();
            }
            Resolved = false;
        }

        #region IDragDropAction Member
        bool _dragging = true;
        public virtual bool Dragging {
            get { return _dragging; }
            set { _dragging = value; }
        }

        public void OnGiveFeedback( GiveFeedbackEventArgs e ) {}

        public void OnQueryContinueDrag( QueryContinueDragEventArgs e ) {}

        public void OnDragOver( DragEventArgs e ) {
            Point pt = control.PointToClient(new Point(e.X, e.Y));
            MouseEventArgs em = new MouseEventArgs(MouseButtons.None, 0, pt.X, pt.Y, 0);
            this.OnMouseMove(em);
        }

        public void OnDragDrop( DragEventArgs e ) {}
        public void OnDragLeave( EventArgs e ) { }

        #endregion
    }
}