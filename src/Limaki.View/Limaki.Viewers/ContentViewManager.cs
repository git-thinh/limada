/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 */


using System;
using System.IO;
using Limada.Model;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs.Extensions;
using Limada.View;
using Limada.VisualThings;
using Limaki.Model.Content;
using System.Linq;
using Limaki.Graphs;
using Limaki.View.Visualizers;
using Limaki.View.UI.GraphScene;
using Limaki.Viewers.StreamViewers;
using Limaki.Visuals;
using Xwt.Drawing;

namespace Limaki.Viewers {

    public class ContentViewManager:IDisposable {

        public IGraphSceneDisplay<IVisual, IVisualEdge> SheetControl { get; set; }
        public ISheetManager SheetManager { get; set; }

        public Color BackColor = SystemColors.Background;
        public object Parent = null;

        public event Action<object, Action> AttachBackend = null;
        public event Action<object> Attach = null;
        public event Action<object> DeAttach = null;

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler> (); }
        }

       

        IThing currentThing = null;
        bool IsStreamOwner = true;
        private ContentViewerProvider _providers = null;
        public ContentViewerProvider Providers { get { return _providers ?? (_providers = Registry.Pool.TryGetCreate<ContentViewerProvider>()); } }
        public ContentVisualViewerProvider ThingContentViewProviders { get { return Providers as ContentVisualViewerProvider; } }

        protected void AttachViewer(ContentViewer viewer, IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            if (viewer is SheetViewer) {
                var sheetView = (SheetViewer)viewer;
                sheetView.SheetControl = this.SheetControl;
                sheetView.SheetManager = this.SheetManager;
            }

            viewer.BackColor = this.BackColor;
            viewer.Parent = this.Parent;
            if (this.Attach != null) {
                viewer.Attach -= this.Attach;
                viewer.Attach += this.Attach;
            }

            if (AttachBackend != null) {
                AttachBackend(viewer.Backend, () => viewer.OnShow());
            }
        }

        protected void LoadStreamThing (ContentStreamViewer controller, IThingGraph graph, IStreamThing thing) {
            try {
                controller.IsStreamOwner = IsStreamOwner;
                if (controller.ContentId != thing.Id) {
                    SaveStream(graph, controller);

                    var info = ThingStreamFacade.GetContent(graph, thing);
                    if (controller is SheetViewer) {
                        info.Source = thing.Id;
                    }

                    if (controller is HtmlViewer) {
                        var htmlViewr = (HtmlViewer)controller;
                        htmlViewr.ContentThing = thing;
                        htmlViewr.ThingGraph = graph;
                    }


                    controller.SetContent(info);
                    controller.ContentId = thing.Id;
                }

                currentThing = thing;

            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } finally {
                thing.ClearRealSubject(!IsStreamOwner);
            }
        }

        protected void LoadThing (ContentVisualViewer controller, IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            try {
                //if (controller.CurrentThingId != thing.Id) {
                //    //SaveStream(graph, controller);
                //}
                var thing = graph.ThingOf(visual);
                if (controller.ContentId != thing.Id) {
                    controller.ContentId = thing.Id;
                    controller.SetContent(graph, visual);
                }

                currentThing = thing;

            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } finally {
                
            }
        }


        protected void LoadThing (IGraph<IVisual, IVisualEdge> visualGraph, IVisual visual) {
            var graph = visualGraph.Source<IVisual, IVisualEdge,IThing, ILink>();

            if (visual != null && graph != null) {
                 var thing = graph.ThingOf(visual);
                 if (thing != null) {
                     var controller = ThingContentViewProviders.Supports(visualGraph, visual);
                     if (controller != null) {
                         AttachViewer(controller, graph, visual);
                         LoadThing(controller, visualGraph, visual);
                     }
                     var streamThing = thing as IStreamThing;
                     if (streamThing != null) {
                         var streamController = Providers.Supports(streamThing.StreamType);
                         
                         if (streamController != null) {
                             AttachViewer(streamController, graph, visual);
                             LoadStreamThing(streamController, graph.Two as IThingGraph, streamThing);
                         }
                     }
                 }
             }
        }

        [TODO("Refactor this to use State")]
        public void SaveStream(IThingGraph graph, ContentStreamViewer controller) {
            if (graph != null && controller.CanSave() && controller.ContentId != 0){
                var thing = graph.GetById(controller.ContentId) as IStreamThing;
                if (thing != null) {
                    var info = new Content<Stream> ();
                    controller.Save (info);
                    new ThingStreamFacade ().SetStream (graph, thing, info);
                    info.Data.Dispose ();
                    info.Data = null;
                    info = null;
                }
            }
        }

        public void SaveStream(IThingGraph graph) {
            if (graph == null)
                return;

            foreach (var controller in Providers.Viewers.OfType<ContentStreamViewer>()) {
                SaveStream (graph, controller);
            }
        }

        public void ChangeViewer(object sender, GraphSceneEventArgs<IVisual, IVisualEdge> e) {
            if (e.Item != null) {
                LoadThing(e.Scene.Graph,e.Item);
            }
        }

        public void Clear() {
            foreach (var controller in Providers.Viewers) {
                controller.Clear();
            }
            currentThing = null;
        }

        public void Dispose() {
            foreach (var controller in Providers.Viewers) {
                controller.Dispose ();
            }
            

        }
    }
}