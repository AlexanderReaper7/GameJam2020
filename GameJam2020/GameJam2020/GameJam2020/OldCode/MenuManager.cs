using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

namespace PartiAppen
{
    class MenuManager
    {
        public enum Menues
        {
            Menu,
            Program,
            OmOss,
            Press
        }

        private Menues state = Menues.Menu;

        public Menues State
        {
            get => state;
            set
            {
                menu.CurrentPage = (int)value;
                state = value;
            }
        }

        public int MenuesAmount => Enum.GetNames(typeof(Menues)).Length;

        // A variable for resetting the scroll
        int scrollReturn;

        // Create menu pages
        Menu menu = new Menu(Enum.GetNames(typeof(Menues)).Length);

        public static Point MousePosition => Mouse.GetState().Position;

        public void LoadMenu(ContentManager content)
        {
            // Fonts
            SpriteFont menuFont = content.Load<SpriteFont>(@"Fonts/Main");
            SpriteFont titleFont = content.Load<SpriteFont>(@"Fonts/Title");
            SpriteFont subFont = content.Load<SpriteFont>(@"Fonts/Sub");
            SpriteFont textFont = content.Load<SpriteFont>(@"Fonts/Text");
            SpriteFont smallFont = content.Load<SpriteFont>(@"Fonts/Small");


            // Color theme
            Color primary = new Color(3,169,244), primaryLight = new Color(103,218,255), primaryDark = new Color(0,122,193);
            Color backColor = primaryLight, highLightColor = primary;
            Color logoBlue = new Color(24, 20, 111), logoYellow = new Color(88, 80, 161);
            Rectangle mainRec = new Rectangle(4, 1080 - 82 * (MenuesAmount - 1), 712, 80);
            Rectangle logoRect = new Rectangle(720 / 2 - (480 / 2), 20, 480, 480);
            Vector2 padding = new Vector2(-20);
            Texture2D logo = content.Load<Texture2D>(@"Images/logo");
            Texture2D circle = content.Load<Texture2D>(@"Images/newcircle");
            // BackButton is in all pages but menu
            #region BackButton

            Texture2D backArrowBlack = content.Load<Texture2D>(@"Images/2x/baseline_arrow_back_black_48dp");
            Texture2D backArrowWhite = content.Load<Texture2D>(@"Images/2x/baseline_arrow_back_white_48dp");


            for (int i = 1; i < MenuesAmount; i++) // skip first page ( menu )
            {
                // Icon TODO: change rectangle
                menu.Pages[i].AddImageButton(new ImageButton(backArrowWhite, new Rectangle(new Point(40), new Point(80)),logoBlue,logoYellow , () => SetMenuState(Menues.Menu)));
            }

            #endregion

            #region Menu
            // Logo
            menu.Pages[(int)Menues.Menu].AddImage(new Image(logo, logoRect));
            // circle around logo
            int circleSize = 586 + 90;
            menu.Pages[(int)Menues.Menu].AdvancedImages.Add(new AdvancedImage(circle, new Rectangle(new Point((int)(720/2), (int)((logoRect.Location.Y + logoRect.Height)/2) + 0), new Point(circleSize)), logoBlue, 0f, new Vector2(408)));
            
            // Buttons
            menu.Pages[(int)Menues.Menu].AddButtonList(menuFont, mainRec, 80f, new[] { "Vårt Program", "Om Oss", "Press" }, padding, logoBlue, logoYellow, new Action[] {() => SetMenuState(Menues.Program), () => SetMenuState(Menues.OmOss), () => SetMenuState(Menues.Press)});
            /*
            menu.Pages[(int)Menues.Menu].AddText(textFont, new Vector2(20, 540 + 20), false, 
                "Våran sikt är att människan är en gruppvarelse därmed ska samhället " + System.Environment.NewLine
                + "samarbeta för att upplyfta alla människor, sverige ska vara världsledare " + System.Environment.NewLine
                + "som andra länder kan se upp till och ta efter. För att bli världsledare krävs " + System.Environment.NewLine
                + "det att sverige investerar i sin framtid, genom forskning och utbildning. " + System.Environment.NewLine + System.Environment.NewLine 
                + "Världen är mer sammankopplad än någonsin och internationellt " + System.Environment.NewLine
                + "samarbete är vägen till framtiden. För att sverige ska kunna samarbeta " + System.Environment.NewLine
                + "med alla länder så är det viktigt att vi ska fortsätta vara neutrala. " + System.Environment.NewLine
                + "Sveriges universitet ska samarbeta att utveckla teknologier som " + System.Environment.NewLine
                + "andra länder kan nyttja.", Color.Black);
                */

            menu.Pages[(int)Menues.Menu].AddText(menuFont, new Vector2(720 / 2, 610), true, "Mer säker och klimatsmart kärnkraft!", Color.Black);

            menu.Pages[(int)Menues.Menu].AddText(textFont, new Vector2(30, 660), false, 
                "Stamceller, gmo, odlat kött och minskade utsläpp. Sverige ska bidra till " + System.Environment.NewLine
                + "förbättringen av den internationella miljön genom att innovera inom " + System.Environment.NewLine
                + "dessa områden. För att uppnå detta så ska vi investera i forskning " + System.Environment.NewLine
                + "inom dessa områden och spendera mer pengar på forskningsmaterial, " + System.Environment.NewLine
                + "både internationellt och hemma.", Color.Black);
            #endregion

            #region Program

            // Variable to change where the text is.
            int yOffSet = 160;

            menu.Pages[(int)Menues.Program].AddText(titleFont, new Vector2(720/2, 80), true, "Vårt Program", Color.Black);
            
            #region Text

            // Klimat
            menu.Pages[(int)Menues.Program].AddText(menuFont, new Vector2(20, yOffSet + 0), false, "Klimat", Color.Black);

            menu.Pages[(int)Menues.Program].AddText(textFont, new Vector2(20, yOffSet + 50), false,
                "Vi tror på en säker och energieffektiv framtid. Med nya teknologier " + System.Environment.NewLine
                + "kan vi utvinna energi från jord. Vi vill forska och utveckla detta " + System.Environment.NewLine
                + "område för säkrare och billigare elproduktion. ", Color.Black);
            // sub Kärnkraft
            menu.Pages[(int)Menues.Program].AddText(subFont, new Vector2(20, yOffSet + 160), false, "Kärnkraft", Color.Black);

            menu.Pages[(int)Menues.Program].AddText(textFont, new Vector2(20, yOffSet + 200), false,
                "Kärnkraft extremt energieffektivt och har liten klimatpåverkan med en " + System.Environment.NewLine
                + "extremt liten chans för katastrof. Sverige får idag 40% av sin " + System.Environment.NewLine
                + "energi från kärnkraft vi vill expandera det till 50% under de närmaste " + System.Environment.NewLine
                + "20 åren när vindkraftverken som utgör 10% av vår energi går sönder och " + System.Environment.NewLine
                + "behöves ersättas. Dem sista 50% procenten består av 41% vatten som " + System.Environment.NewLine
                + "är effektivt men för dyrt för att vara effektivt att expandera och " + System.Environment.NewLine
                + "9% värmekraft som kommer från energin vi får av att återanvända sopor.", Color.Black);
            // sub Forskning
            menu.Pages[(int)Menues.Program].AddText(subFont, new Vector2(20, yOffSet + 400), false, "Forskning", Color.Black);

            menu.Pages[(int)Menues.Program].AddText(textFont, new Vector2(20, yOffSet + 440), false,
                "Med dagens teknologi så klarar jorden inte mer än 8 miljarder människor " + System.Environment.NewLine
                + "och vi är på en bana mot 10 miljarder där det kommer stanna in. " + System.Environment.NewLine
                + "För att lösa detta så krävs det att vi inoverar och kommer på " + System.Environment.NewLine
                + "nya mer miljövänliga lösningar. Vilket kräver bättre utbildning och " + System.Environment.NewLine
                + "mer pengar investerat på forskning.", Color.Black);


            // Skolan
            menu.Pages[(int)Menues.Program].AddText(menuFont, new Vector2(20, yOffSet + 610), false, "Skolan", Color.Black);

            menu.Pages[(int)Menues.Program].AddText(textFont, new Vector2(20, yOffSet + 660), false,
                "Skolan är verktyget som formar samhället, detta ska våra stadgar " + System.Environment.NewLine
                + "reflektera genom att fortsätta erbjuda gratis skolgång och uppmuntra " + System.Environment.NewLine
                + "till en högre utbildning på högskole- eller universitetsnivå. " + System.Environment.NewLine
                + "Detta är en viktig pelare i demokratin, en välutbildad " + System.Environment.NewLine
                + "befolkning fattar bättre beslut.", Color.Black);
            // sub Friskolan
            menu.Pages[(int)Menues.Program].AddText(subFont, new Vector2(20, yOffSet + 820), false, "Friskolan", Color.Black);

            menu.Pages[(int)Menues.Program].AddText(textFont, new Vector2(20, yOffSet + 860), false,
                "Vi som parti är för friskolor för det skapar konkurrens vilket leder " + System.Environment.NewLine
                + "till bättre tjänster. Staten ska dock styra kunskapskraven samt " + System.Environment.NewLine
                + "leda inspektioner av skolorna för att säkerställa att " + System.Environment.NewLine
                + "friskolorna uppnår kraven som sätts på dem.", Color.Black);
            // sub Lärarbrist
            menu.Pages[(int)Menues.Program].AddText(subFont, new Vector2(20, yOffSet + 1000), false, "Lärarbrist", Color.Black);

            menu.Pages[(int)Menues.Program].AddText(textFont, new Vector2(20, yOffSet + 1040), false,
                "Läraryrket har under lång tid förlorat mer och mer status på grund " + System.Environment.NewLine
                + "av lägre löner och lägre krav för att människor utan lärarutbildning ska " + System.Environment.NewLine
                + "kunna få yrket. Detta har lett till att fler och fler utbildar sig till " + System.Environment.NewLine
                + "andra yrken vilket har lett till en brist på 60,000 lärare. " + System.Environment.NewLine
                + "Till att börja med vill vi öka lärarlöner så att fler ska " + System.Environment.NewLine
                + "vilja få jobbet för att låg lön är en motivation mot att inte bli lärare. " + System.Environment.NewLine
                + "Ett annat problem för lärare är alla jobb vid sidan av " + System.Environment.NewLine
                + "undervisningen dem behöver göra. Vi vill fokusera på att " + System.Environment.NewLine
                + "anställa människor som kan göra dem jobben så lärarna " + System.Environment.NewLine
                + "inte behöver det.", Color.Black);

            // Ekonomi
            menu.Pages[(int)Menues.Program].AddText(menuFont, new Vector2(20, yOffSet + 1340), false, "Ekonomi", Color.Black);
            // sub Skatt
            menu.Pages[(int)Menues.Program].AddText(subFont, new Vector2(20, yOffSet + 1400), false, "Skatt", Color.Black);

            menu.Pages[(int)Menues.Program].AddText(textFont, new Vector2(20, yOffSet + 1440), false,
                "Det nuvarande systemet har visat att fungera. Vi tycker att skatten skall " + System.Environment.NewLine
                + "vara balanserad som vi anser att den är just nu. " + System.Environment.NewLine
                + "Därmed vill vi behålla skattesystemet och fokusera på att finjustera " + System.Environment.NewLine
                + "våra resurstillgångar med att investera dem på ett korrekt sätt.", Color.Black);
            // sub Bidrag
            menu.Pages[(int)Menues.Program].AddText(subFont, new Vector2(20, yOffSet + 1580), false, "Bidrag", Color.Black);

            menu.Pages[(int)Menues.Program].AddText(textFont, new Vector2(20, yOffSet + 1620), false,
                "Vi tror på forskning och därför vill vi ge större summa pengar till " + System.Environment.NewLine
                + "alla Sveriges fantastiska universitet. Vi skall lägga ytterligare " + System.Environment.NewLine
                + "20 miljarder kronor på forskningsmedel och stöd för universitet. " + System.Environment.NewLine
                + "Bidragen kommer vara mer inriktad mot energiforskning. Genom dessa " + System.Environment.NewLine
                + "bidrag så skall vi uppfylla vår profilfråga!", Color.Black);


            #endregion

            menu.Pages[(int)Menues.Program].AddText(titleFont, new Vector2(720/2, -100), true, "*DAB*", Color.Black);

            menu.Pages[(int)Menues.Program].AddImageButton(new ImageButton(backArrowWhite, new Rectangle(new Point(720/2-45, yOffSet + 1800), new Point(80)), logoBlue, logoYellow, () => SetMenuState(Menues.Menu)));


            #endregion

            #region OmOss


            menu.Pages[(int)Menues.OmOss].AddText(titleFont, new Vector2(720 / 2, 80), true, "Om Oss", Color.Black);

            menu.Pages[(int)Menues.OmOss].AddText(menuFont, new Vector2(720 / 2, 760), true,
                "Motto:", Color.Black);

            menu.Pages[(int)Menues.OmOss].AddText(menuFont, new Vector2(720 / 2, 820), true,
                "Satsa mer på forskning inför framtiden!", Color.Black);


            menu.Pages[(int)Menues.OmOss].AddText(subFont, new Vector2(30, 160), false,
                "Partiledare: Samuel Rolandson " + System.Environment.NewLine + System.Environment.NewLine
                + "Gruppledare: Alexander Öberg " + System.Environment.NewLine + System.Environment.NewLine
                + "Trälar: " + System.Environment.NewLine
                + "Vidar Bratvoldsengen, " + System.Environment.NewLine
                + "Noam Robertsson, " + System.Environment.NewLine
                + "Sebastian Thörn, " + System.Environment.NewLine
                + "Carl Elvheim, " + System.Environment.NewLine
                + "Viktor Eriksson, " + System.Environment.NewLine
                + "Aidan Bucci, " + System.Environment.NewLine
                + "William Ekman, " + System.Environment.NewLine
                + "Julius Fernström Roolf, " + System.Environment.NewLine
                + "Lina Älvstrand, " + System.Environment.NewLine
                + "Anton Gunnarsson & " + System.Environment.NewLine
                + "Arvid Rimmerfors", Color.Black);


            #endregion

            #region Press
            menu.Pages[(int)Menues.Press].AddText(titleFont, new Vector2(720 / 2, 80), true, "Press", Color.Black);

            menu.Pages[(int)Menues.Press].AddText(subFont, new Vector2(720 / 2, 200), true, "Musik", Color.Black);
            menu.Pages[(int)Menues.Press].AddText(textFont, new Vector2(720 / 2, 240), true, "shorturl.at/foyVX", Color.Black);


            menu.Pages[(int)Menues.Press].AddText(subFont, new Vector2(720 / 2, 340), true, "Logo", Color.Black);
            menu.Pages[(int)Menues.Press].AddText(textFont, new Vector2(720 / 2, 380), true, "shorturl.at/lnxKR", Color.Black);


            menu.Pages[(int)Menues.Press].AddText(smallFont, new Vector2(5, 1065), false, "*DAB*", Color.Black);


            #endregion
        }

