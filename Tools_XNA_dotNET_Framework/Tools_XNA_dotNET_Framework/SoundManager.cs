using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Tools_XNA
{
    /// <summary>
    /// Olle A 20-04-18
    /// Class that manages all sounds in the game.
    /// All sounds are public static and can be called from everywhere.
    /// </summary>
    public static class SoundManager
    {
        public static SoundEffect Select;
        public static SoundEffect Hurt;
        public static SoundEffect KeyFantasy;
        public static SoundEffect KeyScifi;
        public static SoundEffect DoorOpenFantasy;
        public static SoundEffect DoorOpenScifi;

        public static SoundEffect DispenserScifi;
        public static SoundEffect DispenserFantasy;

        public static SoundEffect TileBreak;
        public static SoundEffect Protal;
        public static SoundEffect NextLevel;
        public static SoundEffect Win;

        private static SoundEffect music;
        public static SoundEffectInstance MusicInstance;

        // Load 
        public static void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            // Load sound effects
            DoorOpenFantasy = Content.Load<SoundEffect>(@"Shared/Sounds/fantasy door");
            DoorOpenScifi = Content.Load<SoundEffect>(@"Shared/Sounds/scifi door");

            KeyFantasy = Content.Load<SoundEffect>(@"Shared/Sounds/fantasy key");
            KeyScifi = Content.Load<SoundEffect>(@"Shared/Sounds/scifi key");

            DispenserScifi = Content.Load<SoundEffect>(@"Shared/Sounds/scifi dispenser");
            DispenserFantasy = Content.Load<SoundEffect>(@"Shared/Sounds/fantasy dispenser");

            TileBreak = Content.Load<SoundEffect>(@"Shared/Sounds/tile break");
            Hurt = Content.Load<SoundEffect>(@"Shared/Sounds/die");

            Protal = Content.Load<SoundEffect>(@"Shared/Sounds/portal");
            NextLevel = Content.Load<SoundEffect>(@"Shared/Sounds/endportal");

            Select = Content.Load<SoundEffect>(@"Shared/Sounds/select");

            // Load music and loop it
            music = Content.Load<SoundEffect>(@"Shared/Sounds/Andromeda Journey");
            MusicInstance = music.CreateInstance();
            MusicInstance.IsLooped = true;
            MusicInstance.Play();
            SoundEffect.MasterVolume = 01f;
        }

        public static void ToggleMusic()
        {
            if (MusicInstance.State == SoundState.Playing)
                MusicInstance.Pause();
            else
                MusicInstance.Play();
        }
        public static void ToggleSound()
        {
            if (SoundEffect.MasterVolume == 1f)
                SoundEffect.MasterVolume = 0;
            else
                SoundEffect.MasterVolume = 1;
        }
    }
}
