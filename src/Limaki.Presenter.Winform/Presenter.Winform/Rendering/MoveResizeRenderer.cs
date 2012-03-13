using Limaki.Drawing;
using Limaki.Drawing.GDI;

using System;
using Limaki.Common;

using Limaki.Actions;
using Limaki.Presenter.GDI.UI;
using Limaki.Presenter.Rendering;
using Limaki.Presenter.UI;
using Xwt;


namespace Limaki.Presenter.Winform {
    public class MoveResizeRenderer : MoveResizeRendererBase {

        public override GripPainterBase GripPainter {
            get {
                if (_gripPainter == null) {
                    _gripPainter = new GripPainter();
                }
                return base.GripPainter;
            }
            set { _gripPainter = value; }
        }

        protected System.Drawing.Drawing2D.Matrix emptyMatrix =
            new System.Drawing.Drawing2D.Matrix();

        public override void OnPaint(IRenderEventArgs e) {
            IShape shape = this.Shape;
            if ((shape != null) && (ShowGrips)) {
                System.Drawing.Graphics g = ((GDISurface)e.Surface).Graphics;

                System.Drawing.Drawing2D.Matrix transform = g.Transform;
                g.Transform = emptyMatrix;

                GripPainter.Render(e.Surface);

                g.Transform = transform;
            }
        }

        bool useRegionForClipping = true;
        private System.Drawing.Region clipRegion = new System.Drawing.Region();
        public override void InvalidateShapeOutline(IShape oldShape, IShape newShape) {
            if (oldShape != null) {
                int halfborder = GripSize + 1;
                
                var decive = this.Device as IGDIControl;

                if (useRegionForClipping) {
                    lock (clipRegion) {
                        clipRegion.MakeInfinite();
                        var a = oldShape.BoundsRect;
                        var b = newShape.BoundsRect;

                        var smaller = DrawingExtensions.Intersect(a, b);
                        var bigger = DrawingExtensions.Union(a, b);
                        smaller = Camera.FromSource(smaller);
                        bigger = Camera.FromSource(bigger);

                        smaller.Inflate(-halfborder, -halfborder);
                        bigger.Inflate(halfborder, halfborder);

                        // this is a mono workaround, as it don't like strange rectangles:
                        smaller = smaller.NormalizedRectangle();
                        bigger = bigger.NormalizedRectangle();

                        // this is a mono workaround, as it don't like strange rectangles:
                        if (smaller.Size == Size.Zero) {
                            clipRegion.Intersect(GDIConverter.Convert(bigger));
                        } else {
                            clipRegion.Intersect(
                                GDIConverter.Convert(RectangleD.FromLTRB(bigger.Left, bigger.Top, bigger.Right, smaller.Top)));
                            clipRegion.Union(
                                GDIConverter.Convert(RectangleD.FromLTRB(bigger.Left, smaller.Bottom, bigger.Right, bigger.Bottom)));
                            clipRegion.Union(
                                GDIConverter.Convert(RectangleD.FromLTRB(bigger.Left, smaller.Top, smaller.Left, smaller.Bottom)));
                            clipRegion.Union(
                                GDIConverter.Convert(RectangleD.FromLTRB(smaller.Right, smaller.Top, bigger.Right, smaller.Bottom)));

                            //clipRegion.Intersect(smaller);
                            //clipRegion.Complement(bigger);
                        }
                        decive.Invalidate(clipRegion);
                    }
                } else {
                    // the have do redraw the oldShape and newShape area
                    var invalidRect = DrawingExtensions.Union(oldShape.BoundsRect, newShape.BoundsRect);
                    // transform rectangle to control coordinates
                    invalidRect = Camera.FromSource(invalidRect);

                    invalidRect.Inflate(halfborder, halfborder);
                    decive.Invalidate(invalidRect);
                }
            }
        }

        protected override void Dispose(bool disposing) {
            base.Dispose (disposing);
            if (disposing) {
                emptyMatrix.Dispose();
                emptyMatrix = null;
            }
        }



    }
}