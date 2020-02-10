using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA_dotNET_Framework
{
    /// <summary>
    ///     An object that can be used to depict a sprite in world space which can detect pixel level collisions with other
    ///     CollidableObjects
    /// </summary>
    public class CollidableObject
    {
        /// <summary>
        ///     The current position of the object on screen
        /// </summary>
        public Vector2 Position; // TODO: change these fields to properties

        /// <summary>
        ///     rectangle of the frame in the sprite sheet 
        /// </summary>
        public Rectangle SourceRectangle;

        /// <summary>
        ///     The origin of the object, by default this is the center point of the sourceRectangle.
        /// </summary>
        public Vector2 Origin;

        /// <summary>
        ///     The rotation factor
        /// </summary>
        private float rotation;


        /// <summary>
        ///     Construct a new CollidableObject with a texture and position in world space.
        ///     Uses a default source rectangle and rotation.
        /// </summary>
        /// <param name="texture">The texture associated with the object</param>
        /// <param name="position">The position of the object in world space</param>
        public CollidableObject(Texture2D texture, Vector2 position) : this(texture, position, new Rectangle(0, 0, texture.Width, texture.Height), Vector2.Zero, 0.0f)
        {
        }

        /// <summary>
        ///     Constructs a new CollidableObject with a default texture, position, sourceRectangle and rotation in world space.
        /// </summary>
        /// <param name="texture">The texture associated with the object</param>
        /// <param name="position">The position of the object in world space</param>
        /// <param name="sourceRectangle">The source rectangle of the object</param>
        /// <param name="origin">the origin offset</param>
        /// <param name="rotation">The rotation factor</param>
        public CollidableObject(Texture2D texture, Vector2 position, Rectangle sourceRectangle, Vector2 origin, float rotation)
        {
            Position = position;
            Rotation = rotation;
            LoadTexture(texture, origin);
            SourceRectangle = sourceRectangle;
        }


        /// <summary>
        ///     The currently loaded texture
        /// </summary>
        public Texture2D Texture { get; private set; }

        /// <summary>
        ///     The rotation factor
        /// </summary>
        public float Rotation
        {
            get {return rotation; }
            set { rotation = value % MathHelper.TwoPi; }
        }


        /// <summary>
        ///     The pixel data of the loaded texture in a 2D array
        /// </summary>
        private Color[,] TextureData { get; set; }

        /// <summary>
        ///     A Matrix based on the current rotation, origin and position.
        /// </summary>
        private Matrix Transform => Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) * Matrix.CreateRotationZ(Rotation) * Matrix.CreateTranslation(new Vector3(Position, 0.0f));

        /// <summary>
        ///     An axis aligned rectangle which fully contains an arbitrarily transformed axis aligned rectangle.
        /// </summary>
        public Rectangle BoundingRectangle => CalculateBoundingRectangle(new Rectangle(0, 0, SourceRectangle.Width, SourceRectangle.Height), Transform);

        /// <summary>
        ///     A simple rectangle based on position, origin and source that does´nt rotate.
        /// </summary>
        public Rectangle SimpleRectangle => new Rectangle((int) (Position.X - Origin.X), (int) (Position.Y - Origin.Y), SourceRectangle.Width, SourceRectangle.Height);

        /// <summary>
        ///     Detects a pixel level collision between this and an other CollidableObject.
        /// </summary>
        /// <param name="collidable">The CollidableObject to check a collision against</param>
        /// <returns>True if colliding, false if not.</returns>
        public bool IsColliding(CollidableObject collidable)
        {
            // If rectangle of objects intersects
            if (BoundingRectangle.Intersects(collidable.BoundingRectangle))
                // And any of the pixels of objects intersect
                if (IntersectPixels(Transform, SourceRectangle, TextureData, collidable.Transform, collidable.SourceRectangle, collidable.TextureData))
                    // Then return true
                    return true;
            // Else return false 
            return false;
        }

        /// <summary>
        ///     Loads a new texture and resets the origin to be the center point of the source rectangle, the previous transformation values
        ///     will be maintained.
        /// </summary>
        /// <param name="texture">The new texture to load</param>
        public void LoadTexture(Texture2D texture)
        {
            // Create a temporary array to store the pixel data in
            Color[] array = new Color[texture.Width * texture.Height];

            Texture = texture;
            // Create a new origin 
            Origin = new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height / 2);
            // Set size of TextureData
            TextureData = new Color[texture.Width, texture.Height];
            // Get texture data 
            Texture.GetData(array);

            // Convert 1D array into 2D array
            // For each row of pixels in this texture
            for (int y = 0; y < texture.Height; y++)
                // For each pixel in this row
            for (int x = 0; x < texture.Width; x++)
                // Set color of this coordinate from array into TextureData
                TextureData[x, y] = array[x + y * texture.Width];
        }

        /// <summary>
        ///     Loads a new texture and origin, the previous transformation values will be maintained.
        /// </summary>
        /// <param name="texture">The new texture to load</param>
        /// <param name="origin">The new origin point</param>
        public void LoadTexture(Texture2D texture, Vector2 origin)
        {
            LoadTexture(texture);
            Origin = origin;
        }

        /// <summary>
        ///     Draws this CollidableObject with Texture, Position, Source, Rotation, and Origin.
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, SourceRectangle, Color.White, Rotation, Origin, Vector2.One, SpriteEffects.None, 0f);
        }

        /// <summary>
        ///     Determines if there is a collision between two collidableobjects
        /// </summary>
        /// <param name="collidableObjectA">first colidable</param>
        /// <param name="collidableObjectB">second collidable</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        public static bool Intersect(CollidableObject collidableObjectA, CollidableObject collidableObjectB)
        {
            // If rectangle of objects intersects
            if (collidableObjectA.BoundingRectangle.Intersects(collidableObjectB.BoundingRectangle))
                // And any of the pixels of objects intersect
                if (IntersectPixels(collidableObjectA.Transform, collidableObjectA.SourceRectangle, collidableObjectA.TextureData,
                        collidableObjectB.Transform, collidableObjectB.SourceRectangle, collidableObjectB.TextureData))
                    // Then return true
                    return true;
            // Else return false 
            return false;
        }

        /// <summary>
        ///     Determines if there is overlap of the non-transparent pixels between two
        ///     sprites.
        /// </summary>
        /// <param name="transformA">World transform of the first sprite.</param>
        /// <param name="widthA">Width of the first sprite's texture.</param>
        /// <param name="heightA">Height of the first sprite's texture.</param>
        /// <param name="dataA">Pixel color data of the first sprite.</param>
        /// <param name="transformB">World transform of the second sprite.</param>
        /// <param name="widthB">Width of the second sprite's texture.</param>
        /// <param name="heightB">Height of the second sprite's texture.</param>
        /// <param name="dataB">Pixel color data of the second sprite.</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        private static bool IntersectPixels(Matrix transformA, Rectangle sourceA, Color[,] dataA, Matrix transformB, Rectangle sourceB, Color[,] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAtoB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAtoB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAtoB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAtoB);

            // For each row of pixels in A
            for (int yA = sourceA.Y; yA < sourceA.Height + sourceA.Y; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = sourceA.X; xA < sourceA.Width + sourceA.X; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int) Math.Round(posInB.X) + sourceB.X;
                    int yB = (int) Math.Round(posInB.Y) + sourceB.Y;

                    // If the pixel lies within the bounds of B
                    if (sourceB.X <= xB && xB < sourceB.Width + sourceB.X && sourceB.Y <= yB && yB < sourceB.Height + sourceB.Y)
                    {
                        // Get the colors of the overlapping pixels
                        // and wrap coordinates to prevent index going out of range
                        Color colorA = dataA[xA % dataA.GetLength(0), yA % dataA.GetLength(1)];
                        Color colorB = dataB[xB % dataB.GetLength(0), yB % dataB.GetLength(1)];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                            // then an intersection has been found
                            return true;
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }

        /// <summary>
        ///     Calculates an axis aligned rectangle which fully contains an arbitrarily
        ///     transformed axis aligned rectangle.
        /// </summary>
        /// <param name="rectangle">Original bounding rectangle.</param>
        /// <param name="transform">World transform of the rectangle.</param>
        /// <returns>A new rectangle which contains the transformed rectangle.</returns>
        private static Rectangle CalculateBoundingRectangle(Rectangle rectangle, Matrix transform)
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

            // Find the minimum and maximum extents of the rectangle in world space
            Vector2 min = Vector2.Min(Vector2.Min(leftTop, rightTop), Vector2.Min(leftBottom, rightBottom));
            Vector2 max = Vector2.Max(Vector2.Max(leftTop, rightTop), Vector2.Max(leftBottom, rightBottom));

            // Return that as a rectangle
            return new Rectangle((int) min.X, (int) min.Y, (int) (max.X - min.X), (int) (max.Y - min.Y));
        }
    }
}