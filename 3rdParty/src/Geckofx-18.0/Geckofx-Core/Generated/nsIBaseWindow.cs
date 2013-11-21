// --------------------------------------------------------------------------------------------
// Version: MPL 1.1/GPL 2.0/LGPL 2.1
// 
// The contents of this file are subject to the Mozilla Public License Version
// 1.1 (the "License"); you may not use this file except in compliance with
// the License. You may obtain a copy of the License at
// http://www.mozilla.org/MPL/
// 
// Software distributed under the License is distributed on an "AS IS" basis,
// WITHOUT WARRANTY OF ANY KIND, either express or implied. See the License
// for the specific language governing rights and limitations under the
// License.
// 
// <remarks>
// Generated by IDLImporter from file nsIBaseWindow.idl
// 
// You should use these interfaces when you access the COM objects defined in the mentioned
// IDL/IDH file.
// </remarks>
// --------------------------------------------------------------------------------------------
namespace Gecko
{
	using System;
	using System.Runtime.InteropServices;
	using System.Runtime.InteropServices.ComTypes;
	using System.Runtime.CompilerServices;
	
	
	/// <summary>
    /// The nsIBaseWindow describes a generic window and basic operations that
    /// can be performed on it.  This is not to be a complete windowing interface
    /// but rather a common set that nearly all windowed objects support.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9DA319F3-EEE6-4504-81A5-6A19CF6215BF")]
	public interface nsIBaseWindow
	{
		
		/// <summary>
        ///Allows a client to initialize an object implementing this interface with
        ///	the usually required window setup information.
        ///	It is possible to pass null for both parentNativeWindow and parentWidget,
        ///	but only docshells support this.
        ///	@param parentNativeWindow - This allows a system to pass in the parenting
        ///		window as a native reference rather than relying on the calling
        ///		application to have created the parent window as an nsIWidget.  This
        ///		value will be ignored (should be nullptr) if an nsIWidget is passed in to
        ///		the parentWidget parameter.
        ///	@param parentWidget - This allows a system to pass in the parenting widget.
        ///		This allows some objects to optimize themselves and rely on the view
        ///		system for event flow rather than creating numerous native windows.  If
        ///		one of these is not available, nullptr should be passed.
        ///	@param x - This is the x co-ordinate relative to the parent to place the
        ///		window.
        ///	@param y - This is the y co-ordinate relative to the parent to place the
        ///		window.
        ///	@param cx - This is the width	for the window to be.
        ///	@param cy - This is the height for the window to be.
        ///	@return	NS_OK - Window Init succeeded without a problem.
        ///				NS_ERROR_UNEXPECTED - Call was unexpected at this time.  Most likely
        ///					due to you calling it after create() has been called.
        ///				NS_ERROR_INVALID_ARG - controls that require either a parentNativeWindow
        ///					or a parentWidget may return invalid arg when they do not
        ///					receive what they are needing.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InitWindow(System.IntPtr parentNativeWindow, System.IntPtr parentWidget, int x, int y, int cx, int cy);
		
		/// <summary>
        ///Tells the window that intialization and setup is complete.  When this is
        ///	called the window can actually create itself based on the setup
        ///	information handed to it.
        ///	@return	NS_OK - Creation was successfull.
        ///				NS_ERROR_UNEXPECTED - This call was unexpected at this time.
        ///					Perhaps create() had already been called or not all
        ///					required initialization had been done.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Create();
		
		/// <summary>
        ///Tell the window that it should destroy itself.  This call should not be
        ///	necessary as it will happen implictly when final release occurs on the
        ///	object.  If for some reaons you want the window destroyed prior to release
        ///	due to cycle or ordering issues, then this call provides that ability.
        ///	@return	NS_OK - Everything destroyed properly.
        ///				NS_ERROR_UNEXPECTED - This call was unexpected at this time.
        ///					Perhaps create() has not been called yet.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Destroy();
		
		/// <summary>
        ///Sets the current x and y coordinates of the control.  This is relative to
        ///	the parent window.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPosition(int x, int y);
		
		/// <summary>
        ///Gets the current x and y coordinates of the control.  This is relatie to the
        ///	parent window.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetPosition(ref int x, ref int y);
		
		/// <summary>
        ///Sets the width and height of the control.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSize(int cx, int cy, [MarshalAs(UnmanagedType.U1)] bool fRepaint);
		
		/// <summary>
        ///Gets the width and height of the control.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetSize(ref int cx, ref int cy);
		
