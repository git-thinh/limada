using System;
using Limaki.Drawing;
using Limaki.Graphs;
using Xwt;

namespace Limaki.View.Layout {
    /// <summary>
    /// maybe this helps to make arranger more clear?
    /// </summary>
    /// <typeparam name="TItem"></typeparam>
    /// <typeparam name="TEdge"></typeparam>
    public class ProxyShape<TItem, TEdge>
        where TEdge : IEdge<TItem>, TItem {
        public ProxyShape(IGraphSceneLocator<TItem, TEdge> locator, TItem item) {
            this.Item = item;
            GetShape = () => locator.GetOrCreateShape(item);
            GetLocation = () => locator.GetLocation(item);
            SetLocation = l => locator.SetLocation(item, l);

            GetSize = () => locator.GetSize(item);
            SetSize = s => GetShape().Size = s;
        }

        Func<IShape> GetShape { get; set; }

        Func<Point> GetLocation { get; set; }
        Action<Point> SetLocation { get; set; }

        Func<Size> GetSize { get; set; }
        Action<Size> SetSize { get; set; }

        public TItem Item { get; protected set; }
        public IShape Shape { get { return GetShape(); } }

        public Point Location {
            get { return GetLocation(); }
            set { SetLocation(value); }
        }

        public Size Size {
            get { return GetSize(); }
            set { SetSize(value); }
        }
    }
}