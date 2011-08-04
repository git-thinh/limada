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

using System;
using Limada.UseCases;
using Limaki.Common;
using Limaki.Data;
using Limaki.UseCases.Viewers;
using Limaki.UseCases.Viewers.ToolStrips;
using Limaki.Presenter.Visuals;
using Limaki.UseCases.Viewers.ToolStripViewers;
using Limaki.Visuals;
using Limaki.Drawing;
using Limaki.Model.Streams;
using System.IO;
using Limada.Presenter;
using Limaki.Presenter.Layout;
using Limaki.Presenter.Display;

namespace Limaki.UseCases {
    public class UseCase:IDisposable {
        protected string _useCaseTitle = "limada::concept";
        public string UseCaseTitle {
            get { return _useCaseTitle; }
            set { _useCaseTitle = value; }
        }

        public void Start() {

            FileManager.OpenFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();
            FileManager.SaveFileDialog.InitialDirectory = Environment.SpecialFolder.MyDocuments.ToString();

            if (!FileManager.OpenCommandLine()) {
                FileManager.ShowEmptyThingGraph();
            }
        }

        bool closeDone = false;
        public void Close() {
            if (!closeDone) {
                SaveChanges();
                FileManager.Close();
                closeDone = true;
            }
        }

        public SplitView0 SplitView { get; set; }
        public SceneHistory SceneHistory { get; set; }
        public ISheetManager SheetManager {get;set;}
        public FavoriteManager FavoriteManager { get; set; }
        
        public DisplayToolController DisplayToolController { get; set; }
        public LayoutToolController LayoutToolController { get; set; }
        public MarkerToolController MarkerToolController { get; set; }
        
        public Get<object> GetCurrentControl { get; set; }
        public Get<IGraphSceneDisplay<IVisual, IVisualEdge>> GetCurrentDisplay { get; set; }


        public Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }
        public Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }

        public FileManager FileManager { get; set; }
        public Action<string> DataPostProcess { get; set; }

        public void OpenFile() {
            SaveChanges();
            FileManager.OpenFile ();
        }

        public virtual void SaveFile() {
            SaveChanges();
            FileManager.Save();
        }

        public void SaveAsFile() {
            SaveChanges();
            FileManager.SaveAsFile ();
        }

        public void ExportCurrentView() {
            var display = GetCurrentDisplay ();
            if (display != null) {
                FileManager.ExportAsThingGraph (display.Data);
            }
        }

        public void ExportThings() {
            var display = GetCurrentDisplay();
            if (display != null) {
                FileManager.ExportThingsAs(display.Data);
            }
        }
        public void ImportThingGraphRaw() {
            SaveChanges();
            FileManager.ShowEmptyThingGraph();
            FileManager.ImportThingGraphRaw();
        }

        public void Search() {
            this.SplitView.DoSearch ();
        }

        public void Dispose() {
            this.SplitView.Dispose ();
            
        }

        public event EventHandler<EventArgs<IStyle>> DisplayStyleChanged = null;
        public void OnDisplayStyleChanged(object sender, EventArgs<IStyle> arg) {
            if (DisplayStyleChanged != null) {
                DisplayStyleChanged(sender, arg);
            }
        }

		public ContentProviderManager ContentProviderManager { get; set; }
		public void ImportContent() {
            ContentProviderManager.OpenFile ();
        }
		
		public void ImportContent(StreamInfo<Stream> content){
			var display=GetCurrentDisplay();
			if(display!=null){
				ContentProviderManager.ImportContent(content,display.Data,display.Layout);
			}                  
		}

        public void ExportContent() {
            ContentProviderManager.SaveFile();
        }

        public StreamInfo<Stream> ExtractContent() {
            var display = GetCurrentDisplay();
            if (display != null) {
                return ContentProviderManager.ExtractContent(display.Data);
            }
            return null;
        }

        public void SaveChanges() {
            var displays = new IGraphSceneDisplay<IVisual, IVisualEdge>[] { SplitView.Display1, SplitView.Display2 };
            SceneHistory.SaveChanges(displays, SheetManager, MessageBoxShow);
            FavoriteManager.SaveChanges(displays);
        }

        public Action<string> StateMessage {get; set;}

        public void AlgignLeft() {
            var display = GetCurrentDisplay();
            var alligner = new Alligner<IVisual, IVisualEdge>(display.Data, display.Layout);
            var items = display.Data.Selected.Elements;
            alligner.AffectedEdges(items);
            alligner.Allign(items, HorizontalAlignment.Left);
            alligner.Proxy.Commit(alligner.Data);
            display.Execute();
        }
    }
}