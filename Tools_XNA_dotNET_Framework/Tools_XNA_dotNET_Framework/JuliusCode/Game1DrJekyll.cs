using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using System.IO;
using System.Security.Policy;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameJam2020;
using static GameJam2020.Tiles;

namespace jekyll
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        #region Classes
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        // Own Made Classes
        TilesMap map1;
        TilesMap mapTown;
        TilesMap mapMarket;
        TilesMap mapStreet;
        TilesMap mapRoom;
        Player player;
        EnemyAI enemyAI;
        #endregion

        #region Game1 Variables

        // Background for the TownMap | Julius 18-11-28
        Texture2D levelBackground;
        Texture2D levelBackgroundNM;

        //image variables //emil 07-11-18
        Texture2D menuPointerImage;
        Texture2D menuImage;
        //more image variables //emil 22-11-18
        Texture2D creditsMenuImage;
        Texture2D optionsMenu;
        //even more image variables //emil 06-12-18
        Texture2D winMenuImage;
        Texture2D loseMenuImage;
        //hopefully the last batch of image variables //emil 07-12-18
        Texture2D drugStoreImage;
        Texture2D itemGreenPotionImage;
        Texture2D itemYellowPotionImage;
        Texture2D itemRedPotionImage;
        Texture2D itemLanternImage;
        Texture2D itemKeyImage;
        Texture2D hydesRoomImage;
        Texture2D jekyllsLabImage;
        Texture2D jekyllsHouseImage;
        Texture2D inventoryImage;
        Texture2D streetImage;
        Texture2D streetNM;
        Texture2D highScoreMenuImage;
        Texture2D itemWhitePowderImage;
        Texture2D marketSquare;
        Texture2D marketSquareNM;

        // Image for game intro | Julius 18-12-10
        Texture2D createGameImage1;
        Texture2D createGameImage2;

        //previous gamestate //emil 21-11-18
        int previousGamestate;

        //variable for chainging selected menu section //emil 07-11-18
        float menuSlection = 1f;

        //variable for where the menu and menu pointer is positioned //emil 07-11-18
        Vector2 menuPosition = new Vector2(0, 0);
        Vector2 menuPointerPosition = new Vector2(85, 51);

        // Free Posision Variable for small stuff, in this case drugstore | Julius 18-12-07
        Vector2 freePosition;
        int freeIntX;
        int freeIntY;
        int i;

        //variable for removing buttonspam //emil 07-11-18
        bool isKeyUp = false;

        // Variables for keymanegement | Julius 18-12-10
        private bool keyUp, keyDown, keyRight, keyLeft, keySelect, keyBack;
        

        // A region for all states and variables to switch between them | Julius 18-12-05
        #region Level Variables
        //all the gamestates used in the game //emil 07-11-18
        // Updated levelState(1) to (6) with names | Julius 18-12-05
        enum Gamestates
        {
            mainMenu,
            exit,
            optionsMenu,
            creditsMenu,
            createGame,
            levelStateDrJekyllRoom,
            levelStateDrJekyllLAB,
            levelStateMrHydeRoom,
            levelStateDrugStore,
            levelStateCityMarket,
            levelStateCityAlley,
            levelState7,
            levelState8,
            levelState9,
            levelStateTownMap,
            winMenu,
            loseMenu,
            highScoreMenu
        }
        Gamestates gamestate = Gamestates.mainMenu;

        // Rectangles for when player is intersecting and changes to that location | Julius 18-12-05
        private Rectangle warpTo_TownMap;
        private Rectangle warpTo_CityAlley;
        private Rectangle warpTo_CityMarket;
        private Rectangle warpTo_DrugStore;
        private Rectangle warpTo_DrJekyllRoom;
        private Rectangle warpTo_DrJekyllLAB;
        private Rectangle warpTo_MrHydeRoom;
        private Texture2D warpHitBoxTexture;

        private Rectangle itemRectangle;
        private bool itemWhitePowder = false;
        private bool itemKey = false;
        private bool itemGreenPotion = false;
        private bool itemYellowPotion = false;
        private bool itemRedPotion = false;
        private bool itemLantern = false;
        // A bool for when to show inventory | Julius 18-12-10
        private bool inventoryShow = false;

        #endregion

        // A region for all variables needed for the scoreboard | Julius 18-12-10
        #region ScoreBoard Variables

        // Variables for the score text | Julius 18-12-09
        Vector2 scorePosition1 = new Vector2(120, 130);
        Vector2 scoreFontSpace = new Vector2(0, 30);
        Vector2 scoreY = new Vector2(0, 1);
        Vector2 scoreX = new Vector2(1, 0);
        SpriteFont scoreFont;
        SpriteFont scoreFontTitle;


        // Variables for Score, Name and ArrayIdentity | Julius 18-12-09
        int playerScore;
        bool playerTimer = false;
        string[] name = { "Null" };
        int nameArray = 0;
        int arrayLeangth = 5;

        // Variables for not saving the score multiple times | Julius 18-12-09
        bool scoreIsKeyUp;

        // The name of the file | Julius 18-12-09
        public readonly string Filename = "saveFile.dat";

        #endregion

        // DeBugging Variables
        bool showCollisions = false;
        

        // Light Effect Variables | Julius 18-12-5
        #region Dynamic Lights Variables

        // Layers
        private RenderTarget2D colorMapRenderTarget;
        private RenderTarget2D normalMapRenderTarget;
        private RenderTarget2D shadowMapRenderTarget;

        // Lightning effect
        private Effect lightEffect;
        private Effect lightCombinedEffect;

        private EffectParameter lightEffectParameterScreenWidth;
        private EffectParameter lightEffectParameterScreenHeight;
        private EffectParameter lightEffectParameterNormalMap;

        private EffectTechnique lightCombinedEffectTechnique;
        private EffectParameter lightCombinedEffectParameterAmbient;
        private EffectParameter lightCombinedEffectParameterLightAmbient;
        private EffectParameter lightCombinedEffectParameterAmbientColor;
        private EffectParameter lightCombinedEffectParameterColorMap;
        private EffectParameter lightCombinedEffectParameterNormalMap;
        private EffectParameter lightCombinedEffectParameterShadowMap;

        // Dynamic Light
        private List<Light> lights = new List<Light>();
        private float specularStrength = 1.0f;

        private EffectTechnique lightEffectTechniquePointLight;
        private EffectParameter lightEffectParameterStrength;
        private EffectParameter lightEffectParameterPosition;
        private EffectParameter lightEffectParameterConeDirection;
        private EffectParameter lightEffectParameterLightColor;
        private EffectParameter lightEffectParameterLightDecay;

        public VertexPositionColorTexture[] Verticies;
        public VertexBuffer VertexBuffer;

        #endregion
        
        // Ambient Light
        private Color ambientLight = new Color(.01f, .01f, .01f, 1);

        // SaveLoad methods | Julius 18-12-10
        #region Save/Load Content
        // SaveFile Content | Julius 18-12-09
        [Serializable]
        public struct SaveData
        {
            public string[] Date;
            public int[] Score;
            // Count = Total amount of Arrays | Julius 18-12-09
            public int Count;

            public SaveData(int count)
            {
                Date = new string[count];
                Score = new int[count];

                Count = count;

            }
        }

        // Function for opening the file and write | Julius 18-12-09
        public static void DoSave(SaveData data, string filename)
        {
            // Open the file, if not exist then create a new one | Julius 18-12-09
            FileStream stream = File.Open(filename, FileMode.Create);
            try
            {
                // Convert to XML and try to open the stream | Julius 18-12-09
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close File | Julius 18-12-09
                stream.Close();
            }

        }

        // Function for opening the file and read | Julius 18-12-09
        public static SaveData LoadData(string Filename)
        {
            SaveData data;

            // Get the path of the save game | Julius 18-12-09
            string fullpath = Filename;

            // Open the file | Julius 18-12-09
            FileStream stream = File.Open(fullpath, FileMode.Open, FileAccess.Read);
            try
            {
                // Read the data from the file | Julius 18-12-09
                XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
                data = (SaveData)serializer.Deserialize(stream);
            }
            finally
            {
                // Close the file | Julius 18-12-09
                stream.Close();
            }

            return (data);

        }

        // Function for easily saving time (score) and sorting it | Julius 18-12-09
        private void SaveHighScore()
        {
            // Create the data to save | Julius 18-12-09
            SaveData data = LoadData(Filename);


            // Look if current time is lower (score) than those on the list | Julius 18-12-09
            int scoreIndex = -1;
            // ForLoop to check every array's value | Julius 18-12-09
            for (int i = 0; i < data.Count; i++)
            {
                if (playerScore < data.Score[i])
                {
                    // Remember which Array | Julius 18-12-09
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex > -1)
            {
                // New highscore found ... put all the other arrays one step lower (making space for the new time) | Julius 18-12-09
                for (int i = data.Count - 1; i > scoreIndex; i--)
                {
                    data.Date[i] = data.Date[i - 1];
                    data.Score[i] = data.Score[i - 1];
                }
                // Save the new score | Julius 18-12-09
                data.Date[scoreIndex] = DateTime.Now.ToString();
                data.Score[scoreIndex] = playerScore;

                DoSave(data, Filename);
            }

        }

        // Function for reseting the scoreboard | Julius 18-12-09
        private void ResetHighScore()
        {
            SaveData data = LoadData(Filename);
            // With a forloop | Julius 18-12-09
            for (int i = 0; i < arrayLeangth; i++)
            {
                data.Date[i] = "null";
                data.Score[i] = 99999 * 1000;
            }

            // Save the data into the file | Julius 18-12-09
            DoSave(data, Filename);

        }
        #endregion


        #endregion



        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            // Changes the game's window size | Julius 18-11-26
            graphics.PreferredBackBufferWidth  = 800;
            graphics.PreferredBackBufferHeight = 600;
            graphics.ApplyChanges();

            // Declare how many different TilesMap shall exist | Julius 18-12-10
            map1 = new TilesMap();
            mapTown = new TilesMap();
            mapMarket = new TilesMap();
            mapRoom = new TilesMap();
            mapStreet = new TilesMap();

            // Check if the file already exist, if not then create one | Julius 18-12-09
            if (!File.Exists(Filename))
            {
                SaveData data = new SaveData(5);
                for (int i = 0; i < arrayLeangth; i++)
                {
                    data.Date[i] = "null";
                    // We need a high value so that the player cannot overcome the score
                    data.Score[i] = 99999 * 1000;
                }

                // Save the data into the file | Julius 18-12-09
                DoSave(data, Filename);
            }


            base.Initialize();
        }
        



        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);


            //loads the menu Pointer //emil 07-11-18
            menuPointerImage = Content.Load<Texture2D>(@"Textures/GUI/MenuPointer");
            //loads the menu //emil 07-11-18
            menuImage = Content.Load<Texture2D>(@"Textures/GUI/Menu");
            //loads the credits menu //emil 22-11-18
            creditsMenuImage = Content.Load<Texture2D>(@"Textures/GUI/CreditsMenu");
            //loads the options menu //emil 22-11-18
            optionsMenu = Content.Load<Texture2D>(@"Textures/GUI/OptionsMenu");
            // Added the highscore menu | Julius 18-12-10
            highScoreMenuImage = Content.Load<Texture2D>(@"Textures/GUI/highscoreBackground");


            // Textures for rectangles that warp a player when it is intersecting | Julius 18-12-05
            warpHitBoxTexture = Content.Load<Texture2D>(@"Textures/Assets/WarpHitBox");

            // win and lose menu images //emil 06-12-18
            winMenuImage = Content.Load<Texture2D>(@"Textures/GUI/Winscreen");
            loseMenuImage = Content.Load<Texture2D>(@"Textures/GUI/GameOver");
            // Added intro images | Julius 18-12-10
            createGameImage1 = Content.Load<Texture2D>(@"Textures/GUI/Intro1");
            createGameImage2 = Content.Load<Texture2D>(@"Textures/GUI/Intro2");

            //loads all of the item images //emil 07-12-18
            itemYellowPotionImage = Content.Load<Texture2D>(@"Textures/Assets/gulPotionFlaska");
            itemGreenPotionImage = Content.Load<Texture2D>(@"Textures/Assets/grönPotionFlaska");
            itemRedPotionImage = Content.Load<Texture2D>(@"Textures/Assets/Rödpotionflaska");
            itemLanternImage = Content.Load<Texture2D>(@"Textures/Assets/Lykta");
            itemKeyImage = Content.Load<Texture2D>(@"Textures/Assets/Nyckel1");
            itemWhitePowderImage = Content.Load<Texture2D>(@"Textures/Assets/WhitePowder1");

            //loads all rooms //emil 07-12-18
            drugStoreImage = Content.Load<Texture2D>(@"Textures/Assets/DrugStore");
            hydesRoomImage = Content.Load<Texture2D>(@"Textures/Assets/hydesRum");
            jekyllsHouseImage = Content.Load<Texture2D>(@"Textures/Assets/jekyllsHus");
            jekyllsLabImage = Content.Load<Texture2D>(@"Textures/Assets/jekyllsLabb");
            marketSquare = Content.Load<Texture2D>(@"Textures/Assets/marketSquare");
            marketSquareNM = Content.Load<Texture2D>(@"Textures/Assets/marketSquareNM");
            streetImage = Content.Load<Texture2D>(@"Textures/Assets/Street");
            streetNM = Content.Load<Texture2D>(@"Textures/Assets/StreetNM");

            //loads inventory image //emil 07-12-18
            // Updated textures | Julius 18-12-11
            inventoryImage = Content.Load<Texture2D>(@"Textures/GUI/Inventory");

            //hydes place

            // Loads textures for player | Julius 18-12-03
            player = new Player(Content.Load<Texture2D>(@"Textures/Assets/DrJekyll"), 1, 39, 69);
            // Loads texture for enemy | Julius 18-12-03
            enemyAI = new EnemyAI(Content.Load<Texture2D>(@"Textures/Assets/MrHyde"), Content.Load<Texture2D>(@"Textures/Assets/MrHydeNM"), 1, 39, 69,player);

            // A region for all the Tiles and stuff | Julius 18-12-03
            #region Tiles and TilesMap
            // Load the textures for the Tiles | Julius 18-12-03
            Tiles.Content = Content;

            // Generates a map of tiles (prototype) | Julius 18-12-03
            map1.Generate(new int[,] {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},

            }, 50);

            // Generates a map of tiles for the town | Julius 18-12-03
            mapTown.Generate(new int[,]
            {
                {1,1,1,1,0,1,1,0,0,1,1,1,1,1,1,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1},
                {1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,1},
                {1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,1},
                {1,0,0,0,0,1,1,0,0,1,1,0,0,0,0,0},
                {1,0,1,1,0,0,0,0,0,0,0,0,1,1,0,1},
                {1,0,1,1,0,0,0,0,0,0,0,0,1,1,0,1},
                {0,0,0,0,0,1,1,0,0,1,1,0,0,0,0,1},
                {1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,1},
                {1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,1},
                {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1},
            }, 50);

            // generates a map of tiles for the market //emil 07-12-18
            mapMarket.Generate(new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1},
                {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1},
                {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1},
                {1,1,1,0,0,0,0,0,0,0,0,0,0,1,1,1},
                {1,1,1,0,0,0,1,1,1,0,0,0,0,1,1,1},
                {1,0,0,0,0,1,1,1,1,1,0,0,0,0,0,1},
                {1,0,0,0,0,1,1,1,1,1,0,0,0,0,0,1},
                {1,0,0,0,0,1,1,1,1,1,0,0,0,0,0,1},
                {1,1,1,0,0,0,1,1,1,0,0,1,1,1,1,1},
                {1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1},
                {1,1,1,0,0,0,0,0,0,0,0,1,1,1,1,1},
            }, 50);

            // generates a map of tiles for all rooms //emil 07-12-18
            mapRoom.Generate(new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
            }, 50);

            //generates a map of tiles for the ally ways //emil 07-12-18
            mapStreet.Generate(new int[,]
            {
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1},
                {1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
                {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            }, 50);

            #endregion

            // A background for the level | Julius 18-12-03
            levelBackground = Content.Load<Texture2D>("Textures/Assets/Level_Background");
            levelBackgroundNM = Content.Load<Texture2D>("Textures/Assets/Level_BackgroundNM");


            // A region full of loading light effects
            #region Dynamic Lights Load

            PresentationParameters pp = GraphicsDevice.PresentationParameters;
            int width = pp.BackBufferWidth;
            int height = pp.BackBufferHeight;
            SurfaceFormat format = pp.BackBufferFormat;

            // Dynamic Light
            Verticies = new VertexPositionColorTexture[4];
            Verticies[0] = new VertexPositionColorTexture(new Vector3(-1, 1, 0), Color.White, new Vector2(0, 0));
            Verticies[1] = new VertexPositionColorTexture(new Vector3(1, 1, 0), Color.White, new Vector2(1, 0));
            Verticies[2] = new VertexPositionColorTexture(new Vector3(-1, -1, 0), Color.White, new Vector2(0, 1));
            Verticies[3] = new VertexPositionColorTexture(new Vector3(1, -1, 0), Color.White, new Vector2(1, 1));

            VertexBuffer = new VertexBuffer(GraphicsDevice, typeof(VertexPositionColorTexture), Verticies.Length, BufferUsage.None);
            VertexBuffer.SetData(Verticies);




            colorMapRenderTarget = new RenderTarget2D(GraphicsDevice, width, height);
            normalMapRenderTarget = new RenderTarget2D(GraphicsDevice, width, height);
            shadowMapRenderTarget = new RenderTarget2D(GraphicsDevice, width, height, false,
            format, pp.DepthStencilFormat, pp.MultiSampleCount, RenderTargetUsage.DiscardContents);

            lightEffect = Content.Load<Effect>("MultiTarget");
            lightCombinedEffect = Content.Load<Effect>("DeferredCombined");

            lightEffectParameterScreenWidth = lightEffect.Parameters["screenWidth"];
            lightEffectParameterScreenHeight = lightEffect.Parameters["screenHeight"];
            lightEffectParameterNormalMap = lightEffect.Parameters["NormalMap"];

            // Dynamic Light
            lightEffectTechniquePointLight = lightEffect.Techniques["DeferredPointLight"];
            lightEffectParameterConeDirection = lightEffect.Parameters["coneDirection"];
            lightEffectParameterLightColor = lightEffect.Parameters["lightColor"];
            lightEffectParameterLightDecay = lightEffect.Parameters["lightDecay"];
            lightEffectParameterPosition = lightEffect.Parameters["lightPosition"];
            lightEffectParameterStrength = lightEffect.Parameters["lightStrength"];


            lightCombinedEffectTechnique = lightCombinedEffect.Techniques["DeferredCombined2"];
            lightCombinedEffectParameterAmbient = lightCombinedEffect.Parameters["ambient"];
            lightCombinedEffectParameterLightAmbient = lightCombinedEffect.Parameters["lightAmbient"];
            lightCombinedEffectParameterAmbientColor = lightCombinedEffect.Parameters["ambientColor"];
            lightCombinedEffectParameterColorMap = lightCombinedEffect.Parameters["ColorMap"];
            lightCombinedEffectParameterNormalMap = lightCombinedEffect.Parameters["NormalMap"];
            lightCombinedEffectParameterShadowMap = lightCombinedEffect.Parameters["ShadingMap"];


            #endregion

            lights.Add(new PointLight()
            {
                IsEnabled = true,
                Color = new Vector4(1.0f, 1.0f, 0.9f, 1.0f),
                Power = 2f,
                LightDecay = 140,
                Position = new Vector3(500, 400, 80)
            });

            // Load font | Julius 18-12-09
            scoreFont = Content.Load<SpriteFont>(@"SpriteFont1");
            scoreFontTitle = Content.Load<SpriteFont>(@"SpriteFont2");
        }


        protected override void UnloadContent()
        {



        }
        
        protected override void Update(GameTime gameTime)
        {

            // Allows the game to exit | Julius 18-11-21
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.End))
                this.Exit();

            







            //sets the variable iskeyup to true when none of the selected keys are pressed //emil 07-11-18
            //added M and E to the roster //emil 21-11-18
            #region Key Logic
            if (Keyboard.GetState().IsKeyUp(Keys.Enter) &&
                Keyboard.GetState().IsKeyUp(Keys.W)     &&
                Keyboard.GetState().IsKeyUp(Keys.D)     &&
                Keyboard.GetState().IsKeyUp(Keys.A)     &&
                Keyboard.GetState().IsKeyUp(Keys.S)     &&
                Keyboard.GetState().IsKeyUp(Keys.Q)     &&
                Keyboard.GetState().IsKeyUp(Keys.Z)     &&
                Keyboard.GetState().IsKeyUp(Keys.X)     &&
                Keyboard.GetState().IsKeyUp(Keys.Back)  &&
                Keyboard.GetState().IsKeyUp(Keys.Up)    &&
                Keyboard.GetState().IsKeyUp(Keys.Down)  &&
                Keyboard.GetState().IsKeyUp(Keys.Left)  &&
                Keyboard.GetState().IsKeyUp(Keys.Right) &&
                Keyboard.GetState().IsKeyUp(Keys.E)     &&
                Keyboard.GetState().IsKeyUp(Keys.M))
            {
                isKeyUp = true;
            }

            // Kode that make some keys have the same purpose/input | Julius 18-12-10
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) ||
                Keyboard.GetState().IsKeyDown(Keys.E) ||
                Keyboard.GetState().IsKeyDown(Keys.X))
                keySelect = true;
            else keySelect = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Back) ||
                Keyboard.GetState().IsKeyDown(Keys.Q) ||
                Keyboard.GetState().IsKeyDown(Keys.Z))
                keyBack = true;
            else keyBack = false;


            if (Keyboard.GetState().IsKeyDown(Keys.Up) ||
                Keyboard.GetState().IsKeyDown(Keys.W))
                keyUp = true;
            else keyUp = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Down) ||
                Keyboard.GetState().IsKeyDown(Keys.S))
                keyDown = true;
            else keyDown = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Left) ||
                Keyboard.GetState().IsKeyDown(Keys.A))
                keyLeft = true;
            else keyLeft = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Right) ||
                Keyboard.GetState().IsKeyDown(Keys.D))
                keyRight = true;
            else keyRight = false;






            #endregion

            // Add time to the score if timer is on

            if (playerTimer == true)
                playerScore += gameTime.ElapsedGameTime.Milliseconds;



            //switch for all the gamestates //emil 07-11-18
            #region Switch GameStates


            //all instances of previousGamestate = Convert.ToInt32(gamestate); sets the current gamestate to previous gamestate //emil 21-11-18
            switch (gamestate)
            {


                #region menu states
                case Gamestates.mainMenu:
                {
                    inventoryShow = false;

                        #region move up or down the menu
                        //decreases the menuselector to ascend through the menu //emil 07-11-18
                        if ((keyUp==true) &&
                            (isKeyUp == true &&
                            menuSlection > 1))
                        {
                            menuSlection--;
                            isKeyUp = false;
                        }



                        //increases the menuselector to desend through the menu //emil 07-11-18
                        if ((keyDown == true) &&
                            (isKeyUp == true &&
                            menuSlection < 5))
                        {
                            menuSlection++;
                            isKeyUp = false;
                        }
                        #endregion


                        #region option in the main menu
                        //changes to new game when the menu selector is 1 //emil 07-11-18
                        if (keySelect==true && isKeyUp == true && menuSlection == 1)
                        {
                            previousGamestate = Convert.ToInt32(gamestate);
                            gamestate = Gamestates.createGame;
                            i = 0;
                            isKeyUp = false;
                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;
                        }



                        //changes to load game if the menu selector is 2 //emil 07-11-18
                        if (keySelect==true && isKeyUp == true && menuSlection == 2)
                        {
                            previousGamestate = Convert.ToInt32(gamestate);
                            gamestate = Gamestates.highScoreMenu;
                            isKeyUp = false;
                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;

                        }



                        //changes to options if the menu selector is 3 //emil 07-11-18
                        if (keySelect == true && isKeyUp == true && menuSlection == 3)
                        {
                            previousGamestate = Convert.ToInt32(gamestate);
                            gamestate = Gamestates.optionsMenu;
                            isKeyUp = false;
                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;

                        }



                        //changes to credits if the menu selector is 4 //emil 07-11-18
                        if (keySelect == true && isKeyUp == true && menuSlection == 4)
                        {
                            previousGamestate = Convert.ToInt32(gamestate);
                            gamestate = Gamestates.creditsMenu;
                            isKeyUp = false;
                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;

                        }



                        //exits the game if the menu selector is 5 //emil 07-11-18
                        if (keySelect == true && isKeyUp == true && menuSlection == 5)
                        {
                            this.Exit();
                        }
                        #endregion


                        #region menuPointerPositions
                        //set the Position of the menu pointer to the differant options depending on the menu selector //emil 07-11-18
                        //Position for new game //emil 07-11-18
                        if (menuSlection == 1)
                        {
                            menuPointerPosition = new Vector2(266, 221);
                        }
                        //Position for load game //emil 07-11-18
                        if (menuSlection == 2)
                        {
                            menuPointerPosition = new Vector2(266, 297);
                        }
                        // Position for options //emil 07-11-18
                        if (menuSlection == 3)
                        {
                            menuPointerPosition = new Vector2(266, 358);
                        }
                        //Position for credits //emil 07-11-18
                        if (menuSlection == 4)
                        {
                            menuPointerPosition = new Vector2(266, 421);
                        }
                        //Position for exit //emil 07-11-18
                        if (menuSlection == 5)
                        {
                            menuPointerPosition = new Vector2(266, 491);
                        }
                        #endregion


                    }
                    break;

                case Gamestates.createGame:
                {
                    inventoryShow = false;
                        // Reset and stop timer/score | Julius 18-12-10
                        playerTimer = false;
                    playerScore = 0;
                        // Key to proceed in the intro //Hampus 18/12/10
                    if (keySelect == true &&
                        isKeyUp == true &&
                        i == 0)
                    {
                        i++;
                        isKeyUp = false;
                    }

                        // Start game //Hampus 18/12/10
                        if (keySelect == true &&
                        isKeyUp == true &&
                        i == 1)
                    {
                        gamestate = Gamestates.levelStateDrJekyllRoom;
                        player.position = (new Vector2(160, 420));
                        // Start timer | Julius 18-12-10
                        playerTimer = true;
                        i = 0;
                        isKeyUp = false;
                    }
                    // Key to move back in the intro //Hampus 18/12/10
                    if (keyBack == true &&
                        isKeyUp == true &&
                        i == 1)
                    {
                        i--;
                        isKeyUp = false;
                    }

                        // Back to menu //Hampus 18/12/10
                        if (keyBack == true &&
                        isKeyUp == true &&
                        i == 0)
                    {
                        gamestate = Gamestates.mainMenu;
                        i = 0;
                        isKeyUp = false;
                    }



                        //key for getting back to the menu //emil 22-11-18
                        if (keyBack==true &&
                            isKeyUp == true)
                        {
                            previousGamestate = Convert.ToInt32(gamestate);
                            gamestate = Gamestates.mainMenu;
                            isKeyUp = false;
                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;
                            
                        }




                    }
                    break;

                case Gamestates.creditsMenu:
                    {
                        inventoryShow = false;

                        //key to get back to the main menu //emil 22-11-18
                        if (keyBack == true &&
                            isKeyUp == true)
                        {
                            previousGamestate = Convert.ToInt32(gamestate);
                            gamestate = Gamestates.mainMenu;
                            isKeyUp = false;
                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;

                        }
                    }
                    break;
                    

                case Gamestates.optionsMenu:
                    {
                        inventoryShow = false;

                        #region move up or down the menu
                        //decreases the menuselector to ascend through the menu //emil 07-11-18
                        if ((keyUp == true) &&
                            (isKeyUp == true &&
                            menuSlection > 1))
                        {
                            menuSlection--;
                            isKeyUp = false;
                        }


                        //increases the menuselector to desend through the menu //emil 07-11-18
                        if ((keyDown == true) &&
                            (isKeyUp == true &&
                            menuSlection < 2))
                        {
                            menuSlection++;
                            isKeyUp = false;
                        }
                        #endregion

                        if (keySelect == true &&
                            (isKeyUp == true &&
                             menuSlection == 1))
                            // Toggle to fullscreen | Julius 18-12-11
                            // Doesn't work? My computer freezes | Julius 18-12-11
                        {
                            //graphics.ToggleFullScreen();
                        }


                            

                            //sets the gamestate to main menu when the BACK key is pressed //emil 21-11-18
                            if (keyBack == true &&
                           isKeyUp == true)
                        {
                            

                            previousGamestate = Convert.ToInt32(gamestate);
                            gamestate = Gamestates.mainMenu;
                            isKeyUp = false;

                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;

                        }
                        //Position for fullscrean //emil 07-11-18
                        if (menuSlection == 1)
                        {
                            menuPointerPosition = new Vector2(524, 174);
                        }
                        //Position for sound on/off //emil 07-11-18
                        if (menuSlection == 2)
                        {
                            menuPointerPosition = new Vector2(524, 282);
                        }

                    }
                    break;





                //win menu gamestate //emil 06-12-18
                case Gamestates.winMenu:
                {
                    inventoryShow = false;

                    itemWhitePowder = false;
                    itemKey = false;
                    itemGreenPotion = false;
                    itemYellowPotion = false;
                    itemRedPotion = false;
                    itemLantern = false;

                        // Stops timer | Julius 18-12-10
                        playerTimer = false;

                        #region move up or down the menu
                        //decreases the menuselector to ascend through the menu //emil 07-11-18
                        if ((keyUp == true) &&
                            (isKeyUp == true &&
                            menuSlection > 1))
                        {
                            menuSlection--;
                            isKeyUp = false;
                        }



                        //increases the menuselector to desend through the menu //emil 07-11-18
                        if ((keyDown == true) &&
                            (isKeyUp == true &&
                            menuSlection < 2))
                        {
                            menuSlection++;
                            isKeyUp = false;
                        }
                        #endregion


                        #region options in the win screen
                        //changes to main menu if the menu selector is 1 //emil 07-11-18
                        if (keySelect == true && isKeyUp == true && menuSlection == 1)
                        {
                            previousGamestate = Convert.ToInt32(gamestate);
                            // Save Highscore | Julius 18-12-10
                            SaveHighScore();
                            gamestate = Gamestates.mainMenu;
                            i = 0;
                            isKeyUp = false;
                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;
                        }



                        //changes to score menu if the menu selector is 2 //emil 07-11-18
                        if (keySelect == true && isKeyUp == true && menuSlection == 2)
                        {
                            previousGamestate = Convert.ToInt32(gamestate);
                            // Save Highscore | Julius 18-12-10
                            SaveHighScore();
                            gamestate = Gamestates.highScoreMenu;
                            isKeyUp = false;
                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;

                        }
                        #endregion



                        //Position for Main menu //emil 07-11-18
                        if (menuSlection == 1)
                        {
                            menuPointerPosition = new Vector2(340, 386);
                        }
                        //Position for score list //emil 07-11-18
                        if (menuSlection == 2)
                        {
                            menuPointerPosition = new Vector2(340, 446);
                        }


                    }
                    break;








                //lose menu gamestate //emil 06-12-18
                case Gamestates.loseMenu:
                    {
                        inventoryShow = false;

                        itemWhitePowder = false;
                        itemKey = false;
                        itemGreenPotion = false;
                        itemYellowPotion = false;
                        itemRedPotion = false;
                        itemLantern = false;

                        // Stops timer | Julius 18-12-10
                        playerTimer = false;

                        #region move up or down the menu
                        //decreases the menuselector to ascend through the menu //emil 07-11-18
                        if ((keyUp == true) &&
                            (isKeyUp == true &&
                            menuSlection > 1))
                        {
                            menuSlection--;
                            isKeyUp = false;
                        }



                        //increases the menuselector to desend through the menu //emil 07-11-18
                        if ((keyDown == true) &&
                            (isKeyUp == true &&
                            menuSlection < 2))
                        {
                            menuSlection++;
                            isKeyUp = false;
                        }
                        #endregion
                        
                        #region options in the lose screen
                        //changes to main menu if the menu selector is 1 //emil 07-11-18
                        if (keySelect == true && isKeyUp == true && menuSlection == 1)
                        {
                            previousGamestate = Convert.ToInt32(gamestate);
                            // Reset time | Julius 18-12-10
                            playerScore = 0;
                            gamestate = Gamestates.mainMenu;
                            i = 0;
                            isKeyUp = false;
                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;
                        }



                        //changes to score menu if the menu selector is 2 //emil 07-11-18
                        if (keySelect == true && isKeyUp == true && menuSlection == 2)
                        {
                            previousGamestate = Convert.ToInt32(gamestate);
                            // Reset time | Julius 18-12-10
                            playerScore = 0;
                            gamestate = Gamestates.highScoreMenu;
                            i = 0;
                            isKeyUp = false;
                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;

                        }
                        #endregion

                        //Position for Main menu //emil 07-11-18
                        if (menuSlection == 1)
                        {
                            menuPointerPosition = new Vector2(295, 391);
                        }
                        //Position for score list //emil 07-11-18
                        if (menuSlection == 2)
                        {
                            menuPointerPosition = new Vector2(295, 450);
                        }


                    }
                    break;

                case Gamestates.highScoreMenu:
                    {
                        inventoryShow = false;

                        // Stops timer | Julius 18-12-10
                        playerTimer = false;

                        //sets the gamestate to main menu when the BACK key is pressed //emil 21-11-18
                        if (keySelect == true &&
                           isKeyUp == true &&
                           keyboard.IsKeyUp(Keys.R) ||
                            keyBack == true &&
                            isKeyUp == true &&
                            keyboard.IsKeyUp(Keys.R))
                        {
                            
                            previousGamestate = Convert.ToInt32(gamestate);
                            gamestate = Gamestates.mainMenu;
                            isKeyUp = false;

                            //sets the menuPointerPosition to 1 //emil 22-11-18
                            menuSlection = 1;
                        }

                        // Reset highscore if first "R" and then "Enter" is pressed | Julius 18-12-10
                        if (keySelect == true &&
                            isKeyUp == true &&
                            keyboard.IsKeyDown(Keys.R))
                            ResetHighScore();


                    }
                    break;









                #endregion


                #region level states
                case Gamestates.levelStateTownMap:
                    {
                        inventoryShow = true;

                        player.Update(gameTime);
                        enemyAI.Update(gameTime, player);

                        // Collision between Player and Tiles, deactives by holding "+" key | Julius 18-11-26
                        if (!keyboard.IsKeyDown(Keys.OemPlus))
                        {
                            foreach (CollisionTiles tile in mapTown.CollisionTiles)
                            {
                                player.Collision(tile.Rectangle, 0, 800, 0, 600);
                                enemyAI.Collision(tile.Rectangle, 0, 800, 0, 600);
                            }

                        if (player.CollisionRectangle.Intersects(enemyAI.CollisionRectangle) && itemRedPotion == false)
                            gamestate = Gamestates.loseMenu;
                        }

                        if (player.CollisionRectangle.Intersects(enemyAI.CollisionRectangle) && itemRedPotion == true)
                            gamestate = Gamestates.winMenu;




                        if (player.CollisionRectangle.Intersects(warpTo_CityMarket = new Rectangle(350, -40, 100, 50)))
                        {
                            gamestate = Gamestates.levelStateCityMarket;
                            player.position = new Vector2(380, 550);
                        }

                        if (player.CollisionRectangle.Intersects(warpTo_CityAlley = new Rectangle(350, 590, 100, 50)))
                        {
                            gamestate = Gamestates.levelStateCityAlley;
                            player.position = new Vector2(330, 360);
                        }

                        if (player.CollisionRectangle.Intersects(warpTo_DrJekyllLAB = new Rectangle(790, 200, 50, 50)) && itemKey == true)
                        {
                            gamestate = Gamestates.levelStateDrJekyllLAB;
                            player.position = new Vector2(50, 480);
                        }

                        if (player.CollisionRectangle.Intersects(warpTo_DrJekyllRoom = new Rectangle(790, 500, 50, 50)))
                        {
                            gamestate = Gamestates.levelStateDrJekyllRoom;
                            player.position = new Vector2(750, 480);
                        }

                        if (player.CollisionRectangle.Intersects(warpTo_DrugStore = new Rectangle(200, -40, 50, 50)))
                        {
                            gamestate = Gamestates.levelStateDrugStore;
                            player.position = new Vector2(300, 100);
                        }

                        if (player.CollisionRectangle.Intersects(warpTo_MrHydeRoom = new Rectangle(-40, 350, 50, 50)))
                        {
                            gamestate = Gamestates.levelStateMrHydeRoom;
                            player.position = new Vector2(750, 480);
                        }
                        

                    }
                    break;



                case Gamestates.levelStateDrJekyllLAB:
                    {
                        inventoryShow = true;


                        player.Update(gameTime);

                        // Collision between Player and Tiles, deactives by holding "+" key | Julius 18-11-26
                        if (!keyboard.IsKeyDown(Keys.OemPlus))
                            foreach (CollisionTiles tile in mapRoom.CollisionTiles)
                            {
                                player.Collision(tile.Rectangle, 0, 800, 0, 600);
                            }

                        if (player.CollisionRectangle.Intersects(itemRectangle = new Rectangle(-40, 50, 120, 300)) &&
                            keyUp == true &&
                            itemWhitePowder == true &&
                            itemKey == true &&
                            itemGreenPotion == true &&
                            itemYellowPotion == true ||
                            player.CollisionRectangle.Intersects(itemRectangle = new Rectangle(60, 410, 380, 50)) &&
                            keySelect == true &&
                            itemWhitePowder == true &&
                            itemKey == true &&
                            itemGreenPotion == true &&
                            itemYellowPotion == true)
                        {
                            itemGreenPotion = false;
                            itemYellowPotion = false;
                            itemWhitePowder = false;
                            itemRedPotion = true;
                        }

                        if (player.CollisionRectangle.Intersects(warpTo_TownMap = new Rectangle(-40, 400, 50, 150)) && keyLeft == true)
                        {
                            gamestate = Gamestates.levelStateTownMap;
                            enemyAI.Position = new Vector2(10, 330);
                            player.position = new Vector2(740, 180);
                        }


                    }
                    break;






                case Gamestates.levelStateDrJekyllRoom:
                {
                    inventoryShow = true;

                        player.Update(gameTime);

                    // Collision between Player and Tiles, deactives by holding "+" key | Julius 18-11-26
                    if (!keyboard.IsKeyDown(Keys.OemPlus))
                        foreach (CollisionTiles tile in mapRoom.CollisionTiles)
                        {
                            player.Collision(tile.Rectangle, 0, 800, 0, 600);
                        }

                    if (player.CollisionRectangle.Intersects(warpTo_TownMap = new Rectangle(680, 300, 120, 300)) && keyUp == true ||
                        player.CollisionRectangle.Intersects(warpTo_TownMap = new Rectangle(680, 300, 120, 300)) && keySelect == true)
                    {
                        gamestate = Gamestates.levelStateTownMap;
                        enemyAI.Position = new Vector2(10, 330);
                        player.position = new Vector2(740, 480);
                    }

                    if (player.CollisionRectangle.Intersects(itemRectangle = new Rectangle(420, 450, 180, 100)) &&
                        keySelect == true)
                        itemLantern = true;


                }
                    break;


                case Gamestates.levelStateMrHydeRoom:
                    {
                        inventoryShow = true;

                        player.Update(gameTime);

                        // Collision between Player and Tiles, deactives by holding "+" key | Julius 18-11-26
                        if (!keyboard.IsKeyDown(Keys.OemPlus))
                            foreach (CollisionTiles tile in mapRoom.CollisionTiles)
                            {
                                player.Collision(tile.Rectangle, 0, 800, 0, 600);
                            }

                        if (player.CollisionRectangle.Intersects(warpTo_TownMap = new Rectangle(790, 480, 50, 100)) && keyRight == true)
                        {
                            gamestate = Gamestates.levelStateTownMap;
                            enemyAI.Position = new Vector2(202, 30);
                            player.position = new Vector2(15, 330);
                        }

                        if (player.CollisionRectangle.Intersects(itemRectangle = new Rectangle(535, 450, 150, 90)) &&
                            keySelect == true)
                            itemGreenPotion = true;



                    }
                    break;

                case Gamestates.levelStateDrugStore:
                    {
                        inventoryShow = true;

                        // Code for moving Pointer in drugstore | Julius 18-12-07
                        if (keyRight == true &&
                            isKeyUp == true &&
                            freeIntX < 9)
                        {
                            freeIntX++;
                            isKeyUp = false;
                        }
                        if (keyLeft == true &&
                            isKeyUp == true &&
                            freeIntX > 0)
                        {
                            freeIntX--;
                            isKeyUp = false;
                        }
                        if (keyUp == true &&
                            isKeyUp == true &&
                            freeIntY > 0)
                        {
                            freeIntY--;
                            isKeyUp = false;
                        }
                        if (keyDown == true &&
                            isKeyUp == true &&
                            freeIntY < 3)
                        {
                            freeIntY++;
                            isKeyUp = false;
                        }

                        if (freeIntX < 5)
                            i = 80;
                        else i = 270;
                        
                        freePosition = new Vector2(i + freeIntX * 54, 176 + freeIntY * 54);
                        
                        if (keySelect == true &&
                            isKeyUp == true &&
                            freeIntX == 6 &&
                            freeIntY == 2)
                        {
                            itemYellowPotion = true;
                            isKeyUp = false;
                        }

                        if (keyBack == true &&
                            isKeyUp == true)
                        {
                            gamestate = Gamestates.levelStateTownMap;
                            i = 0;
                            player.position = new Vector2(205, 0);
                            enemyAI.Position = new Vector2(375, 550);
                            isKeyUp = false;
                        }

 
                    }
                    break;

                case Gamestates.levelStateCityMarket:
                    {
                        inventoryShow = true;

                        player.Update(gameTime);

                        // Collision between Player and Tiles, deactives by holding "+" key | Julius 18-11-26
                        if (!keyboard.IsKeyDown(Keys.OemPlus))
                            foreach (CollisionTiles tile in mapMarket.CollisionTiles)
                            {
                                player.Collision(tile.Rectangle, 0, 800, 0, 600);
                            }

                        if (player.CollisionRectangle.Intersects(warpTo_TownMap = new Rectangle(150, 580, 400, 50)) && keyDown == true)
                        {
                            gamestate = Gamestates.levelStateTownMap;
                            enemyAI.Position = new Vector2(385, 550);
                            player.position = new Vector2(385, 5);
                        }

                        if (player.CollisionRectangle.Intersects(itemRectangle = new Rectangle(600, 50, 50, 250)) &&
                            keySelect == true)
                            itemWhitePowder = true;







                    }
                    break;

                case Gamestates.levelStateCityAlley:
                    {
                        inventoryShow = true;

                        player.Update(gameTime);

                        // Collision between Player and Tiles, deactives by holding "+" key | Julius 18-11-26
                        if (!keyboard.IsKeyDown(Keys.OemPlus))
                            foreach (CollisionTiles tile in mapStreet.CollisionTiles)
                            {
                                player.Collision(tile.Rectangle, 0, 800, 0, 600);
                            }

                        if (player.CollisionRectangle.Intersects(warpTo_TownMap = new Rectangle(320, 330, 60, 50)) && keyUp == true)
                        {
                            gamestate = Gamestates.levelStateTownMap;
                            enemyAI.Position = new Vector2(750, 200);
                            player.position = new Vector2(365, 520);
                        }

                        if (player.CollisionRectangle.Intersects(itemRectangle = new Rectangle(685, 450, 50, 30)) &&
                            keySelect == true)
                            itemKey = true;





                    }
                    break;

                case Gamestates.levelState7:
                    {



 
                    }
                    break;

                case Gamestates.levelState8:
                    {


 
                    }
                    break;

                case Gamestates.levelState9:
                    {


 
                    }
                    break;

                #endregion






            }
            #endregion



            


            // If pressing "0" then show collision tiles | Julius 18-12-03
            if (keyboard.IsKeyDown(Keys.D0))
            {
                showCollisions = true;
            }
            else showCollisions = false;

            // Light positions
            if (itemLantern == true)
            {
            lights[0].Position.X = player.position.X + 12;
            lights[0].Position.Y = player.position.Y + 24;
            }
            else
            {
                lights[0].Position.X = -100000;
                lights[0].Position.Y = -100000;
            }


           


            base.Update(gameTime);
        }
        
        private void DrawColorMap()
        {
            spriteBatch.Begin();

            // GameState play ground | Julius 18-12-03
            if (gamestate == Gamestates.levelStateTownMap)
            {
                spriteBatch.Draw(levelBackground, Vector2.Zero, Color.White);
                // Draw enemy | Julius 18-12-03
                enemyAI.Draw(spriteBatch);
            }

            // GameState Alley | Julius 18-12-10
            if (gamestate == Gamestates.levelStateCityAlley)
            {
                spriteBatch.Draw(streetImage, Vector2.Zero, Color.White);
            }

            // GameState MarketSquare | Julius 18-12-11
            if (gamestate == Gamestates.levelStateCityMarket)
            {
                spriteBatch.Draw(marketSquare, Vector2.Zero, Color.White);
            }


            spriteBatch.End();
        }

        private void DrawNormalMap()
        {
            spriteBatch.Begin();

            // GameState play ground | Julius 18-12-03
            if (gamestate == Gamestates.levelStateTownMap)
            {
                spriteBatch.Draw(levelBackgroundNM, Vector2.Zero, Color.White);
                // Draw enemy | Julius 18-12-03
                enemyAI.DrawNM(spriteBatch);
            }

            // GameState Alley | Julius 18-12-10
            if (gamestate == Gamestates.levelStateCityAlley)
            {
                spriteBatch.Draw(streetNM, Vector2.Zero, Color.White);
            }

            // GameState MarketSquare | Julius 18-12-11
            if (gamestate == Gamestates.levelStateCityMarket)
            {
                spriteBatch.Draw(marketSquareNM, Vector2.Zero, Color.White);
            }

            spriteBatch.End();

            // Deactivate the rander targets to solve them
            GraphicsDevice.SetRenderTarget(null);
        }

        #region Draw maps
        private void DrawCombinedMaps()
        {
            lightCombinedEffect.CurrentTechnique = lightCombinedEffectTechnique;
            lightCombinedEffectParameterAmbient.SetValue(1f);
            lightCombinedEffectParameterLightAmbient.SetValue(4);
            lightCombinedEffectParameterAmbientColor.SetValue(ambientLight.ToVector4());
            lightCombinedEffectParameterColorMap.SetValue(colorMapRenderTarget);
            lightCombinedEffectParameterNormalMap.SetValue(normalMapRenderTarget);
            lightCombinedEffectParameterShadowMap.SetValue(shadowMapRenderTarget);
            lightCombinedEffect.CurrentTechnique.Passes[0].Apply();

           
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, lightCombinedEffect);
            spriteBatch.Draw(colorMapRenderTarget, Vector2.Zero, Color.White);
            spriteBatch.End();
            
        }

        // Dynamic Light
        private Texture2D GenerateShadowMap()
        {
            GraphicsDevice.SetRenderTarget(shadowMapRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);

            foreach (var light in lights)
            {
                if (!light.IsEnabled) continue;
                GraphicsDevice.SetVertexBuffer(VertexBuffer);

                // Draw all lightsources

                lightEffectParameterStrength.SetValue(light.ActualPower);
                lightEffectParameterPosition.SetValue(light.Position);
                lightEffectParameterLightColor.SetValue(light.Color);
                lightEffectParameterLightDecay.SetValue(light.LightDecay);

                lightEffect.Parameters["specularStrength"].SetValue(specularStrength);

                if (light.LightType == LightType.Point)
                    lightEffect.CurrentTechnique = lightEffectTechniquePointLight;

                lightEffectParameterScreenWidth.SetValue(GraphicsDevice.Viewport.Width);
                lightEffectParameterScreenHeight.SetValue(GraphicsDevice.Viewport.Height);
                lightEffect.Parameters["ambientColor"].SetValue(ambientLight.ToVector4());
                lightEffectParameterNormalMap.SetValue(normalMapRenderTarget);
                lightEffect.Parameters["ColorMap"].SetValue(colorMapRenderTarget);
                lightEffect.CurrentTechnique.Passes[0].Apply();

                // Add (Black background)
                GraphicsDevice.BlendState = BlendBlack;

                GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, Verticies, 0, 2);

            }

            // Reset RenderTargets
            GraphicsDevice.SetRenderTarget(null);

            return shadowMapRenderTarget;

        }

        public static BlendState BlendBlack = new BlendState()
        {
            ColorBlendFunction = BlendFunction.Add,
            ColorSourceBlend = Blend.One,
            ColorDestinationBlend = Blend.One,

            AlphaBlendFunction = BlendFunction.Add,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One
        };

        #endregion
        
        protected override void Draw(GameTime gameTime)
        {
            GenerateShadowMap();
            GraphicsDevice.Clear(Color.CornflowerBlue);

            

            GraphicsDevice.Clear(Color.Black);

            // Set the render targets
            GraphicsDevice.SetRenderTarget(colorMapRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            DrawColorMap();

            // Clear all render targets
            GraphicsDevice.SetRenderTarget(null);

            // Set the render targets
            GraphicsDevice.SetRenderTarget(normalMapRenderTarget);
            GraphicsDevice.Clear(Color.Transparent);
            DrawNormalMap();

            // Clear all render targets
            GraphicsDevice.SetRenderTarget(null);

            DrawCombinedMaps();



            #region Menu Images
            //draws all the sprites for the main menu
            if (gamestate == Gamestates.mainMenu)
            {
                spriteBatch.Begin();

                //drawing of the menu //emil 07-11-18
                spriteBatch.Draw(menuImage, menuPosition, Color.White);
                //drawing of the menu pointer //emil 07-11-18
                spriteBatch.Draw(menuPointerImage, menuPointerPosition, Color.White);

                spriteBatch.End();
            }

            //draws all of the sprites in the Create game menu //emil 22-11-18
            if (gamestate == Gamestates.createGame)
            {
                spriteBatch.Begin();

                if (i == 0)
                    spriteBatch.Draw(createGameImage1, Vector2.Zero, Color.White);
                if (i == 1)
                    spriteBatch.Draw(createGameImage2, Vector2.Zero, Color.White);

                spriteBatch.End();
            }

            //draws the credits menu //emil 22-11-18
            if (gamestate == Gamestates.creditsMenu)
            {
                spriteBatch.Begin();

                //drawing the menu //emil 22-11-18
                spriteBatch.Draw(creditsMenuImage, menuPosition, Color.White);

                spriteBatch.End();
            }

            //draws the options menu //emil 22-11-18
            if (gamestate == Gamestates.optionsMenu)
            {
                spriteBatch.Begin();

                //drawing the menu //emil 22-11-18
                spriteBatch.Draw(optionsMenu, menuPosition, Color.White);
                //draws the menu pointer //emil 22-11-18
                spriteBatch.Draw(menuPointerImage, menuPointerPosition, Color.White);

                spriteBatch.End();


            }

            //draws the highscore menu | Julius 18-12-10
            if (gamestate == Gamestates.highScoreMenu)
            {
                spriteBatch.Begin();

                //drawing the background | Julius 18-12-10
                spriteBatch.Draw(highScoreMenuImage, menuPosition, Color.White);
                // Draw score and date on highscore list  | Julius 18-12-09
                // Creates a list for the total amount of arrays | Julius 18-12-09
                for (int i = 0; i < arrayLeangth; i++)
                    // if score is less than 99999 seconds (ca 28 hours) then draw the score on screen | Julius 18-12-09
                    if (LoadData(Filename).Score[i] < 99999 * 1000)
                    {
                        spriteBatch.DrawString(scoreFont, "Date: " + LoadData(Filename).Date[i], scorePosition1 + (3 * i + 1) * scoreFontSpace, Color.White);
                        spriteBatch.DrawString(scoreFont, "Time: " + LoadData(Filename).Score[i].ToString() + " milliseconds", scorePosition1 + (3 * i) * scoreFontSpace, Color.White);
                    }

                    if (LoadData(Filename).Score[0] > 9999*1000)
                    {
                        spriteBatch.DrawString(scoreFont, "You have not won yet", scorePosition1 + scoreFontSpace, Color.White);
                    }

                spriteBatch.End();


            }


            #endregion





            if (gamestate == Gamestates.levelStateMrHydeRoom)
            {

                spriteBatch.Begin();
                // Draw room image | Julius 18-12-10
                spriteBatch.Draw(hydesRoomImage, Vector2.Zero, Color.White);
                // Draw player | Julius 18-12-03
                player.Draw(spriteBatch);
                spriteBatch.End();

            }

            if (gamestate == Gamestates.levelStateDrJekyllRoom)
            {

                spriteBatch.Begin();
                // Draw room image | Julius 18-12-10
                spriteBatch.Draw(jekyllsHouseImage, Vector2.Zero, Color.White);
                // Draw player | Julius 18-12-03
                player.Draw(spriteBatch);
                spriteBatch.End();


            }
            //alley way images
            if(gamestate == Gamestates.levelStateCityAlley)
            {
                spriteBatch.Begin();
                // Draw player | Julius 18-12-03
                if (itemLantern == true)
                    player.Draw(spriteBatch);
                spriteBatch.End();
            }

            //market square images //emil 07-12-18
            if (gamestate == Gamestates.levelStateCityMarket)
            {
                spriteBatch.Begin();
                // Draw player | Julius 18-12-03
                if(itemLantern == true)
                player.Draw(spriteBatch);

                spriteBatch.End();
            }

            //jekylls lab images //emil 07-12-18
            if(gamestate == Gamestates.levelStateDrJekyllLAB)
            {
                spriteBatch.Begin();
                //draw lab image //emil 07-12-18
                spriteBatch.Draw(jekyllsLabImage, menuPosition, Color.White);
                // Draw player | Julius 18-12-03
                player.Draw(spriteBatch);
                spriteBatch.End();
            }


            //drug store images //emil 07-12-18
            if (gamestate == Gamestates.levelStateDrugStore)
            {
                spriteBatch.Begin();
                //draw market square image //emil 07-12-18
                spriteBatch.Draw(drugStoreImage, menuPosition, Color.White);
                // Draw selection Pointer | Julius 18-12-03
                spriteBatch.Draw(menuPointerImage, freePosition, Color.White);

                spriteBatch.End();
            }

            //win menu images //emil 07-12-18
            if (gamestate == Gamestates.winMenu)
            {
                spriteBatch.Begin();
                //draw win image //emil 07-12-18
                spriteBatch.Draw(winMenuImage, menuPosition, Color.White);
                // Draw time (score) on screen | Julius 18-12-09
                spriteBatch.DrawString(scoreFontTitle, playerScore.ToString(), new Vector2(350, 284), Color.Chocolate);
                //draws the menu pointer //emil 22-11-18
                spriteBatch.Draw(menuPointerImage, menuPointerPosition, Color.White);

                spriteBatch.End();
            }

            //lose menu images //emil 07-12-18
            if (gamestate == Gamestates.loseMenu)
            {
                spriteBatch.Begin();
                //draw lose image //emil 07-12-18
                spriteBatch.Draw(loseMenuImage, menuPosition, Color.White);
                // Draw time (score) on screen | Julius 18-12-09
                spriteBatch.DrawString(scoreFontTitle, playerScore.ToString(), new Vector2(310, 312), Color.Goldenrod);
                //draws the menu pointer //emil 22-11-18
                spriteBatch.Draw(menuPointerImage, menuPointerPosition, Color.White);

                spriteBatch.End();
            }

            // GameState play ground | Julius 18-12-03
            if (gamestate == Gamestates.levelStateTownMap)
            {
                spriteBatch.Begin();

                // Draw player | Julius 18-12-03
                if(itemLantern == true)
                player.Draw(spriteBatch);

                spriteBatch.End();
                
            }

            // Draw inventory
            if (inventoryShow == true)
            {
                spriteBatch.Begin();
                spriteBatch.Draw(inventoryImage, Vector2.Zero, Color.White);
                if (itemLantern == true)
                    spriteBatch.Draw(itemLanternImage, new Rectangle(3, 547, 50, 50), Color.White);
                if (itemGreenPotion == true)
                    spriteBatch.Draw(itemGreenPotionImage, new Rectangle(59, 547, 50, 50), Color.White);
                if (itemWhitePowder == true)
                    spriteBatch.Draw(itemWhitePowderImage, new Rectangle(115, 547, 50, 50), Color.White);
                if (itemRedPotion == true)
                    spriteBatch.Draw(itemRedPotionImage, new Rectangle(115, 547, 50, 50), Color.White);
                if (itemYellowPotion == true)
                    spriteBatch.Draw(itemYellowPotionImage, new Rectangle(171, 547, 50, 50), Color.White);
                if (itemKey == true)
                    spriteBatch.Draw(itemKeyImage, new Rectangle(240, 547, 24, 50), Color.White);



                spriteBatch.End();
            }






            // Draw Debugging Tools | Julius 18-12-05
            spriteBatch.Begin();
            // Draw collisiontiles if the bool is true | Julius 18-12-03
            // Updated it to include warpZones | Julius 18-12-05
            if (showCollisions == true)
            {
                mapStreet.Draw(spriteBatch);
                spriteBatch.Draw(warpHitBoxTexture, warpTo_CityMarket, Color.LightBlue);
                spriteBatch.Draw(warpHitBoxTexture, warpTo_CityAlley, Color.LightCoral);
                spriteBatch.Draw(warpHitBoxTexture, warpTo_DrJekyllLAB, Color.LightGoldenrodYellow);
                spriteBatch.Draw(warpHitBoxTexture, warpTo_DrJekyllRoom, Color.LightGreen);
                spriteBatch.Draw(warpHitBoxTexture, warpTo_MrHydeRoom, Color.LightSlateGray);
                spriteBatch.Draw(warpHitBoxTexture, warpTo_DrugStore, Color.LightYellow);
                spriteBatch.Draw(warpHitBoxTexture, warpTo_TownMap, Color.White);
                spriteBatch.Draw(warpHitBoxTexture, itemRectangle, Color.Black);
            }
            spriteBatch.End();



            base.Draw(gameTime);
        }
    }
}