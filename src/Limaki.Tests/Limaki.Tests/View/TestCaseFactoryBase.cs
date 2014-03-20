using Limada.IO;
using Limada.Schemata;
using Limada.Usecases;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limada.UseCases;
using Limada.View.VisualThings;
using Limaki.Tests.View;
using Limaki.Tests.View.Display;
using Limaki.Usecases;
using Limaki.View;
using Limaki.View.GraphScene;
using Limaki.View.Vidgets;
using System;
using System.Linq;
using Limaki.View.Visuals;
using Limaki.View.Viz;
using Limaki.View.Viz.Rendering;
using Limaki.View.Viz.Visualizers;
using Limaki.View.Viz.Visuals;
using Xwt;

namespace Limaki.Tests.UseCases {

    public class TestCaseFactoryBase : UsecaseFactory<ConceptUsecase> {

        public Func<BenchmarkOneTests> DisplayTest { get; protected set; }
        public MessageEventHandler TestMessage { get; protected set; }

        public override void Compose (ConceptUsecase useCase) {

            this.TestMessage = (s, m) => useCase.Progress (m, -1, -1);

            this.DisplayTest = () => {
                                   var test = new BenchmarkOneTests();
                                   var testVidget = new VisualsDisplayTest {
                                                                               CreateDisplay = () => new VisualsDisplay()
                                                                           };

                                   test.WriteDetail += TestMessage;
                                   test.TestWindow = useCase.MainWindow;
                                   test.Display = useCase.SplitView.Display1;
                                   test.Setup();

                                   return test;
                               };
        }

        public void ShowQuadTree (IGraphScene<IVisual, IVisualEdge> scene) {
            var vindow = new Vindow();
            var display = new VisualsDisplay();
            vindow.Content = display;

            var quadTreeVisualizer = new QuadTreeVisualizer();
            quadTreeVisualizer.VisualsDisplay = display;
            quadTreeVisualizer.Data = (scene.SpatialIndex as VisualsQuadTreeIndex).GeoIndex;

            vindow.Show();
        }

        public void WCFServiceTest (ConceptUsecase usecase) {
#if WCF
            DataBaseInfo info = new DataBaseInfo();
            info.Server = "http://localhost";
            info.Port = 8000;
            info.Path = "Limada";
            info.Name = "ThingGraphService";
            var handler = new SceneProvider();
            var provider = new WCFThingGraphClientProvider();

            handler.Provider = provider;
            handler.DataBound = usecase.GraphSceneUiManager.DataBound;
            if (handler.Open(info)) {
                usecase.DataPostProcess (provider.host.baseAddress.AbsoluteUri);
            }
#endif
        }

        public void InstrumentLayer (IRenderAction renderAction, IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            var layer = renderAction as ILayer<IGraphScene<IVisual, IVisualEdge>>;
            if (layer != null) {
                layer.Data = () => display.Data;
                layer.Camera = () => display.Viewport.Camera;
            }
            var graphLayer = layer as GraphSceneLayer<IVisual, IVisualEdge>;
            if (graphLayer != null) {
                graphLayer.Layout = () => display.Layout;

            }
            display.EventControler.Add (renderAction);
        }

        public void NoSchemaThingGraph (ConceptUsecase useCase) {
            var display = useCase.GetCurrentDisplay();
            var thingGraph = display.Data.Graph.ThingGraph();
            var schemaGraph = thingGraph as SchemaThingGraph;
            if (schemaGraph != null) {
                schemaGraph.EdgeFilter = e => true;
                schemaGraph.ItemFilter = e => true;
            }
        }

        public void CurrentProblem (ConceptUsecase usecase) {
            try {

                var test = new WebProxyTest ();
                test.TestInfinitLoopIfHtmlContentIsFocused (usecase.GetCurrentDisplay ());

            } catch (Exception e) {
                Registry.Pooled<IExceptionHandler> ().Catch (e, MessageType.OK);
            } finally {

            }
        }

  
    }
}