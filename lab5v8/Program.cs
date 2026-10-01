using System;
using System.Text;

namespace lab5v8
{
    public class Polynomial
    {
        private double[] _coefficients;

        public int Degree => _coefficients.Length > 0 ? _coefficients.Length - 1 : 0;

        public Polynomial(int maxDegree)
        {
            if (maxDegree < 0)
                throw new ArgumentOutOfRangeException(nameof(maxDegree), "Степінь не може бути від'ємним.");

            _coefficients = new double[maxDegree + 1];
        }

        public Polynomial(double[] coefficients)
        {
            ArgumentNullException.ThrowIfNull(coefficients);
            _coefficients = coefficients.Length == 0 ? new double[] { 0 } : (double[])coefficients.Clone();
        }

        // Індексатор для доступу до коефіцієнта за степенем
        public double this[int degree]
        {
            get
            {
                if (degree < 0 || degree >= _coefficients.Length)
                    return 0;
                return _coefficients[degree];
            }
            set
            {
                if (degree < 0)
                    throw new ArgumentOutOfRangeException(nameof(degree), "Степінь не може бути від'ємним.");

                if (degree >= _coefficients.Length)
                {
                    Array.Resize(ref _coefficients, degree + 1);
                }
                _coefficients[degree] = value;
            }
        }

        public double Evaluate(double x)
        {
            double result = 0;
            for (int i = _coefficients.Length - 1; i >= 0; i--)
            {
                result = result * x + _coefficients[i];
            }
            return result;
        }

        public static Polynomial operator +(Polynomial p1, Polynomial p2)
        {
            ArgumentNullException.ThrowIfNull(p1);
            ArgumentNullException.ThrowIfNull(p2);

            int maxDeg = Math.Max(p1.Degree, p2.Degree);
            double[] resultCoeffs = new double[maxDeg + 1];

            for (int i = 0; i <= maxDeg; i++)
            {
                resultCoeffs[i] = p1[i] + p2[i];
            }

            return new Polynomial(resultCoeffs);
        }

        public static Polynomial operator *(Polynomial p, double scalar)
        {
            ArgumentNullException.ThrowIfNull(p);

            double[] resultCoeffs = new double[p._coefficients.Length];
            for (int i = 0; i < p._coefficients.Length; i++)
            {
                resultCoeffs[i] = p._coefficients[i] * scalar;
            }

            return new Polynomial(resultCoeffs);
        }

        public static Polynomial operator *(double scalar, Polynomial p) => p * scalar;

        public static bool operator ==(Polynomial? p1, Polynomial? p2)
        {
            if (ReferenceEquals(p1, p2)) return true;
            if (p1 is null || p2 is null) return false;

            int maxDeg = Math.Max(p1.Degree, p2.Degree);
            for (int i = 0; i <= maxDeg; i++)
            {
                if (Math.Abs(p1[i] - p2[i]) > 1e-9) return false;
            }

            return true;
        }

        public static bool operator !=(Polynomial? p1, Polynomial? p2) => !(p1 == p2);

        public override bool Equals(object? obj)
        {
            if (obj is Polynomial other)
            {
                return this == other;
            }
            return false;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            for (int i = 0; i <= Degree; i++)
            {
                hash.Add(this[i]);
            }
            return hash.ToHashCode();
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            bool isFirst = true;

            for (int i = Degree; i >= 0; i--)
            {
                double coeff = _coefficients[i];
                if (Math.Abs(coeff) < 1e-9 && Degree > 0) continue;

                if (!isFirst)
                {
                    sb.Append(coeff >= 0 ? " + " : " - ");
                }
                else if (coeff < 0)
                {
                    sb.Append("-");
                }

                double absCoeff = Math.Abs(coeff);
                if (Math.Abs(absCoeff - 1.0) > 1e-9 || i == 0)
                {
                    sb.Append(absCoeff);
                }

                if (i > 0)
                {
                    sb.Append("x");
                    if (i > 1)
                    {
                        sb.Append($"^{i}");
                    }
                }

                isFirst = false;
            }

            return sb.Length == 0 ? "0" : sb.ToString();
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Polynomial p1 = new Polynomial(new double[] { 1, 2, 3 });
            Polynomial p2 = new Polynomial(new double[] { 4, 5 });

            Console.WriteLine($"Поліном p1: {p1}");
            Console.WriteLine($"Поліном p2: {p2}\n");

            // Демонстрація індексатора
            Console.WriteLine($"Коефіцієнт біля x^2 в p1: {p1[2]}");
            p1[2] = 10;
            p1[3] = 4;
            Console.WriteLine($"Оновлений p1: {p1}\n");

            // Метод Evaluate
            double xVal = 2.0;
            Console.WriteLine($"Значення p2({xVal}): {p2.Evaluate(xVal)}\n");

            // Оператор +
            Polynomial pSum = p1 + p2;
            Console.WriteLine($"p1 + p2 = {pSum}\n");

            // Оператор *
            Polynomial pScaled = p2 * 3;
            Console.WriteLine($"p2 * 3 = {pScaled}\n");

            // Оператори == та !=
            Polynomial p3 = new Polynomial(new double[] { 4, 5 });
            Console.WriteLine($"p2 == p3: {p2 == p3}");
            Console.WriteLine($"p1 != p2: {p1 != p2}");
        }
    }
}