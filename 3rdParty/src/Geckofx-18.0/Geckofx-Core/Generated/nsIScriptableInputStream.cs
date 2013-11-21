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
// Generated by IDLImporter from file nsIScriptableInputStream.idl
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
    /// nsIScriptableInputStream provides scriptable access to an nsIInputStream
    /// instance.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("3fce9015-472a-4080-ac3e-cd875dbe361e")]
	public interface nsIScriptableInputStream
	{
		
		/// <summary>
        /// Closes the stream.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Close();
		
		/// <summary>
        /// Wrap the given nsIInputStream with this nsIScriptableInputStream.
        ///
        /// @param aInputStream parameter providing the stream to wrap
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Init([MarshalAs(UnmanagedType.Interface)] nsIInputStream aInputStream);
		
		/// <summary>
        /// Return the number of bytes currently available in the stream
        ///
        /// @return the number of bytes
        ///
        /// @throws NS_BASE_STREAM_CLOSED if called after the stream has been closed
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint Available();
		
		/// <summary>
        /// Read data from the stream.
        ///
        /// WARNING: If the data contains a null byte, then this method will return
        /// a truncated string.
        ///
        /// @param aCount the maximum number of bytes to read
        ///
        /// @return the data, which will be an empty string if the stream is at EOF.
        ///
        /// @throws NS_BASE_STREAM_CLOSED if called after the stream has been closed
        /// @throws NS_ERROR_NOT_INITIALIZED if init was not called
        /// </summary>
		[return: MarshalAs(UnmanagedType.LPStr)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		string Read(uint aCount);
		
		/// <summary>
        /// Read data from the stream, including NULL bytes.
        ///
        /// @param aCount the maximum number of bytes to read.
        ///
        /// @return the data from the stream, which will be an empty string if EOF
        /// has been reached.
        ///
        /// @throws NS_BASE_STREAM_WOULD_BLOCK if reading from the input stream
        /// would block the calling thread (non-blocking mode only).
        /// @throws NS_ERROR_FAILURE if there are not enough bytes available to read
        /// aCount amount of data.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ReadBytes(uint aCount, [MarshalAs(UnmanagedType.LPStruct)] nsACStringBase retval);
	}
}
