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

using Limaki.View.Vidgets;
using System;
using System.IO;
using System.Threading;
using Xwt;

namespace Limaki.View.XwtBackend {

    public class WebViewBackend : VidgetBackend<WebViewBackend.WebView>, IWebBrowserBackend, IHistoryAware {

        public class WebView : Xwt.WebView, IHistoryAware {

            bool IHistoryAware.CanGoBack {
                get { return this.CanGoBack; }
            }

            bool IHistoryAware.CanGoForward {
                get { return this.CanGoForward; }
            }

            bool IHistoryAware.GoBack () {
                this.GoBack ();
                return true;
            }

            bool IHistoryAware.GoForward () {
                this.GoForward ();
                return true;
            }

            void IHistoryAware.GoHome () {
                this.Url = "about:blank";
            }
        }

        public new Vidgets.WebBrowserVidget Frontend { get; protected set; }

        public override void InitializeBackend (IVidget frontend, VidgetApplicationContext context) {
            base.InitializeBackend (frontend, context);
            Frontend = (Vidgets.WebBrowserVidget) frontend;
            Widget.Loaded += Widget_Loaded;
        }

        public Stream DocumentStream {
            get { throw new NotImplementedException (); }
            set {
                value.Position = 0;
                var reader = new StreamReader (value);
                string text = reader.ReadToEnd ();
                _loading = true;
                Widget.LoadHtml (text, "");
                value.Position = 0;
            }
        }

        public string DocumentText { 
			get { return "<html><body>Not Supported</body></html>"; }
            set {
                _loading = true;
                Widget.LoadHtml (value, "");
            } 
		}

        public string Url {
            get { return Widget.Url; }
            set {
                _loading = true;
                Widget.Url = value;
            }
        }

        public void Navigate (string urlString) {
            Widget.Url = urlString;
        }

        private bool _loading = false;

        public void WaitFor (System.Func<bool> done) {
            //int i = 0;
            //while (!done () && i < 10) {
            //    Thread.Sleep (5);
            //    i++;
            //}
            Application.MainLoop.DispatchPendingEvents ();
        }

        public void WaitLoaded () {
            while (_loading) {
                Application.MainLoop.DispatchPendingEvents ();
            }
        }

        public void MakeReady () {
            if (_loading)
                Widget.StopLoading ();
            Application.MainLoop.DispatchPendingEvents ();
        }

        public void Widget_Loaded (object sender, EventArgs e) {
            _loading = false;
        }

        public bool CanGoBack { get { return Widget.CanGoBack; } }

        public bool CanGoForward { get { return Widget.CanGoForward; } }

        public bool GoBack () {
            Widget.GoBack ();
            return true;
        }

        public bool GoForward () {
            Widget.GoForward ();
            return true;
        }

        public void GoHome () {
            Widget.Url = "about:blank";
        }

        public void Clear () {
            Widget.Url = "about:blank";
        }

    }
}