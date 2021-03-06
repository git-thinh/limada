/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.View;
using Limaki.View.DragDrop;
using Limaki.View.SwfBackend.DragDrop;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.Vidgets;
using Limaki.View.Viz;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.UI;
using Limaki.View.Viz.Visualizers;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Xwt;
using Xwt.GdiBackend;
using Xwt.SwfBackend;
using ApplicationContext = Limaki.Common.IOC.ApplicationContext;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Size = Xwt.Size;
using SWF = System.Windows.Forms;

namespace Limaki.View.SwfBackend.Viz {

    public abstract class DisplayBackend: UserControl {
        public bool ScrollBarsVisible {
            set {
                this.HScroll = value;
                this.VScroll = value;
            }
        }

    }

    public abstract class DisplayBackend<T> : DisplayBackend, IDisplayBackend<T> {

        public DisplayBackend () { Initialize (); }

        public abstract DisplayFactory<T> CreateDisplayFactory (DisplayBackend<T> backend);


        public bool Opaque {
            get{ return this.GetStyle (System.Windows.Forms.ControlStyles.Opaque);}
            set {
                this.SetStyle (System.Windows.Forms.ControlStyles.Opaque, value);
                if (this.BackendRenderer != null)
                    (this.BackendRenderer as SwfBackendRenderer<T>).Opaque = value;
            }
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                cp.Style &= ~0x02000000;  // Turn off WS_CLIPCHILDREN
                return cp;
            }
        } 

