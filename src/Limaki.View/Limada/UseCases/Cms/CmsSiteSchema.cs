﻿using Limada.Model;
using Limada.Schemata;

namespace Limada.UseCases.Cms {

    public class CmsSiteSchema : Schema {
        public static readonly IThing ChannelRoot = Thing<string> ("Channels", 0x235e97a79cdd3716);
        /// <summary>
        /// root of articles shown on the zine page
        /// </summary>
        public static readonly IThing Articles = Thing<string> ("Articles", 0x7c5ad5257cd97092);

        public virtual void EnsurceDefaultThings (IThingGraph graph) {
            if (graph.GetById (ChannelRoot.Id) == null)
                graph.Add (ChannelRoot);
            if (graph.GetById (Articles.Id) == null)
                graph.Add (Articles);
        }
    }
}