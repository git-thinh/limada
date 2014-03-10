/*
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

using System;
using Limaki.Common;
using Limaki.Usecases.Concept;
using Limaki.View.Visualizers;
using Limaki.Viewers;
using Limaki.Viewers.StreamViewers;
using Xwt;
using Xwt.Drawing;
using Limaki.View.XwtBackend;
using Limaki.Viewers.Vidgets;

namespace Limaki.View.XwtBackend {
   
    public class XwtConceptUseCaseComposer : IXwtConceptUseCaseComposer {

        public IVindowBackend MainWindowBackend { get; set; }
        public Menu Menu { get; set; }
        public Label StatusLabel { get; set; }
        public Size WindowSize { get; set; }
        public Action OnShow { get; set; }

        public virtual void Factor (ConceptUsecase useCase) {
            useCase.MainWindow = new Vindow (MainWindowBackend);

            var mainWindowBackend = MainWindowBackend as Window;
            mainWindowBackend.Size = WindowSize;
            mainWindowBackend.MainMenu = CreateMenu (useCase);

            this.Menu = mainWindowBackend.MainMenu;
                
            var splitViewBackend = useCase.SplitView.Backend as Widget;

            StatusLabel = new Label {
                HeightRequest = 20,
                Text = "hello",
                TextColor = Colors.Black,
                TextAlignment = Alignment.Center,
                Name = "asdsf",
            };
            var box = new VBox {};

            box.PackStart(splitViewBackend, true);
            box.PackEnd(StatusLabel);

            mainWindowBackend.Content = box;

            OnShow += () => (splitViewBackend as Paned).PositionFraction = 0.50;// WindowSize.Width / 2;
        }

        public virtual void Compose (ConceptUsecase useCase) {

            var mainWindowBackend = MainWindowBackend as Window;

            useCase.DataPostProcess =
               dataName => mainWindowBackend.Title = dataName + " - " + useCase.UseCaseTitle;

            useCase.MessageBoxShow = (text, title, buttons) =>
                new XwtMessageBoxShow().Show(text, title, buttons);


            useCase.Progress = (m, i, count) => {
                if (i == -1 && count == -1)
                    StatusLabel.Text = m;
                else
                    StatusLabel.Text = string.Format(m, i, count);
                Application.MainLoop.DispatchPendingEvents();
            };

            mainWindowBackend.CloseRequested += (s, e) => {
                e.AllowClose = MessageDialog.Confirm("Close?", Command.Ok);
                if (e.AllowClose) {
                    useCase.Close();
                    Application.Exit();
                }
            };


            var viewerProvider = Registry.Pool.TryGetCreate<ContentViewerProvider>();

            viewerProvider.Add(new SheetViewer());
            viewerProvider.Add(new ImageContentViewer());

            //viewerProvider.Add(new HtmlContentViewer());
            //viewerProvider.Add(new DigidocContentViewer());
            //viewerProvider.Add(new TextContentViewerWithToolstrip());

        }

        private Menu CreateMenu (ConceptUsecase useCase) {
            var menu = new Menu();

            var l = new Localizer();
            menu.AddItems(

            new MenuItem(l["File"], null, null, new MenuItem[] {
                new MenuItem(l["Open ..."], null, (s, e) => { useCase.OpenFile(); }),
                new MenuItem(l["Save"], null, (s, e) => { useCase.SaveFile(); }),
                new MenuItem(l["SaveAs ..."], null, (s, e) => { useCase.SaveAsFile(); }),
                new MenuItem(l["Export"], null,null, new MenuItem[] {
                    new MenuItem(l["current view ..."], null, (s, e) => { useCase.ExportCurrentView(); }),
                    new MenuItem(l["view as image ..."], null, (s, e) => { this.ExportAsImage(useCase); }),
                    new MenuItem(l["Content ..."], null, (s, e) => { useCase.ExportContent(); }),
                    new MenuItem(l["Report (pdf)..."], null, (s, e) => { useCase.ExportThings(); }),
                }),
				new MenuItem(l["Import"], null, null,new MenuItem[] { 
					new MenuItem(l["Content ..."], null, (s, e) => { useCase.ImportContent(); }),
                    new MenuItem(l["multi content ..."], null, (s, e) => { useCase.ImportGraphCursor(); }),
                    new MenuItem(l["File from previous version ..."], null, (s, e) => { useCase.ImportThingGraphRaw(); })
				}),
                new MenuItem(l["Print ..."], null, (s, e) => { this.Print(useCase); }),
                new MenuItem(l["PrintPreview ..."], null, (s, e) => { this.PrintPreview(useCase); }),
                new MenuItem(l["Exit"], null, (s, e) => { Application.Exit();}),
            }),

            new MenuItem(l["Edit"], null, null, new MenuItem[] {
                new MenuItem(l["Copy"], null, (s, e) => {
                    var display = useCase.GetCurrentDisplay();
                    if (display != null) display.EventControler.Copy();
                }),
                new MenuItem(l["Paste"], null, (s, e) => {
                    var display = useCase.GetCurrentDisplay();
                    if (display != null) display.EventControler.Paste();
                }),
                new MenuItem(l["Search"], null, (s, e) => { useCase.Search(); }),
            }),

            new MenuItem(l["Style"], null, null, new MenuItem[] {
                new MenuItem(l["Layout"], null, (s, e) => { this.ShowLayoutEditor(useCase); }),
                new MenuItem(l["StyleSheet"], null, (s, e) => { this.ShowStyleEditor(useCase); }),
            }),

            new MenuItem(l["Favorites"], null, null, new MenuItem[] {
                new MenuItem(l["Add to favorites"], null, (s, e) => 
                     useCase.FavoriteManager.AddToFavorites(useCase.GetCurrentDisplay().Data)),
                new MenuItem(l["View on open "], null, (s, e) => 
                    useCase.FavoriteManager.SetAutoView(useCase.GetCurrentDisplay().Data)),
            }),

            new MenuItem(l["About"], null, (s, e) => {
                if (About == null) About = new About();
                About.Show();
            })
            );
            return menu;
        }

        private void ExportAsImage (ConceptUsecase useCase) {
            var currentDisplay = useCase.GetCurrentDisplay();
            if (currentDisplay != null && currentDisplay.Data != null) {
                var saveFileDialog = new FileDialogMemento {
                    DefaultExt = "png",
                    Filter = "PNG-Image|*.png|All Files|*.*",
                };

                if (useCase.FileDialogShow(saveFileDialog, false) == DialogResult.Ok) {
                    var image =
                        new ImageExporter(currentDisplay.Data, currentDisplay.Layout) { StyleSheet = currentDisplay.StyleSheet }
                            .ExportImage();
                    if (image != null) {
                        image.Save(saveFileDialog.FileName, ImageFileType.Png);
                        image.Dispose();
                    }
                }
            }
        }

        [TODO]
        private void Print (ConceptUsecase useCase) {
            using (var printDialog = new PrintDialog()) {
                var currentDisplay = useCase.GetCurrentDisplay();
                var man = new PrintManager();
                using (var doc = man.CreatePrintDocument(currentDisplay.Data, currentDisplay.Layout)) {
                    printDialog.Document = doc;
                    if (printDialog.Show() == DialogResult.Ok) {
                        doc.Print();
                    }
                    printDialog.Document = null;
                }
            }
        }

        [TODO]
        private void PrintPreview (ConceptUsecase useCase) {
            throw new System.NotImplementedException();
        }

        [TODO]
        private void ShowStyleEditor (ConceptUsecase useCase) {
            var editor = new StyleEditor();
        }

        private void ShowLayoutEditor (ConceptUsecase useCase) {
            var editor = new LayoutEditor();
        }

        About About { get; set; }


    }
}