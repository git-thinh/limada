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
// Generated by IDLImporter from file nsIServerSocket.idl
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
    /// nsIServerSocket
    ///
    /// An interface to a server socket that can accept incoming connections.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7a9c39cb-a13f-4eef-9bdf-a74301628742")]
	public interface nsIServerSocket
	{
		
		/// <summary>
        /// init
        ///
        /// This method initializes a server socket.
        ///
        /// @param aPort
        /// The port of the server socket.  Pass -1 to indicate no preference,
        /// and a port will be selected automatically.
        /// @param aLoopbackOnly
        /// If true, the server socket will only respond to connections on the
        /// local loopback interface.  Otherwise, it will accept connections
        /// from any interface.  To specify a particular network interface,
        /// use initWithAddress.
        /// @param aBackLog
        /// The maximum length the queue of pending connections may grow to.
        /// This parameter may be silently limited by the operating system.
        /// Pass -1 to use the default value.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Init(int aPort, [MarshalAs(UnmanagedType.U1)] bool aLoopbackOnly, int aBackLog);
		
		/// <summary>
        /// initSpecialConnection
        ///
        /// This method initializes a server socket and offers the ability to have
        /// that socket not get terminated if Gecko is set offline.
        ///
        /// @param aPort
        /// The port of the server socket.  Pass -1 to indicate no preference,
        /// and a port will be selected automatically.
        /// @param aFlags
        /// Flags for the socket.
        /// @param aBackLog
        /// The maximum length the queue of pending connections may grow to.
        /// This parameter may be silently limited by the operating system.
        /// Pass -1 to use the default value.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InitSpecialConnection(int aPort, ulong aFlags, int aBackLog);
		
		/// <summary>
        /// initWithAddress
        ///
        /// This method initializes a server socket, and binds it to a particular
        /// local address (and hence a particular local network interface).
        ///
        /// @param aAddr
        /// The address to which this server socket should be bound.
        /// @param aBackLog
        /// The maximum length the queue of pending connections may grow to.
        /// This parameter may be silently limited by the operating system.
        /// Pass -1 to use the default value.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InitWithAddress(System.IntPtr aAddr, int aBackLog);
		
		/// <summary>
        /// initWithFilename
        ///
        /// This method initializes a Unix domain or "local" server socket. Such
        /// a socket has a name in the filesystem, like an ordinary file. To
        /// connect, a client supplies the socket's filename, and the usual
        /// permission checks on socket apply.
        ///
        /// This makes Unix domain sockets useful for communication between the
        /// programs being run by a specific user on a single machine: the
        /// operating system takes care of authentication, and the user's home
        /// directory or profile directory provide natural per-user rendezvous
        /// points.
        ///
        /// Since Unix domain sockets are always local to the machine, they are
        /// not affected by the nsIIOService's 'offline' flag.
        ///
        /// The system-level socket API may impose restrictions on the length of
        /// the filename that are stricter than those of the underlying
        /// filesystem. If the file name is too long, this returns
        /// NS_ERROR_FILE_NAME_TOO_LONG.
        ///
        /// All components of the path prefix of |aPath| must name directories;
        /// otherwise, this returns NS_ERROR_FILE_NOT_DIRECTORY.
        ///
        /// This call requires execute permission on all directories containing
        /// the one in which the socket is to be created, and write and execute
        /// permission on the directory itself. Otherwise, this returns
        /// NS_ERROR_CONNECTION_REFUSED.
        ///
        /// This call creates the socket's directory entry. There must not be
        /// any existing entry with the given name. If there is, this returns
        /// NS_ERROR_SOCKET_ADDRESS_IN_USE.
        ///
        /// On systems that don't support Unix domain sockets at all, this
        /// returns NS_ERROR_SOCKET_ADDRESS_NOT_SUPPORTED.
        ///
        /// @param aPath nsIFile
        /// The file name at which the socket should be created.
        ///
        /// @param aPermissions unsigned long
        /// Unix-style permission bits to be applied to the new socket.
        ///
        /// Note about permissions: Linux's unix(7) man page claims that some
        /// BSD-derived systems ignore permissions on UNIX-domain sockets;
        /// NetBSD's bind(2) man page agrees, but says it does check now (dated
        /// 2005). POSIX has required 'connect' to fail if write permission on
        /// the socket itself is not granted since 2003 (Issue 6). NetBSD says
        /// that the permissions on the containing directory (execute) have
        /// always applied, so creating sockets in appropriately protected
        /// directories should be secure on both old and new systems.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InitWithFilename([MarshalAs(UnmanagedType.Interface)] nsIFile aPath, uint aPermissions, int aBacklog);
		
		/// <summary>
        /// close
        ///
        /// This method closes a server socket.  This does not affect already
        /// connected client sockets (i.e., the nsISocketTransport instances
        /// created from this server socket).  This will cause the onStopListening
        /// event to asynchronously fire with a status of NS_BINDING_ABORTED.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Close();
		
		/// <summary>
        /// asyncListen
        ///
        /// This method puts the server socket in the listening state.  It will
        /// asynchronously listen for and accept client connections.  The listener
        /// will be notified once for each client connection that is accepted.  The
        /// listener's onSocketAccepted method will be called on the same thread
        /// that called asyncListen (the calling thread must have a nsIEventTarget).
        ///
        /// The listener will be passed a reference to an already connected socket
        /// transport (nsISocketTransport).  See below for more details.
        ///
        /// @param aListener
        /// The listener to be notified when client connections are accepted.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AsyncListen([MarshalAs(UnmanagedType.Interface)] nsIServerSocketListener aListener);
		
		/// <summary>
        /// Returns the port of this server socket.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetPortAttribute();
		
		/// <summary>
        /// Returns the address to which this server socket is bound.  Since a
        /// server socket may be bound to multiple network devices, this address
        /// may not necessarily be specific to a single network device.  In the
        /// case of an IP socket, the IP address field would be zerod out to
        /// indicate a server socket bound to all network devices.  Therefore,
        /// this method cannot be used to determine the IP address of the local
        /// system.  See nsIDNSService::myHostName if this is what you need.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetAddress();
	}
	
	/// <summary>nsIServerSocketConsts </summary>
	public class nsIServerSocketConsts
	{
		
		// <summary>
        // use initWithAddress.
        // </summary>
		public const long LoopbackOnly = 0x00000001;
		
		// <summary>
        // offline.
        // </summary>
		public const long KeepWhenOffline = 0x00000002;
	}
	
	/// <summary>
    /// nsIServerSocketListener
    ///
    /// This interface is notified whenever a server socket accepts a new connection.
    /// The transport is in the connected state, and read/write streams can be opened
    /// using the normal nsITransport API.  The address of the client can be found by
    /// calling the nsISocketTransport::GetAddress method or by inspecting
    /// nsISocketTransport::GetHost, which returns a string representation of the
    /// client's IP address (NOTE: this may be an IPv4 or IPv6 string literal).
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("836d98ec-fee2-4bde-b609-abd5e966eabd")]
	public interface nsIServerSocketListener
	{
		
		/// <summary>
        /// onSocketAccepted
        ///
        /// This method is called when a client connection is accepted.
        ///
        /// @param aServ
        /// The server socket.
        /// @param aTransport
        /// The connected socket transport.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnSocketAccepted([MarshalAs(UnmanagedType.Interface)] nsIServerSocket aServ, [MarshalAs(UnmanagedType.Interface)] nsISocketTransport aTransport);
		
		/// <summary>
        /// onStopListening
        ///
        /// This method is called when the listening socket stops for some reason.
        /// The server socket is effectively dead after this notification.
        ///
        /// @param aServ
        /// The server socket.
        /// @param aStatus
        /// The reason why the server socket stopped listening.  If the
        /// server socket was manually closed, then this value will be
        /// NS_BINDING_ABORTED.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void OnStopListening([MarshalAs(UnmanagedType.Interface)] nsIServerSocket aServ, int aStatus);
	}
}
