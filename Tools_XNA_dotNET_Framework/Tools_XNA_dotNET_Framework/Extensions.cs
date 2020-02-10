using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Tools_XNA
{
    public static class Extensions
    {
        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2(point.X, point.Y);
        }

        public static Point ToPoint(this Vector2 vector2)
        {
            return new Point((int)vector2.X, (int)vector2.Y);
        }

        public static Point Position(this MouseState mouseState)
        {
            return new Point(mouseState.X, mouseState.Y);
        }
    }
}
