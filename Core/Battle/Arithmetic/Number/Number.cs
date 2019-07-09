﻿
using System;

namespace Battle {

[Serializable]
public partial struct Number : IEquatable<Number>, IComparable<Number> {

    public long _serializedValue;

    private const long MAX_VALUE = long.MaxValue;
    private const long MIN_VALUE = long.MinValue;
    private const int NUM_BITS = 64;
    private const int FRACTIONAL_PLACES = 32;
    private const long ONE = 1L << FRACTIONAL_PLACES;
    private const long TEN = 10L << FRACTIONAL_PLACES;
    private const long HALF = 1L << (FRACTIONAL_PLACES - 1);
    private const long PI_TIMES_2 = 0x6487ED511;
    private const long PI = 0x3243F6A88;
    private const long PI_OVER_2 = 0x1921FB544;
    private const int LUT_SIZE = (int)(PI_OVER_2 >> 15);

    // Precision of this type is 2^-32, that is 2,3283064365386962890625E-10
    public static readonly decimal Precision = (decimal)(new Number(1L));//0.00000000023283064365386962890625m;
    public static readonly Number MaxValue = new Number(MAX_VALUE-1);
    public static readonly Number MinValue = new Number(MIN_VALUE+2);
    public static readonly Number One = new Number(ONE);
    public static readonly Number Ten = new Number(TEN);
    public static readonly Number Half = new Number(HALF);

    public static readonly Number Zero = new Number();
    public static readonly Number PositiveInfinity = new Number(MAX_VALUE);
    public static readonly Number NegativeInfinity = new Number(MIN_VALUE+1);
    public static readonly Number NaN = new Number(MIN_VALUE);

    public static readonly Number EN1 = Number.One / 10;
    public static readonly Number EN2 = Number.One / 100;
    public static readonly Number EN3 = Number.One / 1000;
    public static readonly Number EN4 = Number.One / 10000;
    public static readonly Number EN5 = Number.One / 100000;
    public static readonly Number EN6 = Number.One / 1000000;
    public static readonly Number EN7 = Number.One / 10000000;
    public static readonly Number EN8 = Number.One / 100000000;
    public static readonly Number Epsilon = Number.EN3;

    /// <summary>
    /// The value of Pi
    /// </summary>
    public static readonly Number Pi = new Number(PI);
    public static readonly Number PiOver2 = new Number(PI_OVER_2);
    public static readonly Number PiTimes2 = new Number(PI_TIMES_2);
    public static readonly Number PiInv = (Number)0.3183098861837906715377675267M;
    public static readonly Number PiOver2Inv = (Number)0.6366197723675813430755350535M;

    public static readonly Number Deg2Rad = Pi / new Number(180);

    public static readonly Number Rad2Deg = new Number(180) / Pi;

    public static readonly Number LutInterval = (Number)(LUT_SIZE - 1) / PiOver2;

    public Number(int value) {
        _serializedValue = value * ONE;
    }

    /// <summary>
    /// Adds x and y. Performs saturating addition, i.e. in case of overflow, 
    /// rounds to MinValue or MaxValue depending on sign of operands.
    /// </summary>
    public static Number operator +(Number x, Number y) {
        return new Number(x._serializedValue + y._serializedValue);
    }

    /// <summary>
    /// Subtracts y from x. Performs saturating substraction, i.e. in case of overflow, 
    /// rounds to MinValue or MaxValue depending on sign of operands.
    /// </summary>
    public static Number operator -(Number x, Number y) {
        return new Number(x._serializedValue - y._serializedValue);
    }

    public static Number operator *(Number x, Number y) {
        var xl = x._serializedValue;
        var yl = y._serializedValue;

        var xlo = (ulong)(xl & 0x00000000FFFFFFFF);
        var xhi = xl >> FRACTIONAL_PLACES;
        var ylo = (ulong)(yl & 0x00000000FFFFFFFF);
        var yhi = yl >> FRACTIONAL_PLACES;
        var lolo = xlo * ylo;
        var lohi = (long)xlo * yhi;
        var hilo = xhi * (long)ylo;
        var hihi = xhi * yhi;
        var loResult = lolo >> FRACTIONAL_PLACES;
        var midResult1 = lohi;
        var midResult2 = hilo;
        var hiResult = hihi << FRACTIONAL_PLACES;
        var sum = (long)loResult + midResult1 + midResult2 + hiResult;
        Number result;// = default(Number);
        result._serializedValue = sum;
        return result;
    }

