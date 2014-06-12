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
// Generated by IDLImporter from file nsIScriptableBase64Encoder.idl
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
    /// nsIScriptableBase64Encoder efficiently encodes the contents
    /// of a nsIInputStream to a Base64 string.  This avoids the need
    /// to read the entire stream into a buffer, and only then do the
    /// Base64 encoding.
    ///
    /// If you already have a buffer full of data, you should use
    /// btoa instead!
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9479c864-d1f9-45ab-b7b9-28b907bd2ba9")]
	public interface nsIScriptableBase64Encoder
	{
		
		/// <summary>
        /// These methods take an nsIInputStream and return a narrow or wide
        /// string with the contents of the nsIInputStream base64 encoded.
        ///
        /// The stream passed in must support ReadSegments and must not be
        /// a non-blocking stream that will return NS_BASE_STREAM_WOULD_BLOCK.
        /// If either of these restrictions are violated we will abort.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void EncodeToCString([MarshalAs(UnmanagedType.Interface)] nsIInputStream stream, uint length, [MarshalAs(UnmanagedType.LPStruct)] nsACStringBase retval);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void EncodeToString([MarshalAs(UnmanagedType.Interface)] nsIInputStream stream, uint length, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
	}
}
