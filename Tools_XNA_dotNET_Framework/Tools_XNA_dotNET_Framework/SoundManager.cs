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
    static class SoundManager
    {
        public static SoundEffect Select;
        public static SoundEffect Hurt;
        public static SoundEffect PickUp;
        public static SoundEffect NextLevel;
        public static SoundEffect Win;

        private static SoundEffect music;
        public static SoundEffectInstance MusicInstance;

        // Load 
        public static void LoadContent(ContentManager Content, GraphicsDevice GraphicsDevice)
        {
            // Load sound effects
            Select = Content.Load<SoundEffect>(@"Sounds/Select");
            Hurt = Content.Load<SoundEffect>(@"Sounds/Hurt");
            PickUp = Content.Load<SoundEffect>(@"Sounds/Pick Up");
            NextLevel = Content.Load<SoundEffect>(@"Sounds/NextLevel");
            Win = Content.Load<SoundEffect>(@"Sounds/win");

            // Load music and loop it
            music = Content.Load<SoundEffect>(@"Sounds/Ozzed - Lugn Techno");
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
