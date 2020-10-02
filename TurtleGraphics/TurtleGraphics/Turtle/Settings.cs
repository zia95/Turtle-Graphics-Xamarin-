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
    }
}
