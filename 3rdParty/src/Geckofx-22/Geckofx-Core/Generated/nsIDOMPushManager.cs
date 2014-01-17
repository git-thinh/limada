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
// Generated by IDLImporter from file nsIDOMPushManager.idl
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
    /// Client API for SimplePush.
    ///
    /// The SimplePush API allows web applications to use push notifications and be
    /// woken up when something of interest has changed. This frees web applications
    /// from implementing polling, giving better responsiveness and conserving the
    /// device's battery life.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("c7ad4f42-faae-4e8b-9879-780a72349945")]
	public interface nsIDOMPushManager
	{
		
		/// <summary>
        /// Register for a new push endpoint.
        ///
        /// On success, the DOMRequest's result field will be a string URL.  This URL
        /// is the endpoint that can be contacted to wake up the application.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest Register();
		
		/// <summary>
        /// Unregister a push endpoint.
        ///
        /// On success, the DOMRequest's result field will be the endpoint that was
        /// passed in.
        ///
        /// Stops watching for changes to this URL.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest Unregister([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase endpoint);
		
		/// <summary>
        /// Get a list of active registrations for this web app.
        ///
        /// On success, the DOMRequest's result field is an array of endpoints.
        /// For example:
        /// ["https://example.com/notify/1", "https://example.com/notify/2"]
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMDOMRequest Registrations();
	}
}