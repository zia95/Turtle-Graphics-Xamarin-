using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.Reflection;

namespace TurtleGraphics
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class PageMain : ContentPage
    {
        private bool doRunTur;

        public void InvalidateCanvas()
        {
            canvasView.InvalidateSurface();
        }
        public PageMain()
        {
            InitializeComponent();

            Settings.PageMainInstance = this;
            
            canvasView.PaintSurface += CanvasView_PaintSurface;

            var __pos = new SKPoint(0, 0);
            var __pnt = new SKPaint() { Style = SKPaintStyle.StrokeAndFill, StrokeWidth = 1f, StrokeCap = SKStrokeCap.Round, Color = SKColors.White, IsAntialias = true };

            Settings.Turtle = new SkiaTurtleE(__pos, 0, __pnt);


            //this.BtnReset_Clicked(this, EventArgs.Empty);

            

            this.btnRun.Clicked += (s, e) =>
            {
                this.btnRun.IsEnabled = false;
                if(Settings.Turtle.Bitmap == null)
                {
                    var siz = this.canvasView.CanvasSize;

                    Settings.Turtle.Position = new SKPoint(siz.Width / 2, siz.Height / 2);
                    Settings.Turtle.DefaultPosition = Settings.Turtle.Position;
                    Settings.Turtle.DefaultAngle = 0;

                    Settings.Turtle.SetupDisplay((int)siz.Width, (int)siz.Height);


                    Settings.RefreshCanvas();
                    Settings.RefreshSpeed();
                    Settings.Turtle.Reset(true);
                }
                else
                {
                    Settings.RefreshSpeed();
                    Settings.Turtle.Reset(false);
                }

                _turtle = Settings.TurtleSKBitmap[3];


                Settings.PageCommandsInstance.LoadCommands();

                this.doRunTur = true;

            };
            this.btnReset.Clicked += BtnReset_Clicked;
        }

        private void BtnReset_Clicked(object sender, EventArgs e)
        {
            var __pos = new SKPoint(0, 0);
            var __pnt = new SKPaint() { Style = SKPaintStyle.StrokeAndFill, StrokeWidth = Settings.PenSize, StrokeCap = SKStrokeCap.Round, Color = SKColors.White, IsAntialias = true };


            Settings.Turtle = new SkiaTurtleE(__pos, 0, __pnt);

            var siz = this.canvasView.CanvasSize;

            Settings.Turtle.Position = new SKPoint(siz.Width / 2, siz.Height / 2);
            Settings.Turtle.DefaultPosition = Settings.Turtle.Position;
            Settings.Turtle.DefaultAngle = 0;

            Settings.Turtle.SetupDisplay((int)siz.Width, (int)siz.Height);


            Settings.RefreshCanvas();
            Settings.RefreshSpeed();
            Settings.Turtle.Reset(true);
        }

        private SKBitmap _turtle;
        private float last_deg = 0;
        public SKBitmap RotateBitmap(SKBitmap bitmap, float degrees)
        {
            if (degrees == last_deg) return bitmap;
            last_deg = degrees;
            var rotated = new SKBitmap(bitmap.Height, bitmap.Width);

            using (var surface = new SKCanvas(rotated))
            {
                //surface.Translate(rotated.Width, 0);
                surface.RotateDegrees(degrees);
                surface.DrawBitmap(bitmap, 0, 0);
            }

            return rotated;
        }

        SKPaint txt_pen = new SKPaint() { Color = SKColors.White, Style = SKPaintStyle.Fill, TextAlign = SKTextAlign.Right, TextSize = 64.0f };
        SKPaint txt_pen2 = new SKPaint() { Color = SKColors.White, Style = SKPaintStyle.Fill, TextAlign = SKTextAlign.Left, TextSize = 64.0f };
        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(SKColors.Black);
            //canvas.DrawText("<= Commands", e.Info.Width / 2.0f, txt_pen.TextSize, txt_pen);
            //canvas.DrawText("   Settings =>", e.Info.Width / 2.0f, txt_pen.TextSize, txt_pen2);
            
            if (this.doRunTur)
            {
                if (!Settings.Turtle.RunCommands())
                {
                    this.btnRun.IsEnabled = true;
                    this.doRunTur = false;
                }
            }

            if(Settings.Turtle.Bitmap != null)
            {
                canvas.DrawBitmap(Settings.Turtle.Bitmap, 0, 0);
                //_turtle = RotateBitmap(_turtle, Settings.Turtle.Angle);

                //canvas.DrawBitmap(_turtle, Settings.Turtle.Position - new SKPoint(_turtle.Width/2, _turtle.Height/2));
            }

        }
    }
}
