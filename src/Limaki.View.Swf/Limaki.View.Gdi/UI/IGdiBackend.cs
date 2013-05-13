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


using System.Drawing;
using System.Drawing.Drawing2D;

namespace Limaki.View.Gdi.UI {

    public interface IGdiBackend:IVidgetBackend {
        void Invalidate(Region region);
        void Invalidate(GraphicsPath path);
    }
}