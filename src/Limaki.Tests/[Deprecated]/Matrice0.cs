/*
 * Limaki 
 * 
 * This code is free software; you can redistribute it and/or modify it
 * under the terms of the GNU General Public License version 2 only, as
 * published by the Free Software Foundation.
 * 
 * Author: Lytico
 * Copyright (C) 2006-2011 Lytico
 *
 * http://www.limada.org
 * 
 */


using System;
using Xwt;

namespace Limaki.Drawing {

    public class Matrice0 : IDisposable {
        // Internal state.
        /// <summary>
        /// scaleX
        /// </summary>
        protected double m11;
        /// <summary>
        /// shearX
        /// </summary>
        protected double m12;
        /// <summary>
        /// shearY
        /// </summary>
        protected double m21;
        /// <summary>
        /// scaleY
        /// </summary>
        protected double m22;

        protected double dx;
        protected double dy;

        // Constructors.
        public Matrice0() {
            m11 = 1.0d;
            m12 = 0.0d;
            m21 = 0.0d;
            m22 = 1.0d;
            dx = 0.0d;
            dy = 0.0d;
        }
        
       
        public Matrice0(Rectangle rect, Point[] plgpts) {
            TransfRect2Poly(rect, plgpts);
        }
        // helper method, computes transformation from rectangle rect to polygon  plgpts
        private void TransfRect2Poly(Rectangle rect, Point[] plgpts) {
            // check if plgpts defines a polygon with 3 points
            if ((plgpts == null) || (plgpts.Length != 3))
                throw new ArgumentNullException("plgpts", "Argument cannot be null");
            // check if rect is degenerated
            if ((rect.Width == 0.0d) || (rect.Height == 0.0d))
                throw new ArgumentOutOfRangeException("rect");
            // compute transformation of rect to plgpts
            var v1 = new Point(
                plgpts[1].X - plgpts[0].X,
                plgpts[1].Y - plgpts[0].Y
                );
            var v2 = new Point(
                plgpts[2].X - plgpts[0].X,
                plgpts[2].Y - plgpts[0].Y
                );
            this.dx = plgpts[0].X - rect.X / rect.Width * v1.X - rect.Y / rect.Height * v2.X;
            this.dy = plgpts[0].Y - rect.X / rect.Width * v1.Y - rect.Y / rect.Height * v2.Y;
            this.m11 = v1.X / rect.Width;
            this.m12 = v1.Y / rect.Width;
            this.m21 = v2.X / rect.Height;
            this.m22 = v2.Y / rect.Height;
        }

        public Matrice0 Clone() {
            return new Matrice0(this);
        }

        public Matrice0(double m11, double m12, double m21, double m22,
                      double dx, double dy) {
            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
            this.dx = dx;
            this.dy = dy;
        }

        internal Matrice0(Matrice0 matrice) {
            this.m11 = matrice.m11;
            this.m12 = matrice.m12;
            this.m21 = matrice.m21;
            this.m22 = matrice.m22;
            this.dx = matrice.dx;
            this.dy = matrice.dy;
        }

        /// <summary>
        /// Get the elements of this matrix:
        /// {[0]=scaleX, [1]=shearX, [2]=shearY, [3]=scaleY, [4]=dx, [5]=dy}
        /// </summary>
        public double[] Elements {
            get {
                return new double[] {m11, m12, m21, m22, dx, dy};
            }
            set {
                m11 = value[0];
                m12 = value[1];
                m21 = value[2];
                m22 = value[3];
                dx = value[4];
                dy = value[5];
            }
        }

        // Determine if this is the identity matrix.
        public bool IsIdentity {
            get {
                return (m11 == 1.0d && m12 == 0.0d &&
                        m21 == 0.0d && m22 == 1.0d &&
                        dx == 0.0d && dy == 0.0d);
            }
        }

        // Determine if the matrix is invertible.
        public bool IsInvertible {
            get {
                return (Determinant() != 0.0d);
            }
        }

        // Get the X offset value.
        public double OffsetX {
            get {
                return dx;
            }
        }

        // Get the Y offset value.
        public double OffsetY {
            get {
                return dy;
            }
        }

        // Dispose of this matrix.
        public void Dispose() {
            // Nothing to do here because there is no disposable state.
        }

        // Determine if two matrices are equal.
        public override bool Equals(Object obj) {
            var other = (obj as Matrice0);
            if (other != null) {
                return (other.m11 == m11 && other.m12 == m12 &&
                        other.m21 == m21 && other.m22 == m22 &&
                        other.dx == dx && other.dy == dy);
            } else {
                return false;
            }
        }

        // Get a hash code for this object.
        public override int GetHashCode() {
            return (int)(m11 + m12 + m21 + m22 + dx + dy);
        }

