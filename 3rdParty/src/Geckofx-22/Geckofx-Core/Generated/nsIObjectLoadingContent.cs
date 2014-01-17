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
// Generated by IDLImporter from file nsIObjectLoadingContent.idl
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
    /// This interface represents a content node that loads objects.
    ///
    /// Please make sure to update the MozObjectLoadingContent WebIDL
    /// interface to mirror this interface when changing it.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("e2ef99fe-f7d3-422f-a7b4-834e8bdde710")]
	public interface nsIObjectLoadingContent
	{
		
		/// <summary>
        /// The actual mime type (the one we got back from the network
        /// request) for the element.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetActualTypeAttribute([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aActualType);
		
		/// <summary>
        /// Gets the type of the content that's currently loaded. See
        /// the constants above for the list of possible values.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetDisplayedTypeAttribute();
		
		/// <summary>
        /// Gets the content type that corresponds to the give MIME type.  See the
        /// constants above for the list of possible values.  If nothing else fits,
        /// TYPE_NULL will be returned.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetContentTypeForMIMEType([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aMimeType);
		
		/// <summary>
        /// Returns the plugin instance if it has already been instantiated. This
        /// will never instantiate the plugin and so is safe to call even when
        /// content script must not execute.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetPluginInstanceAttribute();
		
		/// <summary>
        /// Tells the content about an associated object frame.
        /// This can be called multiple times for different frames.
        ///
        /// This is noscript because this is an internal method that will go away, and
        /// because nsIObjectFrame is unscriptable.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void HasNewFrame(System.IntPtr aFrame);
		
		/// <summary>
        /// If this object is in going to be printed, this method
        /// returns the nsIObjectFrame object which should be used when
        /// printing the plugin. The returned nsIFrame is in the original document,
        /// not in the static clone.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetPrintFrame();
		
		/// <summary>
        /// Notifications from pluginhost that our instance crashed or was destroyed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PluginDestroyed();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PluginCrashed([MarshalAs(UnmanagedType.Interface)] nsIPluginTag pluginTag, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase pluginDumpID, [MarshalAs(UnmanagedType.LPStruct)] nsAStringBase browserDumpID, [MarshalAs(UnmanagedType.U1)] bool submittedCrashReport);
		
		/// <summary>
        /// This method will play a plugin that has been stopped by the
        /// click-to-play plugins or play-preview features.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PlayPlugin();
		
		/// <summary>
        /// This attribute will return true if the current content type has been
        /// activated, either explicitly or by passing checks that would have it be
        /// click-to-play or play-preview.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetActivatedAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void StopPluginInstance();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SyncStartPluginInstance();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void AsyncStartPluginInstance();
		
		/// <summary>
        /// Puts the tag in the "waiting on a channel" state and adopts this
        /// channel. This does not override the normal logic of examining attributes
        /// and the channel type, so the load may cancel this channel if it decides not
        /// to use one.
        ///
        /// This assumes:
        /// - This tag has not begun loading yet
        /// - This channel has not yet hit OnStartRequest
        /// - The caller will continue to pass channel events to us as a listener
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void InitializeFromChannel([MarshalAs(UnmanagedType.Interface)] nsIRequest request);
		
		/// <summary>
        /// Requests the plugin instance for scripting, attempting to spawn it if
        /// appropriate.
        ///
        /// The first time content js tries to access a pre-empted plugin
        /// (click-to-play or play preview), an event is dispatched.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr ScriptRequestPluginInstance([MarshalAs(UnmanagedType.U1)] bool callerIsContentJS);
		
		/// <summary>
        /// The URL of the data/src loaded in the object. This may be null (i.e.
        /// an <embed> with no src).
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURI GetSrcURIAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetPluginFallbackTypeAttribute();
		
		/// <summary>
        /// If this object currently owns a running plugin, regardless of whether or
        /// not one is pending spawn/despawn.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetHasRunningPluginAttribute();
		
		/// <summary>
        /// This method will disable the play-preview plugin state.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CancelPlayPreview();
	}
	
	/// <summary>nsIObjectLoadingContentConsts </summary>
	public class nsIObjectLoadingContentConsts
	{
		
		// <summary>
        // See notes in nsObjectLoadingContent.h
        // </summary>
		public const ulong TYPE_LOADING = 0;
		
		// 
		public const ulong TYPE_IMAGE = 1;
		
		// 
		public const ulong TYPE_PLUGIN = 2;
		
		// 
		public const ulong TYPE_DOCUMENT = 3;
		
		// 
		public const ulong TYPE_NULL = 4;
		
		// <summary>
        // The content type is not supported (e.g. plugin not installed)
        // </summary>
		public const ulong PLUGIN_UNSUPPORTED = 0;
		
		// <summary>
        // Showing alternate content
        // </summary>
		public const ulong PLUGIN_ALTERNATE = 1;
		
		// <summary>
        // The plugin exists, but is disabled
        // </summary>
		public const ulong PLUGIN_DISABLED = 2;
		
		// <summary>
        // The plugin is blocklisted and disabled
        // </summary>
		public const ulong PLUGIN_BLOCKLISTED = 3;
		
		// <summary>
        // The plugin is considered outdated, but not disabled
        // </summary>
		public const ulong PLUGIN_OUTDATED = 4;
		
		// <summary>
        // The plugin has crashed
        // </summary>
		public const ulong PLUGIN_CRASHED = 5;
		
		// <summary>
        // Suppressed by security policy
        // </summary>
		public const ulong PLUGIN_SUPPRESSED = 6;
		
		// <summary>
        // Blocked by content policy
        // </summary>
		public const ulong PLUGIN_USER_DISABLED = 7;
		
		// <summary>
        // The plugin is disabled until the user clicks on it
        // </summary>
		public const ulong PLUGIN_CLICK_TO_PLAY = 8;
		
		// <summary>
        // The plugin is vulnerable (update available)
        // </summary>
		public const ulong PLUGIN_VULNERABLE_UPDATABLE = 9;
		
		// <summary>
        // The plugin is vulnerable (no update available)
        // </summary>
		public const ulong PLUGIN_VULNERABLE_NO_UPDATE = 10;
		
		// <summary>
        // The plugin is in play preview mode
        // </summary>
		public const ulong PLUGIN_PLAY_PREVIEW = 11;
	}
}