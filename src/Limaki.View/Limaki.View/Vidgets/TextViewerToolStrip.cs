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
using Limaki.Iconerias;
using Limaki.Common.Linqish;
using Xwt.Drawing;

namespace Limaki.View.Vidgets {

    public class TextViewerToolStrip:ToolStrip {

        public virtual TextViewer TextViewer { get; protected set; }

        public ComboBox FontFamilyCombo { get; set; }
        public ComboBox FontSizeCombo { get; set; }

        public ToolStripButton BoldButton { get; protected set; }
        public ToolStripButton ItalicButton { get; protected set; }
        public ToolStripButton UnderlineButton { get; protected set; }
        public ToolStripButton StrikeThroughButton { get; protected set; }
        
        public IToolStripCommand BoldCommand { get; set; }
        public IToolStripCommand ItalicCommand { get; set; }
        public IToolStripCommand UnderlineCommand { get; set; }
        public IToolStripCommand StrikeThroughCommand { get; set; }

        public TextViewerToolStrip () {
            Compose ();
        }

        protected virtual void Compose () {

            BoldButton = new ToolStripButton { IsCheckable = true };
            BoldCommand = new ToolStripCommand {
                Action = s => ToggleAttribute<FontWeightTextAttribute> (
                    (a, bold) => a.Weight = (bold ? FontWeight.Bold : FontWeight.Normal), BoldButton),
                Image = Iconery.FontBoldIcon,
                Size = DefaultSize,
                ToolTipText = "Bold"
            };
            BoldButton.SetCommand (BoldCommand);

            ItalicButton = new ToolStripButton { IsCheckable = true };
            ItalicCommand = new ToolStripCommand {
                Action = s => ToggleAttribute<FontStyleTextAttribute> (
                    (a, italic) => a.Style = (italic ? FontStyle.Italic : FontStyle.Normal), ItalicButton),
                Image = Iconery.FontItalicIcon,
                Size = DefaultSize,
                ToolTipText = "Italic"
            };
            ItalicButton.SetCommand (ItalicCommand);

            StrikeThroughButton = new ToolStripButton { IsCheckable = true };
            StrikeThroughCommand = new ToolStripCommand {
                Action = s => ToggleAttribute<StrikethroughTextAttribute> (
                    (a, value) => a.Strikethrough = value, StrikeThroughButton),
                Image = Iconery.FontStrikeThroughIcon,
                Size = DefaultSize,
                ToolTipText = "StrikeThrough"
            };
            StrikeThroughButton.SetCommand (StrikeThroughCommand);

            UnderlineButton = new ToolStripButton { IsCheckable = true };
            UnderlineCommand = new ToolStripCommand {
                Action = s => ToggleAttribute<UnderlineTextAttribute> (
                    (a, value) => a.Underline = value, UnderlineButton),
                Image = Iconery.FontUnderlineIcon,
                Size = DefaultSize,
                ToolTipText = "Underline"
            };
            UnderlineButton.SetCommand (UnderlineCommand);

            FontFamilyCombo = new ComboBox { Width = 100 };
            Font.AvailableFontFamilies.ForEach (f =>
                FontFamilyCombo.Items.Add (f));
            FontFamilyCombo.SelectionChanged += (s, e) => {
                var attr = new FontDataAttribute { FontFamily = FontFamilyCombo.SelectedItem as string };
                TextViewer.SetAttribute (attr);
            };

            var fontFamilyComboHost = new ToolStripItemHost { Child = FontFamilyCombo };

            FontSizeCombo = new ComboBox { Width = 50 };
            new int[] { 6, 8, 10, 12, 14, 16, 18, 24, 32 }
                .ForEach (s => FontSizeCombo.Items.Add (s.ToString ()));

            FontSizeCombo.SelectionChanged += (s, e) => {
                var i = -1d;
                if (FontSizeCombo.SelectedItem != null && double.TryParse (FontSizeCombo.SelectedItem.ToString (), out i)) {
                    var attr = new FontDataAttribute { FontSize = i };
                    TextViewer.SetAttribute (attr);
                }
            };
            var fontSizeComboHost = new ToolStripItemHost { Child = FontSizeCombo };

            this.AddItems (BoldButton, ItalicButton, UnderlineButton, StrikeThroughButton,
                fontFamilyComboHost, fontSizeComboHost);
        }

        private void ToggleAttribute<T> (Action<T, bool> toggle, ToolStripButton button) where T : TextAttribute, new() {
            var attr = new T ();
            var isChecked = button.IsChecked.HasValue && button.IsChecked.Value;
            toggle (attr, isChecked);
            TextViewer.SetAttribute (attr);
        }
        
        public virtual void Attach (TextViewer viewer) {
            this.TextViewer = viewer;
            this.TextViewer.SelectionChanged -= TextViewer_SelectionChanged;
            this.TextViewer.SelectionChanged += TextViewer_SelectionChanged;
            SelectionChanged ();
        }

        void TextViewer_SelectionChanged (object sender, EventArgs e) {
            SelectionChanged ();
        }

        public virtual void SelectionChanged () {

            Action<ToolStripButton, Action> changeButton = (b, a) => {
                var s = b.Action;
                b.Action = null;
                a ();
                b.Action = s;
            };

            var visit = new TextAttributeVisitor {

                FontDataAttribute = attribute => {
                    if (!string.IsNullOrEmpty (attribute.FontFamily))
                        FontFamilyCombo.SelectedItem = attribute.FontFamily;
                    if (attribute.FontSize > 0)
                       FontSizeCombo.SelectedItem = attribute.FontSize.ToString() ;
                },
  
                FontWeightTextAttribute = attribute => 
                    changeButton (BoldButton, () => BoldButton.IsChecked = attribute.Weight == FontWeight.Bold),

                FontStyleTextAttribute = attribute => 
                    changeButton (ItalicButton, () => ItalicButton.IsChecked = attribute.Style == FontStyle.Italic),

                StrikethroughTextAttribute = attribute =>   
                    changeButton (StrikeThroughButton, () => StrikeThroughButton.IsChecked = attribute.Strikethrough),

                UnderlineTextAttribute = attribute =>   
                    changeButton (UnderlineButton, () => UnderlineButton.IsChecked = attribute.Underline),

                BackgroundTextAttribute = attribute => { },
                ColorTextAttribute = attribute => { }
            };

            visit.Visit (TextViewer.GetAttributes ());
        }
    }
}