    public static Number operator /(Number x, Number y) {
        var xl = x._serializedValue;
        var yl = y._serializedValue;

        if (yl == 0) {
            return MAX_VALUE;
            //throw new DivideByZeroException();
        }

        var remainder = (ulong)(xl >= 0 ? xl : -xl);
        var divider = (ulong)(yl >= 0 ? yl : -yl);
        var quotient = 0UL;
        var bitPos = NUM_BITS / 2 + 1;


        // If the divider is divisible by 2^n, take advantage of it.
        while ((divider & 0xF) == 0 && bitPos >= 4) {
            divider >>= 4;
            bitPos -= 4;
        }

        while (remainder != 0 && bitPos >= 0) {
            int shift = CountLeadingZeroes(remainder);
            if (shift > bitPos) {
                shift = bitPos;
            }
            remainder <<= shift;
            bitPos -= shift;

            var div = remainder / divider;
            remainder = remainder % divider;
            quotient += div << bitPos;

            // Detect overflow
            if ((div & ~(0xFFFFFFFFFFFFFFFF >> bitPos)) != 0) {
                return ((xl ^ yl) & MIN_VALUE) == 0 ? MaxValue : MinValue;
            }

            remainder <<= 1;
            --bitPos;
        }

        // rounding
        ++quotient;
        var result = (long)(quotient >> 1);
        if (((xl ^ yl) & MIN_VALUE) != 0) {
            result = -result;
        }

        return new Number(result);
    }

    public static Number operator %(Number x, Number y) {
        return new Number(
            x._serializedValue == MIN_VALUE & y._serializedValue == -1 ?
            0 :
            x._serializedValue % y._serializedValue);
    }

    public static Number operator -(Number x) {
        return x._serializedValue == MIN_VALUE ? MaxValue : new Number(-x._serializedValue);
    }

    public static bool operator ==(Number x, Number y) {
        return x._serializedValue == y._serializedValue;
    }

    public static bool operator !=(Number x, Number y) {
        return x._serializedValue != y._serializedValue;
    }

    public static bool operator >(Number x, Number y) {
        return x._serializedValue > y._serializedValue;
    }

    public static bool operator <(Number x, Number y) {
        return x._serializedValue < y._serializedValue;
    }

    public static bool operator >=(Number x, Number y) {
        return x._serializedValue >= y._serializedValue;
    }

    public static bool operator <=(Number x, Number y) {
        return x._serializedValue <= y._serializedValue;
    }

    public static explicit operator long(Number value) {
        return value._serializedValue >> FRACTIONAL_PLACES;
    }

    //public static implicit operator int(Number value) {
    //    return value.AsInt();
    //}

    public static explicit operator float(Number value) {
    return (float)value._serializedValue / ONE;
    }

    public static explicit operator double(Number value) {
        return (double)value._serializedValue / ONE;
    }

    public static implicit operator Number(long value) {
        return new Number(value * ONE);
    }

    //public static implicit operator Number(float value) {
    //    return new Number((long)(value * ONE));
    //}

    //public static implicit operator Number(double value) {
    //    return new Number((long)(value * ONE));
    //}

    //public static explicit operator Number(decimal value) {
    //    return new Number((long)(value * ONE));
    //}

    public static implicit operator Number(int value) {
        return new Number(value * ONE);
    }

    public static explicit operator decimal(Number value) {
        return (decimal)value._serializedValue / ONE;
    }

    public static implicit operator Number(string value) {
        if (value.Contains(".")) {
            int n = value.Length - value.IndexOf(".") - 1;
            Number result = int.Parse(value.Replace(".",""));
            for (int i = 0; i < n ; i++) {
                result = result / 10;
            }
            return result;
        }

        return (Number)int.Parse(value);
    }

