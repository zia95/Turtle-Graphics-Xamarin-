using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TurtleGraphics.Turtle
{
    public static class SoundManager
    {
        public const string SEQ_INVALID = "TurtleGraphics.Resources.sounds.seq_invalid.wav";


        public enum SND_ID
        {
            SEQ_INVALID,
        };


        public static string GetSoundById(SND_ID id)
        {
            switch (id)
            {
                case SND_ID.SEQ_INVALID: return SEQ_INVALID;
            }
            throw new NotSupportedException($"ID:{id}, is not supported.");
        }

        public static void Play(SND_ID id) => Play(GetSoundById(id));
        public static void Play(string snd)
        {
            var assembly = typeof(App).Assembly;
            Stream audioStream = assembly.GetManifestResourceStream(snd);

            if (audioStream != null)
            {
                var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                player.Load(audioStream);
                player.Play();
            }
        }
    }
}
