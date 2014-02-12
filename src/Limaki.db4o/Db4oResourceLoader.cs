﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Limaki.Common;
using Limada.Model;
using Limaki.Common.IOC;
using Limaki.Data;
using Limada.Data;
using Limaki.Contents.IO;

namespace Limaki.db4o {

    public class Db4oResourceLoader : IContextRecourceLoader {

        public void ApplyResources(IApplicationContext context) {
            var thingGraphProvider = context.Pool.TryGetCreate<ContentIoPool<Iori,ThingGraphContent>>();
            thingGraphProvider.Add(new Db4oThingGraphIo());

            var thingGraphRepairProvider = context.Pool.TryGetCreate<ContentIoPool<IThingGraphRepair, Iori>>();
            thingGraphRepairProvider.Add(new Limada.Data.db4o.Db4oRepairer());
            
        }
    }
}