    /// <summary>
    /// Returns a number indicating the sign of a Fix64 number.
    /// Returns 1 if the value is positive, 0 if is 0, and -1 if it is negative.
    /// </summary>
    public static int Sign(Number value) {
        return
            value._serializedValue < 0 ? -1 :
            value._serializedValue > 0 ? 1 :
            0;
    }

    /// <summary>
    /// Returns the absolute value of a Fix64 number.
    /// Note: Abs(Fix64.MinValue) == Fix64.MaxValue.
    /// </summary>
    public static Number Abs(Number value) {
        if (value._serializedValue == MIN_VALUE) {
            return MaxValue;
        }

        // branchless implementation, see http://www.strchr.com/optimized_abs_function
        var mask = value._serializedValue >> 63;
        return new Number((value._serializedValue + mask) ^ mask);
    }

    /// <summary>
    /// Returns the absolute value of a Fix64 number.
    /// FastAbs(Fix64.MinValue) is undefined.
    /// </summary>
    public static Number FastAbs(Number value) {
        // branchless implementation, see http://www.strchr.com/optimized_abs_function
        var mask = value._serializedValue >> 63;
        return new Number((value._serializedValue + mask) ^ mask);
    }


    /// <summary>
    /// Returns the largest integer less than or equal to the specified number.
    /// </summary>
    public static Number Floor(Number value) {
        // Just zero out the fractional part
        return new Number((long)((ulong)value._serializedValue & 0xFFFFFFFF00000000));
    }

    /// <summary>
    /// Returns the smallest integral value that is greater than or equal to the specified number.
    /// </summary>
    public static Number Ceiling(Number value) {
        var hasFractionalPart = (value._serializedValue & 0x00000000FFFFFFFF) != 0;
        return hasFractionalPart ? Floor(value) + One : value;
    }

    /// <summary>
    /// Rounds a value to the nearest integral value.
    /// If the value is halfway between an even and an uneven value, returns the even value.
    /// </summary>
    public static Number Round(Number value) {
        var fractionalPart = value._serializedValue & 0x00000000FFFFFFFF;
        var integralPart = Floor(value);
        if (fractionalPart < 0x80000000) {
            return integralPart;
        }
        if (fractionalPart > 0x80000000) {
            return integralPart + One;
        }
        // if number is halfway between two values, round to the nearest even number
        // this is the method used by System.Math.Round().
        return (integralPart._serializedValue & ONE) == 0
                   ? integralPart
                   : integralPart + One;
    }

    /// <summary>
    /// Adds x and y performing overflow checking. Should be inlined by the CLR.
    /// </summary>
    public static Number OverflowAdd(Number x, Number y) {
        var xl = x._serializedValue;
        var yl = y._serializedValue;
        var sum = xl + yl;
        // if signs of operands are equal and signs of sum and x are different
        if (((~(xl ^ yl) & (xl ^ sum)) & MIN_VALUE) != 0) {
            sum = xl > 0 ? MAX_VALUE : MIN_VALUE;
        }
        return new Number(sum);
    }

    /// <summary>
    /// Adds x and y witout performing overflow checking. Should be inlined by the CLR.
    /// </summary>
    public static Number FastAdd(Number x, Number y) {
        return new Number(x._serializedValue + y._serializedValue);
    }

    /// <summary>
    /// Subtracts y from x witout performing overflow checking. Should be inlined by the CLR.
    /// </summary>
    public static Number OverflowSub(Number x, Number y) {
        var xl = x._serializedValue;
        var yl = y._serializedValue;
        var diff = xl - yl;
        // if signs of operands are different and signs of sum and x are different
        if ((((xl ^ yl) & (xl ^ diff)) & MIN_VALUE) != 0) {
            diff = xl < 0 ? MIN_VALUE : MAX_VALUE;
        }
        return new Number(diff);
    }

