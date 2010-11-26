/*
 * Limaki 
 * Version 0.071
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
using System.Text;
using NUnit.Framework;
using Limaki.Common;
using Limaki.Graphs;
using Limaki.UnitTest;
using Limaki.Tests.Graph.Model;

namespace Limaki.Tests.Graph.Basic {
    public class GraphViewTest: TestBase {
        /// <summary>
        /// this tests a semantic error in GraphView
        /// GraphView should only contain edges contained in view
        /// under all cirumstances
        /// but e.g. Graph.Fork breaks this rule
        /// </summary>
        [Test]
        public  void EdgeNotInView() {
            IGraph<IGraphItem, IGraphEdge> data = new Graph<IGraphItem, IGraphEdge> ();
            IGraph<IGraphItem, IGraphEdge> view = new Graph<IGraphItem, IGraphEdge>();
            GraphView<IGraphItem, IGraphEdge> graphView = new GraphView<IGraphItem, IGraphEdge> ();
            graphView.Data = data;
            graphView.View = view;

            IGraphItem one = new GraphItem<string> ("1");
            IGraphItem two = new GraphItem<string>("2");
            IGraphItem three = new GraphItem<string>("3");
            data.Add (one);
            data.Add(two);
            data.Add(three);
            IGraphEdge link = new GraphEdge (one, two);
            IGraphEdge linklink = new GraphEdge (link, three);
            data.Add (linklink);
            view.Add (one);
            view.Add (two);
            view.Add (link);
            Assert.IsTrue(data.Contains(link), "view has to contain link");
            Assert.IsTrue(data.Contains(linklink), "view has to contain linklink");
            Assert.IsFalse (view.Contains (linklink),"view must not contain linklink");
            Assert.IsFalse(graphView.Contains(linklink), "graphview must not contain linklink");
            view.Add (linklink);
            view.Remove (link);
            Assert.IsFalse(graphView.Contains(linklink), "graphview must not contain linklink");
            view.Add (link);

            IGraph<IGraphItem, IGraphEdge> graph = graphView;

            foreach (IGraphEdge edge in graphView.Fork(three)) {
                Assert.IsFalse(edge.Equals(linklink), "graphview must not contain linklink");
            }

            bool found = false;
            foreach (IGraphEdge edge in data.Fork(three)) {
                if (edge == linklink) {
                    found = true;
                    break;
                }
            }
            Assert.IsTrue(found, "data.fork has to contain linklink");
        }
    }
}
