using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace TurtleGraphics
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {



        public MainPage()
        {
            InitializeComponent();


            canvasView.PaintSurface += CanvasView_PaintSurface;

            //this.btnMoveForward.Clicked += (s,e) => { this.turtle.Forward(50); this.btnMoveForward.Text = $"Forward: {this.turtle.Position}"; };
            this.btnSetForward.Clicked += (s, e) => 
            {
                turtle.SetForwardDistance(float.Parse(this.entForwardMovement.Text), 2); 
            };
            this.btnSetBackward.Clicked += (s, e) =>
            {
                turtle.SetBackwardDistance(float.Parse(this.entBackwardMovement.Text), 2);
            };


            this.btnSetRotate.Clicked += (s, e) => 
            { 
                this.turtle.Angle = float.Parse(this.entRotationMovement.Text);
            };

            this.btnResetPos.Clicked += (s, e) =>
            {
                this.turtle.Position = tur_org;
            };

            Device.StartTimer(TimeSpan.FromSeconds(1f / 60), () => { canvasView.InvalidateSurface(); return true; });
        }

        SkiaTurtle turtle = null;
        SKPoint tur_org = SKPoint.Empty;
        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.CornflowerBlue);



            if(turtle == null)
            {
                tur_org = new SKPoint(e.Info.Width / 2, e.Info.Height / 2);
                turtle = new SkiaTurtle(tur_org, 0,
            new SKPaint() { Style = SKPaintStyle.StrokeAndFill, StrokeWidth = 10f, StrokeCap = SKStrokeCap.Round, Color = SKColors.Red, IsAntialias = true },
            new SKBitmap(e.Info.Width, e.Info.Height));

                turtle.Canvas.Clear(SKColors.Black);
            }


            this.lblInfo.Text = $"Position: {this.turtle.Position}; Rotation: {this.turtle.Angle};\nDistanceToCover: {turtle.RemainingDistance}; Steps: {turtle.DistanceSteps};";

            turtle.DoMovement();
            canvas.DrawBitmap(turtle.Bitmap, 0, 0);

        }
    }
}
