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
// Generated by IDLImporter from file nsIUnicharInputStream.idl
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
    /// Abstract unicode character input stream
    /// @see nsIInputStream
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d5e3bd80-6723-4b92-b0c9-22f6162fd94f")]
	public interface nsIUnicharInputStream
	{
		
		/// <summary>
        /// Reads into a caller-provided character array.
        ///
        /// @return The number of characters that were successfully read. May be less
        /// than aCount, even if there is more data in the input stream.
        /// A return value of 0 means EOF.
        ///
        /// @note To read more than 2^32 characters, call this method multiple times.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint Read([MarshalAs(UnmanagedType.LPArray, SizeParamIndex=1)] string aBuf, uint aCount);
		
		/// <summary>
        /// Low-level read method that has access to the stream's underlying buffer.
        /// The writer function may be called multiple times for segmented buffers.
        /// ReadSegments is expected to keep calling the writer until either there is
        /// nothing left to read or the writer returns an error.  ReadSegments should
        /// not call the writer with zero characters to consume.
        ///
        /// @param aWriter the "consumer" of the data to be read
        /// @param aClosure opaque parameter passed to writer
        /// @param aCount the maximum number of characters to be read
        ///
        /// @return number of characters read (may be less than aCount)
        /// @return 0 if reached end of file (or if aWriter refused to consume data)
        ///
        /// @throws NS_BASE_STREAM_WOULD_BLOCK if reading from the input stream would
        /// block the calling thread (non-blocking mode only)
        /// @throws <other-error> on failure
        ///
        /// NOTE: this function may be unimplemented if a stream has no underlying
        /// buffer
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint ReadSegments(System.IntPtr aWriter, System.IntPtr aClosure, uint aCount);
		
		/// <summary>
        /// Read into a string object.
        /// @param aCount The number of characters that should be read
        /// @return The number of characters that were read.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint ReadString(uint aCount, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aString);
		
		/// <summary>
        /// Close the stream and free associated resources. This also closes the
        /// underlying stream, if any.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Close();
	}
}
