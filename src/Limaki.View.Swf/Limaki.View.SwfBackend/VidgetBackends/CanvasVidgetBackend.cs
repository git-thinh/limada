/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.Windows.Forms;
using Limaki.View;
using Limaki.View.Vidgets;
using Xwt.GdiBackend;
using Xwt;

namespace Limaki.View.SwfBackend.VidgetBackends {

    public class CanvasVidgetBackend : UserControl, ICanvasVidgetBackend {

        protected override void OnPaint (PaintEventArgs e) {

            base.OnPaint(e);

            if (Frontend != null)
                using (var graphics = new GdiContext(e.Graphics)) {
                    Frontend.DrawContext(new Xwt.Drawing.Context(graphics, Toolkit.CurrentEngine), e.ClipRectangle.ToXwt());
                }
        }


        #region IVidgetBackend-Implementation

        public ICanvasVidget Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (ICanvasVidget) frontend;
        }

        IVidget IVidgetBackend.Frontend { get { return this.Frontend; } }

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        void IVidgetBackend.SetFocus () { this.Focus (); }

        #endregion

    }
}