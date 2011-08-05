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
 * http://limada.sourceforge.net
 * 
 */

using System;
using System.Collections.Generic;
using Limaki.Actions;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using Limaki.Graphs;
using Limaki.Presenter.Layout;
using Limaki.Presenter.UI;
using Limaki.Visuals;

namespace Limaki.Presenter.Visuals.UI {
    public static class SceneTools {

        public static void ChangeShape(IGraphScene<IVisual, IVisualEdge> scene, IVisual visual, IShape newShape) {
            if (visual != null && !(visual is IVisualEdge)) {
                if (newShape != null) {
                    newShape = (IShape)newShape.Clone();
                    newShape.Location = visual.Shape.Location;
                    newShape.Size = visual.Shape.Size;
                    var changeShape =
                        new ActionCommand<IVisual, IShape>(
                            visual,
                            newShape,
                            delegate(IVisual target, IShape shape) { target.Shape = shape; });
                    scene.Requests.Add(changeShape);
                    if (visual.Shape is VectorShape || newShape is VectorShape) {
                        scene.Requests.Add(new LayoutCommand<IVisual>(visual, LayoutActionType.Justify));
                    }
                    foreach (IVisualEdge edge in scene.Twig(visual)) {
                        scene.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
                    }
                }
            }
        }

        public static void ChangeStyle(IGraphScene<IVisual, IVisualEdge> scene, IVisual visual, IStyleGroup newStyle) {
            if (visual != null) {
                if (newStyle != null) {
                   var changeStyle = 
                       new ActionCommand<IVisual, IStyleGroup>(visual,newStyle,(target, style) => target.Style = style);
                    scene.Requests.Add(changeStyle);
                    if (visual.Shape is VectorShape) {
                        scene.Requests.Add(new LayoutCommand<IVisual>(visual, LayoutActionType.Justify));
                    }
                    foreach (var edge in scene.Twig(visual)) {
                        scene.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
                    }
                }
            }
        }

        public static void ChangeMarkers(IGraphScene<IVisual, IVisualEdge> scene, IEnumerable<IVisual> elements, string text) {
            if (scene.Markers != null) {
                scene.Markers.ChangeMarkers (elements, text);
                foreach (var visual in elements) {
                    scene.Requests.Add(new LayoutCommand<IVisual>(visual, LayoutActionType.Justify));
                }
            }
        }

        public static IVisualEdge CreateEdge(IGraphScene<IVisual, IVisualEdge> scene) {
            IVisualEdge edge = null;
            if (scene != null && scene.Markers != null) {
                edge = scene.Markers.CreateDefaultEdge() as IVisualEdge;
            } 
            if (edge == null){
                var factory = Registry.Factory.Create<IVisualFactory> ();
                edge = factory.CreateEdge ("�");
            }
            return edge;
        }

        public static void CreateEdge(IGraphScene<IVisual, IVisualEdge> scene, IVisual root, IVisual leaf) {
            if (scene != null && leaf != null && root != null && root != leaf) {
                IVisualEdge edge = CreateEdge (scene);
                
                edge.Root = root;
                edge.Leaf = leaf;
                scene.Add(edge);
                if (scene.Markers != null) {
                    object marker = scene.Markers.DefaultMarker;
                    scene.Graph.OnChangeData (edge, marker);
                }
                scene.Graph.OnGraphChanged(edge, GraphChangeType.Add);
                scene.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Invoke));
                scene.Requests.Add(new LayoutCommand<IVisual>(edge, LayoutActionType.Justify));
            }
        }

        public static void AddItem(IGraphScene<IVisual, IVisualEdge> scene, IVisual item, IGraphLayout<IVisual,IVisualEdge> layout, PointI pt) {
            bool allowAdd = true;
            if (scene == null)
                return;
            if (item is IVisualEdge) {
                IVisualEdge edge = (IVisualEdge)item;
                allowAdd = scene.Contains(edge.Root) && scene.Contains(edge.Leaf);
            }
            if (allowAdd) {
                GraphSceneFacade<IVisual,IVisualEdge> facade =
                    new GraphSceneFacade<IVisual, IVisualEdge>(delegate() { return scene; }, layout);
                facade.Add(item, pt);
            }
        }

        public static IVisual PlaceVisual(IGraphScene<IVisual, IVisualEdge> scene, IVisual root, IVisual visual, IGraphLayout<IVisual, IVisualEdge> layout) {
            if (visual != null && scene !=null) {
                PointI pt = (PointI)layout.Border;
                if (root != null) {
                    pt = root.Shape[Anchor.LeftBottom];
                }
                AddItem(scene, visual, layout, pt);
                SceneTools.CreateEdge(scene, root, visual);
            }

            return visual;
        }

        public static void CleanScene(IGraphScene<IVisual, IVisualEdge> scene) {
            if (scene != null) {
                var graphView = scene.Graph as GraphView<IVisual, IVisualEdge>;
                if (graphView!=null) {
                    ( (GraphView<IVisual, IVisualEdge>) scene.Graph ).One.Clear ();
                    scene.ClearView ();
                    Registry.ApplyProperties<MarkerContextProcessor, IGraphScene<IVisual, IVisualEdge>>(scene);
                } else {
                    throw new ArgumentException ("scene.Graph must be a GraphView");
                }
            }
        }
    }
}