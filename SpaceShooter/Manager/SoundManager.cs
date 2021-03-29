using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

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
            SoundEffects.Add("coins", content.Load<SoundEffect>("Music/Coins"));
            SoundEffects.Add("flame", content.Load<SoundEffect>("Music/flame"));
            SoundEffects.Add("gameover", content.Load<SoundEffect>("Music/gameover"));
            SoundEffects.Add("reload", content.Load<SoundEffect>("Music/reload"));
            SoundEffects.Add("upgrade", content.Load<SoundEffect>("Music/upgrade"));
            SoundEffects.Add("no ammo", content.Load<SoundEffect>("Music/no_ammo"));
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
            MediaPlayer.Volume = 0.5f;
        }
        public void PlayEffect(string effect)
        {
            SoundEffects[effect].Play(0.3f,0.3f,1f);
        }
    }
}
