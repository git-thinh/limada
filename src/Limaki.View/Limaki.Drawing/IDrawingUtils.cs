﻿
using Xwt;
using Xwt.Drawing;

namespace Limaki.Drawing {
    public interface IDrawingUtils {
       
        object GetCustomLineCap(double arrowWidth, double arrowHeigth);
       
        Pen CreatePen ( Color color );
        Matrice NativeMatrice();
        Size GetTextDimension(string text, IStyle style);
    }
}