        // Invert this matrix.
        public void Invert() {
            double m11, m12, m21, m22, dx, dy;
            double determinant;

            // Compute the determinant and check it.
            determinant = Determinant();
            if (determinant != 0.0d) {
                // Get the answer into temporary variables.
                m11 = this.m22 / determinant;
                m12 = -(this.m12 / determinant);
                m21 = -(this.m21 / determinant);
                m22 = this.m11 / determinant;
                dx = (this.m21 * this.dy - this.m22 * this.dx)
                     / determinant;
                dy = (this.m12 * this.dx - this.m11 * this.dy)
                     / determinant;

                // Write the temporary variables back to the matrix.
                this.m11 = m11;
                this.m12 = m12;
                this.m21 = m21;
                this.m22 = m22;
                this.dx = dx;
                this.dy = dy;
            }
        }

        // Multiply two matrices and write the result into this one.
        private void Multiply(Matrice0 matrix1, Matrice0 matrix2) {
            double m11, m12, m21, m22, dx, dy;

            // Compute the result within temporary variables,
            // to prevent overwriting "matrix1" or "matrix2",
            // during the calculation, as either may be "this".
            m11 = matrix1.m11 * matrix2.m11 +
                  matrix1.m12 * matrix2.m21;
            m12 = matrix1.m11 * matrix2.m12 +
                  matrix1.m12 * matrix2.m22;
            m21 = matrix1.m21 * matrix2.m11 +
                  matrix1.m22 * matrix2.m21;
            m22 = matrix1.m21 * matrix2.m12 +
                  matrix1.m22 * matrix2.m22;
            dx = matrix1.dx * matrix2.m11 +
                 matrix1.dy * matrix2.m21 +
                 matrix2.dx;
            dy = matrix1.dx * matrix2.m12 +
                 matrix1.dy * matrix2.m22 +
                 matrix2.dy;

            // Write the result back into the "this" object.
            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
            this.dx = dx;
            this.dy = dy;
        }

        // Multiply two matrices.
        public void Multiply(Matrice0 matrice) {
            if (matrice == null) {
                throw new ArgumentNullException("matrix");
            }
            Multiply(matrice, this);
        }
        public void Multiply(Matrice0 matrice, MatrixOrder order) {
            if (matrice == null) {
                throw new ArgumentNullException("matrix");
            }
            if (order == MatrixOrder.Prepend) {
                Multiply(matrice, this);
            } else {
                Multiply(this, matrice);
            }
        }

        // Reset this matrix to the identity matrix.
        public void SetIdentity() {
            m11 = 1.0d;
            m12 = 0.0d;
            m21 = 0.0d;
            m22 = 1.0d;
            dx = 0.0d;
            dy = 0.0d;
        }

        // Perform a rotation on this matrix.
        // lytico: PI/180 as const
        public const double PiDiv180 = Math.PI/180.0d;
        public void Rotate(double angle) {

            double m11, m12, m21, m22;

            var radians = (angle * PiDiv180);
            var cos = (Math.Cos(radians));
            var sin = (Math.Sin(radians));

            m11 = cos * this.m11 + sin * this.m21;
            m12 = cos * this.m12 + sin * this.m22;
            m21 = cos * this.m21 - sin * this.m11;
            m22 = cos * this.m22 - sin * this.m12;

            this.m11 = m11;
            this.m22 = m22;
            this.m12 = m12;
            this.m21 = m21;
            

        }
        public void Rotate(double angle, MatrixOrder order) {

            double m11, m12, m21, m22, dx, dy;

            var radians = (angle * (PiDiv180));
            var cos = (double)(Math.Cos(radians));
            var sin = (double)(Math.Sin(radians));
	
            if(order == MatrixOrder.Prepend)
            {
                m11 = cos * this.m11 + sin * this.m21;
                m12 = cos * this.m12 + sin * this.m22;
                m21 = cos * this.m21 - sin * this.m11;
                m22 = cos * this.m22 - sin * this.m12;

                this.m11 = m11;
                this.m12 = m12;
                this.m21 = m21;
                this.m22 = m22;
            }
            else
            {
                m11 = this.m11 * cos - this.m12 * sin;
                m12 = this.m11 * sin + this.m12 * cos;
                m21 = this.m21 * cos - this.m22 * sin;
                m22 = this.m21 * sin + this.m22 * cos;
                dx  = this.dx  * cos - this.dy  * sin;
                dy  = this.dx  * sin + this.dy  * cos;

                this.m11 = m11;
                this.m12 = m12;
                this.m21 = m21;
                this.m22 = m22;
                this.dx  = dx;
                this.dy  = dy;
            }

        }

        // Rotate about a specific point.
        public void RotateAt(double angle, Point point) {
            Translate(point.X, point.Y);
            Rotate(angle);
            Translate(-point.X, -point.Y);
        }
        public void RotateAt(double angle, Point point, MatrixOrder order) {
            if (order == MatrixOrder.Prepend) {
                Translate(point.X, point.Y);
                Rotate(angle);
                Translate(-point.X, -point.Y);
            } else {
                Translate(-point.X, -point.Y);
                Rotate(angle, MatrixOrder.Append);
                Translate(point.X, point.Y);
            }
        }

