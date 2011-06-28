﻿using Limaki.Presenter.Viewers.Winform;
using System.Drawing.Printing;
using Limaki.Visuals;
using Limaki.Drawing;
using System.Windows.Forms;

namespace Limaki.UseCases.Winform {
    public class PrintManager {
        ImageExporter painter = null;

        public PrintDocument CreatePrintDocument(IGraphScene<IVisual, IVisualEdge> scene, IGraphLayout<IVisual, IVisualEdge> layout) {
        
            this.painter = new ImageExporter(scene, layout);

            painter.Viewport.ClipOrigin = scene.Shape.Location;

            PrintDocument doc = new PrintDocument();
            doc.PrintPage += new PrintPageEventHandler(doc_PrintPage);
            return doc;
        }

        void doc_PrintPage(object sender, PrintPageEventArgs e) {
            painter.Paint(e.Graphics, e.PageBounds);
            e.HasMorePages = false;
        }


    }
}