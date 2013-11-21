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
// Generated by IDLImporter from file nsIPrefService.idl
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
    /// The nsIPrefService interface is the main entry point into the back end
    /// preferences management library. The preference service is directly
    /// responsible for the management of the preferences files and also facilitates
    /// access to the preference branch object which allows the direct manipulation
    /// of the preferences themselves.
    ///
    /// @see nsIPrefBranch
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("decb9cc7-c08f-4ea5-be91-a8fc637ce2d2")]
	public interface nsIPrefService
	{
		
		/// <summary>
        /// Called to read in the preferences specified in a user preference file.
        ///
        /// @param aFile The file to be read.
        ///
        /// @note
        /// If nullptr is passed in for the aFile parameter the default preferences
        /// file(s) [prefs.js, user.js] will be read and processed.
        ///
        /// @return NS_OK File was read and processed.
        /// @return Other File failed to read or contained invalid data.
        ///
        /// @see savePrefFile
        /// @see nsIFile
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ReadUserPrefs([MarshalAs(UnmanagedType.Interface)] nsIFile aFile);
		
		/// <summary>
        /// Called to completely flush and re-initialize the preferences system.
        ///
        /// @return NS_OK The preference service was re-initialized correctly.
        /// @return Other The preference service failed to restart correctly.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ResetPrefs();
		
		/// <summary>
        /// Called to reset all preferences with user set values back to the
        /// application default values.
        ///
        /// @return NS_OK Always.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ResetUserPrefs();
		
		/// <summary>
        /// Called to write current preferences state to a file.
        ///
        /// @param aFile The file to be written.
        ///
        /// @note
        /// If nullptr is passed in for the aFile parameter the preference data is
        /// written out to the current preferences file (usually prefs.js.)
        ///
        /// @return NS_OK File was written.
        /// @return Other File failed to write.
        ///
        /// @see readUserPrefs
        /// @see nsIFile
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SavePrefFile([MarshalAs(UnmanagedType.Interface)] nsIFile aFile);
		
		/// <summary>
        /// Call to get a Preferences "Branch" which accesses user preference data.
        /// Using a Set method on this object will always create or set a user
        /// preference value. When using a Get method a user set value will be
        /// returned if one exists, otherwise a default value will be returned.
        ///
        /// @param aPrefRoot The preference "root" on which to base this "branch".
        /// For example, if the root "browser.startup." is used, the
        /// branch will be able to easily access the preferences
        /// "browser.startup.page", "browser.startup.homepage", or
        /// "browser.startup.homepage_override" by simply requesting
        /// "page", "homepage", or "homepage_override". nullptr or ""
        /// may be used to access to the entire preference "tree".
        ///
        /// @return nsIPrefBranch The object representing the requested branch.
        ///
        /// @see getDefaultBranch
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIPrefBranch GetBranch([MarshalAs(UnmanagedType.LPStr)] string aPrefRoot);
		
		/// <summary>
        /// Call to get a Preferences "Branch" which accesses only the default
        /// preference data. Using a Set method on this object will always create or
        /// set a default preference value. When using a Get method a default value
        /// will always be returned.
        ///
        /// @param aPrefRoot The preference "root" on which to base this "branch".
        /// For example, if the root "browser.startup." is used, the
        /// branch will be able to easily access the preferences
        /// "browser.startup.page", "browser.startup.homepage", or
        /// "browser.startup.homepage_override" by simply requesting
        /// "page", "homepage", or "homepage_override". nullptr or ""
        /// may be used to access to the entire preference "tree".
        ///
        /// @note
        /// Few consumers will want to create default branch objects. Many of the
        /// branch methods do nothing on a default branch because the operations only
        /// make sense when applied to user set preferences.
        ///
        /// @return nsIPrefBranch The object representing the requested default branch.
        ///
        /// @see getBranch
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsIPrefBranch GetDefaultBranch([MarshalAs(UnmanagedType.LPStr)] string aPrefRoot);
	}
}
