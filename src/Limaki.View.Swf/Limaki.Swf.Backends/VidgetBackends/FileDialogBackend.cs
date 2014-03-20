/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2008-2014 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Windows.Forms;
using Limaki.View.Vidgets;
using Xwt;
using SWF = System.Windows.Forms;

namespace Limaki.View.Swf.Backends {

    public abstract class FileDialogBackend : IFileDialogBackend {

        protected FileDialogMemento _dialog = null;

        public virtual void Compose (FileDialogMemento dialog) { this._dialog = dialog; }

        protected virtual bool Run<T> (IVidget parent) where T : SWF.FileDialog, new() {
            if (_dialog == null)
                throw new ArgumentException();
            var fileDialog = new T();
            Converter.FileDialogSetValue(fileDialog, _dialog);
            SWF.Application.DoEvents();
            IWin32Window owner = null;
            if (parent != null) {
                var back = parent.Backend as SWF.Control;
                if (back != null)
                    owner = back.FindForm();
            }
            var result = fileDialog.ShowDialog();
            fileDialog.Dispose();
            if (result == SWF.DialogResult.OK) {
                Converter.FileDialogSetValue(_dialog, fileDialog);
                return true;
            }
            return false;
        }

        public abstract bool Run (IVidget parent);

        public virtual void Cleanup () { }

        public IFileDialogVidget Frontend { get; protected set; }

        public void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            this.Frontend = frontend as IFileDialogVidget;
        }

        public Size Size {
            get { throw new ArgumentException(); }
        }

        public void Update () { }

        public void Invalidate () { }

        public void Invalidate (Rectangle rect) { }
        public void Dispose () { }
    }

    public class OpenFileDialogBackend : FileDialogBackend, IOpenfileDialogBackend {

        public override bool Run (IVidget parent) { return base.Run<SWF.OpenFileDialog>(parent); }

    }

    public class SaveFileDialogBackend : FileDialogBackend, ISavefileDialogBackend {

        public override bool Run (IVidget parent) { return base.Run<SWF.SaveFileDialog>(parent); }

    }
}