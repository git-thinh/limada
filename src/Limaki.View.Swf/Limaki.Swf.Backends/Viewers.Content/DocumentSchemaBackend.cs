/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 */


using System.Windows.Forms;
using Limaki.Drawing;
using Limaki.View.Visualizers;
using Limaki.Visuals;
using Limaki.View.Swf.Visualizers;
using System.Drawing;
using Limada.View;
using Xwt.Gdi.Backend;

namespace Limaki.View.Swf.Backends {
    
    public partial class DocumentSchemaBackend : UserControl, IZoomTarget, IDocumentSchemaBackend {

        public DocumentSchemaViewer Frontend { get; set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (DocumentSchemaViewer)frontend;
        }

        //TODO: replace with factory methods
        public DocumentSchemaBackend():this(new SwfDocumentSchemaViewer()) {}

        public DocumentSchemaBackend(DocumentSchemaViewer frontend) {
            this.Frontend = frontend;
            Compose();
        }

        private void Compose () {

            var pagesDisplayBackend = new SwfVisualsDisplayBackend() {
                Dock = System.Windows.Forms.DockStyle.Right,
                Width = Frontend.GetDefaultWidth(),
                TabStop = false
            };

            var panel = new Panel { Dock = System.Windows.Forms.DockStyle.Fill, BackColor = Color.White };
            this.SuspendLayout();

            Frontend.PagesDisplay = pagesDisplayBackend.Display as IGraphSceneDisplay<IVisual, IVisualEdge>;
            Frontend.Compose();

            var splitter = new System.Windows.Forms.Splitter { Dock = DockStyle.Right };
            this.Controls.AddRange(new Control[] { panel, splitter, pagesDisplayBackend });

            this.ResumeLayout(false);
            this.PerformLayout();

            this.PerformLayout();
            Application.DoEvents();

            Frontend.AttachContentViewerBackend = contentViewer => {
                var contentControl = (contentViewer.Backend as System.Windows.Forms.Control);
                if (contentControl.Dock != DockStyle.Fill)
                    contentControl.Dock = DockStyle.Fill;
                
                if (!panel.Controls.Contains(contentControl)) {
                    panel.Controls.Clear();
                    panel.Controls.Add(contentControl);
                    Application.DoEvents();
                }
            };

        }

        void SplitContainer_KeyDown(object sender, KeyEventArgs e) {
            e.Handled = false;
        }

        #region IZoomTarget Member

        public ZoomState ZoomState {
            get { return Frontend.ZoomState; }
            set { Frontend.ZoomState = value; }
        }

        public double ZoomFactor {
            get { return Frontend.ZoomFactor; }
            set { Frontend.ZoomFactor = value; }
        }

        public void UpdateZoom () {
            Frontend.UpdateZoom();
        }

        #endregion

        #region IVidgetBackend-Implementation

        Xwt.Rectangle IVidgetBackend.ClientRectangle {
            get { return this.ClientRectangle.ToXwt(); }
        }

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt(); }
        }


        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate(rect.ToGdi());
        }

        Xwt.Point IVidgetBackend.PointToClient (Xwt.Point source) { return PointToClient(source.ToGdi()).ToXwt(); }

        #endregion
    }
}