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

namespace Limaki.Tests.Graph.Basic {
    public abstract class BasicGraphTests<TItem, TEdge> : TestBase
        where TEdge : IEdge<TItem> {

        public abstract BasicTestDataFactory<TItem, TEdge> GetFactory();

        BasicTestDataFactory<TItem, TEdge> _data = null;
        public virtual BasicTestDataFactory<TItem, TEdge> Data {
            get {
                if (_data == null) {
                    _data = GetFactory();
                }
                return _data;
            }
        }

        protected IGraph<TItem, TEdge> _graph = null;
        public virtual IGraph<TItem, TEdge> Graph {
            get {
                if (_graph == null) {
                    _graph = new Graph<TItem, TEdge>();
                }
                return _graph;
            }
            set { _graph = value; }
        }

        public virtual void ReportGraph(IGraph<TItem, TEdge> graph, string reportHeader) {
            this.ReportDetail(reportHeader);
            foreach (System.Collections.Generic.KeyValuePair<TItem, ICollection<TEdge>> kvp in graph.ItemsWithEdges()) {
                bool first = true;
                string emptyString = new string(' ', kvp.Key.ToString().Length);
                foreach (TEdge edge in kvp.Value) {
                    if (first) {
                        if (edge != null)
                            this.ReportDetail("\t" + kvp.Key.ToString() + "\t: " + edge.ToString());
                        else
                            this.ReportDetail("\t" + kvp.Key.ToString() + "\t: <null>");
                        first = false;
                    } else {
                        this.ReportDetail("\t" + emptyString + "\t: " + edge.ToString());
                    }
                }
            }
        }

        public void FillGraph(IGraph<TItem, TEdge> graph, BasicTestDataFactory<TItem, TEdge> data) {
            foreach (TEdge edge in data.Edges) {
                graph.Add(edge);
            }            
        }

        void ResetAndFillGraph(BasicTestDataFactory<TItem, TEdge> data, IGraph<TItem, TEdge> graph) {
            graph.Clear();
            FillGraph (graph,data);
        }



        public bool dataListReported = false;

        public virtual void InitGraphTest(string TestName) {
            this.ReportDetail(TestName);
            if (!dataListReported) {
                this.ReportDetail("Data:");
                foreach (TEdge edge in Data.Edges) {
                    this.ReportDetail("\t" + edge.ToString());
                }
                dataListReported = true;
            }
            ResetAndFillGraph(Data, Graph);
            ReportGraph(Graph, "Graph with Data:");
        }

        public virtual void FullReportGraph(IGraph<TItem, TEdge> graph, string reportHeader) {
            ReportGraph(graph, reportHeader);
            this.ReportDetail("Foreach Item in graph");
            foreach (TItem item in graph) {
                this.ReportDetail("\t" + item);
            }
            this.ReportDetail("Foreach Edge in graph.Edges() ");
            foreach (TEdge edge in graph.Edges()) {
                this.ReportDetail("\t" + edge);
            }
            this.ReportDetail("Foreach Item Foreach Edge in graph.Edges(Item)");
            foreach (TItem item in graph) {
                bool first = true;
                string placeHolder = "default(" + typeof(TItem).Name + ")";
                if (item != null)
                    placeHolder = new string(' ', item.ToString().Length);
                foreach (TEdge edge in graph.Edges(item)) {
                    if (first) {
                        this.ReportDetail("\t" + item + "\t: " + edge.ToString());
                        first = false;
                    } else {
                        this.ReportDetail("\t" + placeHolder + "\t: " + edge.ToString());
                    }
                }
                if (first) {
                    this.ReportDetail("\t" + item + "\t: <no edges>");
                }
            }
        }

        [Test]
        public virtual void AddNothing() {
            InitGraphTest("** Add (default(" + typeof(TItem).Name + ")");
            Graph.Add(Data.Nothing);
            FullReportGraph(Graph, "Added:\t(default(" + typeof(TItem).Name + ")");
            Graph.Remove(Data.Nothing);
            FullReportGraph(Graph, "Removed:\t(default(" + typeof(TItem).Name + ")");
            ReportSummary();
        }

        [Test]
        public virtual void AddSingle() {
            InitGraphTest("** Add " + Data.Single.ToString());
            Graph.Add(Data.Single);
            FullReportGraph(Graph, "Added:\t" + Data.Single.ToString());
            if (Graph.ItemIsStorable) {
                Assert.IsTrue(Graph.Contains(Data.Single));
            }
            InitGraphTest("** Remove " + Data.Single.ToString());
            Graph.Remove(Data.Single);
            FullReportGraph(Graph, "Removed:\t" + Data.Single.ToString());
            IsRemoved(Data.Single);
            ReportSummary();
        }

        [Test]
        public virtual void AddData() {
            InitGraphTest("** AddData");


            Assert.IsTrue(Graph.Contains(Data.One));
            Assert.IsTrue(Graph.Contains(Data.Two));
            Assert.IsTrue(Graph.Contains(Data.Three));
            Assert.IsTrue(Graph.Contains(Data.Aside));

            foreach (TEdge edge in Data.Edges) {
                Assert.IsTrue(Graph.Contains(edge));
            }
            ReportSummary();
        }

        [Test]
        public virtual void RemoveEdge() {
            InitGraphTest("** Remove link");
            Graph.Remove(Data.OneAside);
            FullReportGraph(Graph, "Removed:\t" + Data.OneAside);
            Assert.IsFalse(Graph.Contains(Data.OneAside));
            if (Graph.EdgeIsItem) {
                IsRemoved((TItem)(object)Data.OneAside);
            }
            if (Graph.ItemIsStorable) {
                Assert.IsTrue(Graph.Contains(Data.OneAside.Root));
                Assert.IsTrue(Graph.Contains(Data.OneAside.Leaf));
            }
            ReportSummary();
        }

        [Test]
        public virtual void RemoveEdgeAsItem() {
            InitGraphTest("** Remove link as Item");
            if (Graph.EdgeIsItem) {
                TItem item = (TItem)(object)Data.OneAside;
                Graph.Remove(item);
                FullReportGraph (Graph, "Removed:\t" + Data.OneAside);
                Assert.IsFalse (Graph.Contains (Data.OneAside));
                IsRemoved ((TItem) (object) Data.OneAside);
                if (Graph.ItemIsStorable) {
                    Assert.IsTrue (Graph.Contains (Data.OneAside.Root));
                    Assert.IsTrue (Graph.Contains (Data.OneAside.Leaf));
                }
                ReportSummary ();
            }
        }

        public virtual void IsRemoved(TItem item) {
            if (Graph.ItemIsStorable) {
                Assert.IsFalse(Graph.Contains(item));
            }

            int count = 0;
            foreach (TEdge edge in Graph.Edges(item)) {
                count++;
            }
            Assert.IsTrue(count == 0);

        }

        [Test]
        public virtual void RemoveItem() {
            InitGraphTest("** Remove item");
            Graph.Remove(Data.Two);
            FullReportGraph(Graph, "Removed:\t" + Data.Two.ToString());
            IsRemoved(Data.Two);

            Graph.Remove(Data.OneAside.Root);
            Graph.Remove(Data.OneAside.Leaf);
            FullReportGraph(Graph,
                            "Removed:\t" + Data.OneAside.Root + "\n" +
                            "Removed:\t" + Data.OneAside.Leaf);
            IsRemoved(Data.OneAside.Root);
            IsRemoved(Data.OneAside.Leaf);
            ReportSummary();
        }

        [Test]
        public virtual void AddEdge() {
            Graph.Clear ();
            Graph.Add (Data.OneAside);
            Assert.IsTrue (Graph.Contains (Data.OneAside.Root));
            Assert.IsTrue(Graph.Contains(Data.OneAside.Leaf));
            FullReportGraph(Graph, "*** AddEdge data.OneAside");
            
            Graph.Clear ();
            if (Graph.EdgeIsItem) {
                Graph.Add(Data.TwoThree_One);
                
                Assert.IsTrue(Graph.Contains(Data.TwoThree_One.Root));
                Assert.IsTrue(Graph.Contains(Data.TwoThree_One.Leaf));

                TEdge edge = (TEdge)(object) Data.TwoThree_One.Root;
                Assert.IsTrue (Graph.Contains (edge.Root));
                Assert.IsTrue (Graph.Contains (edge.Leaf));
                FullReportGraph (Graph, "*** AddEdge data.TwoThree_One");
            }
            ReportSummary();
        }

        [Test]
        public virtual void ChangeEdge() {
            InitGraphTest("** ChangeEdge\t"+
                          Data.OneTwo.ToString()+"\t"+
                          Data.One+"\tto\t"+Data.Three);
            Graph.ChangeEdge (Data.OneTwo, Data.One, Data.Three);
            FullReportGraph(Graph, "Changed:\t" +
                                   Data.OneTwo.ToString());
            Assert.AreEqual (Data.OneTwo.Root, Data.Three);
            Assert.IsFalse (Graph.Edges (Data.One).Contains (Data.OneTwo));
            Assert.IsTrue( Graph.Edges(Data.Three).Contains(Data.OneTwo));
            ReportSummary();
        }

        [Test]
        public virtual void AllTests() {
            AddNothing();

            Tickers.Resume();
            AddData();

            Tickers.Resume();
            AddSingle();
            
            Tickers.Resume();
            RemoveEdge();

            Tickers.Resume();
            RemoveEdgeAsItem ();

            Tickers.Resume();
            RemoveItem();
            
            Tickers.Resume();
            AddEdge ();

            Tickers.Resume();
            ChangeEdge ();
        }
        }
}
