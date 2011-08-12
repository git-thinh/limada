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
using System.IO;
using Limada.Presenter;
using Limada.View;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Model.Streams;
using Limaki.Presenter;
using Limaki.Presenter.Visuals;
using Limaki.Presenter.Visuals.UI;
using Limaki.Visuals;
using Limaki.Presenter.UI;
using Limaki.Presenter.Display;
using System.Diagnostics;

namespace Limaki.UseCases.Viewers {
    public class SplitView0 : ISplitView, IDisposable, ICheckable {
        #region Initialize
        public void Initialize() {
            Display1.BackColor = KnownColors.FromKnownColor(KnownColor.Window);

            Display1.ZoomState = ZoomState.Original;
            Display1.SelectAction.Enabled = true;
            Display1.MouseScrollAction.Enabled = false;

            InitializeDisplay(Display1);
            InitializeDisplay(Display2);

            CurrentDisplay = Display1;
        }

        public void InitializeDisplay(IGraphSceneDisplay<IVisual, IVisualEdge> display) {
            StyleSheets styleSheets = Registry.Pool.TryGetCreate<StyleSheets>();
            IStyleSheet styleSheet = null;

            if (styleSheets.TryGetValue(display.StyleSheet.Name, out styleSheet)) {
                display.StyleSheet = styleSheet;
            } else {
                styleSheets.Add(display.StyleSheet.Name, display.StyleSheet);
            }

            display.SceneFocusChanged -= SceneFocusChanged;
            display.SceneFocusChanged += SceneFocusChanged;

            if (DeviceInitializeDisplay !=null) {
                DeviceInitializeDisplay (display);
            }
        }

        #endregion 

        public IGraphSceneDisplay<IVisual, IVisualEdge> Display1 { get; set; }
        public IGraphSceneDisplay<IVisual, IVisualEdge> Display2 { get; set; }
        public object Parent { get; set; }

        public event Action<IGraphSceneDisplay<IVisual, IVisualEdge>> DeviceInitializeDisplay = null;

        public Action<object, Action> AttachControl { get; set; }
        public Action<string, string, Action<string>> ShowTextDialog { get; set; }

        #region View-Switching
        SplitViewMode _viewMode = SplitViewMode.GraphStream;
        public SplitViewMode ViewMode {
            get { return _viewMode; }
            set {
                if (_viewMode != value) {
                    if (value == SplitViewMode.GraphStream)
                        this.GraphStreamView();
                    else if (value == SplitViewMode.GraphGraph)
                        this.GraphGraphView();
                    _viewMode = value;
                    OnViewChanged();
                }
            }
        }

        IGraphSceneDisplay<IVisual, IVisualEdge> _currentDisplay = null;
        public IGraphSceneDisplay<IVisual, IVisualEdge> CurrentDisplay {
            get { return _currentDisplay; }
            protected set {
                _currentDisplay = value;
                CurrentControl = value;
            }
        }

        private object locker = new object();
        object _currentControl = null;
        public object CurrentControl {
            get { return _currentControl; }
            protected set {
                lock (locker) {
                    bool isChange = _currentControl != value;
                    if (value is IGraphSceneDisplay<IVisual, IVisualEdge>) {
                        _currentDisplay = (IGraphSceneDisplay<IVisual, IVisualEdge>)value;
                    }
                    _currentControl = value;
                    if (isChange) {
                        OnCurrentControlChanged(value);
                        OnViewChanged();
                    }
                }
            }
        }

        public Action<object> FocusCatcher { get; set; }
        
        public void DisplayGotFocus(object sender) {
            CurrentDisplay = sender as IGraphSceneDisplay<IVisual, IVisualEdge>;
        }

        public void ControlGotFocus(object sender) {
            CurrentControl = sender;
        }

        public event Action DeviceGraphGraphView = null;
        public event Action DeviceGraphStreamView = null;
        public event Action DeviceToggleView = null;

        protected IExceptionHandler ExceptionHandler {
            get { return Registry.Pool.TryGetCreate<IExceptionHandler>(); }
        }

        public void ToggleView() {
            if (DeviceToggleView != null) {
                DeviceToggleView();
            }
            var display = Display1;
            Display1 = Display2;
            Display2 = display;
        }

        public void GraphGraphView() {
            if (DeviceGraphGraphView != null) {
                DeviceGraphGraphView();
            }
        }

        public void GraphStreamView() {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null &&
                currentDisplay.Data != null &&
                currentDisplay.Data.Focused != null) {
                var fce = new GraphSceneEventArgs<IVisual, IVisualEdge>(currentDisplay.Data, currentDisplay.Data.Focused);
                ContentViewManager.ChangeViewer(currentDisplay, fce);
            }
            if (DeviceGraphStreamView != null) {
                DeviceGraphStreamView();
            }
        }

        public event EventHandler ViewChanged = null;
        public void OnViewChanged() {
            if (ViewChanged != null) {
                ViewChanged(this,new EventArgs());
            }
        }
        
