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

using System.Windows.Forms;
using Limaki.Graphs;
using Limaki.Common;
using Limaki.View.Visuals;

namespace Limaki.View.Swf.DragDrop {
    public abstract class VisualsDataObjectHandler<OUT> : DataObjectHandler<IGraph<IVisual, IVisualEdge>, IVisual, OUT>, IVisualsDataObjectHandler {
        public override IVisual GetData(IDataObject data, IGraph<IVisual,IVisualEdge> graph) {
            object odata = data.GetData(HandledType);
            IVisual result = null;
            if (odata is OUT) {
                result = Registry.Pooled<IVisualFactory>().CreateItem< OUT > ((OUT)odata);
            }
            return result;
        }
    }
}