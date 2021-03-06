/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Graphs;
using Limaki.View.GraphScene;
using Limaki.View.Viz.Modelling;


namespace Limaki.View.Viz.Mesh {
	

    /// <summary>
    /// handles the backing Graphs of Scenes
    /// that is the <see cref="IGraphPair{TSinkItem, TSourceItem, TSinkEdge, TSourceEdge}.Source"/>
    /// containing the entities
    /// </summary>
    /// <typeparam name="TSinkItem"></typeparam>
    /// <typeparam name="TSourceItem"></typeparam>
    /// <typeparam name="TSinkEdge"></typeparam>
    /// <typeparam name="TSourceEdge"></typeparam>
    public class GraphSceneDisplayMeshBackHandler<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge> :
		
		IGraphSceneDisplayMeshBackHandler<TSinkItem, TSinkEdge> ,
	    IGraphSceneMeshBackHandler<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>

		where TSinkEdge : IEdge<TSinkItem>, TSinkItem
        where TSourceEdge : IEdge<TSourceItem>, TSourceItem {

        ICollection<IGraph<TSourceItem, TSourceEdge>> _backGraphs = new HashSet<IGraph<TSourceItem, TSourceEdge>> ();
        public ICollection<IGraph<TSourceItem, TSourceEdge>> BackGraphs { get { return _backGraphs; } }

        public Func<ICollection<IGraphScene<TSinkItem, TSinkEdge>>> Scenes { get; set; }
        public Func<ICollection<IGraphSceneDisplay<TSinkItem, TSinkEdge>>> Displays { get; set; }

        public void RegisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph) {
            RegisterBackGraph (BackGraphOf (graph));
        }

        public void UnregisterBackGraph (IGraph<TSinkItem, TSinkEdge> graph) {
            var backGraph = BackGraphOf (graph);
            UnregisterBackGraph (backGraph);
        }

        public void RegisterBackGraph (IGraph<TSourceItem, TSourceEdge> root) {
            if (!BackGraphs.Contains (root)) {
                BackGraphs.Add (root);
                root.GraphChange -= this.BackGraphChange;
                root.GraphChange += this.BackGraphChange;
                root.ChangeData -= this.BackGraphChangeData;
                root.ChangeData += this.BackGraphChangeData;
            }
        }

        public void UnregisterBackGraph (IGraph<TSourceItem, TSourceEdge> root) {
			UnregisterBackGraph (root,false);
        }

        public void Clear () {
            foreach (var g in BackGraphs.ToArray ()) {
                UnregisterBackGraph (g);
            }
        }

        public IEnumerable<IGraph<TSourceItem, TSourceEdge>> BackGraphsOf (IGraph<TSourceItem, TSourceEdge> graph) {
            return BackGraphs.Where (g => g == graph || g.UnWrapped () == graph || g.RootSink () == graph);
        }

        public void UnregisterBackGraph (IGraph<TSourceItem, TSourceEdge> root, bool forced) {
            root = BackGraphsOf (root).FirstOrDefault ();
            var remove = forced || !ScenesOfBackGraph (root).Any ();
            if (root != null && remove) {
                root.GraphChange -= this.BackGraphChange;
                root.ChangeData -= this.BackGraphChangeData;
                BackGraphs.Remove (root);
            }
        }

        /// <summary>
        /// returns the backing Graph
        /// </summary>
        /// <param name="graph"></param>
        /// <returns></returns>
        public IGraph<TSourceItem, TSourceEdge> BackGraphOf (IGraph<TSinkItem, TSinkEdge> graph) {
            var source = graph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            if (source != null)
                return source.Source;
            return null;
        }

        public IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> ScenesOfBackGraph (IGraph<TSourceItem, TSourceEdge> backGraph) {
            if (backGraph == null)
                return new IGraphScene<TSinkItem, TSinkEdge>[0];
            backGraph = backGraph.Unwrap ();
            return Scenes ().Where (s => BackGraphOf (s.Graph).Unwrap () == backGraph);
        }