        public event Action<object> CurrentControlChanged = null;
        public void OnCurrentControlChanged(object control) {
            if (CurrentControlChanged != null) {
                CurrentControlChanged(control);
            }
        }

        #endregion

        public void ChangeData(IGraphScene<IVisual, IVisualEdge> scene) {
            ClearHistory();

            CurrentDisplay = null;

            Display1.Data = scene;
            FavoriteManager.GoHome(Display1, true);

            Display2.Data = null;

            ContentViewManager.Clear();

            Registry.ApplyProperties<MarkerContextProcessor, IGraphScene<IVisual, IVisualEdge>>(Display1.Data);

            new WiredDisplays().MakeSideDisplay(Display1, Display2);

            Registry.ApplyProperties<MarkerContextProcessor, IGraphScene<IVisual, IVisualEdge>>(Display2.Data);

            GraphGraphView();
            GraphStreamView();

            CurrentDisplay = Display1;
        }

        #region StreamView

        private ContentViewManager _contentViewManager = null;
        public ContentViewManager ContentViewManager {
            get {
                if (_contentViewManager == null) {
                    _contentViewManager = new ContentViewManager();
                }
                
                _contentViewManager.Parent = this.Parent;
                _contentViewManager.BackColor = Display1.BackColor;

                _contentViewManager.AttachControl -= this.AttachControl;
                _contentViewManager.AttachControl += this.AttachControl;
                _contentViewManager.Attach -= this.FocusCatcher;
                _contentViewManager.Attach += this.FocusCatcher;

                _contentViewManager.SheetManager = this.SheetManager;
                
                return _contentViewManager;
            }
            set { _contentViewManager = value; }
        }

        IGraphSceneDisplay<IVisual,IVisualEdge> AdjacentDisplay(IGraphSceneDisplay<IVisual,IVisualEdge> display) {
            if (display == Display2)
                return Display1;
            else
                return Display2;
        }

        void SceneFocusChanged(object sender, GraphSceneEventArgs<IVisual , IVisualEdge> e) {

            if (ViewMode != SplitViewMode.GraphStream)
                return;
            
            var display = sender as IGraphSceneDisplay<IVisual,IVisualEdge>;
            var adjacent = AdjacentDisplay(display);
            try {
                display.EventControler.UserEventsDisabled = true;
                adjacent.EventControler.UserEventsDisabled = true;
                ContentViewManager.SheetControl = adjacent;
                ContentViewManager.ChangeViewer(sender, e);
            } catch (Exception ex) {
                ExceptionHandler.Catch(ex, MessageType.OK);
            } finally {
                display.EventControler.UserEventsDisabled = false;
                adjacent.EventControler.UserEventsDisabled = false;
            }
        }

        #endregion

        #region SheetManagement

        public ISheetManager SheetManager {get;set;}

        public void SaveDocument() {
            if (CurrentControl == CurrentDisplay) {
                var display = this.CurrentDisplay;
                if (SheetManager.IsSaveable(display.Data)) {
                    var info = display.Info;
                    ShowTextDialog("Sheet:", info.Name, SaveSheet);
                }
            } else {
                this.ContentViewManager.SaveStream(CurrentDisplay.Data.Graph.ThingGraph());
            }
        }

        public void SaveSheet(string name) {
            var currentDisplay = this.CurrentDisplay;
            if (currentDisplay != null) {
                var info = currentDisplay.Info;
                info.Name = name;
                SheetManager.SaveInGraph(currentDisplay.Data, currentDisplay.Layout, info);
                currentDisplay.Info = info;
                FavoriteManager.AddToSheets(currentDisplay.Data.Graph, currentDisplay.DataId);
            }
        }

        public void NewSheet() {
            var currentDisplay = this.CurrentDisplay;
            SceneHistory.Store(currentDisplay, SheetManager);
            var info = SheetManager.CreateSheet(currentDisplay.Data);
            currentDisplay.Info = info;
            currentDisplay.DeviceRenderer.Render();
            OnViewChanged();
        }

        #endregion

        #region Search

        public void Search(string name) {
            var currentDisplay = this.CurrentDisplay;
            SceneHistory.Store (currentDisplay, SheetManager);
            var search = new SearchHandler ();
            search.LoadSearch (currentDisplay.Data, currentDisplay.Layout, name);
            currentDisplay.DataId = 0;
            new State { Hollow = true }.CopyTo(currentDisplay.State);
            currentDisplay.Text = name;
            currentDisplay.Viewport.Reset ();
            currentDisplay.DeviceRenderer.Render ();
            OnViewChanged ();
        }

        public void DoSearch() {
            var currentDisplay = this.CurrentDisplay;
            if (new SearchHandler().IsSearchable(currentDisplay.Data)) {
                ShowTextDialog("Search:", currentDisplay.Text,this.Search);
            }
        }

        #endregion

        #region History


        public SceneHistory SceneHistory { get; set; }
        
