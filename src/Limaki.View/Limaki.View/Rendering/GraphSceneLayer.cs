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

using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.View.UI;

namespace Limaki.View.Rendering {
    public class GraphSceneLayer<TItem, TEdge> : Layer<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {

        public virtual Get<IGraphSceneLayout<TItem, TEdge>> Layout { get; set; }

        public override void OnPaint(IRenderEventArgs e) {
            this.Renderer.Render(this.Data, e);
        }

        public override void DataChanged() { }

        }
}