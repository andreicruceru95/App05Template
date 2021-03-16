using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceShooter.Manager
{
    /// <summary>
    /// Sound manager is a singleton that manages any sounds used in the game.
    /// </summary>
    public class SoundManager
    {
        private static SoundManager instance;

        private ContentManager content;

        private Dictionary<string, Song> Songs;
        private Dictionary<string, SoundEffect> SoundEffects;

        public static SoundManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SoundManager();
                }
                return instance;
            }
        }
        /// <summary>
        /// Initialize collections.
        /// </summary>
        private void InitializeSounds()
        {
            Songs = new Dictionary<string, Song>();
            SoundEffects = new Dictionary<string, SoundEffect>();

            Songs.Add("background1", content.Load<Song>("Music/Music_BG"));
            SoundEffects.Add("laser", content.Load<SoundEffect>("Music/laser"));
        }
        /// <summary>
        /// Initialize Singleton.
        /// </summary>
        /// <param name="content">Content pipeline manager.</param>
        public void Initialize()
        {
            content = ContentLoader.Instance.Content;
            // repeat background music
            MediaPlayer.IsRepeating = true;

            InitializeSounds();
        }
        /// <summary>
        /// Return a specific sound effect from a <TKey, TValue> Collection of sound effects.
        /// </summary>
        /// <param name="soundEffect">TKey</param>
        /// <returns>TValue</returns>
        public SoundEffect GetSoundEffect(string soundEffect)
        {
            return SoundEffects[soundEffect];
        }
        /// <summary>
        /// Return a specific song from a <TKey, TValue> Collection of songs.
        /// </summary>
        /// <param name="song">TKey</param>
        /// <returns>TValue</returns>
        public void PlayMusic(string song)
        {
            MediaPlayer.Play(Songs[song]);
        }
    }
}
