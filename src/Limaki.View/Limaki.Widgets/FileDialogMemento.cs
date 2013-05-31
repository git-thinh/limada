/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2010 Lytico
 *
 * http://www.limada.org
 * 
 */

using System.ComponentModel;
using System.IO;

namespace Limaki.Viewers {
    public class FileDialogMemento {
        [DefaultValue(true)]
        public bool AddExtension { get; set; }

        [DefaultValue(true)]
        public bool AutoUpgradeEnabled { get; set; }

        [DefaultValue(false)]
        public virtual bool CheckFileExists { get; set; }

        [DefaultValue(true)]
        public bool CheckPathExists { get; set; }

        [DefaultValue("")]
        public string DefaultExt { get; set; }

        [DefaultValue(true)]
        public bool DereferenceLinks { get; set; }

        [DefaultValue("")]
        public string FileName { get; set; }

        public string[] FileNames { get; set; }

        [DefaultValue("")]
        [Localizable(true)]
        public string Filter { get; set; }

        [DefaultValue(1)]
        public int FilterIndex { get; set; }

        [DefaultValue("")]
        public string InitialDirectory { get; set; }

        [DefaultValue(false)]
        public bool RestoreDirectory { get; set; }

        [DefaultValue(false)]
        public bool ShowHelp { get; set; }

        [DefaultValue(false)]
        public bool SupportMultiDottedExtensions { get; set; }

        [DefaultValue("")]
        [Localizable(true)]
        public string Title { get; set; }

        [DefaultValue(true)]
        public bool ValidateNames { get; set; }

        public void SetFileName(string fileName) {
            this.FileName = Path.GetFileNameWithoutExtension(fileName);
            this.InitialDirectory = Path.GetDirectoryName(fileName);
            this.Filter = Path.GetExtension(fileName);
        }

        public void ResetFileName () {
            SetFileName(this.FileName);
        }
    }
}