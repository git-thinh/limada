/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using Limaki.Common;
using Limada.View;
using Limada.Usecases;
using Limaki.View.Visualizers;
using Limaki.Viewers;
using Limaki.Visuals;
using Limaki.Viewers.ToolStripViewers;
using Limaki.Viewers.Vidgets;

namespace Limaki.Usecases.Concept {

    public class ConceptUsecaseComposer : IComposer<ConceptUsecase> {

        public void Factor(ConceptUsecase useCase) {

            useCase.SplitView = new SplitView0();

            useCase.SheetManager = Registry.Factory.Create<ISheetManager>();
            useCase.VisualsDisplayHistory = new VisualsDisplayHistory ();

            useCase.GraphSceneUiManager = new ThingGraphUiManager {
                OpenFileDialog = new FileDialogMemento(),
                SaveFileDialog = new FileDialogMemento()
            };

            useCase.ContentStreamUiManager = new ContentStreamUiManager {
                OpenFileDialog = new FileDialogMemento(),
                SaveFileDialog = new FileDialogMemento()
            };

            useCase.FavoriteManager = new FavoriteManager();

            useCase.DisplayToolStrip = new DisplayModeToolStrip();
            useCase.ArrangerToolStrip = new ArrangerToolStrip();
            useCase.SplitViewToolStrip = new SplitViewToolStrip();
            useCase.LayoutToolStrip = new LayoutToolStrip();
            useCase.MarkerToolStrip = new MarkerToolStrip();

            useCase.FileDialogShow = this.FileDialogShow;

            Registry.Factory.Add<ContentViewerProvider, ContentVisualViewerProvider>();
        }

        public void Compose(ConceptUsecase useCase) {
            
            var splitView = useCase.SplitView;
            useCase.GetCurrentDisplay = () => splitView.CurrentDisplay;
            useCase.GetCurrentControl = () => splitView.CurrentWidget;

            useCase.SheetManager.SheetRegistered = sceneInfo => {
                useCase.VisualsDisplayHistory.Store(sceneInfo);
                //useCase.SplitViewToolStrip.Attach(splitView.CurrentDisplay);
            };
            splitView.VisualsDisplayHistory = useCase.VisualsDisplayHistory;
            splitView.SheetManager = useCase.SheetManager;
            
            splitView.FavoriteManager = useCase.FavoriteManager;
            useCase.FavoriteManager.SheetManager = useCase.SheetManager;

            useCase.SplitViewToolStrip.SplitView = useCase.SplitView;
            useCase.SplitViewToolStrip.SheetManager = useCase.SheetManager;

            splitView.CurrentWidgetChanged += c => useCase.DisplayToolStrip.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.LayoutToolStrip.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.MarkerToolStrip.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.SplitViewToolStrip.Attach(c);
            splitView.CurrentWidgetChanged += c => useCase.ArrangerToolStrip.Attach(c);
            
            useCase.DisplayStyleChanged += splitView.DoDisplayStyleChanged;

            splitView.Check();

            var fileManager = useCase.GraphSceneUiManager;
            fileManager.FileDialogShow = useCase.FileDialogShow;
            fileManager.MessageBoxShow = useCase.MessageBoxShow;

            fileManager.DataBound = scene => splitView.ChangeData(scene);
            fileManager.DataPostProcess = useCase.DataPostProcess;

            fileManager.Progress = useCase.Progress;
            fileManager.ApplicationQuit = useCase.ApplicationQuit;
            
            var streamManager = useCase.ContentStreamUiManager;
            streamManager.FileDialogShow = useCase.FileDialogShow;
            streamManager.MessageBoxShow = useCase.MessageBoxShow;
            streamManager.Progress = useCase.Progress;
        }

        public DialogResult FileDialogShow (FileDialogMemento value, bool open) {
            FileDialogVidget fileDialog = null;
            if (open) {
                fileDialog = new OpenfileDialogVidget(value);
            } else
                fileDialog = new SavefileDialogVidget(value);
            var result = DialogResult.Cancel;
            if (fileDialog.Run())
                result = DialogResult.Ok;
            fileDialog.Dispose();

            return result;
        }
    }
}