        protected void Initialize () {
            if (Registry.ConcreteContext == null) {
                var resourceLoader = new SwfContextResourceLoader ();
                Registry.ConcreteContext = new ApplicationContext ();
                resourceLoader.ApplyResources (Registry.ConcreteContext);
            }

            this.AllowDrop = true;
            this.AutoScroll = true;

            if (!this.DesignMode) {
                var controlStyle =
                    System.Windows.Forms.ControlStyles.UserPaint
                    | System.Windows.Forms.ControlStyles.AllPaintingInWmPaint
                    | System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer;

 
                this.SetStyle (controlStyle, true);

                Opaque = true; //!Commons.Mono; // opaque works on mono too, but is slower
            }

            
        }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            Display<T> display = null;
            var factory = CreateDisplayFactory (this);
            if (frontend != null)
                display = (Display<T>) frontend;
            else
                display = factory.Create ();
            _display = display;
            factory.Compose (display);
            if (!this.DesignMode) {
                this._backendRenderer.Opaque = this.Opaque;
            }
        }

        IVidgetEventSink EventSink { get; set; }
        public void InitializeEvents (IVidgetEventSink eventSink) {
            EventSink = eventSink;
            this.ComposeEvents (EventSink);
        }

        private IDisplay<T> _display = null;

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public IDisplay<T> Display {
            get {
                if (_display == null) {
                    InitializeBackend (null, null);
                }
                return _display;
            }
            set { _display = value; }
        }

        IDisplay IDisplayBackend.Frontend { get { return this.Display; } set { this.Display = value as IDisplay<T>; } }

        IDisplay<T> IDisplayBackend<T>.Frontend { get { return this.Display; } set { this.Display = value; } }

        protected SwfBackendRenderer<T> _backendRenderer = null;

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public virtual IBackendRenderer BackendRenderer { get { return _backendRenderer; } set { _backendRenderer = value as SwfBackendRenderer<T>; } }

        protected SwfViewport _backendViewPort = null;

        [Browsable (false)]
        [DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
        public virtual IViewport BackendViewPort { get { return _backendViewPort; } set { _backendViewPort = value as SwfViewport; } }

        public override Color BackColor {
            get { return base.BackColor; }
            set {
                base.BackColor = value;
                var color = GdiConverter.ToXwt (value);
                if (!Display.BackColor.Equals (color)) {
                    Display.BackColor = color;
                }

            }
        }

        protected override void OnPaint (System.Windows.Forms.PaintEventArgs e) {
            base.OnPaint (e);
            _backendRenderer.OnPaint (e);
        }

        protected override void OnSizeChanged (System.EventArgs e) {
            if (_backendViewPort != null) {
                _backendViewPort.OnSizeChanged (e, base.OnSizeChanged);
            } else {
                base.OnSizeChanged (e);
            }

        }

        protected override void OnScroll (System.Windows.Forms.ScrollEventArgs se) {
            if (_backendViewPort != null) {
                _backendViewPort.OnScroll (se);
            } else {
                base.OnScroll (se);
            }
        }

        private const int WM_KEYDOWN = 0x0100;
        private const int WM_KEYUP = 0x0101;
        private const int WM_SYSKEYDOWN = 0x0104;
        private const int WM_SYSKEYUP = 0x0105;

        /// <summary>
        /// workaround as cursor keys don't rise OnKeyDown-Events
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey (ref System.Windows.Forms.Message msg, System.Windows.Forms.Keys keyData) {
            bool result = false;
            if (this.Focused &&
                (keyData == System.Windows.Forms.Keys.Down || keyData == System.Windows.Forms.Keys.Up || keyData == System.Windows.Forms.Keys.Left || keyData == System.Windows.Forms.Keys.Right)) {
                var e = new KeyEventArgs (keyData);
                if (msg.Msg == WM_KEYDOWN)
                    OnKeyDown (e);
                if (msg.Msg == WM_KEYUP)
                    OnKeyUp (e);
                result = true;
            } else {
                result = base.ProcessCmdKey (ref msg, keyData);
            }
            return result;
        }

        protected override void OnKeyDown (KeyEventArgs e) {
            Debug.WriteLine (string.Format ("{0} {1} {2}", e.KeyCode, e.KeyData, e.KeyValue));
            var ev = Converter.Convert (e, this.PointToClient (MousePosition));
            if (Display.Data != null)
                Display.ActionDispatcher.OnKeyPressed (ev);
            if (!ev.Handled)
                base.OnKeyDown (e);
        }

        protected override void OnKeyPress (System.Windows.Forms.KeyPressEventArgs e) {
            base.OnKeyPress (e);
            //if (Display.Data != null)
            //    Display.EventControler.OnKeyPress(new KeyActionPressEventArgs(e.KeyChar));
        }

        protected override void OnKeyUp (KeyEventArgs e) {
            base.OnKeyUp (e);
            if (Display.Data != null)
                Display.ActionDispatcher.OnKeyReleased (Converter.Convert (e, this.PointToClient (MousePosition)));
        }

        protected override void OnMouseDown (System.Windows.Forms.MouseEventArgs e) {
            base.OnMouseDown (e);
            if (Display.Data != null)
                Display.ActionDispatcher.OnMouseDown (Converter.Convert (e));
        }

        protected override void OnMouseHover (EventArgs e) {
            base.OnMouseHover (e);

            var pos = this.PointToClient (MousePosition);
            MouseActionEventArgs mouseEventArgs =
                new MouseActionEventArgs (
                    Converter.Convert (MouseButtons),
                    Converter.ConvertModifiers (System.Windows.Forms.Form.ModifierKeys),
                    0, pos.X, pos.Y, 0);
            if (Display.Data != null)
                Display.ActionDispatcher.OnMouseHover (mouseEventArgs);

        }

        protected override void OnMouseMove (System.Windows.Forms.MouseEventArgs e) {
            base.OnMouseMove (e);
            if (Display.Data != null)
                Display.ActionDispatcher.OnMouseMove (Converter.Convert (e));
        }

        protected override void OnMouseWheel (System.Windows.Forms.MouseEventArgs e) {
            if (Display.Data != null) {
                var ev = Converter.Convert (e);
                if (ev.Modifiers == Xwt.ModifierKeys.None) {
                    // remark: another possibility would be to call OnScroll or _backenViewPort.OnScroll
                    base.OnMouseWheel (e);
                    Display.Viewport.Update ();
                } else if (ev.Modifiers == Xwt.ModifierKeys.Control) {
                    var zoomAction = Display.ActionDispatcher.GetAction<ZoomAction> ();
                    if (zoomAction != null) {
                        zoomAction.Zoom (ev.Location, ev.Delta > 0);
                    }
                }
            }
        }

        protected override void OnMouseUp (System.Windows.Forms.MouseEventArgs e) {
            base.OnMouseUp (e);
            if (Display.Data != null)
                Display.ActionDispatcher.OnMouseUp (Converter.Convert (e));
        }

        #region DragDrop
        
        //this is called by Control.DoDragDrop
        protected override void OnGiveFeedback (System.Windows.Forms.GiveFeedbackEventArgs e) { base.OnGiveFeedback (e); }

        //this is called by Control.DoDragDrop
        protected override void OnQueryContinueDrag (System.Windows.Forms.QueryContinueDragEventArgs e) { base.OnQueryContinueDrag (e); }

        private DragDropAction DragDropActionFromKeyState (int keyState, DragDropAction allowedEffect) {

            // Set the effect based upon the KeyState.
            if ((keyState & (8 + 32)) == (8 + 32) &&
                (allowedEffect & DragDropAction.Link) == DragDropAction.Link) {
                // KeyState 8 + 32 = CTL + ALT

                // Link drag and drop effect.
                return DragDropAction.Link;

            } else if ((keyState & 32) == 32 &&
                       (allowedEffect & DragDropAction.Link) == DragDropAction.Link) {

                // ALT KeyState for link.
                return DragDropAction.Link;

            } else if ((keyState & 4) == 4 &&
                       (allowedEffect & DragDropAction.Move) == DragDropAction.Move) {

                // SHIFT KeyState for move.
                return DragDropAction.Move;

            } else if ((keyState & 8) == 8 &&
                       (allowedEffect & DragDropAction.Copy) == DragDropAction.Copy) {

                // CTL KeyState for copy.
                return DragDropAction.Copy;

            } else if ((allowedEffect & DragDropAction.Move) == DragDropAction.Move) {

                // By default, the drop action should be copy, if allowed.
                return DragDropAction.Copy;

            }
            return DragDropAction.Copy;
        }

        protected override void OnDragOver (System.Windows.Forms.DragEventArgs e) {
            var dropHandler = Display.ActionDispatcher as IDropHandler;
            if (dropHandler != null && Display.Data != null) {
                var ev = e.ToXwtDragOver (this);
                ev.AllowedAction = DragDropActionFromKeyState (e.KeyState, ev.Action);
                dropHandler.DragOver (ev);
                e.Effect = ev.AllowedAction.ToSwf ();
            }
            base.OnDragOver (e);

        }

        protected override void OnDragDrop (System.Windows.Forms.DragEventArgs e) {
            var dropHandler = Display.ActionDispatcher as IDropHandler;
            if (dropHandler != null && Display.Data != null) {
                dropHandler.OnDrop (e.ToXwt (this));
            }
            base.OnDragDrop (e);

        }

        protected override void OnDragLeave (EventArgs e) {
            var dropHandler = Display.ActionDispatcher as IDropHandler;
            if (dropHandler != null && Display.Data != null) {
                dropHandler.DragLeave (e);
            }
            base.OnDragLeave (e);
        }

        #endregion


        #region IVidgetBackend Member

        Size IVidgetBackend.Size { get { return this.Size.ToXwt (); } }
        
        public string ToolTipText { get; set; }
        
        IVidget IVidgetBackend.Frontend { get { return this.Display; } }

        public void QueueDraw() {
            Invalidate ();    
        }

        public void QueueDraw (Xwt.Rectangle rect) {
            this.Invalidate (rect.ToGdi ());
            System.Windows.Forms.Application.DoEvents ();
        }

        void IVidgetBackend.SetFocus () { this.Focus (); }

        #endregion

    
    }

}
