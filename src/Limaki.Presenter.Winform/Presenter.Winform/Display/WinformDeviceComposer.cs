using Limaki.Presenter.Display;
using Limaki.Presenter.UI;
using Limaki.Presenter.Winform.UI;

namespace Limaki.Presenter.Winform.Display {

    public class WinformDeviceComposer<TData> : DeviceComposer<TData, WinformDisplay<TData>> {
        
        public EventControler EventControler { get; set; }
        public override void Factor(Display<TData> display) {
            
//            Device.Display = display;
            display.Device = Device;

			var deviceRenderer = new WinformRenderer<TData> ();
			deviceRenderer.Device = this.Device;
			deviceRenderer.Display = display;
			
            this.DeviceRenderer = deviceRenderer;

            this.EventControler = new WinformEventControler ();
            this.ViewPort = new WinformViewPort (Device);
            this.DeviceCursor = new CursorHandler (Device);

            this.SelectionRenderer = new  SelectionRenderer();
            this.MoveResizeRenderer = new MoveResizeRenderer ();
        }

        public override void Compose(Display<TData> display) {
            this.DeviceRenderer.BackColor = () => display.BackColor;
            Device.DeviceRenderer = this.DeviceRenderer;
            Device.DeviceViewPort = this.ViewPort;

            display.DeviceRenderer = this.DeviceRenderer;
            display.DataLayer = this.DataLayer;
            display.EventControler = this.EventControler;
            display.Viewport = this.ViewPort;
            display.DeviceCursor = this.DeviceCursor;

            this.MoveResizeRenderer.Device = this.Device;
            display.MoveResizeRenderer = this.MoveResizeRenderer;

            this.SelectionRenderer.Device = this.Device;
            display.SelectionRenderer = this.SelectionRenderer;
        }
    }

}