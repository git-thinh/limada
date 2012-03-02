// 
// DrawingTransforms.cs
//  
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
// 
// Copyright (c) 2011 Xamarin Inc
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using Xwt;
using Xwt.Drawing;

namespace Samples
{
	public class DrawingTransforms: Canvas
	{
		public DrawingTransforms ()
		{
		}
		
		protected override void OnDraw (Xwt.Drawing.Context ctx)
		{
			base.OnDraw (ctx);
			
			ctx.SetLineDash (15, 10, 10, 5, 5);
			ctx.Rectangle (100, 100, 100, 100);
			ctx.Stroke ();
			ctx.SetLineDash (0);
			
			ImageBuilder ib = new ImageBuilder (30, 30, ImageFormat.ARGB32);
			ib.Context.Arc (15, 15, 15, 0, 360);
			ib.Context.SetColor (new Color (1, 0, 1));
			ib.Context.Rectangle (0, 0, 5, 5);
			ib.Context.Fill ();
			var img = ib.ToImage ();
			ctx.DrawImage (img, 90, 90);
			ctx.DrawImage (img, 90, 140, 50, 10);
			
			ctx.Arc (190, 190, 15, 0, 360);
			ctx.SetColor (new Color (1, 0, 1, 0.4));
			ctx.Fill ();
			
			ctx.Save ();
			ctx.Translate (90, 220);
			ctx.Pattern = new ImagePattern (img);
			ctx.Rectangle (0, 0, 100, 70);
			ctx.Fill ();
			ctx.Restore ();
			
			ctx.Translate (30, 30);
			double end = 270;
			
			for (double n = 0; n<=end; n += 5) {
				ctx.Save ();
				ctx.Rotate (n);
				ctx.MoveTo (0, 0);
				ctx.RelLineTo (30, 0);
				double c = n / end;
				ctx.SetColor (new Color (c, c, c));
				ctx.Stroke ();
				ctx.Restore ();
			}
		}
	}
}

