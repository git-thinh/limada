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
// Generated by IDLImporter from file nsISerializationHelper.idl
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
    /// Simple scriptable serialization helper. Can be used as a service.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("31654c0f-35f3-44c6-b31e-37a11516e6bc")]
	public interface nsISerializationHelper
	{
		
		/// <summary>
        /// Serialize the object to a base64 string. This string can be later passed
        /// as an input to deserializeObject method.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SerializeToString([MarshalAs(UnmanagedType.Interface)] nsISerializable serializable, [MarshalAs(UnmanagedType.LPStruct)] nsACStringBase retval);
		
		/// <summary>
        /// Takes base64 encoded string that cointains serialization of a single
        /// object. Most commonly, input is result of previous call to
        /// serializeToString.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsISupports DeserializeObject([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase input);
	}
}
