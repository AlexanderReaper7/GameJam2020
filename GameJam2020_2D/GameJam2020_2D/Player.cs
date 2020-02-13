using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Tools_XNA;

namespace GameJam2020_2D
{
    public class Player
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

        Highscore scoreboard;
        string playerName;

        public bool key = false;
        public bool doorOpen = false;

        KeyboardState keyboardState, lastKeyboardState;

        // bool for player death
        public bool playerAlive = true;

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
        public Player(Texture2D textureUp, Texture2D textureDown, Texture2D textureLeft, Texture2D textureRight, TilesMap tileMap, Highscore scoreboard, string playerName)
        {
            texture = textureDown;
            this.textureUp = textureUp;
            this.textureDown = textureDown;
            this.textureLeft = textureLeft;
            this.textureRight = textureRight;
            this.tileMap = tileMap;
            this.scoreboard = scoreboard;
            this.playerName = playerName;

            // Set position to start at // Olle A 200212
            TilePosition = tileMap.StartingPosition;
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

            if (keyboardState.IsKeyDown(Keys.A)) if (lastKeyboardState.IsKeyUp(Keys.A) || keyRepeatTime < 0) InGame.Level = InGame.Levels.preLevel1;
            doCollisionAndMove(movement);
        }

        /// <summary>
        /// Draw
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, tileMap.CollisionTiles[TilePosition].Rectangle, Color.White);
        }

        /// <summary>
        /// Sets a new level and moves to player to the level's starting position // Olle A 200213
        /// </summary>
        /// <param name="tileMap"></param>
        public void NewLevel(TilesMap tileMap)
        {
            this.tileMap = tileMap;
            TilePosition = tileMap.StartingPosition;
        }


        /// <summary>
        /// Method that handles collisions and moving the player // Olle A 200213
        /// </summary>
        /// <param name="movement">Movemet to attempt in number of tiles</param>
        private void doCollisionAndMove(int movement)
        {

            // Wrap in try statement so game doesn't crash in case of attempting illegal move // Olle A 200213
            try
            {
                // Code specific to type of tile // Olle A 200213
                switch (tileMap.CollisionTiles[TilePosition + movement].Type)
                {
                    // Air (no tile)
                    case 0:
                        // TODO: Add death logic // Olle A 200212
                        playerAlive = false;
                        break;

                    // Ground, open door and walkable tiles // Olle A 200213
                    case 101: case 201: case 107: case 207:
                        // Update bool in prev tile // Olle A 200212
                        tileMap.CollisionTiles[prevTilePosition].IsOnTile = false;
                        prevTilePosition = TilePosition;

                        TilePosition += movement;
                        // Update bools in new tile // Olle A 200212
                        tileMap.CollisionTiles[TilePosition].HasBeenWalkedOn = true;
                        tileMap.CollisionTiles[TilePosition].IsOnTile = true;
                        break;

                    // Wall, closed door, dispenser, unwalkable tiles 
                    case 102: case 202: case 103: case 203: case 105: case 205: case 206:
                        // Do nothing // Olle A 200213
                        break;

                    // Door
                    case 106:
                        if (doorOpen == false && doorOpen == true)
                        {
                            doorOpen = true;
                        }

                        tileMap.CollisionTiles[TilePosition + movement].ChangeType(107);
                        break;


                    // keys
                    case 111:
                        if (key == false)
                        {
                            key = true;
                        }

                        tileMap.CollisionTiles[TilePosition + movement].ChangeType(101);
                        TilePosition = TilePosition + movement;
                        break;


                    // End portal
                    case 104: case 204:
                        // Save score
                        scoreboard.SaveHighScore(tileMap.LevelNumber, playerName, tileMap.timer);
                        // Change level
                        InGame.Level++;
                        TilePosition = tileMap.StartingPosition;
                        break;

                    // Trap door
                    case 109:
                        // TODO: Add death logic // Olle A 200212
                        break;

                    // Unspecified tiles do nothing // Olle A 200213
                    default:
                        break;
                }
            }
            catch { }
        }
    }
}