        public IEnumerable<IGraphScene<TSinkItem, TSinkEdge>> ScenesOfBackGraph (IGraph<TSinkItem, TSinkEdge> graph) {
            var backGraph = BackGraphOf (graph);
            return ScenesOfBackGraph (backGraph);
        }

		public virtual IGraph<TSourceItem, TSourceEdge> WrapGraph (IGraph<TSourceItem, TSourceEdge> backGraph) {
			var viz = Registry.Create<ISceneViz<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>> ();
			return viz.WrapGraph (backGraph);
		}

		public virtual IGraph<TSinkItem, TSinkEdge> CreateSinkGraph (IGraph<TSourceItem, TSourceEdge> backGraph) {
			var viz = Registry.Create<ISceneViz<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>> ();
			return viz.CreateSinkGraph (backGraph);
		}

		public virtual IGraphScene<TSinkItem, TSinkEdge> CreateScene (IGraph<TSourceItem, TSourceEdge> backGraph) {
			var viz = Registry.Create<ISceneViz<TSinkItem, TSourceItem, TSinkEdge, TSourceEdge>> ();
			return viz.CreateScene (backGraph);
		}

		public virtual IGraph<TSinkItem, TSinkEdge> CreateSinkGraph (IGraph<TSinkItem, TSinkEdge> source) {

			IGraph<TSinkItem, TSinkEdge> result = null;

			var graphPair = source.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
			if (graphPair != null) {
				var sinkGraph = Activator.CreateInstance (graphPair.Sink.GetType ()) as IGraph<TSinkItem, TSinkEdge>;
				var transformer = Activator.CreateInstance (graphPair.Mapper.Transformer.GetType ());
				result = Activator.CreateInstance (graphPair.GetType (), sinkGraph, graphPair.Source, transformer) as IGraph<TSinkItem, TSinkEdge>;
			}

			return result;
		}

        public IGraphSceneDisplay<TSinkItem, TSinkEdge> DisplayOf (IGraphScene<TSinkItem, TSinkEdge> scene) {
            return Displays ().FirstOrDefault (d => d.Data == scene);
        }

        #region BackGraphEvents

        private void BackGraphChangeData (IGraph<TSourceItem, TSourceEdge> graph, TSourceItem backItem, object data) { }

        protected ICollection<Tuple<IGraph<TSourceItem, TSourceEdge>, TSourceItem, GraphEventType>> graphChanging = new HashSet<Tuple<IGraph<TSourceItem, TSourceEdge>, TSourceItem, GraphEventType>> ();

