﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using Gecko.Listeners;
using Gecko.Windows;
using Gecko.Interop;

// PLZ keep all Windows Forms related code here
namespace Gecko
{
	partial class GeckoWebBrowser
		: Control
	{
		#region Overridden Properties

		[Browsable( false ), DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
		public override Color BackColor
		{
			get { return base.BackColor; }
			set { base.BackColor = value; }
		}

		[Browsable( false ), DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
		public override Image BackgroundImage
		{
			get { return base.BackgroundImage; }
			set { base.BackgroundImage = value; }
		}

		[Browsable( false ), DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
		public override ImageLayout BackgroundImageLayout
		{
			get { return base.BackgroundImageLayout; }
			set { base.BackgroundImageLayout = value; }
		}

		[Browsable( false ), DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
		public override Color ForeColor
		{
			get { return base.ForeColor; }
			set { base.ForeColor = value; }
		}

		[Browsable( false ), DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
		public override Font Font
		{
			get { return base.Font; }
			set { base.Font = value; }
		}

		[Browsable( false ), DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
		public override string Text
		{
			get { return base.Text; }
			set { base.Text = value; }
		}

		#endregion

		#region Overriden WinForms functions

		protected virtual IEnumerable<string> DefaultEvents
		{
			get
			{
				yield return "submit";
				yield return "keydown";
				yield return "keyup";
				yield return "keypress";
				yield return "mousemove";
				yield return "mouseover";
				yield return "mouseout";
				yield return "mousedown";
				yield return "mouseup";
				yield return "click";
				yield return "dblclick";
				yield return "compositionstart";
				yield return "compositionend";
				yield return "contextmenu";
				yield return "DOMMouseScroll";
				yield return "focus";
				yield return "blur";
				// Load event added here rather than DOMDocument as DOMDocument recreated when navigating
				// ths losing attached listener.
				yield return "load";
				yield return "DOMContentLoaded";
				yield return "readystatechange";
				yield return "change";
				yield return "hashchange";
				yield return "dragstart";
				yield return "dragleave";
				yield return "drag";
				yield return "drop";
				yield return "dragend";

			}
		}

		protected ComPtr<nsIDOMEventTarget> EventTarget { get; private set; }

		protected override void OnHandleCreated( EventArgs e )
		{
#if GTK	
			if (Xpcom.IsMono)
			{
				base.OnHandleCreated(e);
				m_wrapper.Init();
			}
#endif
			if ( !this.DesignMode )
			{
				Xpcom.Initialize();
				WindowCreator.Register();
#if !GTK
				LauncherDialogFactory.Register();
#endif

				WebBrowser = Xpcom.CreateInstance<nsIWebBrowser>(Contracts.WebBrowser);
				WebBrowserFocus = ( nsIWebBrowserFocus ) WebBrowser;
				BaseWindow = ( nsIBaseWindow ) WebBrowser;
				WebNav = ( nsIWebNavigation ) WebBrowser;

				WebBrowser.SetContainerWindowAttribute( this );
#if GTK
				if (Xpcom.IsMono)
					BaseWindow.InitWindow(m_wrapper.BrowserWindow.Handle, IntPtr.Zero, 0, 0, this.Width, this.Height);
				else
#endif
				BaseWindow.InitWindow( this.Handle, IntPtr.Zero, 0, 0, this.Width, this.Height );

				
				BaseWindow.Create();

				Guid nsIWebProgressListenerGUID = typeof (nsIWebProgressListener).GUID;
				Guid nsIWebProgressListener2GUID = typeof (nsIWebProgressListener2).GUID;
				WebBrowser.AddWebBrowserListener( this.GetWeakReference(), ref nsIWebProgressListenerGUID );
				WebBrowser.AddWebBrowserListener( this.GetWeakReference(), ref nsIWebProgressListener2GUID );

				if ( UseHttpActivityObserver )
				{
					ObserverService.AddObserver( this, ObserverNotifications.HttpRequests.HttpOnModifyRequest, false );
					Net.HttpActivityDistributor.AddObserver(this);
				}

				// var domEventListener = new GeckoDOMEventListener(this);

				{
					var domWindow = WebBrowser.GetContentDOMWindowAttribute();
					EventTarget = domWindow.GetWindowRootAttribute().AsComPtr();
					Marshal.ReleaseComObject(domWindow);
				}

				foreach (string sEventName in this.DefaultEvents)
				{
					using (var eventType = new nsAString(sEventName))
					{
						EventTarget.Instance.AddEventListener(eventType, this, true, true, 2);
					}
				}

				// history
				{
					var sessionHistory = WebNav.GetSessionHistoryAttribute();
					if ( sessionHistory != null ) sessionHistory.AddSHistoryListener( this );
				}

				BaseWindow.SetVisibilityAttribute( true );

				// this fix prevents the browser from crashing if the first page loaded is invalid (missing file, invalid URL, etc)
				if ( Document != null )
				{
					// only for html documents
					Document.Cookie = "";
				}
			}

			base.OnHandleCreated( e );
		}

		protected override void OnHandleDestroyed( EventArgs e )
		{
			if (BaseWindow != null)
			{
				this.Stop();

				nsIDocShell docShell = Xpcom.QueryInterface<nsIDocShell>(BaseWindow);
				if (docShell != null && !docShell.IsBeingDestroyed())
				{
					try
					{
						var window = Xpcom.QueryInterface<nsIDOMWindow>(docShell);
						if (window != null)
						{
							try
							{
								if (!window.GetClosedAttribute()) window.Close();
							}
							finally
							{
								Xpcom.FreeComObject(ref window);
							}
						}
					}
					finally
					{
						Xpcom.FreeComObject(ref docShell);
					}
				}
				
				if (EventTarget != null)
				{
					//Remove Event Listener			
					foreach (string sEventType in this.DefaultEvents)
					{
						using (var eventType = new nsAString(sEventType))
						{
							EventTarget.Instance.RemoveEventListener(eventType, this, true);
						}
					}
					EventTarget.Dispose();
					EventTarget = null;
				}

				BaseWindow.Destroy();

				Xpcom.FreeComObject(ref CommandParams);

				var webBrowserFocus = this.WebBrowserFocus;
				this.WebBrowserFocus = null;
				Xpcom.FreeComObject(ref webBrowserFocus);
				Xpcom.FreeComObject(ref WebNav);
				Xpcom.FreeComObject(ref BaseWindow);
				Xpcom.FreeComObject(ref WebBrowser);
#if GTK			
				if (m_wrapper != null)
					m_wrapper.Dispose();
#endif
			}

			base.OnHandleDestroyed( e );
		}

		protected override void OnEnter( EventArgs e )
		{
			if ( WebBrowserFocus != null )
				WebBrowserFocus.Activate();
#if GTK
			m_wrapper.SetInputFocus();		
#endif
			base.OnEnter( e );
		}

		protected override void OnLeave( EventArgs e )
		{
			if ( WebBrowserFocus != null && !IsBusy )
				WebBrowserFocus.Deactivate();
#if GTK
			m_wrapper.RemoveInputFocus();		
#endif
			base.OnLeave( e );
		}

		protected override void OnSizeChanged( EventArgs e )
		{
			if ( BaseWindow != null )
			{
				BaseWindow.SetPositionAndSize( 0, 0, ClientSize.Width, ClientSize.Height, true );
			}

			base.OnSizeChanged( e );
		}

		#region protected override void WndProc(ref Message m)

		protected override void WndProc( ref Message m )
		{
			const int WM_GETDLGCODE = 0x87;
			const int DLGC_WANTALLKEYS = 0x4;
			const int WM_MOUSEACTIVATE = 0x21;
			const int MA_ACTIVATE = 0x1;
			const int WM_IME_SETCONTEXT = 0x0281;
			const int WM_PAINT = 0x000F;
			const int WM_SETFOCUS = 0x0007;
			const int WM_KILLFOCUS = 0x0008;


			const int ISC_SHOWUICOMPOSITIONWINDOW = unchecked ((int)0x80000000);
			if ( !DesignMode )
			{
				IntPtr focus;
				switch ( m.Msg )
				{
					case WM_GETDLGCODE:
						m.Result = ( IntPtr ) DLGC_WANTALLKEYS;
						return;
					case WM_SETFOCUS:
						break;
					case WM_MOUSEACTIVATE:
						// TODO FIXME: port for Linux
						if ( Xpcom.IsWindows )
						{
							m.Result = ( IntPtr ) MA_ACTIVATE;
							focus = User32.GetFocus();
							// Console.WriteLine( "focus {0:X8}, Handle {1:X8}", focus.ToInt32(), Handle.ToInt32() );
							if ( !IsSubWindow( Handle, focus ) )
							{
							//	var str = string.Format( "+WM_MOUSEACTIVATE {0:X8} lastfocus", focus.ToInt32() );
							//	System.Diagnostics.Debug.WriteLine( str );
								Console.WriteLine("Activating");
								if ( WebBrowserFocus != null )
								{
									WebBrowserFocus.Activate();
									Services.WindowWatcher.ActiveWindow = this.Window;
								}
							}
							else
							{
							//	var str = string.Format( "-WM_MOUSEACTIVATE {0:X8} lastfocus", focus.ToInt32() );
							//	System.Diagnostics.Debug.WriteLine( str );
							}
							if ( !this.Window.Equals(Services.WindowWatcher.ActiveWindow) )
							{
								if ( WebBrowserFocus != null )
								{
									WebBrowserFocus.Activate();
									Services.WindowWatcher.ActiveWindow = this.Window;
								}
							}
							return;
						}
						return;
						
					//http://msdn.microsoft.com/en-US/library/windows/desktop/dd374142%28v=vs.85%29.aspx
					case WM_IME_SETCONTEXT:
						focus = User32.GetFocus();
						if ( User32.IsChild( Handle, focus ) )
						{
							break;
						}

						if ( WebBrowserFocus != null )
						{
						//	var str = string.Format( "WM_IME_SETCONTEXT {0} {1} {2} (focus on {3})", m.HWnd.ToString( "X8" ), m.WParam, m.LParam.ToString( "X8" ), focus.ToString( "X8" ) );
						//	System.Diagnostics.Debug.WriteLine( str );


							var param = m.LParam.ToInt32();
							if ( ( param & ISC_SHOWUICOMPOSITIONWINDOW ) != 0 )
							{

							}
							if ( m.WParam == IntPtr.Zero )
							{
								// zero
								RemoveInputFocus();
								WebBrowserFocus.Deactivate();
							}
							else
							{
								// non-zero (1)
								WebBrowserFocus.Activate();
								SetInputFocus();
							}
							return;
						}

						break;
					case WM_PAINT:
						break;
				}
			}
			
			// Firefox 17+ can crash when handing this windows message so we just ignore it.
			if (m.Msg == 0x128 /*WM_UPDATEUISTATE*/)
				return;

			base.WndProc( ref m );
		}

		private bool IsSubWindow( IntPtr window, IntPtr candidate )
		{
			// search parent until desktop (flash window is owned by other process)
			while ( candidate!=IntPtr.Zero )
			{
				candidate = User32.GetParent( candidate );
				if ( window == candidate )
				{
					return true;
				}
			}
			return false;
		}

		#endregion

		protected override void OnPaint( PaintEventArgs e )
		{
			if ( this.DesignMode )
			{
				string versionString =
					( ( AssemblyFileVersionAttribute )
					  Attribute.GetCustomAttribute( GetType().Assembly, typeof (AssemblyFileVersionAttribute) ) ).Version;

				using (
					Brush brush = new System.Drawing.Drawing2D.HatchBrush( System.Drawing.Drawing2D.HatchStyle.SolidDiamond,
					                                                       Color.FromArgb( 240, 240, 240 ), Color.White ) )
					e.Graphics.FillRectangle( brush, this.ClientRectangle );

				e.Graphics.DrawString(
					string.Format( "GeckoFX v{0}\r\n" + "http://bitbucket.org/geckofx/", versionString ),
					SystemFonts.MessageBoxFont,
					Brushes.Black,
					new RectangleF( 2, 2, this.Width - 4, this.Height - 4 ) );
				e.Graphics.DrawRectangle( SystemPens.ControlDark, 0, 0, Width - 1, Height - 1 );
			}
			base.OnPaint( e );
		}

        protected override void OnPrint(PaintEventArgs e)
        {
            base.OnPrint(e);
               
            ImageCreator creator = new ImageCreator(this);
            byte[] mBytes = creator.CanvasGetPngImage((uint)0, (uint)0, (uint)this.Width, (uint)this.Height);
            using (Image image = Image.FromStream(new System.IO.MemoryStream(mBytes)))
            {
                e.Graphics.DrawImage(image, 0.0f, 0.0f);
            }
        }

		#endregion

		#region Internal classes

		#region class ToolTipWindow

		/// <summary>
		/// A window to contain a tool tip.
		/// </summary>
		private class ToolTipWindow : ToolTip
		{
		}

		#endregion

		#endregion


		public void ForceRedraw()
		{
			BaseWindow.Repaint( true );
		}


		#region UserInterfaceThreadInvoke
		/// <summary>
		/// UI platform independent call function from UI thread
		/// </summary>
		/// <param name="action"></param>
		public void UserInterfaceThreadInvoke( Action action )
		{
			if ( InvokeRequired )
			{
				Invoke( new Action( () => SafeAction( action ) ) );
			}
			else
			{
				SafeAction( action );
			}
		}

		/// <summary>
		/// Exception handler for action
		/// </summary>
		/// <param name="action"></param>
		private void SafeAction( Action action )
		{
			try
			{
				action();
			}
			catch ( Exception e )
			{
				System.Diagnostics.Debug.WriteLine( string.Format( "Invoking exception" ) );
			}
		}

		/// <summary>
		/// UI platform independent call function from UI thread
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		public T UserInterfaceThreadInvoke<T>( Func<T> func )
		{
			if ( InvokeRequired )
			{
				return ( T ) Invoke( new Func<T>( () => SafeFunc( func ) ) );
			}
			return SafeFunc( func );
		}

		/// <summary>
		/// exception handler for function
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="func"></param>
		/// <returns></returns>
		private T SafeFunc<T>( Func<T> func )
		{
			T ret = default( T );
			try
			{
				ret = func();
			}
			catch ( Exception e )
			{
				System.Diagnostics.Debug.WriteLine( string.Format( "Invoking exception" ) );
			}
			return ret;
		}
		#endregion
	}
}