        private void ClearHistory() {
            if (SceneHistory != null) {
                SceneHistory.Clear ();
            }

            if (SheetManager != null) {
                SheetManager.Clear();
            }

            Display1.DataId = 0;
            Display1.Text = string.Empty;
            Display2.DataId = 0;
            Display2.Text = string.Empty;
        }

        #endregion 

        #region Navigating

        public void GoHome() {
            var display = this.CurrentDisplay;
            if (display != null) {
                Trace.WriteLine(string.Format("Before Home\tId\t{0}\tName\t{1}",display.Info.Id,display.Info.Name));
                SceneHistory.Store(display, SheetManager);
               
                FavoriteManager.GoHome(display, false);
                OnViewChanged();
                Trace.WriteLine(string.Format("After Home\tId\t{0}\tName\t{1}", display.Info.Id, display.Info.Name));
            }
        }

        public bool CanGoBackOrForward(bool forward) {
            if (SceneHistory == null)
                return false;

            var currentDisplay = this.CurrentDisplay;
            var currentControl = this.CurrentControl;
            if (currentControl == currentDisplay && currentDisplay != null) {
                if (forward)
                    return SceneHistory.CanGoForward();
                else
                    return SceneHistory.CanGoBack();
            } else if (currentControl is INavigateTarget) {
                if (forward)
                    return ((INavigateTarget)currentControl).CanGoForward;
                else
                    return ((INavigateTarget)currentControl).CanGoBack;
            }
            return false;
        }

        public void GoBackOrForward(bool forward) {
            var currentDisplay = this.CurrentDisplay;
            var currentControl = this.CurrentControl;
            if (currentControl == currentDisplay && currentDisplay != null) {
                SceneHistory.Navigate(currentDisplay, SheetManager, forward);
            } else if (currentControl is INavigateTarget) {
                if (forward)
                    ((INavigateTarget)currentControl).GoForward();
                else
                    ((INavigateTarget)currentControl).GoBack();

            }
            OnViewChanged();
        }

        #endregion

        #region Favorites

        public FavoriteManager FavoriteManager {get;set;}

        public void AddFocusedToFavorites() {
            FavoriteManager.AddToFavorites(CurrentDisplay.Data as Scene);
        }

        public void ViewOnOpen() {
            FavoriteManager.SetAutoView(CurrentDisplay.Data as Scene);
        }

        #endregion

        #region Notes

        public void NewNote() {
            ShowTextDialog("Note:", "new note", CreateNewNote);
        }

        public void CreateNewNote(string title) {
            var currentDiplay = CurrentDisplay;
            if (currentDiplay == null)
                return;
            var scene = currentDiplay.Data;
            if (scene == null)
                return;

            Content<Stream> content = new Content<Stream>(
                new MemoryStream(), CompressionType.bZip2, StreamTypes.RTF);

            content.Description = title;

            var writer = new StreamWriter(content.Data);

            writer.Write(@"{\rtf1\ansi\deff0");
            writer.Write(@"{\info{\doccomm limada.note}}");

            writer.Write(@"{\fonttbl{\f0\froman Times New Roman;}}");
            writer.Write(@"\pard\plain ");
            writer.Write(title);
            writer.Write(@"}");
            writer.Flush();
            content.Data.Position = 0;


            var visual = new VisualThingStreamHelper().CreateFromStream(scene.Graph, content);
            var root = scene.Focused;

            var layout = currentDiplay.Layout;

            if (root == null) {
                PointI pt = new PointI(layout.Border.Width, scene.Shape.BoundsRect.Bottom);
                SceneTools.AddItem(scene, visual, layout, pt);
            } else {
                SceneTools.PlaceVisual(scene, root, visual, layout);
            }
            scene.Selected.Clear();
            scene.Focused = visual;
            currentDiplay.Execute();
            currentDiplay.OnSceneFocusChanged();
        }
        #endregion

        public void DoDisplayStyleChanged(object sender, EventArgs<IStyle> arg) {
            if (Display1 != null) {
                Display1.DeviceRenderer.Render();
            }
            if (Display2 != null) {
                Display2.DeviceRenderer.Render();
            }
        }

        public void Dispose() {
            ClearHistory ();
            this.ContentViewManager.Dispose();

        }

        public virtual bool Check() {
            if (this.SceneHistory == null) {
                throw new CheckFailedException(this.GetType(), typeof(SceneHistory));
            }
            if (this.SheetManager == null) {
                throw new CheckFailedException(this.GetType(), typeof(SheetManager));
            }
            if (this.Display1 == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphSceneDisplay<IVisual, IVisualEdge>));
            }
            if (this.Display2 == null) {
                throw new CheckFailedException(this.GetType(), typeof(IGraphSceneDisplay<IVisual, IVisualEdge>));
            }
            if (this.ShowTextDialog == null) {
                throw new CheckFailedException(this.GetType()+"needs a ShowTextDialogAction");
            }
            
            Display1.Check ();
            Display2.Check ();

            return true;
        }
    }
}