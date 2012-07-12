using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.IOC;
using Limaki.UnitTest;
using NUnit.Framework;
using Xwt.Blind.Backend;


namespace Limaki.Tests {
    [TestFixture]
    public class DomainTest:TestBase {
        [TestFixtureSetUp]
        public override void Setup() {
            if (Registry.ConcreteContext == null) {
                new BlindEngine ().RegisterBackends ();
                Xwt.Engine.WidgetRegistry.RegisterBackend (
                    typeof (Xwt.Drawing.SystemColors), typeof (SystemColorsBackend)
                );

                var loader = new ViewContextRecourceLoader();
                Registry.ConcreteContext = new ApplicationContext();
                loader.ApplyResources(Registry.ConcreteContext);
                //var factory = new AppFactory<global::Limada.UseCases.AppResourceLoader>(loader as IBackendContextRecourceLoader);
                
            }
            base.Setup();
        }
    }
}