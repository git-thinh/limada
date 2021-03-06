﻿using System;
using System.IO;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Contents;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limaki.Graphs;
using Limada.View.ContentViewers;
using Limaki.Tests.View;
using Limaki.View;
using Limaki.View.Html5;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.Viz.Visuals;
using Limaki.View.XwtBackend.Viz;
using Limaki.WebServers;
using Xwt.Drawing;
using Xwt.Html5.Backend;
using Xwt.Tests;
using Limaki.View.Visuals;
using Limaki.View.SwfBackend;

namespace Xwt.Html5.TestApp {

    public partial class XwtHtml5TestMainForm : Form {

        public XwtHtml5TestMainForm () {
            InitializeComponent ();
            Compose ();
        }

        private void Compose () {

            var panel1 = new System.Windows.Forms.Panel { Dock = DockStyle.Left, Width = 100 };

            var htmlViewer = new HtmlContentViewer ();
            var browser = ((GeckoWebBrowserBackend) htmlViewer.Backend).Control;
            browser.Dock = DockStyle.Fill;
            htmlViewer.Clear();

            Action<string, string> showInBrowser = (htmlstring, uri) => {
                var r = new WebResponse ();
                var s = new MemoryStream ();
                var writer = new StreamWriter (s);

                writer.Write (htmlstring);
                writer.Flush ();
                s.Position = 0;
                var content = new Content<Stream> (s, CompressionType.None, ContentTypes.HTML);
                content.ContentType = ContentTypes.HTML;
                r.AbsoluteUri = uri;
                htmlViewer.SetContent (r, content);

            };
            //works, but throws some errors in GeckoWebBrowsre (Uristring too long)
            Action<string, string> showInBrowser1 = (htmlstring, uri) => {
                                                       browser.LoadHtml(htmlstring);
                                                       browser.WaitFor(()=>!browser.IsBusy);
                                                   };

            var serverUri = htmlViewer.WebServer.Uri;
            var butt1 = new System.Windows.Forms.Button { Text = "Hello", Dock = DockStyle.Top };
            butt1.Click += (s, e) =>
                           showInBrowser ("<html><body> Hello world.............................................. </body></html>",
                           serverUri + "helloworld.html");
            var butt2 = new System.Windows.Forms.Button { Text = "static line", Dock = DockStyle.Top };
            butt2.Click += (s, e) =>
                           showInBrowser (TestDataOne.SimpleLine, "http://localhost/simpleline");

            var renderer = new Html5PageWriter{CanvasSize=new Size(1000,2000)};

            var butt3 = new System.Windows.Forms.Button { Text = "lineTest", Dock = DockStyle.Top };
            Action<Context> linetest = ctx => {
                ctx.NewPath();
                ctx.MoveTo (100, 150);
                ctx.LineTo (450, 50);
                ctx.Stroke();
            };

            butt3.Click += (s, e) =>
                           showInBrowser (renderer.Page (linetest), "http://localhost/LineTest");
            
            var refPainter = new ReferencePainter ();

            var butt4 = new System.Windows.Forms.Button { Text = "Figures", Dock = DockStyle.Top };
            Action<Context> Figures = ctx => {
                var x = 10;
                var y = 10;
                refPainter.Figures (ctx, 10, 10);;
                //refPainter.SimpleRectangles(ctx, x, y);
                //refPainter.RectangleWithHole(ctx, x+50, y);
                //refPainter.RoundetRectangle(ctx, x+120, y);
                //refPainter.Curves1(ctx, x, y + 60);
                //refPainter.Curves2(ctx, x + 100, y + 60);
                //refPainter.FontAwesome(ctx, 32, c => font.IconAdjust(c));
            };
            butt4.Click += (s, e) =>
                           showInBrowser (renderer.Page (Figures), "http://localhost/Figures");

            var butt5 = new System.Windows.Forms.Button { Text = "Transforms", Dock = DockStyle.Top };
            Action<Context> Transforms = ctx => {
                refPainter.Transforms (ctx, 10, 10);
            };
            butt5.Click += (s, e) =>
                           showInBrowser (renderer.Page (Transforms), "http://localhost/Transforms");

            var butt6 = new System.Windows.Forms.Button { Text = "Texts", Dock = DockStyle.Top };
            Action<Context> Texts = ctx => {
                refPainter.Texts (ctx, 10, 10);
            };
            butt6.Click += (s, e) =>
                           showInBrowser (renderer.Page (Texts), "http://localhost/Texts");


            var butt7 = new System.Windows.Forms.Button { Text = "VisualScene", Dock = DockStyle.Top };

            var maxExample = new SceneExamples().Examples.Count;
            var example = 0;

            Action<Context> VisualScene = ctx => {
                PaintScene (ctx, 10, 10, example++);
                if (example >= maxExample)
                    example = 0;
            };
            butt7.Click += (s, e) =>
                           showInBrowser (renderer.Page (VisualScene), "http://localhost/VisualScene");

            panel1.Controls.AddRange (new[] { butt7, butt6,butt5, butt4, butt3, butt2, butt1, });

            this.Controls.AddRange (new Control[] { browser, panel1 });

            ClearResources();
            ApplyResources();

            refPainter.Font = Xwt.Drawing.Font.FromName ("Default 10");

            this.FormClosing += (s, e) => htmlViewer.Dispose();
        }


        public void ClearResources () {

        }

        public void ApplyResources () {
            var loader = new Html5ContextResourceLoader ();
            Registry.ConcreteContext = new Limaki.Common.IOC.ApplicationContext ();
            loader.ApplyResources (Registry.ConcreteContext);
        }

        IGraphScene<IVisual, IVisualEdge> SceneWithTestData (int example) {
            IGraphScene<IVisual, IVisualEdge> scene = null;
            var examples = new SceneExamples();
            var testData = examples.Examples[example];
            testData.Data.Count = new Random().Next(1,20);
            //testData.Data.AddDensity = true;
            scene = examples.GetScene (testData.Data);

            var view = scene.Graph as SubGraph<IVisual, IVisualEdge>;
            var graph = view.Source;

            foreach (var item in graph.FindRoots (null)) {
                //if (!graph.IsMarker (item))
                    view.Add (item);
            }

            return scene;
        }
        
        Size PaintScene(Context ctx, double x, double y, int example) {

            var scene = SceneWithTestData (example);

            var styleSheets = Registry.Pooled<StyleSheets>();
            var styleSheet = styleSheets[styleSheets.StyleSheetNames[(DateTime.Now.Millisecond%2)+1]];
            //errror here: wrong fontdata! styleSheet.EdgeStyle.DefaultStyle.PaintData = true;

            var worker = new GraphSceneContextVisualizer<IVisual, IVisualEdge> {
                            StyleSheet = styleSheet
                         };

            worker.Compose(scene, new VisualsRenderer());

            worker.Folder.ShowAllData();
            worker.Modeller.Perform();
            worker.Modeller.Finish();

            worker.Painter.Paint(ctx);
            return scene.Shape.Size;
        }

      
    }
}