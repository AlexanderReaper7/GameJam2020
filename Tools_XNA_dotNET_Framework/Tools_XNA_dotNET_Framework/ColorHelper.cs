using System;
using Microsoft.Xna.Framework;

namespace Tools_XNA_dotNET_Framework
{
    public static class ColorHelper
    {
#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
        public struct HSV
#pragma warning restore CS0661 
#pragma warning restore CS0660 
        {
            public double H { get; set; }

            public double S { get; set; }

            public double V { get; set; }

            public HSV(double h, double s, double v)
            {
                this.H = h;
                this.S = s;
                this.V = v;
            }

            public static bool operator ==(HSV a, HSV b)
            {
                return a.Equals(b);
            }

            public static bool operator !=(HSV a, HSV b)
            {
                return !a.Equals(b);
            }

            public bool Equals(HSV hsv)
            {
                return (this.H == hsv.H) && (this.S == hsv.S) && (this.V == hsv.V);
            }
        }

        public static HSV RGBToHSV(Color rgb)
        {
            double delta, min;
            double h = 0, s, v;

            min = Math.Min(Math.Min(rgb.R, rgb.G), rgb.B);
            v = Math.Max(Math.Max(rgb.R, rgb.G), rgb.B);
            delta = v - min;

            if (v == 0.0)
                s = 0;
            else
                s = delta / v;

            if (s == 0)
                h = 0.0;

            else
            {
                if (rgb.R == v)
                    h = (rgb.G - rgb.B) / delta;
                else if (rgb.G == v)
                    h = 2 + (rgb.B - rgb.R) / delta;
                else if (rgb.B == v)
                    h = 4 + (rgb.R - rgb.G) / delta;

                h *= 60;

                if (h < 0.0)
                    h = h + 360;
            }

            return new HSV(h, s, v / 255);
        }

        public static Color HSVToRGB(HSV hsv)
        {
            double r = 0, g = 0, b = 0;

            if (hsv.S == 0)
            {
                r = hsv.V;
                g = hsv.V;
                b = hsv.V;
            }
            else
            {
                int i;
                double f, p, q, t;

                if (hsv.H == 360)
                    hsv.H = 0;
                else
                    hsv.H /= 60;

                i = (int)Math.Truncate(hsv.H);
                f = hsv.H - i;

                p = hsv.V * (1.0 - hsv.S);
                q = hsv.V * (1.0 - (hsv.S * f));
                t = hsv.V * (1.0 - (hsv.S * (1.0 - f)));

                switch (i)
                {
                    case 0:
                        r = hsv.V;
                        g = t;
                        b = p;
                        break;

                    case 1:
                        r = q;
                        g = hsv.V;
                        b = p;
                        break;

                    case 2:
                        r = p;
                        g = hsv.V;
                        b = t;
                        break;

                    case 3:
                        r = p;
                        g = q;
                        b = hsv.V;
                        break;

                    case 4:
                        r = t;
                        g = p;
                        b = hsv.V;
                        break;

                    default:
                        r = hsv.V;
                        g = p;
                        b = q;
                        break;
                }

            }

            return new Color((byte)(r * 255), (byte)(g * 255), (byte)(b * 255));
        }

        public static Color CalculateColorFromRotation(Vector3 rotation)
        {
            // Change Color from rotation
            // X Y Z becomes H S V
            float H = Math.Abs(MathHelper.ToDegrees(rotation.X));
            float S = LinearCosine(rotation.Y);
            float V = LinearCosine(rotation.Z);

            return HSVToRGB(new HSV(H, S, V));
        }

        private static float LinearCosine(float value)
        {
            return Math.Abs((float)(1 - Math.Abs(value % MathHelper.TwoPi) / Math.PI));
        }

        public static Color ToXNAColor(this System.Drawing.Color color)
        {
            return new Color(color.R, color.G, color.B, color.A);
        }

        public static System.Drawing.Color ToSystemColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }


        // TODO: Implement DeltaE color calculation methods, https://en.wikipedia.org/wiki/Color_difference#CIE76
    }
}