		/// <summary>
        ///Convenience function combining the SetPosition and SetSize into one call.
        ///	Also is more efficient than calling both.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetPositionAndSize(int x, int y, int cx, int cy, [MarshalAs(UnmanagedType.U1)] bool fRepaint);
		
		/// <summary>
        ///Convenience function combining the GetPosition and GetSize into one call.
        ///	Also is more efficient than calling both.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetPositionAndSize(ref int x, ref int y, ref int cx, ref int cy);
		
		/// <summary>
        /// Tell the window to repaint itself
        /// @param aForce - if true, repaint immediately
        /// if false, the window may defer repainting as it sees fit.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Repaint([MarshalAs(UnmanagedType.U1)] bool force);
		
		/// <summary>
        ///This is the parenting widget for the control.  This may be null if the
        ///	native window was handed in for the parent during initialization.
        ///	If this	is returned, it should refer to the same object as
        ///	parentNativeWindow.
        ///	Setting this after Create() has been called may not be supported by some
        ///	implementations.
        ///	On controls that don't support widgets, setting this will return a
        ///	NS_ERROR_NOT_IMPLEMENTED error.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetParentWidgetAttribute();
		
		/// <summary>
        ///This is the parenting widget for the control.  This may be null if the
        ///	native window was handed in for the parent during initialization.
        ///	If this	is returned, it should refer to the same object as
        ///	parentNativeWindow.
        ///	Setting this after Create() has been called may not be supported by some
        ///	implementations.
        ///	On controls that don't support widgets, setting this will return a
        ///	NS_ERROR_NOT_IMPLEMENTED error.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetParentWidgetAttribute(System.IntPtr aParentWidget);
		
		/// <summary>
        ///This is the native window parent of the control.
        ///	Setting this after Create() has been called may not be supported by some
        ///	implementations.
        ///	On controls that don't support setting nativeWindow parents, setting this
        ///	will return a NS_ERROR_NOT_IMPLEMENTED error.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetParentNativeWindowAttribute();
		
		/// <summary>
        ///This is the native window parent of the control.
        ///	Setting this after Create() has been called may not be supported by some
        ///	implementations.
        ///	On controls that don't support setting nativeWindow parents, setting this
        ///	will return a NS_ERROR_NOT_IMPLEMENTED error.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetParentNativeWindowAttribute(System.IntPtr aParentNativeWindow);
		
		/// <summary>
        ///This is the handle (HWND, GdkWindow*, ...) to the native window of the
        ///	control, exposed as a DOMString.
        ///	@return DOMString in hex format with "0x" prepended, or empty string if
        ///	mainWidget undefined
        ///	@throws NS_ERROR_NOT_IMPLEMENTED for non-XULWindows
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetNativeHandleAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aNativeHandle);
		
		/// <summary>
        ///Attribute controls the visibility of the object behind this interface.
        ///	Setting this attribute to false will hide the control.  Setting it to
        ///	true will show it.
        ///	 </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetVisibilityAttribute();
		
		/// <summary>
        ///Attribute controls the visibility of the object behind this interface.
        ///	Setting this attribute to false will hide the control.  Setting it to
        ///	true will show it.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetVisibilityAttribute([MarshalAs(UnmanagedType.U1)] bool aVisibility);
		
		/// <summary>
        ///a disabled window should accept no user interaction; it's a dead window,
        ///    like the parent of a modal window.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetEnabledAttribute();
		
		/// <summary>
        ///a disabled window should accept no user interaction; it's a dead window,
        ///    like the parent of a modal window.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetEnabledAttribute([MarshalAs(UnmanagedType.U1)] bool aEnabled);
		
		/// <summary>
        ///Allows you to find out what the widget is of a given object.  Depending
        ///	on the object, this may return the parent widget in which this object
        ///	lives if it has not had to create its own widget.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetMainWidgetAttribute();
		
		/// <summary>
        ///The number of	device pixels per CSS pixel used on this window's current
        ///	screen at the default zoom level.
        ///	This is the value returned by GetDefaultScale() of the underlying widget.
        ///	Note that this may change if the window is moved between screens with
        ///	differing resolutions.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		double GetUnscaledDevicePixelsPerCSSPixelAttribute();
		
		/// <summary>
        /// Give the window focus.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetFocus();
		
		/// <summary>
        ///Title of the window.
        ///	 </summary>
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string GetTitleAttribute();
		
		/// <summary>
        ///Title of the window.
        ///	 </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetTitleAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string aTitle);
	}
}
