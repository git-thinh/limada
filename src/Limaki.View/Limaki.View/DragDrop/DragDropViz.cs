/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 - 2014 Lytico
 *
 * http://www.limada.org
 * 
 */


using Limaki.Common;
using Limaki.Common.Linqish;
using Limaki.Contents;
using Limaki.Graphs;
using Limaki.View.Vidgets;
using Limaki.View.Visuals;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xwt;

namespace Limaki.View.DragDrop {

    public class DragDropViz {

        TransferDataManager _transferDataManager = null;
        public virtual TransferDataManager DataManager { get { return _transferDataManager ?? (_transferDataManager = Registry.Pooled<TransferDataManager>()); } }

        ContentDiggPool _contentDiggPool = null;
        public virtual ContentDiggPool ContentDiggPool { get { return _contentDiggPool ?? (_contentDiggPool = Registry.Pooled<ContentDiggPool>()); } }

        IVisualContentViz _visualContentViz = null;
        public IVisualContentViz VisualContentViz { get { return _visualContentViz ?? (_visualContentViz = Registry.Pooled<IVisualContentViz>()); } }

        public virtual IVisual VisualOfTransferData (IGraph<IVisual, IVisualEdge> graph, ITransferData data) {
            var value = data.GetValue(TransferDataType.FromType(typeof(IVisual)));
            var bytes = value as byte[];
            if (bytes != null) {
                return TransferDataSource.DeserializeValue(bytes) as IVisual;
            }
#if TRACE
            var dt = "";
            data.DataTypes.ForEach (d => dt += d.Id+" | ");
            Trace.WriteLine (dt);
#endif
            Stream stream = null;
            Content<Stream> content = null;
            Action<ContentInfo, Stream> fillContent = (i, s) => {
                bool newContent = content == null || s != content.Data;

                content = new Content<Stream> (content) {
                    Data = newContent ? s : content.Data,
                    ContentType = newContent ? i.ContentType : content.ContentType,
                    Compression = newContent ? i.Compression : content.Compression,
                };


                content = ContentDiggPool.Use (content);
            };

            if (data.Uris.Length > 0) {
                //TODO: handle more then one file
                foreach (var uri in data.Uris.OrderBy (n => n.ToString ())) {
                    var fileName = IoUtils.UriToFileName (uri);
                    if (File.Exists (fileName)) { // TODO: check if filename is directory
                        stream = File.OpenRead (fileName);
                        var sink = DataManager.SinkOf (Path.GetExtension (fileName).TrimStart ('.'));
                        ContentInfo info = null;
                        if (sink != null) {
                            info = sink.Use (stream);
                        } else {
                            info = new ContentInfo ("Unknown", ContentTypes.Unknown, "*", null, CompressionType.neverCompress);
                        }

                        fillContent (info, stream);
                        if (content.Description == null)
                            content.Description = Path.GetFileNameWithoutExtension (fileName);
                        content.Source = fileName;

                        if (data.Uris.Length > 1)
                            Registry.Pooled<IMessageBoxShow> ().Show ("DragDrop multiple files",
                                string.Format ("Only one file {0} will be stored currently", fileName), MessageBoxButtons.Ok);

                        break;
                    }
                }

            } else {
                var dataTypes = data.DataTypes.ToArray ();

                foreach (var s in DataManager.SinksOf (dataTypes)) {
                    value = data.GetValue (s.Item1);
                    var sink = s.Item2;
                    stream = value as Stream;
                    bytes = value as byte[];
                    if (bytes != null)
                        stream = new MemoryStream (bytes);
                    var text = value as string;
                    if (text != null)
                        stream = ByteUtils.AsUnicodeStream (text);
                    if (stream != null) {
                        var info = sink.Use (stream);

                        if (info == null) {
                            info = DataManager.InfoOf (sink, dataTypes).FirstOrDefault ();
                        }

                        if (info != null) {

                            fillContent (info, stream);

                            // TODO: find a better handling of preferences; maybe MimeFingerPrints does the job?
                            if (content.Data == null && (content.Description == null || string.IsNullOrEmpty (content.Description.ToString ())))
                                continue;
                            else
                                break;
                        }
                    }

                }
            }

            if (content != null) {
                var result = VisualContentViz.VisualOfContent (graph, content);
                if (stream is FileStream) {
                    stream.Close ();
                }
                return result;
            }

            return null;
        }

        public virtual TransferDataSource TransferDataOfVisual (IGraph<IVisual, IVisualEdge> graph, IVisual visual) {
            if (graph == null || visual == null)
                return null;
            var result = new TransferDataSource ();
            result.AddValue<string> (visual.Data.ToString ());
            return result;
        }
    }
}