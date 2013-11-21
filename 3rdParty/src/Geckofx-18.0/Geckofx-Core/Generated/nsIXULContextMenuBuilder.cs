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
// Generated by IDLImporter from file nsIXULContextMenuBuilder.idl
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
    /// An interface for initialization of XUL context menu builder
    /// and for triggering of menuitem actions with assigned identifiers.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("eb6b42c0-2f1c-4760-b5ca-bdc9b3ec77d4")]
	public interface nsIXULContextMenuBuilder
	{
		
		/// <summary>
        /// Initialize builder before building.
        ///
        /// @param aDocumentFragment the fragment that will be used to append top
        /// level elements
        ///
        /// @param aGeneratedItemIdAttrName the name of the attribute that will be
        /// used to mark elements as generated and for menuitem identification
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Init([MarshalAs(UnmanagedType.Interface)] nsIDOMDocumentFragment aDocumentFragment, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aGeneratedItemIdAttrName);
		
		/// <summary>
        /// Invoke the action of the menuitem with assigned id aGeneratedItemId.
        ///
        /// @param aGeneratedItemId the menuitem id
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Click([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aGeneratedItemId);
	}
}
