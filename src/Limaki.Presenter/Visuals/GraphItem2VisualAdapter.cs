using Limaki.Graphs;
using Limaki.Model;
using Limaki.Common;

namespace Limaki.Visuals {
    public class GraphItem2VisualAdapter : GraphModelAdapter<IGraphItem, IVisual, IGraphEdge, IVisualEdge> {

        public override IGraphItem CreateItemOne(IGraph<IVisual, IVisualEdge> sender,
                                                 IGraph<IGraphItem, IGraphEdge> target, IVisual item) {
            return new GraphItem<string>(item.ToString());
        }

        public override IGraphEdge CreateEdgeOne(IGraph<IVisual, IVisualEdge> sender,
                                                 IGraph<IGraphItem, IGraphEdge> target, IVisualEdge item) {
            return new GraphEdge();
        }

        public override IVisual CreateItemTwo(IGraph<IGraphItem, IGraphEdge> sender,
                                              IGraph<IVisual, IVisualEdge> target,IGraphItem item) {
            return Registry.Pool.TryGetCreate<IVisualFactory>()
                .CreateItem(item.ToString());
        }

        public override IVisualEdge CreateEdgeTwo(IGraph<IGraphItem, IGraphEdge> sender,
                                                  IGraph<IVisual, IVisualEdge> target, IGraphEdge item) {
            return Registry.Pool.TryGetCreate<IVisualFactory>()
                .CreateEdge(item.ToString());
        }
        public override void ChangeData(IGraph<IGraphItem, IGraphEdge> sender, IGraphItem item, object data) {
            throw new System.Exception("The method or operation is not implemented.");
        }
        public override void ChangeData(IGraph<IVisual, IVisualEdge> sender, IVisual item, object data) {
            var graph = sender as IGraphPair<IVisual, IGraphItem, IVisualEdge, IGraphEdge>;
            if (graph != null) {
                var thing = graph.Get(item);
                thing.Data = data;
                item.Data = data;
            }
        }
    }
}