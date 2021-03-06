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

using Limaki.View.Vidgets;
using System;
using LVV = Limaki.View.Vidgets;

namespace Limaki.View.WpfBackend {

    public class ToolbarButtonBackend : ToolbarItemBackend<ToolbarButton>, IToolbarButtonBackend {

        public new LVV.ToolbarButton Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (LVV.ToolbarButton)frontend;
        }

        public override void Compose () {
            base.Compose ();
            Control.Click -= OnButtonClicked;
            Control.Click += OnButtonClicked;
        }
        public virtual void SetImage (Xwt.Drawing.Image value) {
            Control.Image = value;
        }

        public virtual void SetLabel (string value) {
            Control.Label = value;
        }

        protected virtual void OnButtonClicked (object sender, EventArgs e) {
            if (_action != null)
                _action (this);
        }

        public bool IsCheckable {
            get { return Control.IsCheckable; }
            set { Control.IsCheckable = value; }
        }

        public bool? IsChecked {
            get { return Control.IsChecked; }
            set { Control.IsChecked = value; }
        }

    }
}