using System.Windows.Forms;
using Limaki.Presenter.Winform;
using Limaki.UseCases;
using Limaki.UseCases.Winform;
using Limaki.Common.IOC;
using Limaki.Common;

namespace Limaki.App {
    public class WinformAppFactory : AppFactory<Limada.UseCases.AppResourceLoader> {
        public WinformAppFactory(): base(new WinformContextRecourceLoader()) {}

        public Form MainForm() {
            var result = new Form ();

            CreateUseCase (result);
            
            return result;
        }

        public void CreateUseCase(Form mainform) {
            mainform.Icon = Limaki.Presenter.Properties.Resources.LimadaLogoA;
            mainform.ClientSize = new System.Drawing.Size(632, 406);

            var deviceComposer = new WinformUseCaseComposer();
            deviceComposer.Mainform = mainform;

            var factory = new UseCaseFactory<UseCase>();
            factory.Composer = new UseCaseComposer();
            factory.DeviceComposer = deviceComposer;
            
            var useCase = factory.Create();
            factory.Compose(useCase);

            CallPlugins(factory, useCase);

            useCase.Start();
        }

        public void CallPlugins(UseCaseFactory<UseCase> factory, UseCase useCase) {
            var factories = Registry.Pool.TryGetCreate < UseCaseFactories<UseCase>>();
            foreach(var item in factories) {
                item.Composer = factory.Composer;
                item.DeviceComposer = factory.DeviceComposer;
                item.Compose(useCase);
            }
        }
    }
    
}