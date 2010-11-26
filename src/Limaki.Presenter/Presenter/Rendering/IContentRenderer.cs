/*
 * Limaki 
 * Version 0.081
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


using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.UI;

namespace Limaki.Presenter {
    /// <summary>
    /// this class 
    /// encapsulates the content-specific rendering
    /// it is called by the layers OnPaint-Method
    /// it should have no dependencies on a specific device
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IContentRenderer<T>  {
        void Render(T data, IRenderEventArgs e);
        Get<ICamera> Camera { get; set; }
    }

    public interface IGraphItemRenderer<TItem, TEdge> : IContentRenderer<TItem>
    where TEdge : TItem, IEdge<TItem> {
        Get<IGraphLayout<TItem, TEdge>> Layout { get; set; }
    }

    public interface IGraphSceneRenderer<TItem, TEdge> : IContentRenderer<IGraphScene<TItem, TEdge>>
        where TEdge : TItem, IEdge<TItem> {
        
        IGraphItemRenderer<TItem, TEdge> ItemRenderer { get; set; }

        Get<IGraphLayout<TItem, TEdge>> Layout { get; set; }
        

    }
}