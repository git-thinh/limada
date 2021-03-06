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
// Generated by IDLImporter from file imgIContainer.idl
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
    /// imgIContainer is the interface that represents an image. It allows
    /// access to frames as Thebes surfaces. It also allows drawing of images
    /// onto Thebes contexts.
    ///
    /// Internally, imgIContainer also manages animation of images.
    /// </summary>
	[ComImport()]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8b7db7dd-bfe9-40d3-9114-3a79c0658afd")]
	public interface imgIContainer
	{
		
		/// <summary>
        /// The width of the container rectangle.  In the case of any error,
        /// zero is returned, and an exception will be thrown.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetWidthAttribute();
		
		/// <summary>
        /// The height of the container rectangle.  In the case of any error,
        /// zero is returned, and an exception will be thrown.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetHeightAttribute();
		
		/// <summary>
        /// The intrinsic size of this image in appunits. If the image has no intrinsic
        /// size in a dimension, -1 will be returned for that dimension. In the case of
        /// any error, an exception will be thrown.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetIntrinsicSizeAttribute();
		
		/// <summary>
        /// The (dimensionless) intrinsic ratio of this image. In the case of any error,
        /// an exception will be thrown.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		uint GetIntrinsicRatioAttribute();
		
		/// <summary>
        /// The type of this image (one of the TYPE_* values above).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ushort GetTypeAttribute();
		
		/// <summary>
        /// Direct C++ accessor for 'type' attribute, for convenience.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ushort GetType();
		
		/// <summary>
        /// Whether this image is animated. You can only be guaranteed that querying
        /// this will not throw if STATUS_DECODE_COMPLETE is set on the imgIRequest.
        ///
        /// @throws NS_ERROR_NOT_AVAILABLE if the animated state cannot be determined.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool GetAnimatedAttribute();
		
		/// <summary>
        /// Get a surface for the given frame. This may be a platform-native,
        /// optimized surface, so you cannot inspect its pixel data. If you
        /// need that, use gfxASurface::GetAsReadableARGB32ImageSurface or
        /// gfxASurface::CopyToARGB32ImageSurface.
        ///
        /// @param aWhichFrame Frame specifier of the FRAME_* variety.
        /// @param aFlags Flags of the FLAG_* variety
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetFrame(uint aWhichFrame, uint aFlags);
		
		/// <summary>
        /// Whether the given frame is opaque; that is, needs the background painted
        /// behind it.
        ///
        /// @param aWhichFrame Frame specifier of the FRAME_* variety.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool FrameIsOpaque(uint aWhichFrame);
		
		/// <summary>
        /// Attempts to create an ImageContainer (and Image) containing the current
        /// frame. Only valid for RASTER type images.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetImageContainer(System.IntPtr aManager);
		
		/// <summary>
        /// Draw a frame onto the context specified.
        ///
        /// @param aContext The Thebes context to draw the image to.
        /// @param aFilter The filter to be used if we're scaling the image.
        /// @param aUserSpaceToImageSpace The transformation from user space (e.g.,
        /// appunits) to image space.
        /// @param aFill The area in the context to draw pixels to. When aFlags includes
        /// FLAG_CLAMP, the image will be extended to this area by clampling
        /// image sample coordinates. Otherwise, the image will be
        /// automatically tiled as necessary.
        /// @param aSubimage The area of the image, in pixels, that we are allowed to
        /// sample from.
        /// @param aViewportSize
        /// The size (in CSS pixels) of the viewport that would be available
        /// for the full image to occupy, if we were drawing the full image.
        /// (Note that we might not actually be drawing the full image -- we
        /// might be restricted by aSubimage -- but we still need the full
        /// image's viewport-size in order for SVG images with the "viewBox"
        /// attribute to position their content correctly.)
        /// @param aSVGContext If non-null, SVG-related rendering context such as
        /// overridden attributes on the image document's root <svg>
        /// node. Ignored for raster images.
        /// @param aWhichFrame Frame specifier of the FRAME_* variety.
        /// @param aFlags Flags of the FLAG_* variety
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void Draw(gfxContext aContext, gfxGraphicsFilter aFilter, gfxMatrix aUserSpaceToImageSpace, gfxRect aFill, [MarshalAs(UnmanagedType.Interface)] nsIntRect aSubimage, uint aViewportSize, System.IntPtr aSVGContext, uint aWhichFrame, uint aFlags);
		
		/// <summary>
        /// Ensures that an image is decoding. Calling this function guarantees that
        /// the image will at some point fire off decode notifications. Calling draw()
        /// or getFrame() triggers the same mechanism internally. Thus, if you want to
        /// be sure that the image will be decoded but don't want to access it until
        /// then, you must call requestDecode().
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RequestDecode();
		
		/// <summary>
        /// This is equivalent to requestDecode() but it also decodes some of the image.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void StartDecoding();
		
		/// <summary>
        /// Returns true if no more decoding can be performed on this image. Images
        /// with errors return true since they cannot be decoded any further. Note that
        /// because decoded images may be discarded, isDecoded() may return false even
        /// if it has returned true in the past.
        /// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		bool IsDecoded();
		
		/// <summary>
        /// Increments the lock count on the image. An image will not be discarded
        /// as long as the lock count is nonzero. Note that it is still possible for
        /// the image to be undecoded if decode-on-draw is enabled and the image
        /// was never drawn.
        ///
        /// Upon instantiation images have a lock count of zero.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void LockImage();
		
		/// <summary>
        /// Decreases the lock count on the image. If the lock count drops to zero,
        /// the image is allowed to discard its frame data to save memory.
        ///
        /// Upon instantiation images have a lock count of zero. It is an error to
        /// call this method without first having made a matching lockImage() call.
        /// In other words, the lock count is not allowed to be negative.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void UnlockImage();
		
		/// <summary>
        /// If this image is unlocked, discard its decoded data.  If the image is
        /// locked or has already been discarded, do nothing.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RequestDiscard();
		
		/// <summary>
        /// Indicates that this imgIContainer has been triggered to update
        /// its internal animation state. Likely this should only be called
        /// from within nsImageFrame or objects of similar type.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void RequestRefresh(ulong aTime);
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		ushort GetAnimationModeAttribute();
		
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAnimationModeAttribute(ushort aAnimationMode);
		
		/// <summary>
        ///Methods to control animation </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void ResetAnimation();
		
		/// <summary>
        /// Returns an index for the requested animation frame (either FRAME_FIRST or
        /// FRAME_CURRENT).
        ///
        /// The units of the index aren't specified, and may vary between different
        /// types of images. What you can rely on is that on all occasions when
        /// getFrameIndex(FRAME_CURRENT) returns a certain value,
        /// draw(..FRAME_CURRENT..) will draw the same frame. The same holds for
        /// FRAME_FIRST as well.
        ///
        /// @param aWhichFrame Frame specifier of the FRAME_* variety.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		float GetFrameIndex(uint aWhichFrame);
		
		/// <summary>
        /// Returns the inherent orientation of the image, as described in the image's
        /// metadata (e.g. EXIF).
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		System.IntPtr GetOrientation();
		
		/// <summary>
        /// Returns the delay, in ms, between the first and second frame. If this
        /// returns 0, there is no delay between first and second frame (i.e., this
        /// image could render differently whenever it draws).
        ///
        /// If this image is not animated, or not known to be animated (see attribute
        /// animated), returns -1.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		int GetFirstFrameDelay();
		
		/// <summary>
        /// If this is an animated image that hasn't started animating already, this
        /// sets the animation's start time to the indicated time.
        ///
        /// This has no effect if the image isn't animated or it has started animating
        /// already; it also has no effect if the image format doesn't care about
        /// animation start time.
        ///
        /// In all cases, animation does not actually begin until startAnimation(),
        /// resetAnimation(), or requestRefresh() is called for the first time.
        /// </summary>
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType=MethodCodeType.Runtime)]
		void SetAnimationStartTime(ulong aTime);
	}
	
	/// <summary>imgIContainerConsts </summary>
	public class imgIContainerConsts
	{
		
		// <summary>
        // Enumerated values for the 'type' attribute (below).
        // </summary>
		public const ulong TYPE_RASTER = 0;
		
		// 
		public const ulong TYPE_VECTOR = 1;
		
		// <summary>
        // Flags for imgIContainer operations.
        //
        // Meanings:
        //
        // FLAG_NONE: Lack of flags
        //
        // FLAG_SYNC_DECODE: Forces synchronous/non-progressive decode of all
        // available data before the call returns. It is an error to pass this flag
        // from a call stack that originates in a decoder (ie, from a decoder
        // observer event).
        //
        // FLAG_DECODE_NO_PREMULTIPLY_ALPHA: Do not premultiply alpha if
        // it's not already premultiplied in the image data.
        //
        // FLAG_DECODE_NO_COLORSPACE_CONVERSION: Do not do any colorspace conversion;
        // ignore any embedded profiles, and don't convert to any particular destination
        // space.
        //
        // FLAG_CLAMP: Extend the image to the fill area by clamping image sample
        // coordinates instead of by tiling. This only affects 'draw'.
        //
        // FLAG_HIGH_QUALITY_SCALING: A hint as to whether this image should be
        // scaled using the high quality scaler. Do not set this if not drawing to
        // a window or not listening to invalidations.
        // </summary>
		public const long FLAG_NONE = 0x0;
		
		// 
		public const long FLAG_SYNC_DECODE = 0x1;
		
		// 
		public const long FLAG_DECODE_NO_PREMULTIPLY_ALPHA = 0x2;
		
		// 
		public const long FLAG_DECODE_NO_COLORSPACE_CONVERSION = 0x4;
		
		// 
		public const long FLAG_CLAMP = 0x8;
		
		// 
		public const long FLAG_HIGH_QUALITY_SCALING = 0x10;
		
		// <summary>
        // Constants for specifying various "special" frames.
        //
        // FRAME_FIRST: The first frame
        // FRAME_CURRENT: The current frame
        //
        // FRAME_MAX_VALUE should be set to the value of the maximum constant above,
        // as it is used for ensuring that a valid value was passed in.
        // </summary>
		public const ulong FRAME_FIRST = 0;
		
		// 
		public const ulong FRAME_CURRENT = 1;
		
		// 
		public const ulong FRAME_MAX_VALUE = 1;
		
		// <summary>
        // Animation mode Constants
        // 0 = normal
        // 1 = don't animate
        // 2 = loop once
        // </summary>
		public const int kNormalAnimMode = 0;
		
		// 
		public const int kDontAnimMode = 1;
		
		// 
		public const int kLoopOnceAnimMode = 2;
	}
}
