/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 */

using Limada.Model;
using Limada.Schemata;
using Limada.VisualThings;
using Limaki;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Graphs;
using Limaki.Graphs.Extensions;
using Limaki.Model.Content;
using Limaki.Model.Content.IO;
using Limaki.Reporting;
using Limaki.Usecases;
using Limaki.View.Layout;
using Limaki.Viewers;
using Limaki.Visuals;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Limada.Usecases {

    public class ContentStreamUiManager : IoUiManager {

        /// <summary>
        /// called after Content is read
        /// </summary>
        public Action<Content<Stream>> ContentIn { get { return ContentStreamIoManager.SinkIn; } set { ContentStreamIoManager.SinkIn = value; } }

        /// <summary>
        /// called to get the Content to be written
        /// </summary>
        public Func<Content<Stream>> ContentOut { get { return ContentStreamIoManager.SinkOut; } set { ContentStreamIoManager.SinkOut = value; } }


        public string ReadFilter { get { return ContentStreamIoManager.ReadFilter; } }
        public string WriteFilter { get { return ContentStreamIoManager.WriteFilter; } }

        private IoUriManager<Stream, Content<Stream>> _contentStreamManager = null;
        public IoUriManager<Stream, Content<Stream>> ContentStreamIoManager { get { return _contentStreamManager ?? (_contentStreamManager = new IoUriManager<Stream, Content<Stream>> { Progress = this.Progress }); } }

        /// <summary>
        /// reads a content from a file
        /// and provides it in ContentIn-Action
        /// </summary>
        public void Read () {

            DefaultDialogValues(OpenFileDialog, ReadFilter);

            if (FileDialogShow(OpenFileDialog, true) == DialogResult.OK) {
                ContentStreamIoManager.ConfigureSinkIo = s => ConfigureSink(s);
                var content = ContentStreamIoManager.ReadSink(IoUtils.UriFromFileName(OpenFileDialog.FileName));
                if (content != null && content.Data != null)
                    ContentStreamIoManager.Close = () => content.Data.Close();
                OpenFileDialog.ResetFileName();
            }
        }

        /// <summary>
        /// gets a content from ContentOut-Func
        /// and writes it into a file
        /// </summary>
        public void Save () {
            if (ContentOut == null)
                return;

            try {
                DefaultDialogValues(SaveFileDialog, WriteFilter);
                var content = ContentOut();
                if (content != null) {
                    var info = ContentStreamIoManager.GetContentInfo(content);
                    if (info != null) {
                        string ext = null;
                        SaveFileDialog.Filter = ContentStreamIoManager.GetFilter(info, out ext) + "All Files|*.*";
                        SaveFileDialog.DefaultExt = ext;
                        SaveFileDialog.SetFileName(content.Source.ToString());
                        if (FileDialogShow(SaveFileDialog, false) == DialogResult.OK) {
                            ContentStreamIoManager.ConfigureSinkIo = s => ConfigureSink(s);
                            ContentStreamIoManager.WriteSink(content, IoUtils.UriFromFileName(SaveFileDialog.FileName));
                            SaveFileDialog.ResetFileName();
                        }
                    }
                }
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(ex, MessageType.OK);
            }
        }

        #region Things-Import-Export

        private IoUriManager<Stream, GraphCursor<IThing, ILink>> _thingGraphFocusIoManager = null;
        public IoUriManager<Stream, GraphCursor<IThing, ILink>> ThingGraphFocusIoManager { get { return _thingGraphFocusIoManager ?? (_thingGraphFocusIoManager = new IoUriManager<Stream, GraphCursor<IThing, ILink>> { Progress = this.Progress }); } }

        protected IoUriManager<Stream, IEnumerable<IThing>> _thingsIoManager = null;
        protected IoUriManager<Stream, IEnumerable<IThing>> ThingsIoManager { get { return _thingsIoManager ?? (_thingsIoManager = new IoUriManager<Stream, IEnumerable<IThing>> { Progress = this.Progress }); } }

        public void ConfigureSink<TSource, TSink> (ISink<TSource, TSink> sink) {
            var report = sink as IReport;
            if (report != null) {
                report.Options.MarginBottom = 0;
                report.Options.MarginLeft = 0;
                report.Options.MarginTop = 0;
                report.Options.MarginRight = 0;
            }
        }

        public void WriteThings (IGraphScene<IVisual, IVisualEdge> scene) {
            try {
                DefaultDialogValues(SaveFileDialog, ThingsIoManager.WriteFilter);
                if (scene != null && scene.HasThingGraph()) {
                    SaveFileDialog.DefaultExt = "pdf";
                    if (FileDialogShow(SaveFileDialog, false) == DialogResult.OK) {
                        ThingsIoManager.SinkOut = () => new VisualThingsSceneViz().SelectedThings(scene);
                        ThingsIoManager.ConfigureSinkIo = s => ConfigureSink(s);
                        ThingsIoManager.WriteSink(IoUtils.UriFromFileName(SaveFileDialog.FileName));
                        SaveFileDialog.ResetFileName();
                    }
                }
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(ex, MessageType.OK);
            }
        }

        public void ReadThingGraphFocus (IGraphScene<IVisual, IVisualEdge> scene) {
            try {
                DefaultDialogValues(OpenFileDialog, ThingGraphFocusIoManager.ReadFilter);
                if (scene != null && scene.HasThingGraph()) {
                    if (FileDialogShow(OpenFileDialog, true) == DialogResult.OK) {
                        var graphFocus = new GraphCursor<IThing, ILink>(scene.Graph.Source<IVisual, IVisualEdge, IThing, ILink>().Two);
                        var uri = IoUtils.UriFromFileName(OpenFileDialog.FileName);
                        ThingGraphFocusIoManager.ConfigureSinkIo = s => ConfigureSink(s);
                        graphFocus = ThingGraphFocusIoManager.ReadSink(uri, graphFocus);
                        new VisualThingsSceneViz().SetDescription(scene, graphFocus.Cursor, OpenFileDialog.FileName);
                        OpenFileDialog.ResetFileName();
                    }
                }
            } catch (Exception ex) {
                Registry.Pool.TryGetCreate<IExceptionHandler>().Catch(ex, MessageType.OK);
            }
        }
       
        #endregion
    }
}