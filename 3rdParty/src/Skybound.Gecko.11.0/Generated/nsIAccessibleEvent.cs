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
// Generated by IDLImporter from file nsIAccessibleEvent.idl
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
	using System.Windows.Forms;
	
	
	/// <summary>
    /// An interface for accessibility events listened to
    /// by in-process accessibility clients, which can be used
    /// to find out how to get accessibility and DOM interfaces for
    /// the event and its target. To listen to in-process accessibility invents,
    /// make your object an nsIObserver, and listen for accessible-event by
    /// using code something like this:
    /// nsCOMPtr<nsIObserverService> observerService =
    /// do_GetService("@mozilla.org/observer-service;1", &rv);
    /// if (NS_SUCCEEDED(rv))
    /// rv = observerService->AddObserver(this, "accessible-event", PR_TRUE);
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7f66a33a-9ed7-4fd4-87a8-e431b0f43368")]
	public interface nsIAccessibleEvent
	{
		
		/// <summary>
        /// The type of event, based on the enumerated event values
        /// defined in this interface.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetEventTypeAttribute();
		
		/// <summary>
        /// The nsIAccessible associated with the event.
        /// May return null if no accessible is available
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIAccessible GetAccessibleAttribute();
		
		/// <summary>
        /// The nsIAccessibleDocument that the event target nsIAccessible
        /// resides in. This can be used to get the DOM window,
        /// the DOM document and the window handler, among other things.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIAccessibleDocument GetAccessibleDocumentAttribute();
		
		/// <summary>
        /// The nsIDOMNode associated with the event
        /// May return null if accessible for event has been shut down
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIDOMNode GetDOMNodeAttribute();
		
		/// <summary>
        /// Returns true if the event was caused by explicit user input,
        /// as opposed to purely originating from a timer or mouse movement
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetIsFromUserInputAttribute();
	}
	
	/// <summary>nsIAccessibleStateChangeEvent </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9addd25d-8fa1-415e-94ec-6038f220d3e4")]
	public interface nsIAccessibleStateChangeEvent
	{
		
		/// <summary>
        /// Returns the state of accessible (see constants declared
        /// in nsIAccessibleStates).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetStateAttribute();
		
		/// <summary>
        /// Returns true if the state is extra state.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsExtraState();
		
		/// <summary>
        /// Returns true if the state is turned on.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsEnabled();
	}
	
	/// <summary>nsIAccessibleTextChangeEvent </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("21e0f8bd-5638-4964-870b-3c8e944ac4c4")]
	public interface nsIAccessibleTextChangeEvent
	{
		
		/// <summary>
        /// Returns offset of changed text in accessible.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetStartAttribute();
		
		/// <summary>
        /// Returns length of changed text.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetLengthAttribute();
		
		/// <summary>
        /// Returns true if text was inserted, otherwise false.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsInserted();
		
		/// <summary>
        /// The inserted or removed text
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetModifiedTextAttribute([MarshalAs(UnmanagedType.LPStruct)] nsAStringBase aModifiedText);
	}
	
	/// <summary>nsIAccessibleCaretMoveEvent </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5675c486-a230-4d85-a4bd-33670826d5ff")]
	public interface nsIAccessibleCaretMoveEvent
	{
		
		/// <summary>
        /// Return caret offset.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetCaretOffsetAttribute();
	}
	
	/// <summary>nsIAccessibleTableChangeEvent </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("df517997-ed52-4ea2-b310-2f8e0fe64572")]
	public interface nsIAccessibleTableChangeEvent
	{
		
		/// <summary>
        /// Return the row or column index.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetRowOrColIndexAttribute();
		
		/// <summary>
        /// Return the number of rows or cols
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetNumRowsOrColsAttribute();
	}
}