    /// <summary>
    /// Subtracts y from x witout performing overflow checking. Should be inlined by the CLR.
    /// </summary>
    public static Number FastSub(Number x, Number y) {
        return new Number(x._serializedValue - y._serializedValue);
    }

    static long AddOverflowHelper(long x, long y, ref bool overflow) {
        var sum = x + y;
        // x + y overflows if sign(x) ^ sign(y) != sign(sum)
        overflow |= ((x ^ y ^ sum) & MIN_VALUE) != 0;
        return sum;
    }

    /// <summary>
    /// Performs multiplication without checking for overflow.
    /// Useful for performance-critical code where the values are guaranteed not to cause overflow
    /// </summary>
    public static Number OverflowMul(Number x, Number y) {
        var xl = x._serializedValue;
        var yl = y._serializedValue;

        var xlo = (ulong)(xl & 0x00000000FFFFFFFF);
        var xhi = xl >> FRACTIONAL_PLACES;
        var ylo = (ulong)(yl & 0x00000000FFFFFFFF);
        var yhi = yl >> FRACTIONAL_PLACES;

        var lolo = xlo * ylo;
        var lohi = (long)xlo * yhi;
        var hilo = xhi * (long)ylo;
        var hihi = xhi * yhi;

        var loResult = lolo >> FRACTIONAL_PLACES;
        var midResult1 = lohi;
        var midResult2 = hilo;
        var hiResult = hihi << FRACTIONAL_PLACES;

        bool overflow = false;
        var sum = AddOverflowHelper((long)loResult, midResult1, ref overflow);
        sum = AddOverflowHelper(sum, midResult2, ref overflow);
        sum = AddOverflowHelper(sum, hiResult, ref overflow);

        bool opSignsEqual = ((xl ^ yl) & MIN_VALUE) == 0;

        // if signs of operands are equal and sign of result is negative,
        // then multiplication overflowed positively
        // the reverse is also true
        if (opSignsEqual) {
            if (sum < 0 || (overflow && xl > 0)) {
                return MaxValue;
            }
        } else {
            if (sum > 0) {
                return MinValue;
            }
        }

        // if the top 32 bits of hihi (unused in the result) are neither all 0s or 1s,
        // then this means the result overflowed.
        var topCarry = hihi >> FRACTIONAL_PLACES;
        if (topCarry != 0 && topCarry != -1 /*&& xl != -17 && yl != -17*/) {
            return opSignsEqual ? MaxValue : MinValue;
        }

        // If signs differ, both operands' magnitudes are greater than 1,
        // and the result is greater than the negative operand, then there was negative overflow.
        if (!opSignsEqual) {
            long posOp, negOp;
            if (xl > yl) {
                posOp = xl;
                negOp = yl;
            } else {
                posOp = yl;
                negOp = xl;
            }
            if (sum > negOp && negOp < -ONE && posOp > ONE) {
                return MinValue;
            }
        }

        return new Number(sum);
    }

    /// <summary>
    /// Performs multiplication without checking for overflow.
    /// Useful for performance-critical code where the values are guaranteed not to cause overflow
    /// </summary>
    public static Number FastMul(Number x, Number y) {
        var xl = x._serializedValue;
        var yl = y._serializedValue;

        var xlo = (ulong)(xl & 0x00000000FFFFFFFF);
        var xhi = xl >> FRACTIONAL_PLACES;
        var ylo = (ulong)(yl & 0x00000000FFFFFFFF);
        var yhi = yl >> FRACTIONAL_PLACES;

        var lolo = xlo * ylo;
        var lohi = (long)xlo * yhi;
        var hilo = xhi * (long)ylo;
        var hihi = xhi * yhi;

        var loResult = lolo >> FRACTIONAL_PLACES;
        var midResult1 = lohi;
        var midResult2 = hilo;
        var hiResult = hihi << FRACTIONAL_PLACES;

        var sum = (long)loResult + midResult1 + midResult2 + hiResult;
        Number result;// = default(Number);
        result._serializedValue = sum;
        return result;
        //return new Number(sum);
    }

