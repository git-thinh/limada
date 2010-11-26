/*
 * Limaki 
 * Version 0.064
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Drawing;
using Limaki.Graphs;

namespace Limaki.Widgets {

    public interface ILinkWidget:IWidget,IEdge<IWidget> {
        Anchor RootAnchor { get; set; }
        Anchor LeafAnchor { get; set; }
    }
}