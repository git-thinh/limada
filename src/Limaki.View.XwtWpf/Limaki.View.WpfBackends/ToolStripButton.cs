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

using Limaki.Drawing.WpfBackend;
using Limaki.View.Vidgets;
using System.Windows.Controls;
using Xwt.WPFBackend;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Controls.Primitives;
using System.Windows.Data;

namespace Limaki.View.WpfBackend {

    public class ToolStripButton : ToggleButton, IToolStripCommandItem, IToolStripItem {

        public ToolStripButton() {
            Compose();
            base.Click += OnEventClick;
        }

        protected virtual void Compose () {
            ComposeStyle ();

            this.Content = ButtonImage;
        }

        protected virtual void ComposeStyle () {
            var style = new Style ();

            // set opacity to 50% if not enabled:
            var trigger = new DataTrigger {
                Value = false,
                Binding = WpfExtensions.Binding (this, o => o.IsEnabled, BindingMode.OneWay),
            };

            var setter = new Setter {
                Property = UIElement.OpacityProperty,
                Value = 0.5,
            };

            trigger.Setters.Add (setter);
            style.Triggers.Add (trigger);

            // use ToolbarStyle as BaseStyle
            var baseStyle = (Style) FindResource (ToolBar.ToggleButtonStyleKey);
            style.TargetType = baseStyle.TargetType;
            style.BasedOn = baseStyle;
            this.Style = style;
        }


        protected ToolStripCommand _command = null;
        public new ToolStripCommand Command {
            get { return _command; }
            set { ToolStripUtils.SetCommand (this, ref _command, value); }
        }

        public IToolStripCommandItem ToggleOnClick { get; set; }

        public virtual Xwt.Size Size {
            get { return new Xwt.Size (this.Width, this.Height); }
            set { 
                //this.Width = value.Width;
                //this.Height = value.Height;
            }
        }

        protected FixedBitmap _innerButton = null;
        protected FixedBitmap ButtonImage {
            get { return _innerButton ?? (_innerButton = new FixedBitmap ()); }
        }

        protected Xwt.Drawing.Image _image = null;
        public virtual Xwt.Drawing.Image Image {
            get { return _image; }
            set {
                if (_image != value) {
                    _image = value;
                    ButtonImage.Source = _image.ToWpf() as BitmapSource;
                    ButtonImage.InvalidateMeasure ();
                    ButtonImage.InvalidateVisual ();
                    //ButtonImage.Width = _image.Width;
                }
            }
        }

        public virtual string Text { get; set; }

        public virtual string ToolTipText { get { return base.ToolTip.ToString(); } set { base.ToolTip = value; } }

        private void OnEventClick (object sender, RoutedEventArgs e) {
            if (_click != null)
                _click (this, e);
        }

        private event System.EventHandler _click;
        public virtual new event System.EventHandler Click {
            add { _click += value;  }
            remove { _click -= value; }
        }

        public bool IsCheckable { get; set; }

        protected override void OnChecked (RoutedEventArgs e) {
            if (!IsCheckable && (!IsChecked.HasValue || IsChecked.Value)) {
                IsChecked = false;
            }
            base.OnChecked (e);
        }

    }
}