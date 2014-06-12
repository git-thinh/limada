/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Limaki.View.Vidgets;

namespace Limaki.View.GtkBackend {

    public class ToolStripDropDownButtonBackend : ToolStripDropDownButton, IToolStripDropDownButtonBackend {

        #region IVidgetBackend Member

        public Vidgets.ToolStripDropDownButton Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (Vidgets.ToolStripDropDownButton)frontend;
        }

        public void Update () { this.VidgetBackendUpdate (); }

        public void Invalidate () { this.VidgetBackendInvalidate (); }

        public void SetFocus () { this.VidgetBackendSetFocus (); }

        public void Invalidate (Xwt.Rectangle rect) { this.VidgetBackendInvalidate (rect); }

        #endregion

        bool _composed = false;
        public void SetImage (Xwt.Drawing.Image image) {
            base.Image = image;
            if (!_composed) {
                Compose ();
                _composed = true;
            }
        }

        public void SetLabel (string value) {
            base.Label = value;
        }

        public void SetToolTip (string value) {
            base.TooltipText = value;
        }

        private System.Action<object> _action;
        public void SetAction (Action<object> value) {
            _action = value;
        }

        protected override void OnButtonClicked (object sender, EventArgs e) {
            base.OnButtonClicked (sender, e);
            if (_action != null)
                _action (this);
        }

        public void InsertItem (int index, IToolStripItemBackend backend) {
            Children.Insert (index, (Gtk.Widget) backend);
        }

        public void RemoveItem (IToolStripItemBackend backend) {
            Children.Remove ((Gtk.Widget)backend);
        }
    }
}