    //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)] 
    public static int CountLeadingZeroes(ulong x) {
        int result = 0;
        while ((x & 0xF000000000000000) == 0) { result += 4; x <<= 4; }
        while ((x & 0x8000000000000000) == 0) { result += 1; x <<= 1; }
        return result;
    }

    /// <summary>
    /// Performs modulo as fast as possible; throws if x == MinValue and y == -1.
    /// Use the operator (%) for a more reliable but slower modulo.
    /// </summary>
    public static Number FastMod(Number x, Number y) {
        return new Number(x._serializedValue % y._serializedValue);
    }

    /// <summary>
    /// Returns the square root of a specified number.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">
    /// The argument was negative.
    /// </exception>
    public static Number Sqrt(Number x) {
        var xl = x._serializedValue;
        if (xl < 0) {
            // We cannot represent infinities like Single and Double, and Sqrt is
            // mathematically undefined for x < 0. So we just throw an exception.
            throw new ArgumentOutOfRangeException("Negative value passed to Sqrt", "x");
        }

        var num = (ulong)xl;
        var result = 0UL;

        // second-to-top bit
        var bit = 1UL << (NUM_BITS - 2);

        while (bit > num) {
            bit >>= 2;
        }

        // The main part is executed twice, in order to avoid
        // using 128 bit values in computations.
        for (var i = 0; i < 2; ++i) {
            // First we get the top 48 bits of the answer.
            while (bit != 0) {
                if (num >= result + bit) {
                    num -= result + bit;
                    result = (result >> 1) + bit;
                } else {
                    result = result >> 1;
                }
                bit >>= 2;
            }

            if (i == 0) {
                // Then process it again to get the lowest 16 bits.
                if (num > (1UL << (NUM_BITS / 2)) - 1) {
                    // The remainder 'num' is too large to be shifted left
                    // by 32, so we have to add 1 to result manually and
                    // adjust 'num' accordingly.
                    // num = a - (result + 0.5)^2
                    //       = num + result^2 - (result + 0.5)^2
                    //       = num - result - 0.5
                    num -= result;
                    num = (num << (NUM_BITS / 2)) - 0x80000000UL;
                    result = (result << (NUM_BITS / 2)) + 0x80000000UL;
                } else {
                    num <<= (NUM_BITS / 2);
                    result <<= (NUM_BITS / 2);
                }

                bit = 1UL << (NUM_BITS / 2 - 2);
            }
        }
        // Finally, if next bit would have been 1, round the result upwards.
        if (num > result) {
            ++result;
        }
        return new Number((long)result);
    }

    /// <summary>
    /// Returns the Sine of x.
    /// This function has about 9 decimals of accuracy for small values of x.
    /// It may lose accuracy as the value of x grows.
    /// Performance: about 25% slower than Math.Sin() in x64, and 200% slower in x86.
    /// </summary>
    public static Number Sin(Number x) {
        bool flipHorizontal, flipVertical;
        var clampedL = ClampSinValue(x._serializedValue, out flipHorizontal, out flipVertical);
        var clamped = new Number(clampedL);

        // Find the two closest values in the LUT and perform linear interpolation
        // This is what kills the performance of this function on x86 - x64 is fine though
        var rawIndex = FastMul(clamped, LutInterval);
        var roundedIndex = Round(rawIndex);
        var indexError = 0;//FastSub(rawIndex, roundedIndex);

        var nearestValue = new Number(SinLut[flipHorizontal ?
            SinLut.Length - 1 - (int)roundedIndex :
            (int)roundedIndex]);
        var secondNearestValue = new Number(SinLut[flipHorizontal ?
            SinLut.Length - 1 - (int)roundedIndex - Sign(indexError) :
            (int)roundedIndex + Sign(indexError)]);

        var delta = FastMul(indexError, FastAbs(FastSub(nearestValue, secondNearestValue)))._serializedValue;
        var interpolatedValue = nearestValue._serializedValue + (flipHorizontal ? -delta : delta);
        var finalValue = flipVertical ? -interpolatedValue : interpolatedValue;
        Number a2 = new Number(finalValue);
        return a2;
    }

