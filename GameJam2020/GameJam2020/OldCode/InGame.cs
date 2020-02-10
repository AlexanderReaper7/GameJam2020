using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Python
{
    // A class that takes care of the whole game
    public class InGame
    {
        private GraphicsDevice gd;
        private ControlScheme input;
        private Settings options;

        private Grid _grid;
        private Snake _snake;
        private Blob _blob;
        private SkyBox _background;

        private Camera _camera;
        private Vector3 _rotationChange;
        //private ParticleSystem _particle;

        // The font
        private SpriteFont mainFont;

        // The score
        public int Score;

        // Timers
        private float moveDelay = 500;
        private float countDownDelay = 4000 -1;
        private float moveTimer;
        private float countDownTimer;
        private bool showCountDownTimer;

        // A separate timer for how long the camera shall float in "top view"
        private float blobCameraTimer;
        // Two bools that decide if the blob has been eaten and starts blobCameraTimer, when it is done, Return is true
        private bool eatBlob, eatBlobReturn;

        // Small variables for making a hold/press function/option for the player
        private bool leftKey, rightKey, keyPressed;

        // Different states that determine if the snake shall move or if the game is over
        public bool GameActive { get; set; }
        public bool GameOver { get; set; }



        // Constructor
        public InGame(ControlScheme controlScheme, GraphicsDevice graphicsDevice, Settings settings)
        {
            gd = graphicsDevice;
            input = controlScheme;
            options = settings;
        }

        // Load all the models and then reset game
        public void Load(ContentManager content)
        {
            // Camera shall be far of in the distance and focus on the left side of the grid
            _camera = new ChaseCamera(new Vector3(0, 2000, 4000), new Vector3(-1000, 0, 0), Vector3.Zero, gd);

            // The grid shall start at an area of 5
            _grid = new Grid(5, 1, 200, content, gd);
            _snake = new Snake(content, gd, _grid);
            _blob = new Blob(content, gd, _grid);
            _background = new SkyBox(content, gd, content.Load<TextureCube>("Models/Background_0"));
            // Reset game (See line 308)
            ResetGame();

            // Load the font for the score text in the upper part of the screen
            mainFont = content.Load<SpriteFont>(@"Fonts/Main");
        }
        


        // 
        public void Update(GameTime gameTime)
        {
            // Always rotate the blob
            _blob.Rotate(-4);
            // Move delay logic based on how big the gid is, how much score you have, divide it with what speed tha player has chosen
            // You move faster when the grid area expands, you move slower when the score gets higher
            moveDelay = (1000 / (float)Math.Sqrt(_grid.Length) + (float)Math.Sqrt(Score))/options.Speed;

            // If game is active, then...
            if (GameActive)
            {
                // Determine what controls that shall be used for the head
                if (!options.ControlsHold)
                {
                    // Checks for input and then locks it, until the keys are up
                    if (input.LeftGame && !keyPressed)
                    {
                        leftKey = true;
                        keyPressed = true;
                    }
                    // Checks for input and then locks it, until the keys are up
                    else if (input.RightGame && !keyPressed)
                    {
                        rightKey = true;
                        keyPressed = true;
                    }
                    // Checks for input and then locks it, until the keys are up
                    if (!input.LeftGame && !input.RightGame)
                    {
                        keyPressed = false;
                    }

                }
                
                // When it's time to move the body
                if (moveTimer <= 0)
                {
                    // Looks at what controls that shall be used for the head
                    if (options.ControlsHold)
                    {
                        _snake.ControlHeadDirection(input.LeftGame, input.RightGame);
                    }
                    else
                    {
                        // LeftKey and RightKey are bools from the if-statement at line 96
                        _snake.ControlHeadDirection(leftKey, rightKey);
                    }
                    // Move the Tail and the rest of the body
                    _snake.MoveBody();
                    // Move the head based on what direction the head has (determined by the if(options.ControlsHold) statements)
                    _snake.MoveHead(_snake.SnakeHead.Direction);

                    // If the snake head collides with a blob
                    if (_snake.CheckCollisionBlob(_blob.X, _blob.Y))
                    {
                        // Spawn a new blob, but if the blob spawn inside the tail, body or head, redo the spawning
                        // I've found out that this doesn't always work...
                        foreach (Body bodyPart in _snake.SnakeBodyParts)
                        {
                            do
                            {
                                _blob.Spawn();
                            } while (_blob.X == _snake.SnakeHead.X && _blob.Y == _snake.SnakeHead.Y
                                     || _blob.X == _snake.SnakeTail.X && _blob.Y == _snake.SnakeTail.Y
                                     || _blob.X == bodyPart.X && _blob.Y == bodyPart.Y);
                        }
                        // Add a score point
                        Score++;
                        // Add a body
                        _snake.AddBody();
                        // Make the camera move up (top view) for 4 movements
                        blobCameraTimer = moveDelay * 4.1f;
                        // Make the camera top view bools true
                        eatBlob = true;
                        eatBlobReturn = true;

                        // Checks if the score is more or equal than half of the grid area
                        if (Score >= (_grid.Length * _grid.Length)/2)
                        {
                            // If it is, add 2 more in grid length (expand the area with one grid module on each side)
                            _grid.Length += 2;
                            // Update the grid models
                            _grid.Generate(_grid.Length);
                            // Update the edge models
                            _grid.GenerateEdge(_grid.Length);
                        }


                    }

                    // If snake collide with edge of the map or collide with it's own body
                    if (_snake.CheckCollisionEdge(_grid.EdgeDistanceFromOrigin(_grid.Length)) || _snake.CheckCollisionBody())
                    {
                        // Make the game over
                        GameOver = true;
                        // Game shall not update anymore
                        GameActive = false;
                        // Return, this is because to block the models from updating to their next position, like say if you collide with edge, then the player would see a snake head outside the border
                        return;
                    }
                    // Update all visual models to their new position
                    _snake.UpdateMovement();
                    // Reset the key presses for options.ControlsHold at line 96
                    leftKey = false;
                    rightKey = false;

                    // If cameraTopView timer is less than move delay (In other words, when the timer is on it's last move out of four) and Return is true, make it false and eatBlob true
                    if (blobCameraTimer < moveDelay && eatBlobReturn)
                    {
                        eatBlobReturn = false;
                        eatBlob = true;
                    }

                    // If eatBlob is true (or as stated in line 191, when Return is true and last round of four),
                    // then make the camera have springiness (for making a smooth movement when camera change position from third to top or vice versa)
                    // Also make the camera go smooth when the snake turns
                    if (_snake.SnakeHead.Turn == 0 || _snake.SnakeHead.Turn == 2 || eatBlob)
                    {
                        ((ChaseCamera) _camera).Springiness = 0.15f;
                        eatBlob = false;
                    }
                    else
                    {
                        // If snake is going straight and have not eaten blob, the camera is instant
                        ((ChaseCamera) _camera).Springiness = 1f;
                    }

                    // At end, reset the timer to it's delay (see line 90)
                    moveTimer = moveDelay;
                }
                else
                {
                    // If timer is not under zero, make the timer go down until it gets to zero
                    moveTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                }
                // Always make the TopViewTimer subtract with elapsed time
                blobCameraTimer -= (float) gameTime.ElapsedGameTime.TotalMilliseconds;

                // If there is time left for TopView
                if (blobCameraTimer > 0)
                {
                    // Make the camera rise 8 times the height of the normal height times a scale (based on when the grid was 13)
                    ((ChaseCamera)_camera).PositionOffset = new Vector3(0, 600*8*((float)_grid.Length / 13), 800);
                }
                else
                {
                    // Make the camera have normal height
                    ((ChaseCamera)_camera).PositionOffset = new Vector3(0, 600, 800);
                }
                // Make the camera focus a little bit above the snakeHead
                ((ChaseCamera)_camera).TargetOffset = new Vector3(0, 200, 0);
                // Make the camera move accordingly by snake head's movement
                ((ChaseCamera)_camera).Move(_snake.SnakeHead.CustomModel.Position, _snake.SnakeHead.CustomModel.Rotation);
                

            }
            // If the game is not active
            else if (!GameActive)
            {
                // Make the camera have a slow movement
                ((ChaseCamera) _camera).Springiness = 0.05f;

                // A scale (based on when the grid was 13)
                float scale = (float)_grid.Length / 13;
                // Rotation change updates and changes, well, rotation, at the speed of 180 divided by 1500
                _rotationChange += new Vector3(0, (float)Math.PI/1500, 0);
                // Zoom out from the play area (based on scale)
                ((ChaseCamera)_camera).PositionOffset = new Vector3(0, 2000 * scale, 4000 * scale);
                // Focus the camera on the left side of the map
                ((ChaseCamera)_camera).TargetOffset = new Vector3(-1000 * scale, 0, 0);
                // Move the camera based on center position and rotation from snakeHead and rotationChange (snakeHead is in there so that the camera will always zoom back from the snake, and not making a 180)
                ((ChaseCamera)_camera).Move(Vector3.Zero, _snake.SnakeHead.CustomModel.Rotation + _rotationChange);

            }
        }

        // Start a game (or continue)
        public void StartGame(GameTime gameTime)
        {
            // Can only start game if game is not active
            if (!GameActive)
            {
                // Reset blob camera timer to under zero (see line 155)
                blobCameraTimer = -1;
                // Reset rotation change (see line 249)
                _rotationChange = Vector3.Zero;
                // Make the camera move with a slow movement to it's position behind snake head
                ((ChaseCamera)_camera).Springiness = 0.05f;
                ((ChaseCamera)_camera).PositionOffset = new Vector3(0, 600, 800);
                ((ChaseCamera)_camera).TargetOffset = new Vector3(0, 200, 0);
                ((ChaseCamera)_camera).Move(_snake.SnakeHead.CustomModel.Position, _snake.SnakeHead.CustomModel.Rotation);
                // Make the countdown tick down with a 150% speed
                countDownTimer -= (float)gameTime.ElapsedGameTime.TotalMilliseconds*1.5f;
                // Make the countdown drawn on screen when starting a game
                showCountDownTimer = true;
                
                // If countdown is 0
                if (countDownTimer <= 0)
                {
                    // Turn on the game
                    GameActive = true;
                    // And make the countdown text disappear
                    showCountDownTimer = false;
                }

            }
        }

        // If pause game (need StartGame (line 261) to start again)
        public void PauseGame()
        {
            // Make the game non-active
            GameActive = false;
            // Reset countdown delay
            countDownTimer = countDownDelay;
        }

        // Update the camera
        public void UpdateCamera()
        {
            _camera.Update();
        }

        // Reset the game
        public void ResetGame()
        {
            // All classes reset
            _snake.Reset();
            _blob.Spawn(1, 2);
            _grid.Length = 5;
            // Generate a new grid
            _grid.Generate(5);
            _grid.GenerateEdge(5);
            // Reset score
            Score = 0;
            // Make gameOver state false
            GameOver = false;
            // Reset countdown delay
            countDownTimer = countDownDelay;
        }

        // Garbage, meant to update the theme and change the models
        public void ApplyChanges(ContentManager content)
        {
            //_snake.Load(content, options.ThemeSnake);
            //_blob.Load(content, options.ThemeBlob);
            //_grid.Load(content, options.ThemeGrid);
            //_background = new SkyBox(content, gd, content.Load<TextureCube>("Models/Background_" + options.ThemeBackground));
            //_grid.Generate(_grid.Length);
            //_grid.GenerateEdge(_grid.Length);
        }




        // Draw method
        public void Draw(SpriteBatch spriteBatch)
        {
            // if game is active, draw score
            if (GameActive)
            {
                // Uses mainFont, using score as text, position is the middle up of the screen and the middle of the text, color is white
                spriteBatch.DrawString(mainFont, Score.ToString(), new Vector2(Game1.screenWidth/2 - (int)mainFont.MeasureString(Score.ToString()).X/2, Game1.screenHeight / 32), Color.LightGray);
                
            }

            // If countdown timer is true and is above 0
            if (showCountDownTimer)
            {
                if (countDownTimer > 0)
                {
                    // If it is under one second, change text from "1" to "GO!"
                    if (countDownTimer <= 1000)
                    {
                        // Uses mainFont, using score as text, position is the middle up of the screen and the middle of the text, color is white
                        spriteBatch.DrawString(mainFont, "GO!", new Vector2(Game1.screenWidth / 2 - (int)mainFont.MeasureString("GO!").X / 2, Game1.screenHeight / 2 - (int)mainFont.MeasureString("GO!").Y / 2), Color.AliceBlue);
                    }
                    else
                    {
                        // Make the countdown string based on an int
                        string countDown = ((int)countDownTimer / 1000).ToString();
                        // Draw the string in the same spot as "GO!" at Line 359
                        spriteBatch.DrawString(mainFont, countDown, new Vector2(Game1.screenWidth / 2 - (int)mainFont.MeasureString(countDown).X / 2, Game1.screenHeight / 2 - (int)mainFont.MeasureString(countDown).Y / 2), Color.AliceBlue);
                    }
                }
            }

            // Draw method for all 3D-objects
            Draw(_camera.View, _camera.Projection, ((ChaseCamera)_camera).Position);
        }

        // Draw all 3D-objects
        public void Draw(Matrix View, Matrix Projection, Vector3 CameraPosition)
        {
            _background.Draw(View, Projection, CameraPosition);
            _grid.Draw(View, Projection, CameraPosition);
            _snake.Draw(View, Projection, CameraPosition);
            _blob.Draw(View, Projection, CameraPosition);
        }

    }
}
