﻿/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limaki.Common.IOC;
using Limaki.Drawing;
using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Limaki.IOC;
using Limaki.View.UI;
using Limaki.Viewers;
using Limaki.Visuals;
using Xwt;
using Xwt.Backends;

namespace Limaki.View.XwtBackend {
    public class XwtContextRecourceLoader : IBackendContextRecourceLoader {

        public virtual void ApplyXwtResources (IApplicationContext context) {
            var tk = Toolkit.CurrentEngine;
            tk.RegisterBackend<SystemColorsBackend, XwtSystemColorsBackend>();

            context.Factory.Add<IExceptionHandler, XwtExeptionHandlerBackend>();
            context.Factory.Add<IDrawingUtils, XwtDrawingUtils>();
            context.Factory.Add<ISystemFonts, XwtSystemFonts>();

            context.Factory.Add<IPainterFactory, DefaultPainterFactory>();

            context.Factory.Add<IUISystemInformation, XwtSystemInformation>();
            context.Factory.Add<IShapeFactory, ShapeFactory>();
            context.Factory.Add<IVisualFactory, VisualFactory>();

            context.Factory.Add<ICursorHandler, XwtCursorHandlerBackend> ();
            context.Factory.Add<IMessageBoxShow, XwtMessageBoxShow> ();
            context.Factory.Add<IProgressHandler, ProgressHandler>();
        }

        public virtual void ApplyResources (IApplicationContext context) {

            new LimakiCoreContextRecourceLoader().ApplyResources(context);

            ApplyXwtResources(context);

            new ViewContextRecourceLoader().ApplyResources(context);

            RegisterBackends(context);
        }

        public virtual void RegisterBackends (IApplicationContext context) {
            var engine = new XwtVidgetToolkitEngineBackend();
            engine.Initialize();
            VidgetToolkit.CurrentEngine = new VidgetToolkit { Backend = engine };
        }
    }
}