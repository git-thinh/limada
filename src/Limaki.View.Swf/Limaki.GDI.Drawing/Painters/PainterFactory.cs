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
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Drawing.Painters;
using Limaki.Drawing.Shapes;
using Xwt;

namespace Limaki.Drawing.Gdi.Painters {

    public class PainterFactory : PainterFactoryBase, IPainterFactory {

        protected override void InstrumentClazzes () {

            base.InstrumentClazzes ();

            if (false) {
                Add<IPainter<IShape<Vector>, Vector>>(() => new VectorPainter());
                Add<IPainter<IVectorShape, Vector>>(() => new VectorPainter());
                Add<IPainter<string>>(() => new StringPainter());
            }

            

        }
    }
}