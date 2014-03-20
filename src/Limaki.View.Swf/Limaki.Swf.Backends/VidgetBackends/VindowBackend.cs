using System.Windows.Forms;
using Limaki.View.Vidgets;
using Xwt.Gdi.Backend;

namespace Limaki.View.Swf.Backends {

    public class VindowBackend : Form, IVindowBackend {

        public void SetContent (IVidget value) {
            var backend = value.Backend as Control;
            if (!this.Controls.Contains (backend)) {
                backend.Dock = DockStyle.Fill;
                this.Controls.Add (backend);
            }
        }

        #region IVidgetBackend-Implementation

        public IVindow Frontend { get; protected set; }

        public virtual void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = (IVindow)frontend;
        }

        Xwt.Size IVidgetBackend.Size {
            get { return this.Size.ToXwt (); }
        }

        void IVidgetBackend.Invalidate (Xwt.Rectangle rect) {
            this.Invalidate (rect.ToGdi ());
        }

        void IVidgetBackend.SetFocus () { this.Focus (); }

        #endregion



    }
}