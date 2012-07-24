﻿using Limaki.Graphs;
using Limaki.View.Rendering;
using Limaki.XwtAdapter;
using Limaki.Drawing;

namespace Limaki.View {

    public class ContextGraphSceneLayer<TItem, TEdge> : GraphSceneLayer<TItem, TEdge>
        where TEdge : TItem, IEdge<TItem> {

        public bool AntiAlias = true;

        public override void OnPaint (IRenderEventArgs e) {
            var surface = ((ContextSurface) e.Surface);
            var ctx = surface.Context;

            var transform = this.Camera.Matrice;
            
            ctx.SetTransform (transform);
            surface.Matrix = transform;

            this.Renderer.Render (this.Data, e);

            ctx.ResetTransform();
        }

        public override void DataChanged () { }
        }
}