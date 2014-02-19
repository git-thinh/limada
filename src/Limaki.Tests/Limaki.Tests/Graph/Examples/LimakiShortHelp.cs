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

using Limaki.Model;
using Limaki.Visuals;
using Limaki.Tests.Graph.Model;
using System.IO;
using Limaki.Graphs;

namespace Limaki.Tests.Visuals {
    public class LimakiShortHelpFactory : EntityGraphFactory {
        public override void Populate(IGraph<IGraphEntity, IGraphEdge> Graph,int start) {
            IGraphEntity item = new GraphEntity<string>("Limaki");
            Graph.Add(item);
            Nodes[1] = item;

            item = new GraphEntity<string>("How To");
            Graph.Add(item);
            Nodes[2] = item;

            Edges[1] = new GraphEdge(Nodes[1], Nodes[2]);
            Graph.Add(Edges[1]);


            item = new GraphEntity<string>("Warning");
            Graph.Add(item);
            Nodes[3] = item;

            Edges[2] = new GraphEdge(Nodes[1], Nodes[3]);
            Graph.Add(Edges[2]);

            item = new GraphEntity<string>("This is a pre-alpha release");
            Graph.Add(item);
            Graph.Add(new GraphEdge(Nodes[3], item));

            item = new GraphEntity<string>("Saved files will NOT be supported in later releases");
            Graph.Add(item);
            Graph.Add(new GraphEdge(Nodes[3], item));

            IGraphEntity topic = new GraphEntity<string>("add new nodes");
            Graph.Add(topic);
            Graph.Add(new GraphEdge(Nodes[2], topic));

            IGraphEntity subtopic = new GraphEntity<string>("click the Add Shape button");
            IGraphEdge subLink = null;

            Graph.Add(subtopic);
            Graph.Add(new GraphEdge(topic, subtopic));

            item = new GraphEntity<string>("Draw new nodes");
            Graph.Add(item);
            Graph.Add(new GraphEdge(subtopic, item));


            topic = new GraphEntity<string>("connect nodes");
            Graph.Add(topic);
            Graph.Add(new GraphEdge(Nodes[2], topic));

            
            subtopic = new GraphEntity<string>("click the select button");
            Graph.Add(subtopic);
            Graph.Add(subLink = new GraphEdge(topic, subtopic));

            item = new GraphEntity<string>("Drag a node(or link) over an other node (or link)");
            Graph.Add(item);
            Graph.Add(new GraphEdge(subLink, item));

            topic = new GraphEntity<string>("edit nodes or links");
            Graph.Add(topic);
            Graph.Add(new GraphEdge(Nodes[2], topic));
            Graph.Add(subLink = new GraphEdge(topic, subtopic));

            item = new GraphEntity<string>("Press F2, edit, cancel with ESC or save with F2 again");
            Graph.Add(item);
            Graph.Add(new GraphEdge(subLink, item));


            topic = new GraphEntity<string>("expand and collapse nodes");
            Graph.Add(topic);
            Graph.Add(new GraphEdge(Nodes[2], topic));

            subtopic = new GraphEntity<string>("press + to expand");
            Graph.Add(subtopic);
            Graph.Add(new GraphEdge(topic, subtopic));

            subtopic = new GraphEntity<string>("press - to collapse");
            Graph.Add(subtopic);
            Graph.Add(new GraphEdge(topic, subtopic));

            subtopic = new GraphEntity<string>("press / to reduce to focused item");
            Graph.Add(subtopic);
            Graph.Add(new GraphEdge(topic, subtopic));

            subtopic = new GraphEntity<string>("press * to show all items");
            Graph.Add(subtopic);
            Graph.Add(new GraphEdge(topic, subtopic));

        }

        public override string Name {
            get { return "LimakiShortHelp"; }
        }
    }
}
