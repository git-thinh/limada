/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Drawing;
using Xwt;
using Xwt.GdiBackend;
using System;

namespace Limaki.View.GdiBackend {

    public class GdiUtils {
        
        public static System.Drawing.Graphics CreateGraphics () {
            return System.Drawing.Graphics.FromImage (
                        new System.Drawing.Bitmap (1, 1,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb));
        }

        private static System.Drawing.Graphics _deviceContext = null;
        public static System.Drawing.Graphics DeviceContext {
            get {
                if (_deviceContext == null) {
                    _deviceContext =
                        System.Drawing.Graphics.FromImage(
                        new System.Drawing.Bitmap(1000, 1000,
                            System.Drawing.Imaging.PixelFormat.Format32bppArgb));
                }
                return _deviceContext;
            }
            set { _deviceContext = value; }
        }

        public static Size GetTextDimension(System.Drawing.Font font, string text, System.Drawing.SizeF textSize) {
            return GetTextDimension(DeviceContext, font, text, GdiConverter.GetDefaultStringFormat(), textSize);
        }

        public static Size GetTextDimension(System.Drawing.Font font, string text,
            System.Drawing.StringFormat stringFormat,
            System.Drawing.SizeF textSize) {

            return GetTextDimension(DeviceContext, font, text, stringFormat, textSize);
        }

        public static Size GetTextDimension(
            System.Drawing.Graphics g,
            System.Drawing.Font font,
            string text,
            System.Drawing.StringFormat stringFormat,
            System.Drawing.SizeF textSize) {
            var result = g.MeasureString(text, font, textSize, stringFormat);
            return new Size(Math.Ceiling(result.Width),Math.Ceiling(result.Height));
        }

   }
}