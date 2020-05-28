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
        private bool doRunTur;

        public void InvalidateCanvas()
        {
            canvasView.InvalidateSurface();
        }
        public MainPage()
        {
            InitializeComponent();

            Settings.PageMainInstance = this;

            canvasView.PaintSurface += CanvasView_PaintSurface;

            var __pos = new SKPoint(0, 0);
            var __pnt = new SKPaint() { Style = SKPaintStyle.StrokeAndFill, StrokeWidth = 1f, StrokeCap = SKStrokeCap.Round, Color = Settings.PenColor.Value, IsAntialias = true };


            Settings.Turtle = new SkiaTurtleE(__pos, 0, __pnt);

            Settings.Refresh();
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


                    Settings.Refresh(true);
                    Settings.Turtle.Reset(true);
                }
                else
                {
                    Settings.Refresh(false);
                    Settings.Turtle.Reset(false);
                }
                

                

                Settings.Turtle.Commands = Settings.PageCommandsInstance.ListCommands.ToList();

                this.doRunTur = true;

            };
        }

        
        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear(Settings.CanvasColor ?? SKColors.Green);

            if(this.doRunTur)
            {
                if (!Settings.Turtle.RunCommands())
                {
                    this.btnRun.IsEnabled = true;
                    this.doRunTur = false;
                }
                    

                
            }

            if(Settings.Turtle.Bitmap != null)
                canvas.DrawBitmap(Settings.Turtle.Bitmap, 0, 0);

        }
    }
}
