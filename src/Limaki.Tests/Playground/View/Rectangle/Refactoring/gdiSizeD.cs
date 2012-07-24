/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Authors: 
 * Mike Kestner (mkestner@speakeasy.net)
 * 
 * Copyright (C) 2004 Novell, Inc (http://www.novell.com)
 *
 * http://www.limada.org
 * 
 */


/* 
 * this file is ported from
 * mono 2.4 - mcs\class\System.Drawing\System.Drawing\SizeF.cs
 */


using System.Globalization;
using System.Runtime.InteropServices;

namespace Xwt0 {
    [ComVisible(true)]
    public struct SizeD {
        // Private height and width fields.
        private double width, height;

        // -----------------------
        // Public Shared Members
        // -----------------------

        /// <summary>
        ///	Empty Shared Field
        /// </summary>
        ///
        /// <remarks>
        ///	An uninitialized SizeF Structure.
        /// </remarks>

        public static readonly SizeD Zero;

        /// <summary>
        ///	Addition Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Addition of two SizeF structures.
        /// </remarks>

        public static SizeD operator +(SizeD sz1, SizeD sz2) {
            return new SizeD(sz1.Width + sz2.Width,
                             sz1.Height + sz2.Height);
        }

        /// <summary>
        ///	Equality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two SizeF objects. The return value is
        ///	based on the equivalence of the Width and Height 
        ///	properties of the two Sizes.
        /// </remarks>

        public static bool operator ==(SizeD sz1, SizeD sz2) {
            return ((sz1.Width == sz2.Width) &&
                    (sz1.Height == sz2.Height));
        }

        /// <summary>
        ///	Inequality Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Compares two SizeF objects. The return value is
        ///	based on the equivalence of the Width and Height 
        ///	properties of the two Sizes.
        /// </remarks>

        public static bool operator !=(SizeD sz1, SizeD sz2) {
            return ((sz1.Width != sz2.Width) ||
                    (sz1.Height != sz2.Height));
        }

        /// <summary>
        ///	Subtraction Operator
        /// </summary>
        ///
        /// <remarks>
        ///	Subtracts two SizeF structures.
        /// </remarks>

        public static SizeD operator -(SizeD sz1, SizeD sz2) {
            return new SizeD(sz1.Width - sz2.Width,
                             sz1.Height - sz2.Height);
        }

        /// <summary>
        ///	SizeF to PointF Conversion
        /// </summary>
        ///
        /// <remarks>
        ///	Returns a PointF based on the dimensions of a given 
        ///	SizeF. Requires explicit cast.
        /// </remarks>

        public static explicit operator PointD(SizeD size) {
            return new PointD(size.Width, size.Height);
        }


        // -----------------------
        // Public Constructors
        // -----------------------

        /// <summary>
        ///	SizeF Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a SizeF from a PointF value.
        /// </remarks>

        public SizeD(PointD pt) {
            width = pt.X;
            height = pt.Y;
        }

        /// <summary>
        ///	SizeF Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a SizeF from an existing SizeF value.
        /// </remarks>

        public SizeD(SizeD size) {
            width = size.Width;
            height = size.Height;
        }

        /// <summary>
        ///	SizeF Constructor
        /// </summary>
        ///
        /// <remarks>
        ///	Creates a SizeF from specified dimensions.
        /// </remarks>

        public SizeD(double width, double height) {
            this.width = width;
            this.height = height;
        }

        // -----------------------
        // Public Instance Members
        // -----------------------

        /// <summary>
        ///	IsEmpty Property
        /// </summary>
        ///
        /// <remarks>
        ///	Indicates if both Width and Height are zero.
        /// </remarks>


        public bool IsEmpty {
            get {
                return ((width == 0.0) && (height == 0.0));
            }
        }

        /// <summary>
        ///	Width Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Width coordinate of the SizeF.
        /// </remarks>

        public double Width {
            get {
                return width;
            }
            set {
                width = value;
            }
        }

        /// <summary>
        ///	Height Property
        /// </summary>
        ///
        /// <remarks>
        ///	The Height coordinate of the SizeF.
        /// </remarks>

        public double Height {
            get {
                return height;
            }
            set {
                height = value;
            }
        }

        /// <summary>
        ///	Equals Method
        /// </summary>
        ///
        /// <remarks>
        ///	Checks equivalence of this SizeF and another object.
        /// </remarks>

        public override bool Equals(object obj) {
            if (!(obj is SizeD))
                return false;

            return (this == (SizeD)obj);
        }

        /// <summary>
        ///	GetHashCode Method
        /// </summary>
        ///
        /// <remarks>
        ///	Calculates a hashing value.
        /// </remarks>

        public override int GetHashCode() {
            return (int)width ^ (int)height;
        }

        public PointD ToPoint() {
            return new PointD(width, height);
        }

        public SizeD ToSize() {
            return new SizeD(width, height);
        }

        /// <summary>
        ///	ToString Method
        /// </summary>
        ///
        /// <remarks>
        ///	Formats the SizeF as a string in coordinate notation.
        /// </remarks>

        public override string ToString() {
            return string.Format("{{Width={0}, Height={1}}}", width.ToString(CultureInfo.CurrentCulture),
                                 height.ToString(CultureInfo.CurrentCulture));
        }


        public static SizeD Add(SizeD sz1, SizeD sz2) {
            return new SizeD(sz1.Width + sz2.Width,
                             sz1.Height + sz2.Height);
        }

        public static SizeD Subtract(SizeD sz1, SizeD sz2) {
            return new SizeD(sz1.Width - sz2.Width,
                             sz1.Height - sz2.Height);
        }

    }
}

//
// System.Drawing.SizeF.cs
//
// Author:
//   Mike Kestner (mkestner@speakeasy.net)
//
// Copyright (C) 2001 Mike Kestner
// Copyright (C) 2004 Novell, Inc. http://www.novell.com
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//