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
using System.Diagnostics;

namespace TurtleGraphics.Turtle
{
    public static class Settings
    {
        public static IEnumerable<KeyValuePair<SkiaTurtleE.CommandTypes, int>> Commands { get; set; }


        public static SkiaTurtleE Turtle { get; set; }

        public static float PenSize { get { return Turtle.Paint.StrokeWidth; } set { Turtle.Paint.StrokeWidth = value; } }

        public static SKColor? CanvasColor { get; set; } = SKColors.Black;

        public static float TurtleSpeed { get; set; } = 0f;

        
        public static void RefreshCanvas()
        {
            if (CanvasColor.HasValue && Turtle.Canvas != null)
            {
                Turtle.Canvas.Clear(CanvasColor.Value);
            }
        }
        public static void UpdateSettingsFromSave()
        {
            CanvasColor = SaveManager.GetCanvasColor();
            TurtleSpeed = SaveManager.GetTurtleSpeed();
            PenSize = SaveManager.GetLineSize();
        }

        public static SKBitmap GetResourceSKBitmap(string resid)
        {
            Assembly assembly = typeof(Pages.Main).Assembly;

            using (Stream stream = assembly.GetManifestResourceStream(resid))
            {
                return SKBitmap.Decode(stream);
            }
        }
        public static IEnumerable<SKBitmap> GetResourceSKBitmap(params string[] resids)
        {
            Assembly assembly = typeof(Pages.Main).Assembly;

            foreach(string resId in resids)
            {
                using (Stream stream = assembly.GetManifestResourceStream(resId))
                {
                    yield return SKBitmap.Decode(stream).Resize(new SKSizeI(16, 16), SKFilterQuality.Low);
                }
            }
        }

        public static ImageSource GetResourceImage(string resid)
        {
            Assembly assembly = typeof(Pages.Main).Assembly;
            return ImageSource.FromResource(resid, assembly);
        }

        public static IEnumerable<ImageSource> GetResourceImage(params string[] resids)
        {
            Assembly assembly = typeof(Pages.Main).Assembly;

            foreach (string resId in resids)
            {
                yield return ImageSource.FromResource(resId, assembly);
            }
        }
    }
}
