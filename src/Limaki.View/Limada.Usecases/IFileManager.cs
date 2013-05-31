/*
 * Limada 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2010-2013 Lytico
 *
 * http://www.limada.org
 */

using System;
using Limaki.Drawing;
using Limaki.Viewers;
using Limaki.Visuals;

namespace Limada.Usecases {

    public interface IFileManager {

        FileDialogMemento OpenFileDialog { get; set; }
        FileDialogMemento SaveFileDialog { get; set; }
        Func<FileDialogMemento, bool, DialogResult> FileDialogShow { get; set; }
        Func<string, string, MessageBoxButtons, DialogResult> MessageBoxShow { get; set; }

        Action<string, int, int> Progress { get; set; }

        Action ApplicationQuit { get; set; }

        Action<IGraphScene<IVisual, IVisualEdge>> DataBound { get; set; }
        Action<string> DataPostProcess { get; set; }

        void OpenFile ();

        bool OpenCommandLineOptions ();
        bool OpenCommandLine ();

        void ShowEmptyThingGraph ();

        void ExportAsThingGraph (IGraphScene<IVisual, IVisualEdge> graphScene);
        void ImportThingGraphRaw ();

        void Save ();
        void SaveAsFile ();

        void Close ();
        
    }
}