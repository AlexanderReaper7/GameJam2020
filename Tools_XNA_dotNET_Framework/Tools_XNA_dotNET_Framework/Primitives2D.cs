using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA_dotNET_Framework
{
    /// <summary>
    /// Contains extension methods for the SpriteBatch class
    /// </summary>
    public static class Primitives2D
    {
        #region Private Members

        private static readonly Dictionary<string, List<Vector2>> circleCache = new Dictionary<string, List<Vector2>>();
        private static readonly Dictionary<string, List<Vector2>> arcCache = new Dictionary<string, List<Vector2>>();
        private static Texture2D pixel;

        #endregion


        #region Private Mzethods

        /// <summary>
        /// initializes the Texture2D pixel private Member with a single white pixel
        /// </summary>
        /// <param name="spriteBatch"></param>
        private static void CreateThePixel(GraphicsResource spriteBatch)
        {
            pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            pixel.SetData(new[] {Color.White});
        }



        /// <summary>
        /// Creates a list of vectors that represents a circle
        /// </summary>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <returns>A list of vectors that, if connected, will create a circle</returns>
        private static List<Vector2> CreateCircle(double radius, int sides)
        {
            // Look for a cached version of this circle
            string circleKey = $"{radius}x{sides}";
            if (circleCache.ContainsKey(circleKey))
            {
                return circleCache[circleKey];
            }

            List<Vector2> vectors = new List<Vector2>();

            const double max = 2.0 * Math.PI;
            double step = max / sides;

            for (double theta = 0.0; theta < max; theta += step)
            {
                vectors.Add(new Vector2((float) (radius * Math.Cos(theta)), (float) (radius * Math.Sin(theta))));
            }

            // then add the first vector again so it's a complete loop
            vectors.Add(new Vector2((float) (radius * Math.Cos(0)), (float) (radius * Math.Sin(0))));

            // Cache this circle so that it can be quickly drawn next time
            circleCache.Add(circleKey, vectors);

            return vectors;
        }


        /// <summary>
        /// Creates a list of vectors that represents an arc
        /// </summary>
        /// <param name="radius">The radius of the arc</param>
        /// <param name="sides">The number of sides to generate in the circle that this will cut out from</param>
        /// <param name="startingAngle">The starting angle of arc, 0 being to the east, increasing as you go clockwise</param>
        /// <param name="radians">The radians to draw, clockwise from the starting angle</param>
        /// <returns>A list of vectors that, if connected, will create an arc</returns>
        private static List<Vector2> CreateArc(float radius, int sides, float startingAngle, float radians)
        {
            // Look for a cached version of this arc
            string arcKey = $"{radius}x{sides}x{startingAngle}x{radians}";
            if (arcCache.ContainsKey(arcKey))
            {
                return arcCache[arcKey];
            }

            List<Vector2> points = new List<Vector2>();
            points.AddRange(CreateCircle(radius, sides));
            // Remove the last point because it's a duplicate of the first
            points.RemoveAt(points.Count - 1); 

            // The circle starts at (radius, 0)
            double curAngle = 0.0;
            double anglePerSide = MathHelper.TwoPi / sides;

            // "Rotate" to the starting point
            while ((curAngle + (anglePerSide / 2.0)) < startingAngle)
            {
                curAngle += anglePerSide;

                // move the first point to the end
                points.Add(points[0]);
                points.RemoveAt(0);
            }

            // Add the first point, just in case we make a full circle
            points.Add(points[0]);

            // Now remove the points at the end of the circle to create the arc
            int sidesInArc = (int) ((radians / anglePerSide) + 0.5);
            points.RemoveRange(sidesInArc + 1, points.Count - sidesInArc - 1);

            return points;
        }

        /// <summary>
        /// Transforms a rectangle and returns the corners coordinates in clockwise order
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="transform"></param>
        /// <returns></returns>
        private static Vector2[] TransformRectangle(Rectangle rectangle, Matrix transform)
        {
            // Get all four corners in local space
            Vector2 leftTop = new Vector2(rectangle.Left, rectangle.Top);
            Vector2 rightTop = new Vector2(rectangle.Right, rectangle.Top);
            Vector2 leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            Vector2 rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            return new[] {leftTop, rightTop, rightBottom, leftBottom};
        }

        #endregion


        #region DrawPoints

        /// <summary>
        /// Draws a list of connecting points
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="position">Where to position the points</param>
        /// <param name="points">The points to connect with lines</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the lines</param>
        public static void DrawPoints(SpriteBatch spriteBatch, Vector2 position, List<Vector2> points, Color color,
            float thickness)
        {
            if (points.Count < 2) throw new ArgumentOutOfRangeException(nameof(points), "Must be two or more specified points.");

            for (int i = 1; i < points.Count; i++)
            {
                DrawLine(spriteBatch, points[i - 1] + position, points[i] + position, color, thickness);
            }
        }

        #endregion


        #region DrawFilledRectangle

        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void DrawFilledRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color)
        {
            if (pixel == null)
            {
                CreateThePixel(spriteBatch);
            }

            // Simply use the function already there
            spriteBatch.Draw(pixel, rect, color);
        }


        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="angle">The angle in radians to draw the rectangle at</param>
        public static void DrawFilledRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float angle)
        {
            if (pixel == null)
            {
                CreateThePixel(spriteBatch);
            }

            spriteBatch.Draw(pixel, rect, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }


        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void DrawFilledRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color)
        {
            DrawFilledRectangle(spriteBatch, location, size, color, 0.0f);
        }


        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="angle">The angle in radians to draw the rectangle at</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void DrawFilledRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color,
            float angle)
        {
            if (pixel == null)
            {
                CreateThePixel(spriteBatch);
            }

            // stretch the pixel between the two vectors
            spriteBatch.Draw(pixel,
                location,
                null,
                color,
                angle,
                Vector2.Zero,
                size,
                SpriteEffects.None,
                0);
        }


        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x">The X coordinate of the left side</param>
        /// <param name="y">The Y coordinate of the upper side</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="color">The color to draw the rectangle in</param>
        public static void DrawFilledRectangle(this SpriteBatch spriteBatch, float x, float y, float w, float h, Color color)
        {
            DrawFilledRectangle(spriteBatch, new Vector2(x, y), new Vector2(w, h), color, 0.0f);
        }


        /// <summary>
        /// Draws a filled rectangle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x">The X coordinate of the left side</param>
        /// <param name="y">The Y coordinate of the upper side</param>
        /// <param name="w">Width</param>
        /// <param name="h">Height</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="angle">The angle of the rectangle in radians</param>
        public static void DrawFilledRectangle(this SpriteBatch spriteBatch, float x, float y, float w, float h, Color color,
            float angle)
        {
            DrawFilledRectangle(spriteBatch, new Vector2(x, y), new Vector2(w, h), color, angle);
        }

        #endregion


        #region DrawRectangle

        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="rect">The rectangle to draw</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the lines</param>
        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float thickness = 1.0f)
        {

            // Top
            DrawLine(spriteBatch, new Vector2(rect.X, rect.Y), new Vector2(rect.Right, rect.Y), color,
                thickness); 
            // Left
            DrawLine(spriteBatch, new Vector2(rect.X + 1f, rect.Y), new Vector2(rect.X + 1f, rect.Bottom + thickness),
                color, thickness); 
            // Bottom
            DrawLine(spriteBatch, new Vector2(rect.X, rect.Bottom), new Vector2(rect.Right, rect.Bottom), color,
                thickness); 
            // Right
            DrawLine(spriteBatch, new Vector2(rect.Right + 1f, rect.Y),
                new Vector2(rect.Right + 1f, rect.Bottom + thickness), color, thickness); 
        }


        /// <summary>
        /// Draws a rectangle with the thickness provided
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="location">Where to draw</param>
        /// <param name="size">The size of the rectangle</param>
        /// <param name="color">The color to draw the rectangle in</param>
        /// <param name="thickness">The thickness of the line</param>
        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 location, Vector2 size, Color color, float thickness = 1.0f)
        {
            DrawRectangle(spriteBatch, new Rectangle((int) location.X, (int) location.Y, (int) size.X, (int) size.Y),
                color, thickness);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Matrix transform, Color color, float thickness = 1.0f)
        {
            List<Vector2> corners = TransformRectangle(rectangle, transform).ToList();
            corners.Add(corners[0]);

            DrawPoints(spriteBatch, Vector2.Zero, corners.ToList(), color, thickness);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, float rotation, Color color, float thickness = 1.0f)
        {
            Matrix rotationMatrix = Matrix.CreateFromAxisAngle(Vector3.UnitZ, rotation);
            DrawRectangle(spriteBatch, rectangle, rotationMatrix, color, thickness);
        }


        #endregion


        #region DrawLine


        /// <summary>
        /// Draws a line from point1 to point2
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x1">The X coordinate of the first point</param>
        /// <param name="y1">The Y coordinate of the first point</param>
        /// <param name="x2">The X coordinate of the second point</param>
        /// <param name="y2">The Y coordinate of the second point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        public static void DrawLine(this SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color,
            float thickness = 1.0f)
        {
            DrawLine(spriteBatch, new Vector2(x1, y1), new Vector2(x2, y2), color, thickness);
        }


        /// <summary>
        /// Draws a line from point1 to point2
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <param name="color">The color to use</param>
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color)
        {
            DrawLine(spriteBatch, point1, point2, color, 1.0f);
        }


        /// <summary>
        /// Draws a line from point1 to point2
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point1">The first point</param>
        /// <param name="point2">The second point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point1, Vector2 point2, Color color,
            float thickness)
        {
            // calculate the distance between the two vectors
            float distance = Vector2.Distance(point1, point2);

            // calculate the angle between the two vectors
            float angle = (float) Math.Atan2(point2.Y - point1.Y, point2.X - point1.X);

            DrawLine(spriteBatch, point1, distance, angle, color, thickness);
        }


        /// <summary>
        /// Draws a line from point1 to point2
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point">The starting point</param>
        /// <param name="length">The length of the line</param>
        /// <param name="angle">The angle of this line from the starting point in radians</param>
        /// <param name="color">The color to use</param>
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color)
        {
            DrawLine(spriteBatch, point, length, angle, color, 1.0f);
        }


        /// <summary>
        /// Draws a line from point1 to point2
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="point">The starting point</param>
        /// <param name="length">The length of the line</param>
        /// <param name="angle">The angle of this line from the starting point</param>
        /// <param name="color">The color to use</param>
        /// <param name="thickness">The thickness of the line</param>
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 point, float length, float angle, Color color,
            float thickness)
        {
            if (pixel == null)
            {
                CreateThePixel(spriteBatch);
            }

            // stretch the pixel between the two vectors
            spriteBatch.Draw(pixel,
                point,
                null,
                color,
                angle,
                Vector2.Zero,
                new Vector2(length, thickness),
                SpriteEffects.None,
                0);
        }

        #endregion


        #region PutPixel

        /// <summary>
        /// Draw a pixel
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color">The color to use</param>
        public static void PutPixel(this SpriteBatch spriteBatch, float x, float y, Color color)
        {
            PutPixel(spriteBatch, new Vector2(x, y), color);
        }

        /// <summary>
        /// Draw a pixel
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="position">Where to draw</param>
        /// <param name="color">The color to use</param>
        public static void PutPixel(this SpriteBatch spriteBatch, Vector2 position, Color color)
        {
            if (pixel == null)
            {
                CreateThePixel(spriteBatch);
            }

            spriteBatch.Draw(pixel, position, color);
        }

        #endregion


        #region DrawCircle

        /// <summary>
        /// Draws a circle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="center">The center of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="color">The color of the circle</param>
        /// <param name="thickness">The thickness of the lines used</param>
        public static void DrawCircle(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides,
            Color color, float thickness = 1.0f)
        {
            DrawPoints(spriteBatch, center, CreateCircle(radius, sides), color, thickness);
        }


        /// <summary>
        /// Draw a circle
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="x">The center X of the circle</param>
        /// <param name="y">The center Y of the circle</param>
        /// <param name="radius">The radius of the circle</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="color">The color of the circle</param>
        /// <param name="thickness">The thickness of the lines used</param>
        public static void DrawCircle(this SpriteBatch spriteBatch, float x, float y, float radius, int sides,
            Color color, float thickness = 1.0f)
        {
            DrawPoints(spriteBatch, new Vector2(x, y), CreateCircle(radius, sides), color, thickness);
        }

        #endregion


        #region DrawArc

        /// <summary>
        /// Draws an arc
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="center">The center of the arc</param>
        /// <param name="radius">The radius of the arc</param>
        /// <param name="sides">The number of sides to generate</param>
        /// <param name="startingAngle">The starting angle of arc, 0 being to the east, increasing as you go clockwise</param>
        /// <param name="radians">The number of radians to draw, clockwise from the starting angle</param>
        /// <param name="color">The color of the arc</param>
        /// <param name="thickness">The thickness of the arc</param>
        public static void DrawArc(this SpriteBatch spriteBatch, Vector2 center, float radius, int sides,
            float startingAngle, float radians, Color color, float thickness = 1.0f)
        {
            List<Vector2> arc = CreateArc(radius, sides, startingAngle, radians);
            DrawPoints(spriteBatch, center, arc, color, thickness);
        }

        #endregion

        
        #region DrawGrid

        /// <summary>
        /// Draws a grid of squares
        /// </summary>
        /// <param name="spriteBatch">The destination drawing surface</param>
        /// <param name="squaresX">The number of squares in the X-axis</param>
        /// <param name="squaresY">The number of squares in the Y-axis</param>
        /// <param name="squareSize">The length of the side of each square</param>
        /// <param name="position"></param>
        /// <param name="color">The color of the lines</param>
        /// <param name="thickness">The thickness of the lines</param>
        public static void DrawGrid(this SpriteBatch spriteBatch, int squaresX, int squaresY, float squareSize, Vector2 position, Color color, float thickness = 1.0f)
        {
            float bottom = squareSize * squaresY + position.Y;
            float right = squareSize * squaresX + position.X;

            // Draw vertical lines
            for (int i = 0; i <= squaresX; i++)
            {
                float x = squareSize * i + position.X;
                spriteBatch.DrawLine(x, position.Y, x, bottom, color, thickness);
            }

            // Draw horizontal lines
            for (int i = 0; i <= squaresY; i++)
            {
                float y = squareSize * i + position.Y;
                spriteBatch.DrawLine(position.X - thickness, y, right, y, color, thickness);
            }
        }


        #endregion
    }
}