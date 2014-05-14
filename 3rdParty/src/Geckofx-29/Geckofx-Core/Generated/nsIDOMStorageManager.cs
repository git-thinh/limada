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
// Generated by IDLImporter from file nsIDOMStorageManager.idl
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
    /// General purpose interface that has two implementations, for localStorage
    /// resp. sessionStorage with "@mozilla.org/dom/localStorage-manager;1" resp.
    /// "@mozilla.org/dom/sessionStorage-manager;1" contract IDs.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8096f9ea-fa61-4960-b5d7-fb30ac42c8d8")]
	public interface nsIDOMStorageManager
	{
		
		/// <summary>
        /// This starts async preloading of a storage cache for scope
        /// defined by the principal.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PrecacheStorage([MarshalAs(UnmanagedType.Interface)] nsIPrincipal aPrincipal);
		
		/// <summary>
        /// Returns instance of DOM storage object for given principal.
        /// A new object is always returned and it is ensured there is
        /// a storage for the scope created.
        ///
        /// @param aPrincipal
        /// Principal to bound storage to.
        /// @param aDocumentURI
        /// URL of the demanding document, used for DOM storage event only.
        /// @param aPrivate
        /// Whether the demanding document is running in Private Browsing mode or not.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMStorage CreateStorage([MarshalAs(UnmanagedType.Interface)] nsIPrincipal aPrincipal, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aDocumentURI, [MarshalAs(UnmanagedType.U1)] bool aPrivate);
		
		/// <summary>
        /// Returns instance of DOM storage object for given principal.
        /// If there is no storage managed for the scope, then null is returned and
        /// no object is created.  Otherwise, an object (new) for the existing storage
        /// scope is returned.
        ///
        /// @param aPrincipal
        /// Principal to bound storage to.
        /// @param aPrivate
        /// Whether the demanding document is running in Private Browsing mode or not.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMStorage GetStorage([MarshalAs(UnmanagedType.Interface)] nsIPrincipal aPrincipal, [MarshalAs(UnmanagedType.U1)] bool aPrivate);
		
		/// <summary>
        /// Clones given storage into this storage manager.
        ///
        /// @param aStorageToCloneFrom
        /// The storage to copy all items from into this manager.  Manager will then
        /// return a new and independent object that contains snapshot of data from
        /// the moment this method was called.  Modification to this new object will
        /// not affect the original storage content we cloned from and vice versa.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CloneStorage([MarshalAs(UnmanagedType.Interface)] nsIDOMStorage aStorageToCloneFrom);
		
		/// <summary>
        /// Returns true if the storage belongs to the given principal and is managed
        /// (i.e. has been created and is cached) by this storage manager.
        ///
        /// @param aPrincipal
        /// Principal to check the storage against.
        /// @param aStorage
        /// The storage object to examine.
        ///
        /// @result
        /// true when the storage object is bound with the principal and is managed
        /// by this storage manager.
        /// false otherwise
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool CheckStorage([MarshalAs(UnmanagedType.Interface)] nsIPrincipal aPrincipal, [MarshalAs(UnmanagedType.Interface)] nsIDOMStorage aStorage);
		
		/// <summary>
        /// @deprecated
        ///
        /// Returns instance of localStorage object for aURI's origin.
        /// This method ensures there is always only a single instance
        /// for a single origin.
        ///
        /// Currently just forwards to the createStorage method of this
        /// interface.
        ///
        /// Extension developers are strongly encouraged to use getStorage
        /// or createStorage method instead.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMStorage GetLocalStorageForPrincipal([MarshalAs(UnmanagedType.Interface)] nsIPrincipal aPrincipal, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aDocumentURI, [MarshalAs(UnmanagedType.U1)] bool aPrivate);
	}
}
