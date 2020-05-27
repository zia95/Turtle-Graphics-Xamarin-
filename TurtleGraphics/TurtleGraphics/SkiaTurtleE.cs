using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace TurtleGraphics
{
    public class SkiaTurtleE : SkiaTurtle
    {
        public int Steps { get; set; }

        private float distance;

        public SkiaTurtleE(SKPoint pos, float ang, SKPaint pnt, SKBitmap bmp) : base(pos, ang, pnt, bmp)
        {

        }

        public void SetForwardDistance(float dist)
        {
            this.distance = dist;
        }

    }
}
