using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System;
using Limaki.Drawing.Gdi;
using Limaki.Drawing;
using Limaki.Common;
using Limaki.View.Rendering;
using Limaki.View.UI;
using Xwt;
using Size = Xwt.Size;
using Rectangle = Xwt.Rectangle;
using Xwt.Gdi.Backend;

namespace Limaki.View.Gdi.UI {

    public class GdiImageLayer : Layer<Image> {

        Image _cache = null;
        Image _value = null;
        bool isCacheOwner = false;

        public override Image Data {
            get {
                bool refresh = _value != base.Data;
                _value = base.Data;
                if (refresh) {
                    DisposeCache();
                    if (_value != null) {
                        _cache = OptimizedImage(_value);
                        isCacheOwner = _cache != _value;
                    }
                    DataChanged();
                }
                return _cache;
            }
        }

        public override void DataChanged() {
            var data = this.Data;
            if (data != null) {
                this.Size = data.Size.ToXwt();
            } else {
                this.Size = Size.Zero;
            }
            hadError = false;
        }

        /// <summary>
        /// returns an optimized image for fast drawing
        /// to have a fast Graphis.DrawImage, a Bitmap should be in PixelFormat.Format32bppPArgb
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public Image OptimizedImage(Image source) {
            if (source.PixelFormat == PixelFormat.Format32bppPArgb) {
                return source;
            } else {
                try {
                    Image result = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppPArgb);

                    using (var g = Graphics.FromImage(result)) {
                        g.InterpolationMode = InterpolationMode.HighQualityBilinear;
                        g.CompositingMode = CompositingMode.SourceCopy;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        // draws image too small:
                        // g.DrawImageUnscaled (source, 0, 0);
                        g.DrawImage(source, 0, 0, source.Width, source.Height);
                        g.Flush();
                    }
                    return result;

                } catch (Exception ex) {
                    ExceptionHandler.Catch(ex, MessageType.OK);
                    return source;
                }
            }
        }

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler>(); }
        }

        protected virtual void DisposeCache() {
            if (isCacheOwner && (_cache != null) && (_cache is IDisposable)) {
                ((IDisposable)_cache).Dispose();
            }
            _cache = null;
        }

        bool hadError = false;
        public override void OnPaint(IRenderEventArgs e) {
            var data = this.Data;
            if (data != null && ! hadError) {
                try {
                    var g = ((GdiSurface)e.Surface).Graphics;
                    g.Transform = ((GdiMatrice)this.Camera.Matrice).Matrix;
                    g.InterpolationMode = InterpolationMode.Low;
                    g.CompositingMode = CompositingMode.SourceCopy;
                    g.CompositingQuality = CompositingQuality.HighSpeed;

                    var rc = Camera.ToSource(e.Clipper.Bounds);

                    rc = rc.Intersect(new Xwt.Rectangle(0, 0, Size.Width, Size.Height));
                    rc = rc.Inflate(new Size(1, 1));

                    g.DrawImage(data, 
                        (float)rc.Location.X,
                        (float)rc.Location.Y,
                        rc.ToGdi (),
                        GraphicsUnit.Pixel);

                    g.Transform.Reset();
                } catch (Exception ex) {
                    ExceptionHandler.Catch(ex, MessageType.OK);
                    hadError = true;
                }
            }
        }


    }
}