        protected virtual void BackGraphChange (object sender, GraphChangeArgs<TSourceItem,TSourceEdge> args) {

            var graph = args.Graph;
            var backItem = args.Item;
            var eventType = args.EventType;
            if (backItem == null)
                return;
            
            var change = Tuple.Create (graph, backItem, eventType);
            if (graphChanging.Contains (change))
                return;

            try {
                graphChanging.Add (change);

                var displays = new HashSet<IGraphSceneDisplay<TSinkItem, TSinkEdge>> ();
                var removeDependencies = false;

                var senderAsSink = (sender as IGraph<TSinkItem, TSinkEdge>).RootSink ();

                var scenes = ScenesOfBackGraph (graph)
                    // leave alone the sender:
                    .Where (s => s.Graph.RootSink () != senderAsSink)
                    .ToArray ();

                Action<TSourceItem> visit = sourceItem => {
                    foreach (var scene in scenes) {

                        var graphPair = scene.Graph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();

                        var sinkItem = default (TSinkItem);

                        if (eventType == GraphEventType.Add) {
                            sinkItem = graphPair.Get (sourceItem);
                            if (sinkItem is TSinkEdge) {
                                SceneEdgeAdd (scene, (TSinkEdge) sinkItem);
                                displays.Add (DisplayOf (scene));
                            }
                            continue;
                        }

                        if (graphPair.Count == 0 || !graphPair.Source2Sink.TryGetValue (sourceItem, out sinkItem))
                            continue;

                        var visible = scene.Contains (sinkItem);

                        if (eventType == GraphEventType.Remove) {
                            if (removeDependencies) {
                                if (visible &&
                                    !scene.Requests
                                        .OfType<DeleteCommand<TSinkItem, TSinkEdge>> ()
                                        .Any (r => sinkItem.Equals (r.Subject))) {

                                    scene.RequestDelete (sinkItem, null);
                                }

                                if (visible)
                                    displays.Add (DisplayOf (scene));

                            } else {
                                if (visible) {
                                    if (scene.Focused != null && scene.Focused.Equals (sinkItem)) {
                                        scene.Focused = default (TSinkItem);
                                    }
                                    scene.Selected.Remove (sinkItem);

                                    scene.Requests.Add (new RemoveBoundsCommand<TSinkItem, TSinkEdge> (sinkItem, scene));
                                    scene.Graph.Twig (sinkItem).ForEach (e =>
                                        scene.Requests.Add (new RemoveBoundsCommand<TSinkItem, TSinkEdge> (e, scene)));
                                    displays.Add (DisplayOf (scene));
                                }
                            }
                        }

                        if (eventType == GraphEventType.Update) {

                            if (backItem is TSourceEdge && sinkItem is TSinkEdge) {

                                SceneEdgeChanged (graph, (TSourceEdge)backItem, scene, (TSinkEdge)sinkItem);
                                if (visible)
                                    displays.Add (DisplayOf (scene));

                            } else {

                                graphPair.UpdateSink (sinkItem);
                                if (visible) {
                                    scene.Requests.Add (new LayoutCommand<TSinkItem> (sinkItem, LayoutActionType.Justify));
                                    displays.Add (DisplayOf (scene));
                                }
                            }
                        }

                    }
                };
                
                if (eventType == GraphEventType.Remove) {
                    try {
                        var dependencies = Registry.Pooled<GraphDepencencies<TSourceItem, TSourceEdge>> ();
                        removeDependencies = true;
                        dependencies.VisitItems (GraphCursor.Create (graph, backItem), visit, eventType);
                    } catch (Exception ex) {
                        Trace.TraceError (ex.Message);
                    } finally {
                        removeDependencies = false;
                    }
                }

                visit (backItem);

                displays.Where (display => display != null)
                        .ForEach (display => {
                            display.Perform ();
                            Xwt.Application.MainLoop.QueueExitAction (() => display.QueueDraw ());
                });

                Action<TSourceItem> visitAfter = sourceItem => {
                    foreach (var scene in scenes) {

                        var graphPair = scene.Graph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();

                        var sinkItem = default (TSinkItem);
                        if (graphPair.Count == 0 || !graphPair.Source2Sink.TryGetValue (sourceItem, out sinkItem))
                            continue;

                        var visible = scene.Contains (sinkItem);
                        if (eventType == GraphEventType.Remove) {
                            SceneItemRemoveAfterDisplayUpdate (scene, sinkItem);
                        }
                    }
                };

                visitAfter (backItem);

                Xwt.Application.MainLoop.DispatchPendingEvents ();

            } catch (Exception ex) {
                Trace.TraceError (ex.Message);
            } finally {
                graphChanging.Remove (change);
            }

        }

        #endregion

        #region needed if BackendHandler is called from Backend, not from Frontend 
        // this are copies from VisualGraphSceneDisplayMeshEvents, should be consolidated

