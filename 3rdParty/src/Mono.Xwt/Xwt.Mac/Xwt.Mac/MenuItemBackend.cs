// 
// MenuItemBackend.cs
//  
// Author:
//       Lluis Sanchez <lluis@xamarin.com>
// 
// Copyright (c) 2011 Xamarin Inc
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using Xwt.Backends;
using MonoMac.AppKit;

namespace Xwt.Mac
{
	public class MenuItemBackend: NSMenuItem, IMenuItemBackend
	{
		public void Initialize (IMenuItemEventSink eventSink)
		{
		}

		public void SetSubmenu (IMenuBackend menu)
		{
			if (menu == null)
				Submenu = null;
			else
				Submenu = ((MenuBackend)menu);
		}

		public string Label {
			get {
				return Title;
			}
			set {
				Title = value;
			}
		}
		
		public void SetType (MenuItemType type)
		{
			throw new NotImplementedException ();
		}
		
		public void SetImage (object imageBackend)
		{
			throw new NotImplementedException ();
		}
		
		public bool Visible {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public bool Sensitive {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public bool Checked {
			get {
				throw new NotImplementedException ();
			}
			set {
				throw new NotImplementedException ();
			}
		}
		
		public void SetSeparator ()
		{
			throw new NotImplementedException ();
		}

		#region IBackend implementation
		public void Initialize (object frontend)
		{
		}

		public void EnableEvent (object eventId)
		{
		}

		public void DisableEvent (object eventId)
		{
		}
		#endregion
	}
}

