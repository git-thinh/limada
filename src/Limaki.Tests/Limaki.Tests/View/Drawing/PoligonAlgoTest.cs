﻿/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;

using Limaki.Drawing;
using Limaki.UnitTest;
using NUnit.Framework;
using System.Collections.Generic;
using Limaki.Tests.View;
using Limaki.Drawing.Shapes;
using Xwt;
using Xwt.Drawing;
using Limaki.Drawing.Painters;
using System.Linq;

namespace Limaki.Tests.Drawing {
    public class PoligonAlgoTest : DomainTest {
        [TestFixtureSetUp]
        public override void Setup() {
            base.Setup();
            ReportPainter.CanvasSize = new Size(1000, 3000);
        }
        private int offsetY = 50;
        [Test]
        public void TestBezierHullBezierRect () {
            var curve = new BezierTestData { Matrix = new Matrix { OffsetX = 50, OffsetY = offsetY } }
                .RoundedRectBezier;

            TestBezierHull(curve);
            offsetY += 200;
        }

        [Test]
        public void TestBezierHullCurve1 () {
            var curve = new BezierTestData { Matrix = new Matrix { OffsetX = 50, OffsetY = offsetY } }
             .Curve1;

            TestBezierHull(curve);

            offsetY += 400;

        }

        public void TestBezierHull (IList<Point> bezier) {
            base.ReportPainter.PushPaint(c => {
                c.SetColor(Colors.Blue);
                c.SetLineWidth(2);
                ContextPainterExtensions.DrawBezier(c, bezier);
                c.Stroke();
            });

            var cps = BezierExtensions.ControlPoints(bezier);
            base.ReportPainter.PushPaint(c => {
                c.SetColor(Colors.Red);
                c.SetLineWidth(1);
                foreach (var p in cps) {
                    c.Arc(p.X, p.Y, 5, 0, 360);
                    c.Stroke();
                }
                ;
            });

            var hull = BezierExtensions.BezierHull(bezier); 
            base.ReportPainter.PushPaint(c => {
                c.SetColor(Colors.Green);
                c.SetLineWidth(1);
                foreach (var p in hull) {
                    c.Arc(p.X, p.Y, 3, 0, 360);
                    c.Fill();
                }
                ContextPainterExtensions.DrawPoligon(c, hull);
                c.Stroke();
            });
            WritePainter();
        }


        [Test]
        public void TestBezierSegmentsCurve1 () {
            var curve = new BezierTestData { Matrix = new Matrix { OffsetX = 50, OffsetY = offsetY } }
             .Curve1;

            PaintSegments(curve);

            offsetY += 400;

        }

        [Test]
        public void TestBezierSegmentsBezierRectangle () {
            var curve = new BezierTestData { Matrix = new Matrix { OffsetX = 50, OffsetY = offsetY } }
             .RoundedRectBezier;

            PaintSegments(curve);

            offsetY += 200;

        }

        private void PaintSegments (IList<Point> curve) {
            var segments = BezierExtensions.BezierSegments(curve);
            var cols = new Color[] {
                                       Colors.Blue,
                                       Colors.Red,
                                       Colors.BlueViolet,
                                       Colors.OrangeRed,
                                       Colors.AliceBlue,
                                       Colors.Orange,
                                       Colors.Aquamarine,
                                       Colors.DodgerBlue
                                   };
           

            ReportPainter.PushPaint(c => {
                var iCol = 0;
                foreach (var seg in segments) {
                    c.SetLineWidth(1);
                    var bb = BezierExtensions.BezierBoundingBox(seg.Start, seg.Cp1, seg.Cp2, seg.End);
                    c.SetColor(Colors.SpringGreen);
                    c.Rectangle(bb);
                    c.Stroke();

                    c.SetColor(cols[iCol++]);
                    c.SetLineWidth(2);
                    ContextPainterExtensions.DrawBezier(c, seg);
                    c.Stroke();

                    var med = BezierExtensions.BezierPoint(seg.Start, seg.Cp1, seg.Cp2, seg.End, .5);
                    c.SetColor(Colors.DarkGray);
                    c.Arc(med.X, med.Y, 5, 0, 360);
                    c.Stroke();

                   
                }
            });

            WritePainter();

        }

    }
}