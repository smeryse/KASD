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
        
        public Complex(double re, double im)
        {
            Re = re;
            Im = im;
        }
        #endregion

        #region Properties / Operations
        
        public double GetReal() => Re;
        
        public double GetImag() => Im;
        
        public double Module() => Math.Sqrt(Re * Re + Im * Im);
        
        public double Argument() => Math.Atan2(Im, Re);
        
        public override string ToString() => $"({Re} + {Im}i)";

        
        public static Complex operator +(Complex a, Complex b) =>
            new Complex(a.Re + b.Re, a.Im + b.Im);

        
        public static Complex operator -(Complex a, Complex b) =>
            new Complex(a.Re - b.Re, a.Im - b.Im);

        
        public static Complex operator *(Complex a, Complex b) =>
            new Complex(a.Re * b.Re - a.Im * b.Im, a.Re * b.Im + a.Im * b.Re);

        
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
