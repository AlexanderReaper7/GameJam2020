using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Tools_XNA
{
    public class Collect
    {

        //Player Placering
        public Vector2 playerPosition;
        //player sprite
        public Texture2D playerSprite;
        //Font
        public SpriteFont font;
        //Score
        public int timeScore = 0;
        //Bool for collecting specific collectible
        public bool collectColl;
        //Playerposition that should work with playerManager
        public Vector3 playerPos;
        //Rect for collect in 2d
        public Rectangle coin1;
        //3D Collectible
        //Position for score on the screeen
        public Vector2 scorePos = new Vector2(100, 100);
        KeyboardState keyBoard;
        GamePadState gamePad;

        public Collect
        (
        int timeScore,
        bool collectColl

        )
        {
            this.timeScore = timeScore;
            this.collectColl = collectColl;

        }
        //LoadContent for the font
        public void LoadContent(ContentManager content)
        {
            // Load font for score
            font = content.Load<SpriteFont>(@"Shared/Fonts/TestFont");

        }
        //Update
        public void Update(GameTime gameTime)
        {

            //Create a new playerpos// //should be changed to set to playerManager position//
            playerPos = new Vector3();
            timeScore += gameTime.ElapsedGameTime.Milliseconds;
            //If for making you only able to collect once
            if (collectColl == false)
            {
                //Updates position if not collected
                coin1 = new Rectangle(300, 300, 20, 20);
                
            }

        }
        //Draw for collect
        public void Draw(SpriteBatch spriteBatch)
        {
            //Changed begin for working with 3d
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, null, null);
            //Draw string using score
            spriteBatch.DrawString(font, timeScore.ToString(), scorePos, Color.White);
            spriteBatch.End();
        }
    }
}