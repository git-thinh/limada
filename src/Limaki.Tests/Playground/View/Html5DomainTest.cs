﻿using Limaki.Common;
using Limaki.Tests;
using Limaki.View;
using Xwt.Engine;
using Xwt.Html5.Backend;
using Limaki.View.Html5;
using Limaki.Common.IOC;
using Limaki.IOC;
using System.IO;

namespace Limaki.Playground.View {
    public class Html5DomainTest:DomainTest {
        public override void Setup () {
            if (Registry.ConcreteContext == null) {
                Registry.ConcreteContext = new ApplicationContext();
                var context = Registry.ConcreteContext;
                new LimakiCoreContextRecourceLoader().ApplyResources(context);
                new Html5ContextRecourceLoader().ApplyHtml5Resources(context);
                new ViewContextRecourceLoader().ApplyResources(context);
            }
            base.Setup();
        }

        Html5PageRenderer _reportPainter = null;
        public virtual Html5PageRenderer ReportPainter {
           get {
               return _reportPainter ?? (_reportPainter=new Html5PageRenderer());
           }
        }

        public virtual void WritePainter(string fileName) {
            using (var file = File.CreateText(fileName)) {
                file.Write(ReportPainter.Page());
            }
        }
        public virtual void WritePainter() {
            WritePainter(this.GetType().Name + ".html");
        }
    }
}