    /// <summary>
    /// Returns a rough approximation of the Sine of x.
    /// This is at least 3 times faster than Sin() on x86 and slightly faster than Math.Sin(),
    /// however its accuracy is limited to 4-5 decimals, for small enough values of x.
    /// </summary>
    public static Number FastSin(Number x) {
        bool flipHorizontal, flipVertical;
        var clampedL = ClampSinValue(x._serializedValue, out flipHorizontal, out flipVertical);

        // Here we use the fact that the SinLut table has a number of entries
        // equal to (PI_OVER_2 >> 15) to use the angle to index directly into it
        var rawIndex = (uint)(clampedL >> 15);
        if (rawIndex >= LUT_SIZE) {
            rawIndex = LUT_SIZE - 1;
        }
        var nearestValue = SinLut[flipHorizontal ?
            SinLut.Length - 1 - (int)rawIndex :
            (int)rawIndex];
        return new Number(flipVertical ? -nearestValue : nearestValue);
    }



    //[MethodImplAttribute(MethodImplOptions.AggressiveInlining)] 
    public static long ClampSinValue(long angle, out bool flipHorizontal, out bool flipVertical) {
        // Clamp value to 0 - 2*PI using modulo; this is very slow but there's no better way AFAIK
        var clamped2Pi = angle % PI_TIMES_2;
        if (angle < 0) {
            clamped2Pi += PI_TIMES_2;
        }

        // The LUT contains values for 0 - PiOver2; every other value must be obtained by
        // vertical or horizontal mirroring
        flipVertical = clamped2Pi >= PI;
        // obtain (angle % PI) from (angle % 2PI) - much faster than doing another modulo
        var clampedPi = clamped2Pi;
        while (clampedPi >= PI) {
            clampedPi -= PI;
        }
        flipHorizontal = clampedPi >= PI_OVER_2;
        // obtain (angle % PI_OVER_2) from (angle % PI) - much faster than doing another modulo
        var clampedPiOver2 = clampedPi;
        if (clampedPiOver2 >= PI_OVER_2) {
            clampedPiOver2 -= PI_OVER_2;
        }
        return clampedPiOver2;
    }

    /// <summary>
    /// Returns the cosine of x.
    /// See Sin() for more details.
    /// </summary>
    public static Number Cos(Number x) {
        var xl = x._serializedValue;
        var rawAngle = xl + (xl > 0 ? -PI - PI_OVER_2 : PI_OVER_2);
        Number a2 = Sin(new Number(rawAngle));
        return a2;
    }

    /// <summary>
    /// Returns a rough approximation of the cosine of x.
    /// See FastSin for more details.
    /// </summary>
    public static Number FastCos(Number x) {
        var xl = x._serializedValue;
        var rawAngle = xl + (xl > 0 ? -PI - PI_OVER_2 : PI_OVER_2);
        return FastSin(new Number(rawAngle));
    }

    /// <summary>
    /// Returns the tangent of x.
    /// </summary>
    /// <remarks>
    /// This function is not well-tested. It may be wildly inaccurate.
    /// </remarks>
    public static Number Tan(Number x) {
        var clampedPi = x._serializedValue % PI;
        var flip = false;
        if (clampedPi < 0) {
            clampedPi = -clampedPi;
            flip = true;
        }
        if (clampedPi > PI_OVER_2) {
            flip = !flip;
            clampedPi = PI_OVER_2 - (clampedPi - PI_OVER_2);
        }

        var clamped = new Number(clampedPi);

        // Find the two closest values in the LUT and perform linear interpolation
        var rawIndex = FastMul(clamped, LutInterval);
        var roundedIndex = Round(rawIndex);
        var indexError = FastSub(rawIndex, roundedIndex);

        var nearestValue = new Number(TanLut[(int)roundedIndex]);
        var secondNearestValue = new Number(TanLut[(int)roundedIndex + Sign(indexError)]);

        var delta = FastMul(indexError, FastAbs(FastSub(nearestValue, secondNearestValue)))._serializedValue;
        var interpolatedValue = nearestValue._serializedValue + delta;
        var finalValue = flip ? -interpolatedValue : interpolatedValue;
        Number a2 = new Number(finalValue);
        return a2;
    }

