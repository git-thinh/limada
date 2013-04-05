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

using Limaki.Drawing;
using Limaki.Drawing.Shapes;
using NUnit.Framework;
using Limaki.Common;
using Xwt;
using Xwt.Drawing;
using Limaki.Drawing.Painters;
using System;

namespace Limaki.Tests.View.Drawing.Shapes {

    public class ShapeTest:DomainTest {

        [TestFixtureSetUp]
        public override void Setup () {
            base.Setup();
            ReportPainter.CanvasSize = new Size(1000, 3000);
        }

        public void TestClone(IShape shape) {
            IShape clone = (IShape)shape.Clone();
            TestShape (clone, shape.Location, shape.Size);
        }

        public void TestShape(IShape shape, Point location, Size size) {
            Assert.AreEqual (shape.Location, location, "wrong location");
            Assert.AreEqual (shape.Size, size, "wrong size");
        }
        
        [Test]
        public void CloneTest() {
            CloneTest<RectangleShape> ();
            CloneTest<RoundedRectangleShape> ();
            CloneTest<VectorShape> ();
        }

        public void CloneTest<T>() where T : IShape, new() {
            ReportSummary ("CloneTest<"+typeof(T).Name+">");
            var locaction = new Point (5, 10);
            var size = new Size (20, 40);

            IShape shape = new T ();
            shape.Location = locaction;
            shape.Size = size;

            TestShape (shape, locaction, size);
            TestClone (shape);
                    
        }

        [Test]
        public void ShapeFactoryTest() {
            var shapeFactory = new Limaki.Drawing.Shapes.ShapeFactory();
            var shapeR = shapeFactory.Shape<Rectangle>(
                new Point(10, 10),
                new Size(20, 100),true
                );
            Assert.IsNotNull(shapeR);
            var shape = shapeFactory.Shape(typeof (Rectangle), new Point(10, 10),
                                           new Size(20, 100),true);
            Assert.IsNotNull(shape);
        }

        [Test]
        public void PainterFactoryTest() {
            var factory = Registry.Pool.TryGetCreate<IPainterFactory>();
            var shape = new RectangleShape();
            IPainter _painter = factory.CreatePainter(shape.GetType());
            Assert.IsNotNull(_painter);
            _painter = factory.CreatePainter(typeof(string));
            Assert.IsNotNull(_painter);
        }

        [Test]
        public void TestBezierRectangleShapeResize () {
            var rect = new Rectangle(10, 10, 200, 100);
            var shape = new BezierRectangleShape(rect);

            ReportPainter.PushPaint(c => {
                c.SetLineWidth(1);
                c.SetColor(Colors.Red);
                c.Rectangle(shape.Data);
                c.Stroke();

                c.SetColor(Colors.Blue);
                ContextPainterExtensions.DrawBezier(c, shape.BezierPoints);
                c.Stroke();

                c.SetColor(Colors.Yellow);
                c.Rectangle(shape.BoundsRect);
                c.Stroke();


            });

            Action<Point, Size> prove = (l, s) => {
                Assert.AreEqual(shape.BoundsRect.Location, shape.Location);
                Assert.AreEqual(shape.BoundsRect.Size, shape.Size);
                Assert.AreEqual(shape.DataSize, shape.Data.Size);
                Assert.AreEqual(l, shape.Location);
                Assert.AreEqual(s, shape.Size);
            };

            Assert.AreEqual(shape.DataSize, rect.Size);
            prove(shape.BoundsRect.Location, shape.BoundsRect.Size);

            Action<Rectangle> proveResize = (r) => {
                var reset = shape.Data;
                var resetBounds = shape.BoundsRect;
                shape.Location = r.Location;
                prove(r.Location, shape.Size);

                shape.Size = r.Size;
                prove(r.Location, r.Size);

                shape.Data = reset;
                prove(resetBounds.Location, resetBounds.Size);

                shape.Location = r.Location;
                shape.Size = r.Size;
                prove(r.Location, r.Size);
            };

            var resize = shape.BoundsRect.Inflate(3, 3);
            proveResize(resize);

            resize = shape.BoundsRect.Inflate(-7, -7);
            proveResize(resize);

           

            WritePainter();
        }
    }
}