        protected virtual void SceneEdgeChanged (IGraph<TSourceItem, TSourceEdge> sourceGraph, TSourceEdge sourceEdge, IGraphScene<TSinkItem, TSinkEdge> sinkScene, TSinkEdge sinkEdge) {
            
            var sinkGraph = (sinkScene.Graph as IGraphPair<TSinkItem, TSinkItem, TSinkEdge, TSinkEdge>);
            var sinkSource = sinkGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> ();
            var root = sinkSource.Get (sourceEdge.Root);
            var leaf = sinkSource.Get (sourceEdge.Leaf);

            if (!sinkEdge.Root.Equals(root) || !sinkEdge.Leaf.Equals(leaf)) {

                bool isEdgeVisible = sinkGraph.Sink.Contains (sinkEdge);
                bool makeEdgeVisible = sinkGraph.Sink.Contains (root) && sinkGraph.Sink.Contains (leaf);

                Action<TSinkItem, bool> changeEdge = (item, isRoot) => {
                    if (makeEdgeVisible)
                        sinkGraph.ChangeEdge (sinkEdge, item, isRoot);
                    else
                        sinkGraph.Source.ChangeEdge (sinkEdge, item, isRoot);
                };

                if (!sinkEdge.Root.Equals(root)) changeEdge (root, true);
                if (!sinkEdge.Leaf.Equals(leaf)) changeEdge (leaf, false);
                
                if (makeEdgeVisible) {
                    var changeList = new TSinkEdge[] { sinkEdge }.Union (sinkGraph.Source.Twig (sinkEdge));
                    foreach (var edge in changeList) {
                        var showTwig = (sinkScene.Contains (edge.Root) && sinkScene.Contains (edge.Leaf));

                        var doAdd = (edge.Equals(sinkEdge) && !isEdgeVisible) ||
                                    (!sinkGraph.Sink.Contains (edge) && showTwig);

                        if (doAdd) {
                            sinkGraph.Sink.Add (edge);
                            sinkScene.Requests.Add (new LayoutCommand<TSinkItem> (edge, LayoutActionType.Invoke));
                        }
                        if (showTwig || edge.Equals(sinkEdge))
                            sinkScene.Requests.Add (new LayoutCommand<TSinkItem> (edge, LayoutActionType.Justify));
                    }
                } else {
                    var changeList = new TSinkEdge[] { sinkEdge }.Union (sinkGraph.Sink.Twig (sinkEdge));
                    foreach (var edge in changeList) {
                        sinkGraph.Sink.Remove (edge);
                        sinkScene.Requests.Add (new RemoveBoundsCommand<TSinkItem, TSinkEdge> (edge, sinkScene));
                    }
                }
            }
        }

        /// <summary>
        /// removes items after display perform is done
        /// </summary>
        /// <param name="sinkScene"></param>
        /// <param name="sinkItem"></param>
        protected virtual void SceneItemRemoveAfterDisplayUpdate (IGraphScene<TSinkItem, TSinkEdge> sinkScene, TSinkItem sinkItem) {
            var graphs = new Stack<IGraph<TSinkItem, TSinkEdge>> ();
            graphs.Push (sinkScene.Graph);
            while (graphs.Count > 0) {
                var graph = graphs.Pop ();
                var sinkGraph = graph as ISinkGraph<TSinkItem, TSinkEdge>;
                if (graph.Contains (sinkItem)) {
                    if (sinkGraph != null)
                        sinkGraph.RemoveSinkItem (sinkItem);
                    else
                        graph.Remove (sinkItem);
                }
                var graphPair = graph as IGraphPair<TSinkItem, TSinkItem, TSinkEdge, TSinkEdge>;
                if (graphPair != null)
                    graphs.Push (graphPair.Source);
            }

        }

        protected virtual void SceneEdgeAdd (IGraphScene<TSinkItem, TSinkEdge> sinkScene, TSinkEdge sinkEdge) {
            if (sinkScene.Contains (sinkEdge.Root) && (sinkScene.Contains (sinkEdge.Leaf))) {
                sinkScene.Graph.Add (sinkEdge);
                sinkScene.Requests.Add (new LayoutCommand<TSinkItem> (sinkEdge, LayoutActionType.Invoke));
                sinkScene.Requests.Add (new LayoutCommand<TSinkItem> (sinkEdge, LayoutActionType.Justify));
            }
        }

        public virtual TSinkItem LookUp (IGraph<TSinkItem, TSinkEdge> sourceGraph, IGraph<TSinkItem, TSinkEdge> sinkGraph, TSinkItem lookItem) {
            var source = sourceGraph as IGraphPair<TSinkItem, TSinkItem, TSinkEdge, TSinkEdge>;
            var sink = sinkGraph as IGraphPair<TSinkItem, TSinkItem, TSinkEdge, TSinkEdge>;
            if (sourceGraph.Source<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> () != null) {
                return source.LookUp<TSinkItem, TSinkEdge, TSourceItem, TSourceEdge> (                   sink, lookItem);
            }
            return default (TSinkItem);
        }

        #endregion

    }
}