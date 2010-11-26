/*
 * Limaki 
 * Version 0.081
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://limada.sourceforge.net
 * 
 */

using Limaki.Presenter.Widgets;
using Limaki.Presenter.Widgets.UI;
using Limaki.Widgets;

namespace Limaki.UseCases.Viewers.ToolStrips {
    public class MarkerToolController : ToolController<WidgetDisplay, IMarkerTool> {
        public override void Attach(object sender) {
            var display = sender as WidgetDisplay;
            if (display != null) {
                this.CurrentDisplay = display;
                Tool.Attach(display.Data);
            }
        }

        public virtual void ChangeMarkers(string marker) {
            var display = CurrentDisplay;
            if (display != null) {
                Scene scene = display.Data;
                if (scene.Markers != null) {
                    SceneTools.ChangeMarkers(scene, scene.Selected.Elements, marker);
                }
                display.Execute();
            }
        }
    }
}