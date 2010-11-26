/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2008 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using System;
using Limada.UseCases;
using Limaki.Common;
using Limaki.Data;
using Limaki.Presenter.Display;
using Limaki.UseCases.Viewers;
using Limaki.UseCases.Viewers.ToolStrips;
using Limaki.Presenter.Widgets;
using Limaki.Widgets;
using Limaki.Drawing;
using Limaki.Model.Streams;
using System.IO;

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

        public void Close() {
            FileManager.Close();
        }


        public SplitView SplitView { get; set; }
        public SceneHistory SceneHistory { get; set; }
        public ISheetManager SheetManager {get;set;}
        
        public DisplayToolController DisplayToolController { get; set; }
        public LayoutToolController LayoutToolController { get; set; }
        public MarkerToolController MarkerToolController { get; set; }
        
        public Get<object> GetCurrentControl { get; set; }
        public Get<WidgetDisplay> GetCurrentDisplay { get; set; }


        public Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }
        public Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }

        public FileManager FileManager { get; set; }
        public Action<string> DataPostProcess { get; set; }

        public void OpenFile() {
            FileManager.OpenFile ();
        }

        public void SaveAsFile() {
            FileManager.SaveAsFile ();
        }

        public void ExportCurrentView() {
            var display = GetCurrentDisplay ();
            if (display != null) {
                FileManager.ExportAsFile (display.Data);
            }
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

		public StreamImportManager StreamImportManager { get; set; }
		public void ImportContent() {
            StreamImportManager.OpenFile ();
        }
		
		public void ImportContent(StreamInfo<Stream> content){
			var display=GetCurrentDisplay();
			if(display!=null){
				StreamImportManager.ImportContent(content,display.Data,display.Layout);
			}                  
		}

        public void ExportContent() {
            StreamImportManager.SaveFile();
        }

        public StreamInfo<Stream> GetExportContent() {
            var display = GetCurrentDisplay();
            if (display != null) {
                return StreamImportManager.ExportContent(display.Data);
            }
            return null;
        }
		
    }

    public class UseCaseComposer : IComposer<UseCase> {
        public void Factor(UseCase useCase) {
            useCase.SheetManager = Registry.Factory.Create<ISheetManager>();
            useCase.SceneHistory = new SceneHistory ();
            useCase.FileManager = new FileManager ();
            useCase.FileManager.OpenFileDialog = new FileDialogMemento();
            useCase.FileManager.SaveFileDialog = new FileDialogMemento();
			useCase.StreamImportManager = new StreamImportManager();
			useCase.StreamImportManager.OpenFileDialog = new FileDialogMemento();
            useCase.StreamImportManager.SaveFileDialog = new FileDialogMemento();
        }

        public void Compose(UseCase useCase) {
            
            var splitView = useCase.SplitView;
            useCase.GetCurrentDisplay = () => splitView.CurrentDisplay;
            useCase.GetCurrentControl = () => splitView.CurrentControl;

            splitView.SceneHistory = useCase.SceneHistory;
            splitView.SheetManager = useCase.SheetManager;
            
            splitView.CurrentControlChanged += (c) => useCase.DisplayToolController.Attach(c);
            splitView.CurrentControlChanged += (c) => useCase.LayoutToolController.Attach(c);
            splitView.CurrentControlChanged += (c) => useCase.MarkerToolController.Attach(c);

            useCase.DisplayStyleChanged += splitView.DoDisplayStyleChanged;

            var fileManager = useCase.FileManager;
            fileManager.FileDialogShow = useCase.FileDialogShow;
            fileManager.MessageBoxShow = useCase.MessageBoxShow;

            fileManager.DataBound = (scene) => splitView.ChangeData(scene);
            fileManager.DataPostProcess = useCase.DataPostProcess;

            
            splitView.Check ();
			
			var streamManager = useCase.StreamImportManager;
			streamManager.FileDialogShow = useCase.FileDialogShow;
            streamManager.MessageBoxShow = useCase.MessageBoxShow;
			streamManager.Import = useCase.ImportContent;
            streamManager.Export = useCase.GetExportContent;
        }
    }
}