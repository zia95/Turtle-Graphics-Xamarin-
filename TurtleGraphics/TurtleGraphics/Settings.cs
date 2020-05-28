using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

using Xamarin.Forms;

using SkiaSharp;

namespace TurtleGraphics
{
    public static class Settings
    {
        public static SkiaTurtleE Turtle { get; set; }

        public static PageCommands PageCommandsInstance { get; set; }
        public static MainPage PageMainInstance { get; set; }

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
    }
}
