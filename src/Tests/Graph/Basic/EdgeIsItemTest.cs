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
using Limaki.Graphs;
using Limaki.Tests.Graph.Model;
using NUnit.Framework;

namespace Limaki.Tests.Graph.Basic {
    public class EdgeIsItemDataFactory : BasicTestDataFactory<Item<string>, EdgeItem<string>> {
        protected override void CreateItems() {
            One = new Item<string>("One");
            Two = new Item<string>("Two");
            Three = new Item<string>("Three");
            Aside = new Item<string>("Aside");
            Single = new Item<string>("Single");
        }
        protected override void CreateEdges() {
            base.CreateEdges();
            TwoThree_One = new EdgeItem<string> (TwoThree, One);
        }
        public override IEnumerable<EdgeItem<string>> Edges {
            get {
                foreach (EdgeItem<string> edge in base.Edges)
                    yield return edge;

                yield return TwoThree_One;

            }
        } 
    }

    public class EdgeIsItemGraphTest : BasicGraphTests<Item<string>, EdgeItem<string>> {
        public override BasicTestDataFactory<Item<string>, EdgeItem<string>> GetFactory() {
            return new EdgeIsItemDataFactory();
        }
        [Test]
        public override void AddNothing() {
            base.AddNothing();
        }
        [Test]
        public override void AddData() {
            base.AddData();
        }
        [Test]
        public override void RemoveEdge() {
            base.RemoveEdge();
        }
        [Test]
        public override void RemoveItem() {
            base.RemoveItem();
            InitGraphTest ("** remove "+Data.One);
            Graph.Remove(Data.One);
            FullReportGraph (Graph,"Removed:\t" + Data.One);

        }
        [Test]
        public override void AddSingle() {
            base.AddSingle();
        }
        [Test]
        public override void AddEdge() {
            base.AddEdge();
        }

        [Test]
        public override void ChangeEdge() {
            base.ChangeEdge();
        }

        [Test]
        public override void AllTests() {
            base.AllTests();
        }
    }
}