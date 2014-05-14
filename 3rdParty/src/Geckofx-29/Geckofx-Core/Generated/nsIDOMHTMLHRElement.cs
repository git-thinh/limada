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
// Generated by IDLImporter from file nsIDOMHTMLHRElement.idl
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
    /// The nsIDOMHTMLHRElement interface is the interface to a [X]HTML hr
    /// element.
    ///
    /// This interface is trying to follow the DOM Level 2 HTML specification:
    /// http://www.w3.org/TR/DOM-Level-2-HTML/
    ///
    /// with changes from the work-in-progress WHATWG HTML specification:
    /// http://www.whatwg.org/specs/web-apps/current-work/
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("30771953-b9f4-44de-b0fe-e490949af98b")]
	public interface nsIDOMHTMLHRElement
	{
		
		/// <summary>
        /// The nsIDOMHTMLHRElement interface is the interface to a [X]HTML hr
        /// element.
        ///
        /// This interface is trying to follow the DOM Level 2 HTML specification:
        /// http://www.w3.org/TR/DOM-Level-2-HTML/
        ///
        /// with changes from the work-in-progress WHATWG HTML specification:
        /// http://www.whatwg.org/specs/web-apps/current-work/
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetAlignAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAlign);
		
		/// <summary>
        /// The nsIDOMHTMLHRElement interface is the interface to a [X]HTML hr
        /// element.
        ///
        /// This interface is trying to follow the DOM Level 2 HTML specification:
        /// http://www.w3.org/TR/DOM-Level-2-HTML/
        ///
        /// with changes from the work-in-progress WHATWG HTML specification:
        /// http://www.whatwg.org/specs/web-apps/current-work/
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAlignAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aAlign);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetNoShadeAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetNoShadeAttribute([MarshalAs(UnmanagedType.U1)] bool aNoShade);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetSizeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aSize);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSizeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aSize);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetWidthAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aWidth);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetWidthAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aWidth);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetColorAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aColor);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetColorAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aColor);
	}
}
