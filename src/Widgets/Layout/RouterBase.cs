/*
 * Limaki 
 * Version 0.07
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

namespace Limaki.Widgets.Layout {
    public class RouterBase : IRouter {
        public virtual void routeEdge(IEdgeWidget edge) {
            if (edge.Root is IEdgeWidget) {
                edge.RootAnchor = Anchor.Center;
            }
            if (edge.Leaf is IEdgeWidget) {
                edge.LeafAnchor = Anchor.Center;
            }
        }
    }
}