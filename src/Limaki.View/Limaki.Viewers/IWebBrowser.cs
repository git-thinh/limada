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
 * http://www.limada.org
 */

#define nowinform
using Limaki.View;
using System;
using System.ComponentModel;
using System.IO;
using System.Net;

namespace Limaki.Viewers {
    public interface IWebBrowser {
        [DefaultValue(true)]
        bool AllowNavigation { get; set; }

        [DefaultValue(true)]
        bool AllowWebBrowserDrop { get; set; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        bool CanGoBack { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        bool CanGoForward { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        Stream DocumentStream { get; set; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        string DocumentText { get; set; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        string DocumentTitle { get;  }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        string DocumentType { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        bool IsBusy { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        bool IsOffline { get; }

        [BrowsableAttribute(false)]
        [DesignerSerializationVisibilityAttribute(DesignerSerializationVisibility.Hidden)]
        string StatusText { get; }

        [BindableAttribute(true)]
        [DefaultValue(null)]
        //[TypeConverter(typeof(WebBrowserUriTypeConverter))]
            Uri Url { get; set; }

        bool GoBack();
        bool GoForward();
        void GoHome();
        void MakeReady();
        void Navigate(string urlString);
        void Navigate(Uri url);
        void Navigate(string urlString, bool newWindow);
        void Navigate(string urlString, string targetFrameName);
        void Navigate(Uri url, bool newWindow);
        void Navigate(Uri url, string targetFrameName);
        void Navigate(string urlString, string targetFrameName, byte[] postData, string additionalHeaders);
        void Navigate(Uri url, string targetFrameName, byte[] postData, string additionalHeaders);
        void Refresh();

        void AfterNavigate (Func<bool> done);

        void Stop();
        void GoSearch();
        void ShowPageSetupDialog();
        void ShowPrintPreviewDialog();
        void ShowPropertiesDialog();
        void ShowSaveAsDialog();

        [BrowsableAttribute(false)]
        event EventHandler CanGoBackChanged;

        [BrowsableAttribute(false)]
        event EventHandler CanGoForwardChanged;

        [BrowsableAttribute(false)]
        event EventHandler DocumentTitleChanged;

        event EventHandler FileDownload;
        
        event CancelEventHandler NewWindow;

        [BrowsableAttribute(false)]
        event EventHandler StatusTextChanged;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        event EventHandler PaddingChanged;


    }

    public interface IWebBrowserBackend : IWebBrowser, IVidgetBackend {
        
    }

    public interface IWebBrowserWithProxy {
        void SetProxy(IPAddress adress, int port, object webBrowser);
    }

    public interface IGeckoWebBrowserBackend : IWebBrowserBackend, IWebBrowserWithProxy { }
}