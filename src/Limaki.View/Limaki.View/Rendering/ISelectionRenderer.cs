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


using System;
using Limaki.Common;
using Limaki.Drawing;

namespace Limaki.View.Rendering {
    public interface ISelectionRenderer : IRenderAction {
        IShape Shape { get; set; }
        
        bool ShowGrips { get; set; }
        int GripSize { get; set; }
        IStyle Style { get; set; }
        
        Action<IShape> UpdateGrip { get; set; }
        void UpdateSelection();
        void InvalidateShapeOutline(IShape oldShape, IShape newShape);
        void Clear();

        IWidgetBackend Backend { get; set; }
        Func<IClipper> Clipper { get; set; }
        Func<ICamera> Camera { get; set; }
    }
}