using Limaki.Drawing;
using Limaki.View.Viewers.Swf;
using System.Drawing.Printing;
using System.Windows.Forms;
using Limaki.Visuals;

namespace Limaki.Swf.Backends.UseCases {
    public class PrintManager {
        ImageExporter painter = null;

        public PrintDocument CreatePrintDocument(IGraphScene<IVisual, IVisualEdge> scene, IGraphSceneLayout<IVisual, IVisualEdge> layout) {
        
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