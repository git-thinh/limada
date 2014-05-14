using Xwt.Backends;

namespace Limaki.View.Vidgets {
    [BackendType(typeof(ITextViewerWithToolstripBackend))]
    public class TextViewerWithToolstrip : Vidget {
        TextViewer _textViewer = null;
        public TextViewer TextViewer {
            get {
                if (_textViewer == null) {
                    _textViewer = new TextViewer();
                }
                return _textViewer;
            }
        }

        ITextViewerWithToolstripBackend _backend = null;
        public virtual new ITextViewerWithToolstripBackend Backend {
            get {
                if (_backend == null) {
                    _backend = BackendHost.Backend as ITextViewerWithToolstripBackend;
                }
                return _backend;
            }
            set { _backend = value; }
        }

        public override void Dispose () {

        }
    }
}