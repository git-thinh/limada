using NUnit.Framework;


using Xwt.Backends;
using Xwt.Engine;
using Xwt.Drawing;
using System;
using Limaki.Painting;
using System.Threading;
using Xwt;


namespace Limaki.Tests.Sandbox {
    public class PaintContextTest : DomainTest {

        [Test]
        public void TestPureXwt() {
            //WidgetRegistry.RegisterBackend(typeof(PaintContext), typeof(Limaki.GDI.Painting.PaintContextBackendHandler));
            //WidgetRegistry.RegisterBackend(typeof(PaintCanvas), typeof(Limaki.Swf.Painting.PaintCanvasBackend));

            var control = new Canvas();

        }

        [Test]
        public void TestTheadStatic() {
            var tread = new Thread(new ThreadStart(CreateContext));
            tread.Start();
            tread = new Thread(new ThreadStart(CreateContext));
            tread.Start();
        }

        void CreateContext() {
            var context = new Xwt.Drawing.Context(WidgetRegistry.MainRegistry, new object());
            Assert.IsNotNull(context);
            
        }
    }
}