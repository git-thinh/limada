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
// Generated by IDLImporter from file nsITextToSubURI.idl
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
    ///This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this
    /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8B042E24-6F87-11d3-B3C8-00805F8A6670")]
	public interface nsITextToSubURI
	{
		
		/// <summary>
        ///This Source Code Form is subject to the terms of the Mozilla Public
        /// License, v. 2.0. If a copy of the MPL was not distributed with this
        /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
		[return: MarshalAs(UnmanagedType.LPStr)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string ConvertAndEscape([MarshalAs(UnmanagedType.LPStr)] string charset, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string text);
		
		[return: MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string UnEscapeAndConvert([MarshalAs(UnmanagedType.LPStr)] string charset, [MarshalAs(UnmanagedType.LPStr)] string text);
		
		/// <summary>
        /// Unescapes the given URI fragment (for UI purpose only)
        /// Note:
        /// <ul>
        /// <li> escaping back the result (unescaped string) is not guaranteed to
        /// give the original escaped string
        /// <li> In case of a conversion error, the URI fragment (escaped) is
        /// assumed to be in UTF-8 and converted to AString (UTF-16)
        /// <li> Always succeeeds (callers don't need to do error checking)
        /// </ul>
        ///
        /// @param aCharset the charset to convert from
        /// @param aURIFragment the URI (or URI fragment) to unescape
        /// @return Unescaped aURIFragment  converted to unicode
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnEscapeURIForUI([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aCharset, [MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aURIFragment, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
		
		/// <summary>
        /// Unescapes only non ASCII characters in the given URI fragment
        /// note: this method assumes the URI as UTF-8 and fallbacks to the given
        /// charset if the charset is an ASCII superset
        ///
        /// @param aCharset the charset to convert from
        /// @param aURIFragment the URI (or URI fragment) to unescape
        /// @return Unescaped aURIFragment  converted to unicode
        /// @throws NS_ERROR_UCONV_NOCONV when there is no decoder for aCharset
        /// or error code of nsIUnicodeDecoder in case of conversion failure
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnEscapeNonAsciiURI([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aCharset, [MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aURIFragment, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase retval);
	}
}
