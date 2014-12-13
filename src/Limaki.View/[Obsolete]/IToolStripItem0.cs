/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using Xwt;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {

    [Obsolete]
    public interface IToolStripItem0 {
        Image Image { get; set; }
        string Label { get; set; }
        string ToolTipText { get; set; }
        Size Size { get; set; }
        event EventHandler Click;
    }

}