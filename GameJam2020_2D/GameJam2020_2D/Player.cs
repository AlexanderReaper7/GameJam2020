﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Tools_XNA;

namespace GameJam2020_2D
{
    class Player
    {
        public TilesMap tileMap;
        // Texture currently in use // Olle A 20-02-12
        private Texture2D texture;
        // All textures // Olle A 20-02-12
        private Texture2D textureUp;
        private Texture2D textureDown;
        private Texture2D textureLeft;
        private Texture2D textureRight;

        // Position as a int in the tile map list // Olle A 20-02-12
        public int TilePosition = 1;
        // Previous position // Olle A 20-02-12
        private int prevTilePosition;
        KeyboardState keyboardState, lastKeyboardState;

        /// <summary>
        /// Sets the last key to pressed to enable the repeat function // Emil C.A. 200212
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool KeyPressed(Keys key)
        {
            return keyboardState.IsKeyDown(key) && lastKeyboardState.IsKeyUp(key);
        }

        // Float that decides how long delay on the repeat of keypress // Emil C.A. 200212
        float keyRepeatTime;
        const float keyRepeatDelay = 0.5f;

        public Vector2 Position
        {
            get { return new Vector2(tileMap.CollisionTiles[TilePosition].Rectangle.X, tileMap.CollisionTiles[TilePosition].Rectangle.Y); }
        }


        /// <summary>
        /// Constructor // Olle A 200212
        /// </summary>
        /// <param name="textureUp"></param>
        /// <param name="textureDown"></param>
        /// <param name="textureLeft"></param>
        /// <param name="textureRight"></param>
        /// <param name="tileMap"></param>
        public Player(Texture2D textureUp, Texture2D textureDown, Texture2D textureLeft, Texture2D textureRight, TilesMap tileMap)
        {
            texture = textureDown;
            this.textureUp = textureUp;
            this.textureDown = textureDown;
            this.textureLeft = textureLeft;
            this.textureRight = textureRight;
            this.tileMap = tileMap;
        }

        public void Update(GameTime gameTime)
        {
            lastKeyboardState = keyboardState;
            keyboardState = Keyboard.GetState();
            // Movement to do this update // Olle A 200212
            int movement = 0;

            float seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (keyboardState.IsKeyDown(Keys.Right))
            {
                if (lastKeyboardState.IsKeyUp(Keys.Right) || keyRepeatTime < 0)
                {
                    keyRepeatTime = keyRepeatDelay;
                    //do key logic // Emil C.A. 200212
                    movement -= tileMap.TileMapWidth;
                    // Change texture // Olle A 200212
                    texture = textureRight; 

                }
                else
                    keyRepeatTime -= seconds;
            }
            if (keyboardState.IsKeyDown(Keys.Left))
            {
                if (lastKeyboardState.IsKeyUp(Keys.Left) || keyRepeatTime < 0)
                {
                    keyRepeatTime = keyRepeatDelay;
                    //do key logic // Emil C.A. 200212
                    movement += tileMap.TileMapWidth;
                    // Change texture // Olle A 200212
                    texture = textureLeft;


                }
                else
                    keyRepeatTime -= seconds;
            }
            if (keyboardState.IsKeyDown(Keys.Up))
            {
                if (lastKeyboardState.IsKeyUp(Keys.Up) || keyRepeatTime < 0)
                {
                    keyRepeatTime = keyRepeatDelay;
                    //do key logic // Emil C.A. 200212
                    movement -= 1;
                    // Change texture // Olle A 200212
                    texture = textureUp;

                }
                else
                    keyRepeatTime -= seconds;
            }
            if (keyboardState.IsKeyDown(Keys.Down))
            {
                if (lastKeyboardState.IsKeyUp(Keys.Down) || keyRepeatTime < 0)
                {
                    keyRepeatTime = keyRepeatDelay;
                    //do key logic // Emil C.A. 200212
                    movement += 1;
                    // Change texture // Olle A 200212
                    texture = textureDown;


                }
                else
                    keyRepeatTime -= seconds;
            }


            // Do movement // Olle A 200212
            try
            {
                // Fail if tile is air // Olle A 200212
                if (tileMap.CollisionTiles[TilePosition + movement].Type == 0)
                {
                    // TODO: Add death logic
                }
                // Otherwise move // Olle A 200212
                else
                {
                    // Update bool in prev tile // Olle A 200212
                    tileMap.CollisionTiles[prevTilePosition].IsOnTile = false;
                    prevTilePosition = TilePosition;

                    TilePosition += movement;
                    // Update bools in new tile // Olle A 200212
                    tileMap.CollisionTiles[TilePosition].HasBeenWalkedOn = true;
                    tileMap.CollisionTiles[TilePosition].IsOnTile = true;
                    
                }
            }
            catch { }

        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, tileMap.CollisionTiles[TilePosition].Rectangle, Color.White);
        }
    }
}