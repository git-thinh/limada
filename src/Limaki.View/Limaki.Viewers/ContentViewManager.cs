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
using Limaki.Model.Streams;
using System.Linq;
using Limaki.Graphs;
using Limaki.View.Display;
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

        public event Action<object, Action> AttachControl = null;
        public event Action<object> Attach = null;
        public event Action<object> DeAttach = null;

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler> (); }
        }

       

        IThing currentThing = null;
        bool IsStreamOwner = true;
        private ContentViewProviders _providers = null;
        public ContentViewProviders Providers { get { return _providers ?? (_providers = Registry.Pool.TryGetCreate<ContentViewProviders>()); } }
        public ThingContentViewProviders ThingContentViewProviders { get { return Providers as ThingContentViewProviders; } }

        protected void AttachController(ViewerController controller, IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            if (controller is SheetViewerController) {
                var sheetView = (SheetViewerController)controller;
                sheetView.SheetControl = this.SheetControl;
                sheetView.SheetManager = this.SheetManager;
            }

            controller.BackColor = this.BackColor;
            controller.Parent = this.Parent;
            if (this.Attach != null) {
                controller.Attach -= this.Attach;
                controller.Attach += this.Attach;
            }

            if (AttachControl != null) {
                AttachControl(controller.Backend, () => controller.OnShow());
            }
        }

        protected void LoadStreamThing (StreamViewerController controller, IThingGraph graph, IStreamThing thing) {
            try {
                controller.IsStreamOwner = IsStreamOwner;
                if (controller.CurrentThingId != thing.Id) {
                    SaveStream(graph, controller);

                    var info = ThingStreamFacade.GetContent(graph, thing);
                    if (controller is SheetViewerController) {
                        info.Source = thing.Id;
                    }

                    if (controller is HtmlViewerController) {
                        var htmlViewr = (HtmlViewerController)controller;
                        htmlViewr.ContentThing = thing;
                        htmlViewr.ThingGraph = graph;
                    }


                    controller.SetContent(info);
                    controller.CurrentThingId = thing.Id;
                }

                currentThing = thing;

            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } finally {
                thing.ClearRealSubject(!IsStreamOwner);
            }
        }

        protected void LoadThing (ThingViewerController controller, IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            try {
                //if (controller.CurrentThingId != thing.Id) {
                //    //SaveStream(graph, controller);
                //}
                var thing = graph.ThingOf(visual);
                if (controller.CurrentThingId != thing.Id) {
                    controller.CurrentThingId = thing.Id;
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
                         AttachController(controller, graph, visual);
                         LoadThing(controller, visualGraph, visual);
                     }
                     var streamThing = thing as IStreamThing;
                     if (streamThing != null) {
                         var streamController = Providers.Supports(streamThing.StreamType);
                         
                         if (streamController != null) {
                             AttachController(streamController, graph, visual);
                             LoadStreamThing(streamController, graph.Two as IThingGraph, streamThing);
                         }
                     }
                 }
             }
        }

        [TODO("Refactor this to use State")]
        public void SaveStream(IThingGraph graph, StreamViewerController controller) {
            if (graph != null && controller.CanSave() && controller.CurrentThingId != 0){
                var thing = graph.GetById(controller.CurrentThingId) as IStreamThing;
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

            foreach (var controller in Providers.Viewers.OfType<StreamViewerController>()) {
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