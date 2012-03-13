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
 * http://limada.sourceforge.net
 * 
 */


using Limaki.Drawing;
using Xwt;

namespace Limaki.View.UI {
    public interface IUISystemInformation {
        Size DragSize { get; }
        int DoubleClickTime { get; }
        int VerticalScrollBarWidth { get; }
        int HorizontalScrollBarHeight { get; }
    }
}