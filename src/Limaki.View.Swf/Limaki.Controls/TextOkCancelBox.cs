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
 */


using System;
using System.Windows.Forms;
using Limaki.Common;
using Limaki.Drawing;
using Limaki.View;
using Limaki.Viewers;
using Xwt;
using Xwt.Gdi;
using DialogResult=Limaki.Viewers.DialogResult;
using Limaki.Drawing.Gdi;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Xwt.Gdi.Backend;

namespace Limaki.Swf.Backends {
    public partial class TextOkCancelBox : UserControl, ITextOkCancelBox {
        public TextOkCancelBox() {
            InitializeComponent();
            ActiveControl = this.TextBox;
            Result = DialogResult.None;
        }


        public DialogResult Result { get; set; }

        private void buttonOk_Click(object sender, EventArgs e) {
            Text = TextBox.Text;
            DoFinish (DialogResult.OK);
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            DoFinish(DialogResult.Cancel);
        }

        public string Title {
            get { return nameLabel.Text; }
            set { nameLabel.Text = value; }
        }


        public Action<string> OnOk { get; set; }
        public event EventHandler<TextOkCancelBoxEventArgs> Finish = null;
        void DoFinish(DialogResult result) {
            if (Finish != null) {
                Finish(this, new TextOkCancelBoxEventArgs(result, OnOk));
            }
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return) {
                Text = TextBox.Text;
                DoFinish(DialogResult.OK);
            } else if (e.KeyCode == Keys.Escape) {
                DoFinish (DialogResult.Cancel);
            }
        }

        #region IControl Member

        Rectangle IWidgetBackend.ClientRectangle {
            get { return GdiConverter.ToXwt (this.ClientRectangle); }
        }

        Size IWidgetBackend.Size {
            get { return Limaki.Drawing.Gdi.GDIConverter.Convert (this.Size); }
        }

        void IWidgetBackend.Update() {
            this.Update();
        }

        void IWidgetBackend.Invalidate() {
            this.Invalidate();
        }

        void IWidgetBackend.Invalidate(Rectangle rect) {
            this.Invalidate(GDIConverter.Convert(rect));
        }

        Point IWidgetBackend.PointToClient(Point source) {
            return GDIConverter.Convert (this.PointToClient (GDIConverter.Convert (source)));
        }

        #endregion
    }
}