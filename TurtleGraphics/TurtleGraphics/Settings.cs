using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;
using System.IO;

using Xamarin.Forms;

using SkiaSharp;
using Xamarin.Forms.Internals;
using System.Linq;
using System.Xml.Serialization;

namespace TurtleGraphics
{
    public static class Settings
    {
        
        public static readonly string[] TurtleImageResourceIds =
            {
            "TurtleGraphics.graphics.turtle.1.png",
            "TurtleGraphics.graphics.turtle.2.png",
            "TurtleGraphics.graphics.turtle.3.png",
            "TurtleGraphics.graphics.turtle.4.png",
            "TurtleGraphics.graphics.turtle.5.png",
            "TurtleGraphics.graphics.turtle.6.png",
            "TurtleGraphics.graphics.turtle.7.png",
        };

        private static List<ImageSource> __imgs = null;
        public static List<ImageSource> TurtleImages 
        { 
            get 
            {
                if(__imgs == null)
                    __imgs = new List<ImageSource>(GetResourceImage(PageMainInstance, TurtleImageResourceIds));
                return __imgs;
            } 
        }

        private static List<SKBitmap> __skbitmap = null;
        public static List<SKBitmap> TurtleSKBitmap
        {
            get
            {
                if (__skbitmap == null)
                    __skbitmap = new List<SKBitmap>(GetResourceSKBitmap(PageMainInstance, TurtleImageResourceIds));
                return __skbitmap;
            }
        }

        public static SkiaTurtleE Turtle { get; set; }

        public static PageCommands PageCommandsInstance { get; set; }
        public static PageMain PageMainInstance { get; set; }

        public static float PenSize { get { return Turtle.Paint.StrokeWidth; } set { Turtle.Paint.StrokeWidth = value; } }

        public static SKColor? CanvasColor { get; set; } = SKColors.Black;

        public static float TurtleSpeed { get; set; } = 0f;

        public static void RefreshSpeed()
        {
            if(TurtleSpeed > 0)
            {
                Turtle.DistanceSteps = 1;
                Device.StartTimer(TimeSpan.FromSeconds(1f / TurtleSpeed), () => { PageMainInstance.InvalidateCanvas(); return true; });
            }
            else
            {
                Turtle.DistanceSteps = 0;
                Device.StartTimer(TimeSpan.FromSeconds(1f / 60f), () => { PageMainInstance.InvalidateCanvas(); return true; });
            }
        }
        public static void RefreshCanvas()
        {
            if (CanvasColor.HasValue && Turtle.Canvas != null)
            {
                Turtle.Canvas.Clear(CanvasColor.Value);
            }
        }



        public static SKColor ColorXToSK(Color clr) => new SKColor((byte)clr.R, (byte)clr.G, (byte)clr.B, (byte)clr.A);
        public static Color ColorSKToX(SKColor clr) => Color.FromRgba((double)clr.Red, (double)clr.Green, (double)clr.Blue, (double)clr.Alpha);


        public static SKBitmap GetResourceSKBitmap(string resid, ContentPage page = null)
        {
            Assembly assembly = (page ?? PageMainInstance).GetType().GetTypeInfo().Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resid))
            {
                return SKBitmap.Decode(stream);
            }
        }
        public static IEnumerable<SKBitmap> GetResourceSKBitmap(ContentPage page = null, params string[] resids)
        {
            Assembly assembly = (page ?? PageMainInstance).GetType().GetTypeInfo().Assembly;

            foreach(string resId in resids)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resId))
                {
                    yield return SKBitmap.Decode(stream).Resize(new SKSizeI(16, 16), SKFilterQuality.Low);
                }
            }
        }

        public static ImageSource GetResourceImage(string resid, ContentPage page = null)
        {
            Assembly assembly = (page ?? PageMainInstance).GetType().GetTypeInfo().Assembly;
            return ImageSource.FromResource(resid, assembly);
        }

        public static IEnumerable<ImageSource> GetResourceImage(ContentPage page = null, params string[] resids)
        {
            Assembly assembly = (page ?? PageMainInstance).GetType().GetTypeInfo().Assembly;

            foreach (string resId in resids)
            {
                yield return ImageSource.FromResource(resId, assembly);
            }
        }



        public const string S_KEY_COMMANDS_LIST = "clst";


        public static List<ObservableCollection<SkiaTurtleE.CommandInfo>> CommandsListGet()
        {
            return (Application.Current.Properties.ContainsKey(S_KEY_COMMANDS_LIST) ? 
                TGScript.FromJson<List<ObservableCollection<SkiaTurtleE.CommandInfo>>>((string)Application.Current.Properties[S_KEY_COMMANDS_LIST]): null);
        }
        public static void CommandsListSet(List<ObservableCollection<SkiaTurtleE.CommandInfo>> cmds_list)
        {
            Application.Current.Properties[S_KEY_COMMANDS_LIST] = TGScript.ToJson(cmds_list);
        }
        public static void CommandsListSave(ObservableCollection<SkiaTurtleE.CommandInfo> commands, string tag)
        {
            if (commands == null || commands.Count <= 0) return;

            if (String.IsNullOrWhiteSpace(tag)) throw new ArgumentException("tag must be a unique identifier.");

            var cmds = CommandsListGet() ?? new List<ObservableCollection<SkiaTurtleE.CommandInfo>>();

            if (tag != null)
            {
                //commands.ForEach((c) => { c.Tag = tag; });

                commands = new ObservableCollection<SkiaTurtleE.CommandInfo>(commands.ToList().ConvertAll((x) => { x.Tag = tag; return x; }));
            }
            
            cmds.Add(commands);

            CommandsListSet(cmds);
        }
        public static bool CommandsListTagIsUnique(string tag)
        {
            var cmds = CommandsListGet();
            if(cmds != null)
            {
                foreach(var c in cmds)
                {
                    if (c[0].Tag.Equals(tag))
                        return false;
                }
            }
            return true;
        }




        public static class Sound
        {
            public const string seq_invalid = "TurtleGraphics.sounds.seq_invalid.wav";


            public enum SND_ID
            {
                SEQ_INVALID,
            };

            
            public static string GetSoundById(SND_ID id)
            {
                switch(id)
                {
                    case SND_ID.SEQ_INVALID: return seq_invalid;
                }
                throw new NotSupportedException($"ID:{id}, is not supported.");
            }

            public static void Play(SND_ID id) => Play(GetSoundById(id));
            public static void Play(string snd)
            {
                var assembly = typeof(App).GetTypeInfo().Assembly;
                Stream audioStream = assembly.GetManifestResourceStream(snd);

                if(audioStream != null)
                {
                    var player = Plugin.SimpleAudioPlayer.CrossSimpleAudioPlayer.Current;
                    player.Load(audioStream);
                    player.Play();
                }
            }
            
        }

    }
}
