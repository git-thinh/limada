using System;
using Limaki.Drawing;
using Limaki.View.Visualizers;
using Limaki.Visuals;

namespace Limaki.Viewers.ToolStripViewers {
    public class SplitViewDumnmy : ISplitView {
        public event EventHandler ViewChanged;
        public virtual void ToggleView() { }

        SplitViewMode _viewMode = SplitViewMode.GraphStream;
        public SplitViewMode ViewMode {
            get { return _viewMode; }
            set {
                if (_viewMode != value) {
                    if (value == SplitViewMode.GraphStream)
                        this.SetGraphStreamView();
                    else if (value == SplitViewMode.GraphGraph)
                        this.SetGraphGraphView();
                }
                _viewMode = value;
            }
        }

        public virtual void SetGraphGraphView() { }
        public virtual void SetGraphStreamView() { }

        public virtual bool CanGoBackOrForward(bool forward) {
            return false;
        }

        public virtual void GoHome() { }
        public virtual void GoBackOrForward(bool forward) { }
        public virtual void NewSheet() { }
        public virtual void NewNote() { }
        public virtual void SaveDocument() { }
        public virtual void LoadSheet(SceneInfo info) { }
        public virtual IGraphSceneDisplay<IVisual, IVisualEdge> AdjacentDisplay(IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            return null;
        }
    }
}