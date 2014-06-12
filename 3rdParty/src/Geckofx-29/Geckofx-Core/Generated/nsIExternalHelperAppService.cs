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
// Generated by IDLImporter from file nsIExternalHelperAppService.idl
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
    /// The external helper app service is used for finding and launching
    /// platform specific external applications for a given mime content type.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("9e456297-ba3e-42b1-92bd-b7db014268cb")]
	public interface nsIExternalHelperAppService
	{
		
		/// <summary>
        /// Binds an external helper application to a stream listener. The caller
        /// should pump data into the returned stream listener. When the OnStopRequest
        /// is issued, the stream listener implementation will launch the helper app
        /// with this data.
        /// @param aMimeContentType The content type of the incoming data
        /// @param aRequest The request corresponding to the incoming data
        /// @param aWindowContext Use GetInterface to retrieve properties like the
        /// dom window or parent window...
        /// The service might need this in order to bring up dialogs.
        /// @param aForceSave True to always save this content to disk, regardless of
        /// nsIMIMEInfo and other such influences.
        /// @return A nsIStreamListener which the caller should pump the data into.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIStreamListener DoContent([MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aMimeContentType, [MarshalAs(UnmanagedType.Interface)] nsIRequest aRequest, [MarshalAs(UnmanagedType.Interface)] nsIInterfaceRequestor aWindowContext, [MarshalAs(UnmanagedType.U1)] bool aForceSave);
		
		/// <summary>
        /// Returns true if data from a URL with this extension combination
        /// is to be decoded from aEncodingType prior to saving or passing
        /// off to helper apps, false otherwise.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool ApplyDecodingForExtension([MarshalAs(UnmanagedType.LPStruct)] nsAUTF8StringBase aExtension, [MarshalAs(UnmanagedType.LPStruct)] nsACStringBase aEncodingType);
	}
	
	/// <summary>
    /// This is a private interface shared between external app handlers and the platform specific
    /// external helper app service
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("6613e2e7-feab-4e3a-bb1f-b03200d544ec")]
	public interface nsPIExternalAppLauncher
	{
		
		/// <summary>
        /// mscott --> eventually I should move this into a new service so other
        /// consumers can add temporary files they want deleted on exit.
        /// @param aTemporaryFile A temporary file we should delete on exit.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DeleteTemporaryFileOnExit([MarshalAs(UnmanagedType.Interface)] nsIFile aTemporaryFile);
		
		/// <summary>
        /// Delete a temporary file created inside private browsing mode when
        /// the private browsing mode has ended.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void DeleteTemporaryPrivateFileWhenPossible([MarshalAs(UnmanagedType.Interface)] nsIFile aTemporaryFile);
	}
	
	/// <summary>
    /// A helper app launcher is a small object created to handle the launching
    /// of an external application.
    ///
    /// Note that cancelling the load via the nsICancelable interface will release
    /// the reference to the launcher dialog.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("acf2a516-7d7f-4771-8b22-6c4a559c088e")]
	public interface nsIHelperAppLauncher : nsICancelable
	{
		
		/// <summary>
        /// Call this method to request that this object abort whatever operation it
        /// may be performing.
        ///
        /// @param aReason
        /// Pass a failure code to indicate the reason why this operation is
        /// being canceled.  It is an error to pass a success code.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void Cancel(int aReason);
		
		/// <summary>
        /// The mime info object associated with the content type this helper app
        /// launcher is currently attempting to load
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIMIMEInfo GetMIMEInfoAttribute();
		
		/// <summary>
        /// The source uri
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIURI GetSourceAttribute();
		
		/// <summary>
        /// The suggested name for this file
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetSuggestedFileNameAttribute([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase aSuggestedFileName);
		
		/// <summary>
        /// Saves the final destination of the file. Does not actually perform the
        /// save.
        /// NOTE: This will release the reference to the
        /// nsIHelperAppLauncherDialog.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SaveToDisk([MarshalAs(UnmanagedType.Interface)] nsIFile aNewFileLocation, [MarshalAs(UnmanagedType.U1)] bool aRememberThisPreference);
		
		/// <summary>
        /// Remembers that aApplication should be used to launch this content. Does
        /// not actually launch the application.
        /// NOTE: This will release the reference to the nsIHelperAppLauncherDialog.
        /// @param aApplication nsIFile corresponding to the location of the application to use.
        /// @param aRememberThisPreference TRUE if we should remember this choice.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void LaunchWithApplication([MarshalAs(UnmanagedType.Interface)] nsIFile aApplication, [MarshalAs(UnmanagedType.U1)] bool aRememberThisPreference);
		
		/// <summary>
        /// Callback invoked by nsIHelperAppLauncherDialog::promptForSaveToFileAsync
        /// after the user has chosen a file through the File Picker (or dismissed it).
        /// @param aFile The file that was chosen by the user (or null if dialog was dismissed).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SaveDestinationAvailable([MarshalAs(UnmanagedType.Interface)] nsIFile aFile);
		
		/// <summary>
        /// The following methods are used by the progress dialog to get or set
        /// information on the current helper app launcher download.
        /// This reference will be released when the download is finished (after the
        /// listener receives the STATE_STOP notification).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetWebProgressListener([MarshalAs(UnmanagedType.Interface)] nsIWebProgressListener2 aWebProgressListener);
		
		/// <summary>
        /// The file we are saving to
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIFile GetTargetFileAttribute();
		
		/// <summary>
        /// The executable-ness of the target file
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetTargetFileIsExecutableAttribute();
		
		/// <summary>
        /// Time when the download started
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetTimeDownloadStartedAttribute();
		
		/// <summary>
        /// The download content length, or -1 if the length is not available.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		long GetContentLengthAttribute();
	}
}
