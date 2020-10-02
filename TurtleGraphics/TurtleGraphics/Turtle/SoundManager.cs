using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace TurtleGraphics.Turtle
{
    public static class SoundManager
    {
        public const string SND_CLICK = "TurtleGraphics.Resources.sounds.click.wav";
        public const string SND_ERROR = "TurtleGraphics.Resources.sounds.error.wav";

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
