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

using System.Collections.Generic;
using Limaki.Tests.Graph.Model;
using Limaki.Visuals;
using NUnit.Framework;

namespace Limaki.Tests.Graph.Wrappers {
    public class GCJohnBostonFoldingTest : SceneFacadeTest<GCJohnBostonGraphFactory> {
        public IEnumerable<IVisual> JohnGoBostonNodes {
            get {
                yield return Mock.Factory.Node[1]; // Person
                yield return Mock.Factory.Node[2]; // John
                yield return Mock.Factory.Node[3]; // City
                yield return Mock.Factory.Node[4]; // Boston
                yield return Mock.Factory.Node[5]; // Go
            }
        }

        public IEnumerable<IVisual> JohnGoBoston {
            get {
                yield return Mock.Factory.Node[1]; // Person
                yield return Mock.Factory.Node[2]; // John
                yield return Mock.Factory.Node[3]; // City
                yield return Mock.Factory.Node[4]; // Boston
                yield return Mock.Factory.Node[5]; // Go
                yield return Mock.Factory.Edge[1]; // [Person->John]
                yield return Mock.Factory.Edge[2]; // [City->Boston]
                yield return Mock.Factory.Edge[3]; // [[Person->John]->Go]
                yield return Mock.Factory.Edge[4]; // [[[Person->John]->Go]->[City->Boston]]
            }
        }

        public override IEnumerable<IVisual> FullExpanded {
            get {
                return new IVisual[] {
                    Mock.Factory.Node[1], // Person
                    Mock.Factory.Node[2], // John
                    Mock.Factory.Node[3], // City
                    Mock.Factory.Node[4], // Boston
                    Mock.Factory.Node[5], // Go
                    Mock.Factory.Node[6], // Bus
                    Mock.Factory.Edge[1], // [Person->John]
                    Mock.Factory.Edge[2], // [City->Boston]
                    Mock.Factory.Edge[3], // [[Person->John]->Go]
                    Mock.Factory.Edge[4], // [[[Person->John]->Go]->[City->Boston]]
                    Mock.Factory.Edge[5], // [[[[Person->John]->Go]->[City->Boston]]->Bus]
                };
            }
        }

        [Test]
        public override void FullExpandTest() {
            base.FullExpandTest();
        }

        [Test]
        public void FullExandAndPerson() {
            this.FullExpandTest();
            this.Person();
        }

        [Test]
        public void Person() {
            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[1]; // Person
            Mock.SceneFacade.CollapseToFocused();

            Assert.AreEqual(Mock.Scene.Graph.Count, 1);
            AreEquivalent(new IVisual[] { Mock.Factory.Node[1] }, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Expand (false);

            IVisual[] PersonExpanded = new IVisual[] {
                Mock.Factory.Node[1], // Person
                Mock.Factory.Node[2], // John
                Mock.Factory.Node[5], // Go
                Mock.Factory.Edge[1], // [Person->John]
                Mock.Factory.Edge[3], // [[Person->John]->Go]
            };
            AreEquivalent(PersonExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Node[5]; // Go
            Mock.SceneFacade.Expand(false);

            AreEquivalent(FullExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.SceneFacade.Collapse ();
            AreEquivalent(PersonExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            Mock.Scene.Selected.Clear();
            Mock.Scene.Focused = Mock.Factory.Edge[1]; // [Person->John]
            Mock.SceneFacade.Expand(false);

            AreEquivalent(FullExpanded, Mock.Scene.Graph);
            TestShapes(Mock.Scene);

            this.ReportSummary();

        }
    }
}