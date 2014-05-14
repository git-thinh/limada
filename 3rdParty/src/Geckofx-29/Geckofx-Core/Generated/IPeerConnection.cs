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
// Generated by IDLImporter from file IPeerConnection.idl
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
    /// Manager interface to PeerConnection.js so it is accessible from C++.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("c2218bd2-2648-4701-8fa6-305d3379e9f8")]
	public interface IPeerConnectionManager
	{
		
		/// <summary>
        /// Manager interface to PeerConnection.js so it is accessible from C++.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool HasActivePeerConnection(uint innerWindowID);
	}
	
	/// <summary>
    ///Do not confuse with nsIDOMRTCPeerConnection. This interface is purely for
    /// communication between the PeerConnection JS DOM binding and the C++
    /// implementation in SIPCC.
    ///
    /// See media/webrtc/signaling/include/PeerConnectionImpl.h
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("d7dfe148-0416-446b-a128-66a7c71ae8d3")]
	public interface IPeerConnectionObserver
	{
	}
	
	/// <summary>
    ///Do not confuse with nsIDOMRTCPeerConnection. This interface is purely for
    /// communication between the PeerConnection JS DOM binding and the C++
    /// implementation in SIPCC.
    ///
    /// See media/webrtc/signaling/include/PeerConnectionImpl.h
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("c9c31639-1a49-4533-8429-f6a348c4d8c3")]
	public interface IPeerConnection
	{
	}
	
	/// <summary>IPeerConnectionConsts </summary>
	public class IPeerConnectionConsts
	{
		
		// <summary>
        //Do not confuse with nsIDOMRTCPeerConnection. This interface is purely for
        // communication between the PeerConnection JS DOM binding and the C++
        // implementation in SIPCC.
        //
        // See media/webrtc/signaling/include/PeerConnectionImpl.h
        // </summary>
		public const ulong kHintAudio = 0x00000001;
		
		// 
		public const ulong kHintVideo = 0x00000002;
		
		// 
		public const long kActionNone = -1;
		
		// 
		public const long kActionOffer = 0;
		
		// 
		public const long kActionAnswer = 1;
		
		// 
		public const long kActionPRAnswer = 2;
		
		// 
		public const long kIceGathering = 0;
		
		// 
		public const long kIceWaiting = 1;
		
		// 
		public const long kIceChecking = 2;
		
		// 
		public const long kIceConnected = 3;
		
		// 
		public const long kIceFailed = 4;
		
		// <summary>
        //for readyState on Peer Connection </summary>
		public const long kNew = 0;
		
		// 
		public const long kNegotiating = 1;
		
		// 
		public const long kActive = 2;
		
		// 
		public const long kClosing = 3;
		
		// 
		public const long kClosed = 4;
		
		// <summary>
        //for 'type' in DataChannelInit dictionary </summary>
		public const ulong kDataChannelReliable = 0;
		
		// 
		public const ulong kDataChannelPartialReliableRexmit = 1;
		
		// 
		public const ulong kDataChannelPartialReliableTimed = 2;
		
		// <summary>
        //Constants for 'name' in error callbacks </summary>
		public const ulong kNoError = 0;
		
		// <summary>
        // Test driver only
        // </summary>
		public const ulong kInvalidConstraintsType = 1;
		
		// 
		public const ulong kInvalidCandidateType = 2;
		
		// 
		public const ulong kInvalidMediastreamTrack = 3;
		
		// 
		public const ulong kInvalidState = 4;
		
		// 
		public const ulong kInvalidSessionDescription = 5;
		
		// 
		public const ulong kIncompatibleSessionDescription = 6;
		
		// 
		public const ulong kIncompatibleConstraints = 7;
		
		// 
		public const ulong kIncompatibleMediaStreamTrack = 8;
		
		// 
		public const ulong kInternalError = 9;
		
		// 
		public const ulong kMaxErrorType = 9;
	}
}
