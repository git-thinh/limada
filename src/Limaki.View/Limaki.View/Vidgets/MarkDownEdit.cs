﻿/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2015 Lytico
 *
 * http://www.limada.org
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Limaki.View;
using Xwt.Backends;
using Limaki.Common;

namespace Limaki.View.Vidgets {

    public interface IMarkdownViewer {
        void Load (Stream stream);
        void Clear ();
    }

    public interface IMarkdownEditor : IMarkdownViewer {
        void Save (Stream stream);
    }


    public interface IMarkdownEditBackend : IVidgetBackend {
        bool IsEmpty { get; }
        void Activate (IMarkdownViewer viewer);
        void Compose (IMarkdownViewer viewer);
    }


    public interface IMarkdownEdit : IVidget {
        bool InEdit { get; set; }
        void Save (Stream stream);
        void Load (Stream stream);
        void Clear ();
    }

    public class MarkdownPlainTextView : PlainTextBox, IMarkdownEditor {

        void IMarkdownEditor.Save (Stream stream) {
            stream.Position = 0;
            stream.SetLength (0);
            var writer = new StreamWriter (stream);
            writer.Write (Backend.Text);
            writer.Flush ();
            stream.Position = 0;
        }

        void IMarkdownViewer.Load (Stream stream) {
            var reader = new StreamReader (stream);
            base.Text = reader.ReadToEnd ();
            stream.Position = 0;

        }

        void IMarkdownViewer.Clear () {
            base.Text = string.Empty;
        }
    }

    public class MarkdownViewer : WebBrowserVidget, IMarkdownViewer {

        void IMarkdownViewer.Load (Stream stream) {
            string html = null;
            stream.Position = 0;
            var reader = new StreamReader (stream);
            var s = reader.ReadToEnd ();
            var c = CommonMark.CommonMarkConverter.Convert (s);
            html = "<html><body>" + c + "</body></html>";
            stream.Position = 0;
            base.DocumentText = html;
        }

        void IMarkdownViewer.Clear () {
            base.Clear ();
        }
    }

    [BackendType (typeof (IMarkdownEditBackend))]
    public class MarkdownEdit : Vidget, IMarkdownEdit {

        IMarkdownEditBackend _backend = null;
        public virtual new IMarkdownEditBackend Backend {
            get {
                if (_backend == null) {
                    _backend = BackendHost.Backend as IMarkdownEditBackend;
                }
                return _backend;
            }
            set { _backend = value; }
        }

        IMarkdownEditor _editor = null;
        public IMarkdownEditor Editor {
            get {
                if (_editor == null) {
                    _editor = new MarkdownPlainTextView { ShowFrame = false };
                    Backend.Compose (_editor);
                }
                return _editor;
            }
        }

        IMarkdownViewer _viewer = null;
        public IMarkdownViewer Viewer {
            get {
                if (_viewer == null) {
                    _viewer = new MarkdownViewer ();
                    Backend.Compose (_viewer);
                }
                return _viewer;
            }
        }

        public Stream Markdown { get; set; }

        bool _inEdit = false;
        public bool InEdit {
            get { return _inEdit; }
            set {
                if (value && !_inEdit)
                    StartEdit ();
                else if(_inEdit)
                    EndEdit ();
                _inEdit = value;
            }
        }

        public void StartEdit () {
            Backend.Activate (Editor);
            Editor.Load (this.Markdown);
            _inEdit = true;
        }

        public void EndEdit () {
            Editor.Save (this.Markdown);
            Viewer.Load (this.Markdown);
            _inEdit = false;
            Backend.Activate (Viewer);
        }

        public void Save (Stream stream) {
            if (Editor != null) {
                Editor.Save (this.Markdown);
            }
            if (stream != this.Markdown) {
                this.Markdown.CopyTo (stream);
            }
        }

        public void Load (Stream stream) {
            this.Markdown = new MemoryStream();
            stream.CopyTo (this.Markdown);
            this.Markdown.Position = 0;
            if (Backend.IsEmpty)
                Backend.Activate (Viewer);
            Viewer.Load (stream);
            Editor.Load (stream);
        }

        public void Clear () {
            this.Markdown = null;
            Editor.Clear ();
            Viewer.Clear ();
        }

        public override void Dispose () {
            var disp = Editor as IDisposable;
            if (disp != null)
                disp.Dispose ();
            disp = Viewer as IDisposable;
            if (disp != null)
                disp.Dispose ();
        }

    }
}