/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Drawing.Gdi;
using Limaki.Drawing.Gdi.Painters;
using Limaki.Drawing.Shapes;
using Limaki.IOC;
using Limaki.Model.Content;
using Limaki.Swf.Backends;
using Limaki.View.DragDrop;
using Limaki.View.Swf.Backends;
using Limaki.View.Swf.Visualizers;
using Limaki.View.UI;
using Limaki.View.Visuals.Visualizers;
using Limaki.Viewers;
using Limaki.Visuals;
using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Xwt;
using Xwt.Gdi.Backend;


namespace Limaki.View.Swf {

   
    /// <summary>
    /// ApplicationContextRecourceLoader to use
    /// System.Windows.Forms
    /// </summary>
    public class SwfContextRecourceLoader : IBackendContextRecourceLoader {

        public virtual void ApplyResources(IApplicationContext context) {

            new LimakiCoreContextRecourceLoader ().ApplyResources (context);

            var tk = Toolkit.CreateToolkit<GdiEngine>(false);
            tk.RegisterBackend<Xwt.Backends.SystemColorsBackend, Xwt.Gdi.Backend.SystemColorsGdiBackend>();
            tk.SetActive();
            

            context.Factory.Add<IExceptionHandler,SwfExeptionHandlerBackend>();
            context.Factory.Add<IProgressHandler, ProgressHandler>();
            context.Factory.Add<IDrawingUtils,GdiDrawingUtils>();
            context.Factory.Add<ISystemFonts, GdiSystemFonts>();
            context.Factory.Add<IPainterFactory, PainterFactory>();

            context.Factory.Add<IUISystemInformation, SwfSystemInformation>();
            context.Factory.Add<IShapeFactory, ShapeFactory>();
            context.Factory.Add<IVisualFactory,VisualFactory>();
            
            context.Factory.Add<ICursorHandler, CursorHandlerBackend>();
            context.Factory.Add<IMessageBoxShow, MessageBoxShow>();

            context.Factory.Add<IDisplay<System.Drawing.Image>, ImageDisplay>();
            context.Factory.Add<IDisplay<IGraphScene<IVisual,IVisualEdge>>, VisualsDisplay>();

            context.Factory.Add<IWebBrowserBackend>(() => CreateWebBrowserBackend());

            new ViewContextRecourceLoader().ApplyResources(context);

            RegisterBackends(context);
            RegisterDragDropFormats(context);
        }

        public virtual void RegisterBackends (IApplicationContext context) {
            var engine = new SwfVidgetToolkitEngineBackend();
            engine.Initialize();
            VidgetToolkit.CurrentEngine = new VidgetToolkit { Backend = engine };
        }

        public static bool GeckoFailed = false;
        public IWebBrowserBackend CreateWebBrowserBackend () {
            Control _backend = null;
            if (GeckoFailed || OS.Mono) { //(true) { //|| OS.IsWin64Process
                _backend = new WebBrowserBackend();
                GeckoFailed = true;
                Trace.WriteLine("No Gecko");
            } else {
                try {
                    var gecko = Activator.CreateInstance(
                       this.GetType().Assembly.FullName,
                       typeof(WebBrowserBackend).Namespace + ".GeckoWebBrowserBackend");
                    if (gecko != null)
                        _backend = (Control)gecko.Unwrap();
                    else
                        throw new Exception();
                } catch {
                    GeckoFailed = true;
                    return CreateWebBrowserBackend();
                }
                Thread.Sleep(0);
            }
            return _backend as IWebBrowserBackend;
        }

        public virtual void RegisterDragDropFormats (IApplicationContext context) {

            Registry.Factory.Add<IDragDropBackendHandler>(args => new DragDropBackendHandler(args[0] as IVidgetBackend));

            var man = new TransferDataManager();
            man.RegisterSome();
            man.TransferContentTypes.Add(System.Windows.Forms.DataFormats.Html.ToLower(), ContentTypes.HTML);
            man.TransferContentTypes.Add(System.Windows.Forms.DataFormats.Rtf.ToLower(), ContentTypes.RTF);

            // TODO: register the others
            //man.TransferContentTypes.Add(System.Windows.Forms.DataFormats.UnicodeText, 0);

        }
    }
}

