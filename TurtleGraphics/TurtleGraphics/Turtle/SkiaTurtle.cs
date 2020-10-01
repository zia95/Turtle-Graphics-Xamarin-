using System;
using System.Collections.Generic;
using System.Text;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace TurtleGraphics.Turtle
{
    public class SkiaTurtle
    {
        public SKPaint Paint { get; set; }

        public SKBitmap Bitmap { get; private set; }

        public SKCanvas Canvas { get; private set; }

        public SKPoint Position { get; set; }

        public SKPoint DefaultPosition { get; set; }
        public float DefaultAngle { get; set; }

        private float ang = 0;
        public float Angle { get { return ang; } set { ang = value % 360; } }

        public bool PenUp { get; set; }

        public float RemainingDistance { get; set; }
        public int DistanceSteps { get; set; }

        public SkiaTurtle(SKPoint pos, float ang, SKPaint pnt)
        {
            this.Position = pos;
            this.DefaultPosition = this.Position;
            this.Angle = ang;
            this.DefaultAngle = this.Angle;
            this.Paint = pnt;
            
        }

        public void SetupDisplay(int width, int height, SKColor? clear_color = null)
        {
            this.Bitmap = (this.Bitmap == null) ? new SKBitmap(width, height) : this.Bitmap.Resize(new SKSizeI(width, height), SKFilterQuality.High);

            this.Canvas = new SKCanvas(this.Bitmap);

            if(clear_color.HasValue)
                this.Canvas.Clear(clear_color.Value);
        }

        public void Forward(float distance)
        {
            SKPoint newpt = this.calc_forward(distance);

            if(!this.PenUp) 
                this.Canvas.DrawLine(this.Position, newpt, this.Paint);
            this.Position = newpt;
        }

        public void Backward(float distance) => Forward(-distance);


        public void SetForwardDistance(float dist, int steps)
        {
            this.RemainingDistance = dist;
            this.DistanceSteps = steps;
        }
        public void SetBackwardDistance(float dist, int steps) => SetForwardDistance(-dist, steps);


        public bool DoMovement()
        {
            if (this.RemainingDistance == 0)
                return false;

            if(this.RemainingDistance > 0)
            {
                this.RemainingDistance -= this.DistanceSteps;
                float stp = this.DistanceSteps;

                if(this.RemainingDistance < 0)
                {
                    stp += this.RemainingDistance;
                    this.RemainingDistance = 0f;
                }
                this.Forward(stp);
                
            }
            else
            {
                this.RemainingDistance += this.DistanceSteps;
                float stp = -this.DistanceSteps;

                if (this.RemainingDistance > 0)
                {
                    stp -= this.RemainingDistance;
                    this.RemainingDistance = 0f;
                }
                this.Forward(stp);
            }
            return true;
        }
        
        public SKPoint calc_forward(float distance = 10)
        {
            var angleRadians = this.Angle * Math.PI / 180;
            var newX = this.Position.X + (distance * Math.Sin(angleRadians));
            var newY = this.Position.Y + (distance * Math.Cos(angleRadians));
            return new SKPoint((float)newX, (float)newY);
        }

    }
}