        private void SetMenuState(Menues newState) { State = newState; }

        private int mouseScroll = 0, previousMouseScroll = 0;
        private const float scrollMultiplier = 0.6f, rotationMultiplier = 0.2f;
        private DateTime prevTime = DateTime.Now, time = DateTime.Now;

        public void Update()
        {
            menu.Update();

            // Update mouse
            previousMouseScroll = mouseScroll;
            mouseScroll = Mouse.GetState().ScrollWheelValue;
            int deltaScroll = previousMouseScroll - mouseScroll;
            // Update time
            prevTime = time;
            time = DateTime.Now;
            float deltaTime = (float)(time - prevTime).TotalSeconds;

            TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));

            Color logoBlue = new Color(24, 20, 111), logoYellow = new Color(252,214,3);
            // Menu specific logic
            switch (State)
            {
                case Menues.Menu:
                    // reset camera
                    Game1.camera.Position = Vector2.Zero;
                    // Rotate circle thing
                    menu.Pages[0].AdvancedImages[0].rotation += deltaTime * rotationMultiplier;
                    // Remove this line to disable color shifting
                    menu.Pages[0].AdvancedImages[0].tint = Color.Lerp(logoBlue, logoYellow, (float)((Math.Sin(t.TotalSeconds * 2) + 1) / 2));
                    break;
                case Menues.Program:
                    if (Game1.camera.Position.Y < -100) scrollReturn++;
                    else if (Game1.camera.Position.Y < 0) scrollReturn = 2;
                    else scrollReturn = 0;

                    // move camera
                    Game1.camera.Position = new Vector2(Game1.camera.Position.X, MathHelper.Clamp(Game1.camera.Position.Y + scrollReturn + (deltaScroll * scrollMultiplier), -200, 1000) );
                    break;
                case Menues.OmOss:
                    // reset camera
                    Game1.camera.Position = Vector2.Zero;
                    break;
                case Menues.Press:
                    // reset camera
                    Game1.camera.Position = Vector2.Zero;
                    break;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            menu.Draw(spriteBatch);
        }
    }
}
