using System;
using System.Linq;

namespace Task2.Collections
{
        class Complex
    {
        #region Fields
        private double Re;
        private double Im;
        #endregion

        #region Constructors
        // Initializes a complex number with real and imaginary parts
        public Complex(double re, double im)
        {
            Re = re;
            Im = im;
        }
        #endregion

        #region Properties / Operations
        // Returns the real part
        public double GetReal() => Re;
        // Returns the imaginary part
        public double GetImag() => Im;
        // Returns the modulus (magnitude)
        public double Module() => Math.Sqrt(Re * Re + Im * Im);
        // Returns the argument (angle)
        public double Argument() => Math.Atan2(Im, Re);
        // String representation
        public override string ToString() => $"({Re} + {Im}i)";

        // Addition
        public static Complex operator +(Complex a, Complex b) =>
            new Complex(a.Re + b.Re, a.Im + b.Im);

        // Subtraction
        public static Complex operator -(Complex a, Complex b) =>
            new Complex(a.Re - b.Re, a.Im - b.Im);

        // Multiplication
        public static Complex operator *(Complex a, Complex b) =>
            new Complex(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + a.Im * b.Re);

        // Division
        public static Complex operator /(Complex a, Complex b)
        {
            double denom = b.Re * b.Re + b.Im * b.Im;
            if (denom == 0) throw new DivideByZeroException();
            return new Complex(
                (a.Re * b.Re + a.Im * b.Im) / denom,
                (a.Im * b.Re - a.Re * b.Im) / denom
            );
        }
        #endregion
    }
}
