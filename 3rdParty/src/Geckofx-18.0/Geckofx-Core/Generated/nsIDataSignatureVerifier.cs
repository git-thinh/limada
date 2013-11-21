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
// Generated by IDLImporter from file nsIDataSignatureVerifier.idl
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
    /// An interface for verifying that a given string of data was signed by the
    /// private key matching the given public key.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("0a84b3d5-6ba9-432d-89da-4fbd0b0f2aec")]
	public interface nsIDataSignatureVerifier
	{
		
		/// <summary>
        /// Verifies that the data matches the data that was used to generate the
        /// signature.
        ///
        /// @param aData      The data to be tested.
        /// @param aSignature The signature of the data, base64 encoded.
        /// @param aPublicKey The public part of the key used for signing, DER encoded
        /// then base64 encoded.
        /// @returns true if the signature matches the data, false if not.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool VerifyData([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aData, [MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aSignature, [MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aPublicKey);
	}
}