    public static Number Atan(Number y) {
        return Atan2(y, 1);
    }

    public static Number Atan2(Number y, Number x) {
        var yl = y._serializedValue;
        var xl = x._serializedValue;
        if (xl == 0) {
            if (yl > 0) {
                return PiOver2;
            }
            if (yl == 0) {
                return Zero;
            }
            return -PiOver2;
        }
        Number atan;
        var z = y / x;

        Number sm = Number.EN2 * 28;
        // Deal with overflow
        if (One + sm * z * z == MaxValue) {
            return y < Zero ? -PiOver2 : PiOver2;
        }

        if (Abs(z) < One) {
            atan = z / (One + sm * z * z);
            if (xl < 0) {
                if (yl < 0) {
                    return atan - Pi;
                }
                return atan + Pi;
            }
        } else {
            atan = PiOver2 - z / (z * z + sm);
            if (yl < 0) {
                return atan - Pi;
            }
        }
        return atan;
    }

    public static Number Asin(Number value) {
        return FastSub(PiOver2, Acos(value));
    }

    public static Number Acos(Number value) {
        if (value == 0) {
            return Number.PiOver2;
        }

        bool flip = false;
        if (value < 0) {
            value = -value;
            flip = true;
        }

        // Find the two closest values in the LUT and perform linear interpolation
        var rawIndex = FastMul(value, LUT_SIZE);
        var roundedIndex = Round(rawIndex);
        if (roundedIndex >= LUT_SIZE) {
            roundedIndex = LUT_SIZE - 1;
        }

        var indexError = FastSub(rawIndex, roundedIndex);
        var nearestValue = new Number(AcosLut[(int)roundedIndex]);

        var nextIndex = (int)roundedIndex + Sign(indexError);
        if (nextIndex >= LUT_SIZE) {
            nextIndex = LUT_SIZE - 1;
        }

        var secondNearestValue = new Number(AcosLut[nextIndex]);

        var delta = FastMul(indexError, FastAbs(FastSub(nearestValue, secondNearestValue)))._serializedValue;
        Number interpolatedValue = new Number(nearestValue._serializedValue + delta);
        Number finalValue = flip ? (Number.Pi - interpolatedValue) : interpolatedValue;

        return finalValue;
    }

    public float AsFloat() {
        return (float)this;
    }

    public int AsInt() {
        return (int)this;
    }

    public long AsLong() {
        return (long)this;
    }

    public double AsDouble() {
        return (double)this;
    }

    public decimal AsDecimal() {
        return (decimal)this;
    }

    public static float ToFloat(Number value) {
        return (float)value;
    }

    public static int ToInt(Number value) {
        return (int)value;
    }

    //public static Number FromFloat(float value) {
    //    return (Number)value;
    //}

    public static bool IsInfinity(Number value) {
        return value == NegativeInfinity || value == PositiveInfinity;
    }

    public static bool IsNaN(Number value) {
        return value == NaN;
    }

    public override bool Equals(object obj) {
        return obj is Number && ((Number)obj)._serializedValue == _serializedValue;
    }

    public override int GetHashCode() {
        return _serializedValue.GetHashCode();
    }

    public bool Equals(Number other) {
        return _serializedValue == other._serializedValue;
    }

    public int CompareTo(Number other) {
        return _serializedValue.CompareTo(other._serializedValue);
    }

    public override string ToString() {
        return ((float)this).ToString();
    }

    public static Number FromRaw(long rawValue) {
        return new Number(rawValue);
    }

    /// <summary>
    /// The underlying integer representation
    /// </summary>
    public long RawValue { get { return _serializedValue; } }

    /// <summary>
    /// This is the constructor from raw value; it can only be used interally.
    /// </summary>
    /// <param name="rawValue"></param>
    Number(long rawValue) {
        _serializedValue = rawValue;
    }

}

}
