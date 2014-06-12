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
// Generated by IDLImporter from file nsITreeView.idl
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
    ///This Source Code Form is subject to the terms of the Mozilla Public
    /// License, v. 2.0. If a copy of the MPL was not distributed with this
    /// file, You can obtain one at http://mozilla.org/MPL/2.0/. </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("091116f0-0bdc-4b32-b9c8-c8d5a37cb088")]
	public interface nsITreeView
	{
		
		/// <summary>
        /// The total number of rows in the tree (including the offscreen rows).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetRowCountAttribute();
		
		/// <summary>
        /// The selection for this view.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		nsITreeSelection GetSelectionAttribute();
		
		/// <summary>
        /// The selection for this view.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetSelectionAttribute([MarshalAs(UnmanagedType.Interface)] nsITreeSelection aSelection);
		
		/// <summary>
        /// A whitespace delimited list of properties.  For each property X the view
        /// gives back will cause the pseudoclasses  ::-moz-tree-cell(x),
        /// ::-moz-tree-row(x), ::-moz-tree-twisty(x), ::-moz-tree-image(x),
        /// ::-moz-tree-cell-text(x).  to be matched on the pseudoelement
        /// ::moz-tree-row.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetRowProperties(int index, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// A whitespace delimited list of properties for a given cell.  Each
        /// property, x, that the view gives back will cause the pseudoclasses
        /// ::-moz-tree-cell(x), ::-moz-tree-row(x), ::-moz-tree-twisty(x),
        /// ::-moz-tree-image(x), ::-moz-tree-cell-text(x). to be matched on the
        /// cell.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCellProperties(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// Called to get properties to paint a column background.  For shading the sort
        /// column, etc.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetColumnProperties([MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// Methods that can be used to test whether or not a twisty should be drawn,
        /// and if so, whether an open or closed twisty should be used.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsContainer(int index);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsContainerOpen(int index);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsContainerEmpty(int index);
		
		/// <summary>
        /// isSeparator is used to determine if the row at index is a separator.
        /// A value of true will result in the tree drawing a horizontal separator.
        /// The tree uses the ::moz-tree-separator pseudoclass to draw the separator.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsSeparator(int index);
		
		/// <summary>
        /// Specifies if there is currently a sort on any column. Used mostly by dragdrop
        /// to affect drop feedback.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsSorted();
		
		/// <summary>
        /// Methods used by the drag feedback code to determine if a drag is allowable at
        /// the current location. To get the behavior where drops are only allowed on
        /// items, such as the mailNews folder pane, always return false when
        /// the orientation is not DROP_ON.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool CanDrop(int index, int orientation, [MarshalAs(UnmanagedType.Interface)] nsIDOMDataTransfer dataTransfer);
		
		/// <summary>
        /// Called when the user drops something on this view. The |orientation| param
        /// specifies before/on/after the given |row|.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Drop(int row, int orientation, [MarshalAs(UnmanagedType.Interface)] nsIDOMDataTransfer dataTransfer);
		
		/// <summary>
        /// Methods used by the tree to draw thread lines in the tree.
        /// getParentIndex is used to obtain the index of a parent row.
        /// If there is no parent row, getParentIndex returns -1.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetParentIndex(int rowIndex);
		
		/// <summary>
        /// hasNextSibling is used to determine if the row at rowIndex has a nextSibling
        /// that occurs *after* the index specified by afterIndex.  Code that is forced
        /// to march down the view looking at levels can optimize the march by starting
        /// at afterIndex+1.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool HasNextSibling(int rowIndex, int afterIndex);
		
		/// <summary>
        /// The level is an integer value that represents
        /// the level of indentation.  It is multiplied by the width specified in the
        /// :moz-tree-indentation pseudoelement to compute the exact indendation.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetLevel(int index);
		
		/// <summary>
        /// The image path for a given cell. For defining an icon for a cell.
        /// If the empty string is returned, the :moz-tree-image pseudoelement
        /// will be used.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetImageSrc(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetProgressMode(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// The value for a given cell. This method is only called for columns
        /// of type other than |text|.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCellValue(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// The text for a given cell.  If a column consists only of an image, then
        /// the empty string is returned.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void GetCellText(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// Called during initialization to link the view to the front end box object.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetTree([MarshalAs(UnmanagedType.Interface)] nsITreeBoxObject tree);
		
		/// <summary>
        /// Called on the view when an item is opened or closed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ToggleOpenState(int index);
		
		/// <summary>
        /// Called on the view when a header is clicked.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CycleHeader([MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// Should be called from a XUL onselect handler whenever the selection changes.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SelectionChanged();
		
		/// <summary>
        /// Called on the view when a cell in a non-selectable cycling column (e.g., unread/flag/etc.) is clicked.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void CycleCell(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// isEditable is called to ask the view if the cell contents are editable.
        /// A value of true will result in the tree popping up a text field when
        /// the user tries to inline edit the cell.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsEditable(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// isSelectable is called to ask the view if the cell is selectable.
        /// This method is only called if the selection style is |cell| or |text|.
        /// XXXvarga shouldn't this be called isCellSelectable?
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsSelectable(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// setCellValue is called when the value of the cell has been set by the user.
        /// This method is only called for columns of type other than |text|.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetCellValue(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase value);
		
		/// <summary>
        /// setCellText is called when the contents of the cell have been edited by the user.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetCellText(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase value);
		
		/// <summary>
        /// A command API that can be used to invoke commands on the selection.  The tree
        /// will automatically invoke this method when certain keys are pressed.  For example,
        /// when the DEL key is pressed, performAction will be called with the "delete" string.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PerformAction([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string action);
		
		/// <summary>
        /// A command API that can be used to invoke commands on a specific row.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PerformActionOnRow([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string action, int row);
		
		/// <summary>
        /// A command API that can be used to invoke commands on a specific cell.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void PerformActionOnCell([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string action, int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
	}
	
	/// <summary>nsITreeViewConsts </summary>
	public class nsITreeViewConsts
	{
		
		// 
		public const int DROP_BEFORE = -1;
		
		// 
		public const int DROP_ON = 0;
		
		// 
		public const int DROP_AFTER = 1;
		
		// <summary>
        // The progress mode for a given cell. This method is only called for
        // columns of type |progressmeter|.
        // </summary>
		public const int PROGRESS_NORMAL = 1;
		
		// 
		public const int PROGRESS_UNDETERMINED = 2;
		
		// 
		public const int PROGRESS_NONE = 3;
	}
	
	/// <summary>
    /// The following interface is not scriptable and MUST NEVER BE MADE scriptable.
    /// Native treeviews implement it, and we use this to check whether a treeview
    /// is native (and therefore suitable for use by untrusted content).
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("46c90265-6553-41ae-8d39-7022e7d09145")]
	public interface nsINativeTreeView : nsITreeView
	{
		
		/// <summary>
        /// The total number of rows in the tree (including the offscreen rows).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetRowCountAttribute();
		
		/// <summary>
        /// The selection for this view.
        /// </summary>
		[return: MarshalAs(UnmanagedType.Interface)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new nsITreeSelection GetSelectionAttribute();
		
		/// <summary>
        /// The selection for this view.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SetSelectionAttribute([MarshalAs(UnmanagedType.Interface)] nsITreeSelection aSelection);
		
		/// <summary>
        /// A whitespace delimited list of properties.  For each property X the view
        /// gives back will cause the pseudoclasses  ::-moz-tree-cell(x),
        /// ::-moz-tree-row(x), ::-moz-tree-twisty(x), ::-moz-tree-image(x),
        /// ::-moz-tree-cell-text(x).  to be matched on the pseudoelement
        /// ::moz-tree-row.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetRowProperties(int index, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// A whitespace delimited list of properties for a given cell.  Each
        /// property, x, that the view gives back will cause the pseudoclasses
        /// ::-moz-tree-cell(x), ::-moz-tree-row(x), ::-moz-tree-twisty(x),
        /// ::-moz-tree-image(x), ::-moz-tree-cell-text(x). to be matched on the
        /// cell.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetCellProperties(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// Called to get properties to paint a column background.  For shading the sort
        /// column, etc.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetColumnProperties([MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// Methods that can be used to test whether or not a twisty should be drawn,
        /// and if so, whether an open or closed twisty should be used.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool IsContainer(int index);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool IsContainerOpen(int index);
		
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool IsContainerEmpty(int index);
		
		/// <summary>
        /// isSeparator is used to determine if the row at index is a separator.
        /// A value of true will result in the tree drawing a horizontal separator.
        /// The tree uses the ::moz-tree-separator pseudoclass to draw the separator.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool IsSeparator(int index);
		
		/// <summary>
        /// Specifies if there is currently a sort on any column. Used mostly by dragdrop
        /// to affect drop feedback.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool IsSorted();
		
		/// <summary>
        /// Methods used by the drag feedback code to determine if a drag is allowable at
        /// the current location. To get the behavior where drops are only allowed on
        /// items, such as the mailNews folder pane, always return false when
        /// the orientation is not DROP_ON.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool CanDrop(int index, int orientation, [MarshalAs(UnmanagedType.Interface)] nsIDOMDataTransfer dataTransfer);
		
		/// <summary>
        /// Called when the user drops something on this view. The |orientation| param
        /// specifies before/on/after the given |row|.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void Drop(int row, int orientation, [MarshalAs(UnmanagedType.Interface)] nsIDOMDataTransfer dataTransfer);
		
		/// <summary>
        /// Methods used by the tree to draw thread lines in the tree.
        /// getParentIndex is used to obtain the index of a parent row.
        /// If there is no parent row, getParentIndex returns -1.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetParentIndex(int rowIndex);
		
		/// <summary>
        /// hasNextSibling is used to determine if the row at rowIndex has a nextSibling
        /// that occurs *after* the index specified by afterIndex.  Code that is forced
        /// to march down the view looking at levels can optimize the march by starting
        /// at afterIndex+1.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool HasNextSibling(int rowIndex, int afterIndex);
		
		/// <summary>
        /// The level is an integer value that represents
        /// the level of indentation.  It is multiplied by the width specified in the
        /// :moz-tree-indentation pseudoelement to compute the exact indendation.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetLevel(int index);
		
		/// <summary>
        /// The image path for a given cell. For defining an icon for a cell.
        /// If the empty string is returned, the :moz-tree-image pseudoelement
        /// will be used.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetImageSrc(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new int GetProgressMode(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// The value for a given cell. This method is only called for columns
        /// of type other than |text|.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetCellValue(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// The text for a given cell.  If a column consists only of an image, then
        /// the empty string is returned.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void GetCellText(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase retval);
		
		/// <summary>
        /// Called during initialization to link the view to the front end box object.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SetTree([MarshalAs(UnmanagedType.Interface)] nsITreeBoxObject tree);
		
		/// <summary>
        /// Called on the view when an item is opened or closed.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void ToggleOpenState(int index);
		
		/// <summary>
        /// Called on the view when a header is clicked.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void CycleHeader([MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// Should be called from a XUL onselect handler whenever the selection changes.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SelectionChanged();
		
		/// <summary>
        /// Called on the view when a cell in a non-selectable cycling column (e.g., unread/flag/etc.) is clicked.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void CycleCell(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// isEditable is called to ask the view if the cell contents are editable.
        /// A value of true will result in the tree popping up a text field when
        /// the user tries to inline edit the cell.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool IsEditable(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// isSelectable is called to ask the view if the cell is selectable.
        /// This method is only called if the selection style is |cell| or |text|.
        /// XXXvarga shouldn't this be called isCellSelectable?
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new bool IsSelectable(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// setCellValue is called when the value of the cell has been set by the user.
        /// This method is only called for columns of type other than |text|.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SetCellValue(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase value);
		
		/// <summary>
        /// setCellText is called when the contents of the cell have been edited by the user.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void SetCellText(int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col, [MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.AStringMarshaler")] nsAStringBase value);
		
		/// <summary>
        /// A command API that can be used to invoke commands on the selection.  The tree
        /// will automatically invoke this method when certain keys are pressed.  For example,
        /// when the DEL key is pressed, performAction will be called with the "delete" string.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void PerformAction([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string action);
		
		/// <summary>
        /// A command API that can be used to invoke commands on a specific row.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void PerformActionOnRow([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string action, int row);
		
		/// <summary>
        /// A command API that can be used to invoke commands on a specific cell.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		new void PerformActionOnCell([MarshalAs(UnmanagedType.CustomMarshaler, MarshalType = "Gecko.CustomMarshalers.WStringMarshaler")] string action, int row, [MarshalAs(UnmanagedType.Interface)] nsITreeColumn col);
		
		/// <summary>
        /// The following interface is not scriptable and MUST NEVER BE MADE scriptable.
        /// Native treeviews implement it, and we use this to check whether a treeview
        /// is native (and therefore suitable for use by untrusted content).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void EnsureNative();
	}
}
