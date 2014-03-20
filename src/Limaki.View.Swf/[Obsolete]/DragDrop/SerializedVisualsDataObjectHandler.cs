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

using Limaki.Graphs;
using Limaki.View.Visuals;

namespace Limaki.View.Swf.DragDrop {
    public class SerializedVisualsDataObjectHandler : SerializedDataObjectHandler<IGraph<IVisual, IVisualEdge>, IVisual>, IVisualsDataObjectHandler {
        public override string[] DataFormats {
            get { return new string[] { "Limaki_IVisual-Binary" }; }
        }
    }
}