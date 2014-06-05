/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2013 Lytico
 *
 * http://www.limada.org
 * 
 */


using System.ComponentModel;
using System.Windows.Forms;
using Limaki.View;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;
using ToolStrip = System.Windows.Forms.ToolStrip;
using ToolStripItem = System.Windows.Forms.ToolStripItem;

namespace Limaki.View.SwfBackend.Viz.ToolStrips {

    public partial class DisplayModeToolStripBackend : ToolStripBackend, IDisplayModeToolStripBackend {
        public DisplayModeToolStripBackend () {
            InitializeComponent();
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DisplayModeToolStrip Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (DisplayModeToolStrip)frontend;
            Compose();
        }

        private void Compose () {

            var selectButton = new ToolStripDropDownButtonBackend { Command = Frontend.SelectCommand, DisplayStyle = ToolStripItemDisplayStyle.Image };
            selectButton.DropDownItems.AddRange(new ToolStripItem[] { 
                new ToolStripMenuItemEx { Command = Frontend.PanningCommand,ToggleOnClick = selectButton, DisplayStyle = ToolStripItemDisplayStyle.Image},
            });
            var zoomButton = new ToolStripDropDownButtonBackend { Command = Frontend.ZoomInOutCommand, DisplayStyle = ToolStripItemDisplayStyle.ImageAndText };
            zoomButton.DropDownItems.AddRange(new ToolStripItem[] {
                new ToolStripMenuItemEx { Command = Frontend.FitToScreenCommand, DisplayStyle=ToolStripItemDisplayStyle.Text },
                new ToolStripMenuItemEx { Command = Frontend.FitToWidthCommand, DisplayStyle=ToolStripItemDisplayStyle.Text},
                new ToolStripMenuItemEx { Command = Frontend.FitToHeigthCommand, DisplayStyle=ToolStripItemDisplayStyle.Text},
                new ToolStripMenuItemEx { Command = Frontend.OriginalSizeCommand, DisplayStyle=ToolStripItemDisplayStyle.Text},
            });

            zoomButton.MouseDown += (s, e) => Frontend.ZoomInOut(Converter.Convert(e));
            this.Items.AddRange(new ToolStripItem[] {
               selectButton,
               zoomButton,
            });
        }
    }
}