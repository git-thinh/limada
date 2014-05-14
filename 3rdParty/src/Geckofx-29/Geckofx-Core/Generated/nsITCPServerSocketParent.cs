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
// Generated by IDLImporter from file nsITCPServerSocketParent.idl
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
    /// Interface required to allow the TCP server-socket object in the parent process
    /// to talk to the parent IPC actor.
    /// It is used in the server socket implementation on the parent side.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("161ffc9f-54d3-4f21-a536-4166003d0e1d")]
	public interface nsITCPServerSocketParent
	{
		
		/// <summary>
        /// Trigger a callback in the content process when the socket accepts any request.
        ///
        /// @param socket
        /// The socket generated in accepting any open request on the parent side.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendCallbackAccept([MarshalAs(UnmanagedType.Interface)] nsITCPSocketParent socket);
		
		/// <summary>
        /// Trigger a callback in the content process when an error occurs.
        ///
        /// @param message
        /// The error message.
        /// @param filename
        /// The file name in which the error occured.
        /// @param lineNumber
        /// The line number in which the error occured.
        /// @param columnNumber
        /// The column number in which the error occured.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SendCallbackError([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase message, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase filename, uint lineNumber, uint columnNumber);
	}
}