        // Apply a scale factor to this matrix.
        public void Scale(double scaleX, double scaleY) {
            m11 *= scaleX;
            m22 *= scaleY;
            m12 *= scaleX;
            m21 *= scaleY;
        }
        public void Scale(double scaleX, double scaleY, MatrixOrder order) {
            if (order == MatrixOrder.Prepend) {
                m11 *= scaleX;
                m12 *= scaleX;
                m21 *= scaleY;
                m22 *= scaleY;
            } else {
                m11 *= scaleX;
                m12 *= scaleY;
                m21 *= scaleX;
                m22 *= scaleY;
                dx *= scaleX;
                dy *= scaleY;
            }
        }

        // Apply a shear factor to this matrix.
        public void Shear(double shearX, double shearY) {
            double m11, m12, m21, m22;

            m11 = this.m11 + this.m21 * shearY;
            m12 = this.m12 + this.m22 * shearY;
            m21 = this.m11 * shearX + this.m21;
            m22 = this.m12 * shearX + this.m22;

            this.m11 = m11;
            this.m12 = m12;
            this.m21 = m21;
            this.m22 = m22;
        }

        public void Shear(double shearX, double shearY, MatrixOrder order) {
            if (order == MatrixOrder.Prepend) {
                double m11, m12, m21, m22;

                m11 = this.m11 + this.m21 * shearY;
                m12 = this.m12 + this.m22 * shearY;
                m21 = this.m11 * shearX + this.m21;
                m22 = this.m12 * shearX + this.m22;

                this.m11 = m11;
                this.m12 = m12;
                this.m21 = m21;
                this.m22 = m22;
            } else {
                double m11, m12, m21, m22, dx, dy;

                m11 = this.m11 + this.m12 * shearX;
                m12 = this.m11 * shearY + this.m12;
                m21 = this.m21 + this.m22 * shearX;
                m22 = this.m21 * shearY + this.m22;
                dx = this.dx + this.dy * shearX;
                dy = this.dx * shearY + this.dy;

                this.m11 = m11;
                this.m12 = m12;
                this.m21 = m21;
                this.m22 = m22;
                this.dx = dx;
                this.dy = dy;
            }
        }

        
        public void Transform(Point[] pts) {
            if (pts == null) {
                throw new ArgumentNullException("pts");
            }
            int posn;
            double x, y;
            for (posn = pts.Length - 1; posn >= 0; --posn) {
                x = pts[posn].X;
                y = pts[posn].Y;
                pts[posn].X = x * m11 + y * m21 + dx;
                pts[posn].Y = x * m12 + y * m22 + dy;
            }
        }

        // Transform a list of vectors.
        
        public void TransformVectors(Point[] pts) {
            if (pts == null) {
                throw new ArgumentNullException("pts");
            }
            int posn;
            double x, y;
            for (posn = pts.Length - 1; posn >= 0; --posn) {
                x = pts[posn].X;
                y = pts[posn].Y;
                pts[posn].X = x * m11 + y * m21;
                pts[posn].Y = x * m12 + y * m22;
            }
        }

        public void VectorTransformPoints(Point[] pts) {
            TransformVectors(pts);
        }
        

        // Translate the matrix.
        public void Translate(double offsetX, double offsetY) {
            dx += offsetX * m11 + offsetY * m21;
            dy += offsetX * m12 + offsetY * m22;
        }
        public void Translate(double offsetX, double offsetY, MatrixOrder order) {
            if (order == MatrixOrder.Prepend) {
                dx += offsetX * m11 + offsetY * m21;
                dy += offsetX * m12 + offsetY * m22;
            } else {
                dx += offsetX;
                dy += offsetY;
            }
        }

        // Clone a matrix.
        public static Matrice0 Clone(Matrice0 matrice) {
            if (matrice != null) {
                return new Matrice0(matrice.m11, matrice.m12,
                                  matrice.m21, matrice.m22,
                                  matrice.dx, matrice.dy);
            } else {
                return null;
            }
        }

        // Transform a particular point - faster version for only one point.
        internal void TransformPoint(double x, double y, out double ox, out double oy) {
            ox = x * m11 + y * m21 + dx;
            oy = x * m12 + y * m22 + dy;
        }

        // Transform a size value according to the inverse transformation.
        internal void TransformSizeBack(double width, double height,
                                        out double owidth, out double oheight) {
            double m11, m12, m21, m22;
            double determinant;

            // Compute the determinant and check it.
            determinant = Determinant();
            if (determinant != 0.0d) {
                // Get the answer into temporary variables.
                // We ignore dx and dy because we don't need them.
                m11 = this.m22 / determinant;
                m12 = -(this.m12 / determinant);
                m21 = -(this.m21 / determinant);
                m22 = this.m11 / determinant;

                // Compute the final values.
                owidth = width * m11 + height * m21;
                oheight = width * m12 + height * m22;
            } else {
                owidth = width;
                oheight = height;
            }
        }
        // private helper method to compute the determinant
        private double Determinant() {
            return this.m11 * this.m22 - this.m12 * this.m21;
        }

        // Workaround for calculation new font size, if a transformation is set
        // this does only work for scaling, not for rotation or multiply transformations
        // Normally we should stretch or shrink the font.
        public double TransformFontSize(double fIn) {
            return Math.Abs(Math.Min(this.m11, this.m22) * fIn);
        }


    }

    public enum MatrixOrder {
        Prepend = 0,
        Append = 1
    }

}


