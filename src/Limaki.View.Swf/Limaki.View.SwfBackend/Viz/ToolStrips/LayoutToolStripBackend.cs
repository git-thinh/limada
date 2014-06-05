/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2013 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.Drawing.Styles;
using Limada.Usecases;
using Limaki.View;
using Limaki.View.SwfBackend.VidgetBackends;
using Limaki.View.Vidgets;
using Limaki.View.Viz.Visualizers.ToolStrips;

namespace Limaki.View.SwfBackend.Viz.ToolStrips {

    public partial class LayoutToolStripBackend : ToolStripBackend, ILayoutToolStripBackend {

        public new LayoutToolStrip Frontend { get; set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            this.Frontend = (LayoutToolStrip)frontend;
            Compose();
        }

        public LayoutToolStripBackend () {
            InitializeComponent();
        }

        protected virtual void Compose () {

            var shapeCombo = new ToolStripShapeComboBox {
                AutoSize = false,
                Margin = new System.Windows.Forms.Padding (0),
                Size = new System.Drawing.Size (80, 27),
                ToolTipText = "change shape of visual",
            };
            shapeCombo.SelectedIndexChanged += (s, e) => Frontend.ShapeChange(shapeCombo.ShapeComboBoxControl.SelectedItem as IShape);
            var styleSheets = Registry.Pooled<StyleSheets>();
            shapeCombo.ShapeComboBoxControl.ShapeLayout.StyleSheet = styleSheets[styleSheets.StyleSheetNames[1]];

            this.StyleSheetCombo = new ToolStripComboBox {
                AutoSize = false,
                Name = "styleSheetCombo",
                Size = new System.Drawing.Size (121, 27),
                ToolTipText = "Stylesheet",

            };

            StyleSheetCombo.SelectedIndexChanged += StyleSheetSelectedIndexChanged;
            StyleSheetCombo.KeyDown += StyleSheetKeyDown;

            var styleDialogButton = new ToolStripButtonBackend {
                Checked = false,
                CheckState = CheckState.Unchecked,
                DisplayStyle = ToolStripItemDisplayStyle.Image,
                Image = global::Limaki.Iconerias.Iconery.StyleItem,
                Size = new Xwt.Size (24, 24),
                ToolTipText = "set style for selected items",
                Text = "SetStyle",
            };
            styleDialogButton.Click += (s, e) => {
                var style = Frontend.StyleToChange ();
                if (style != null) {
                    var styleDialog = new SwfConceptUseCaseComposer ().ComposeStyleEditor (style, (s1, e1) => Frontend.StyleChange (style));
                    styleDialog.Show ();
                }
            };

            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                                                                             this.StyleSheetCombo,
                                                                             shapeCombo
                                                                         });
#if ! DISTRI
            this.Items.Add(styleDialogButton);
#endif
            InitLayoutTools();
        }


        void InitLayoutTools () {
            StyleSheetCombo.Items.Clear();
            var styleSheets = Registry.Pooled<StyleSheets>();
            foreach (var styleSheet in styleSheets.Values) {
                StyleSheetCombo.Items.Add(styleSheet.Name);
            }
            Application.DoEvents();
        }

        public void AttachStyleSheet (string sheetName) {
            StyleSheetCombo.SelectedIndexChanged -= StyleSheetSelectedIndexChanged;
            StyleSheetCombo.SelectedItem = sheetName;
            StyleSheetCombo.SelectedIndexChanged += StyleSheetSelectedIndexChanged;
        }

        public void DetachStyleSheet (string oldSheetName) {
            StyleSheetCombo.SelectedItem = null;
            StyleSheetCombo.SelectedIndexChanged -= StyleSheetSelectedIndexChanged;
            InitLayoutTools();
        }


        private void StyleSheetSelectedIndexChanged (object sender, EventArgs e) {
            var styleSheetCombo = sender as ToolStripComboBox;
            if (styleSheetCombo != null)
                Frontend.StyleSheetChange(styleSheetCombo.SelectedItem.ToString());
        }

        private void StyleSheetKeyDown (object sender, KeyEventArgs e) {
            var styleSheetCombo = sender as ToolStripComboBox;
            if (styleSheetCombo != null) {
                if (e.KeyCode == Keys.Enter) {
                    int i = styleSheetCombo.Items.Add(StyleSheetCombo.Text);
                    styleSheetCombo.SelectedIndex = i;
                }
            }
        }



    }
}