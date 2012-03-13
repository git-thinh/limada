using Font = System.Drawing.Font;
using Xwt.Backends;
using Xwt.Drawing;

namespace Xwt.Gdi.Backend {

    public class FontBackendHandler : IFontBackendHandler {

        public object CreateFromName(string fontName, double size) {
            return new Font(fontName, (float)size);
        }

        #region IFontBackendHandler implementation

        public object Copy(object handle) {
            Font d = (Font)handle;
            return d.Clone();
        }


        public object SetSize(object handle, double size) {
            var d = (Font)handle;
            if (d.Size != (int)size) {
                d = new Font(d.FontFamily, (float)size, d.Style);
            }
            return d;
        }

        public object SetFamily(object handle, string family) {
            var d = (Font)handle;
            if (d.FontFamily.Name != family) {
                d = new Font(family, d.Size, d.Style);
            }
            return d;
        }

        System.Drawing.FontStyle Convert(FontStyle style, FontWeight weight) {
            var result = System.Drawing.FontStyle.Regular;
            if (FontStyle.Italic == style || FontStyle.Oblique == style)
                result |= System.Drawing.FontStyle.Italic;
            if (FontWeight.Heavy == weight || FontWeight.Bold == weight || FontWeight.Ultrabold == weight)
                result |= System.Drawing.FontStyle.Bold;
            return result;
        }

         FontStyle Convert(System.Drawing.FontStyle style) {
             if (style == System.Drawing.FontStyle.Italic)
                 return FontStyle.Italic;
             return FontStyle.Normal;

         }

         FontWeight ConvertW(System.Drawing.FontStyle style) {
             if (style == System.Drawing.FontStyle.Bold)
                 return FontWeight.Bold;
             return FontWeight.Normal;

         }

        public object SetStyle(object handle, FontStyle style) {
            var d = (Font)handle;
            var oldStyle = Convert(d.Style);
            var w = ConvertW(d.Style);

            if (oldStyle != style) {
                d = new Font(d.FontFamily, d.Size, Convert(style, w));
            }
            return d;

        }

        public object SetWeight(object handle, FontWeight weight) {
            var d = (Font)handle;
            var oldW = ConvertW(d.Style);
            var s = Convert(d.Style);

            if (oldW != weight) {
                d = new Font(d.FontFamily, d.Size, Convert(s, weight));
            }
            return d;
        }

        public object SetStretch(object handle, FontStretch stretch) {
           
            return handle;
        }

        public double GetSize(object handle) {
            var d = (Font)handle;
            return d.SizeInPoints;
        }

        public string GetFamily(object handle) {
            var d = (Font)handle;
            return d.FontFamily.Name;
        }

        public FontStyle GetStyle(object handle) {
            var d = (Font)handle;
            return Convert(d.Style);
        }

        public FontWeight GetWeight(object handle) {
            var d = (Font)handle;
            return ConvertW(d.Style);
        }

        public FontStretch GetStretch(object handle) {
            return FontStretch.Normal;
        }
        #endregion


    }
}
