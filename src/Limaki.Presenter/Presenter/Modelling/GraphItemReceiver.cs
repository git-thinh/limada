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


using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Presenter.Layout;
using Limaki.Presenter.UI;

namespace Limaki.Presenter {
    public class GraphItemReceiver<TItem,TEdge> : IModelReceiver<TItem> 
        where TEdge:TItem,IEdge<TItem> {

        public virtual IGraphScene<TItem,TEdge> Data { get; set; }
        public virtual IGraphLayout<TItem, TEdge> Layout { get; set; }

        public Action<ICommand<TItem>> BeforeExecute { get; set; }
        public Action<ICommand<TItem>> AfterExecute { get; set; }

        public virtual void Execute(ICommand<TItem> request) {
            if (request is LayoutCommand<TItem>) {
                LayoutCommand<TItem> layoutCommand = (LayoutCommand<TItem>)request;
                if (layoutCommand.Parameter == LayoutActionType.Justify) {
                    Layout.Justify(layoutCommand.Subject);
                } else if (layoutCommand.Parameter == LayoutActionType.Perform) {
                    Layout.Perform(layoutCommand.Subject);
                } else if (layoutCommand.Parameter == LayoutActionType.Invoke) {
                    Layout.Invoke(layoutCommand.Subject);
                } else if (layoutCommand.Parameter == LayoutActionType.AddBounds) {
                    Layout.AddBounds(layoutCommand.Subject);
                }
            } else if (request is LayoutCommand<TItem, IShape>) {
                LayoutCommand<TItem, IShape> layoutCommand = (LayoutCommand<TItem, IShape>)request;
                if (layoutCommand.Parameter == LayoutActionType.Justify) {
                    Layout.Justify(layoutCommand.Subject, layoutCommand.Parameter2);
                } else if (layoutCommand.Parameter == LayoutActionType.Perform) {
                    Layout.Perform(layoutCommand.Subject);
                } else if (layoutCommand.Parameter == LayoutActionType.Invoke) {
                    Layout.Invoke(layoutCommand.Subject, layoutCommand.Parameter2);
                } else if (layoutCommand.Parameter == LayoutActionType.AddBounds) {
                    Layout.AddBounds(layoutCommand.Subject);
                }
            } else if (request is DeleteCommand<TItem,TEdge>) {
                request.Execute();
            } else {
                RectangleI invalid = RectangleI.Empty;
                var shape = Data.ItemShape (request.Subject);

                if (shape != null) {
                    invalid = shape.BoundsRect;
                }

                request.Execute();
                
                if (invalid != RectangleI.Empty)
                    Data.UpdateBounds(request.Subject, invalid);
                else
                    Data.AddBounds(request.Subject);
            }
            if(request is IDirtyCommand){
                Data.State.Dirty = true;
            }
        }

        protected int tolerance = 5;
        public virtual void Execute(ICollection<ICommand<TItem>> requests) {
            if (Data != null && Data.Requests.Count != 0) {
                bool clipChanged = false;

                foreach (var command in requests) {
                    if (command != null && command.Subject != null) {

                        if (BeforeExecute != null) {
                            BeforeExecute(command);
                        }

                        Execute(command);

                        if (AfterExecute != null) {
                            AfterExecute(command);
                        }


                        clipChanged = true;
                    }
                }
            }
        }
        }
}