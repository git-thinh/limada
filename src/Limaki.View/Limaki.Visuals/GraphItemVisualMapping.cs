using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model;

namespace Limaki.Visuals {
    public class GraphItemVisualMapping : GraphMapping {

        /// <summary>
        /// looks if source is
        /// - IGraphPair<IVisual, IGraphItem, IVisualEdge, IGraphEdge>
        /// if so, 
        /// creates a new GraphPair according to source
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override IGraph<TItem, TEdge> CloneGraphPair<TItem, TEdge>(
            IGraph<TItem, TEdge> source) {

            IGraph<TItem, TEdge> targetGraph = null;
            if (source is IGraphPair<IVisual, IGraphItem, IVisualEdge, IGraphEdge>) {
                targetGraph = new LiveGraphPair<IVisual, IGraphItem, IVisualEdge, IGraphEdge>(
                                  new VisualGraph(),
                                  ((IGraphPair<IVisual, IGraphItem, IVisualEdge, IGraphEdge>)source).Two,
                                  new GraphItem2VisualAdapter().ReverseAdapter())
                              as IGraph<TItem, TEdge> ;
            } else  if (Next != null) {
                Next.CloneGraphPair<TItem, TEdge> (source);
            }

            return targetGraph;
        }

        public override TItem LookUp<TItem,TEdge>(
            IGraphPair<TItem, TItem, TEdge, TEdge> sourceGraph,
            IGraphPair<TItem, TItem, TEdge, TEdge> targetGraph,
            TItem sourceitem)  {

            TItem item = default(TItem);
            if (sourceGraph == null || targetGraph == null || sourceitem == null)
                return item;

            if (sourceGraph.Source<TItem, TEdge, IGraphItem, IGraphEdge>() != null) {
                return sourceGraph.LookUp<TItem, TEdge, IGraphItem, IGraphEdge>(targetGraph, sourceitem);
            } else if (Next != null) {
                return Next.LookUp<TItem,TEdge> (sourceGraph,targetGraph,sourceitem);
            }

            return item;

        }
    }
}