using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Reflection;
using System.IO;

using Xamarin.Forms;

using SkiaSharp;

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
        public static MainPage PageMainInstance { get; set; }

        public static float PenSize { get { return Turtle.Paint.StrokeWidth; } set { Turtle.Paint.StrokeWidth = value; } }
        public static SKColor? PenColor { get; set; } = SKColors.Red;

        public static SKColor? CanvasColor { get; set; } = SKColors.Black;

        public static float TurtleSpeed { get; set; } = 0f;

        public static void Refresh(bool canvas_reset_force = false)
        {
            if(PenColor.HasValue)
            {
                Turtle.Paint.Color = PenColor.Value;
            }
            
            if(CanvasColor.HasValue && Turtle.Canvas != null && canvas_reset_force)
            {
                Turtle.Canvas.Clear(CanvasColor.Value);
            }

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

    }
}
