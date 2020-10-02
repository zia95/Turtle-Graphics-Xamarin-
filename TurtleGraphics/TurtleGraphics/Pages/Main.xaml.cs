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
using TurtleGraphics.Turtle;

namespace TurtleGraphics.Pages
{
    [DesignTimeVisible(false)]
    public partial class Main : ContentPage
    {
        private bool doRunTur;

        
        //start_or_reset if null means all buttons
        private void update_button_image(bool? start_or_reset, bool on_or_off)
        {
            if(start_or_reset == null || start_or_reset == false)
            {
                this.btnReset.Source = ImageSource.FromResource(
                    on_or_off ? "TurtleGraphics.Resources.images.btn_reset_on.png" : "TurtleGraphics.Resources.images.btn_reset_off.png",
                    typeof(Pages.Main).Assembly);
            }
            if (start_or_reset == null || start_or_reset == true)
            {
                this.btnStart.Source = ImageSource.FromResource(
                    on_or_off ? "TurtleGraphics.Resources.images.btn_start_on.png" : "TurtleGraphics.Resources.images.btn_start_off.png",
                    typeof(Pages.Main).Assembly);
            }
        }

        public void RefreshSpeed()
        {
            if (Turtle.Settings.TurtleSpeed > 0)
            {
                Turtle.Settings.Turtle.DistanceSteps = 1;
                Device.StartTimer(TimeSpan.FromSeconds(1f / Turtle.Settings.TurtleSpeed), () => { canvasView.InvalidateSurface(); return true; });
            }
            else
            {
                Turtle.Settings.Turtle.DistanceSteps = 0;
                Device.StartTimer(TimeSpan.FromSeconds(1f / 60f), () => { canvasView.InvalidateSurface(); return true; });
            }
        }

        public Main()
        {
            InitializeComponent();

            
            canvasView.PaintSurface += CanvasView_PaintSurface;

            var __pos = new SKPoint(0, 0);
            var __pnt = new SKPaint() { Style = SKPaintStyle.StrokeAndFill, StrokeWidth = 1f, StrokeCap = SKStrokeCap.Round, Color = SKColors.White, IsAntialias = true };

            Turtle.Settings.Turtle = new Turtle.SkiaTurtleE(__pos, 0, __pnt);

            Turtle.Settings.UpdateSettingsFromSave();
            this.update_button_image(null, false);


            this.btnStart.Clicked += (s, e) =>
            {
                this.btnStart.IsEnabled = false;
                if(Turtle.Settings.Turtle.Bitmap == null)
                {
                    var siz = this.canvasView.CanvasSize;

                    Turtle.Settings.Turtle.Position = new SKPoint(siz.Width / 2, siz.Height / 2);
                    Turtle.Settings.Turtle.DefaultPosition = Turtle.Settings.Turtle.Position;
                    Turtle.Settings.Turtle.DefaultAngle = 0;

                    Turtle.Settings.Turtle.SetupDisplay((int)siz.Width, (int)siz.Height);


                    Turtle.Settings.RefreshCanvas();
                    this.RefreshSpeed();
                    Turtle.Settings.Turtle.Reset(true);
                }
                else
                {
                    this.RefreshSpeed();
                    Turtle.Settings.Turtle.Reset(false);
                }
                //load any preset commands if commands are null...
                if ((Turtle.Settings.Commands == null || Turtle.Settings.Commands.Count() <= 0) && SaveManager.PresetCommands != null)
                {
                    Turtle.Settings.Commands = SaveManager.PresetCommands[new Random().Next(0, SaveManager.PresetCommands.Length)].Value;
                }

                Turtle.Settings.Turtle.Commands = Turtle.Settings.Commands.ToArray();
                

                this.doRunTur = true;

                this.update_button_image(true, this.doRunTur);
                Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
            };
            this.btnReset.Clicked += BtnReset_Clicked;
            
        }

        private void BtnReset_Clicked(object sender, EventArgs e)
        {
            var __pos = new SKPoint(0, 0);
            var __pnt = new SKPaint() { Style = SKPaintStyle.StrokeAndFill, StrokeWidth = Turtle.Settings.PenSize, StrokeCap = SKStrokeCap.Round, Color = SKColors.White, IsAntialias = true };




            Turtle.Settings.Turtle = new Turtle.SkiaTurtleE(__pos, 0, __pnt);

            var siz = this.canvasView.CanvasSize;

            Turtle.Settings.Turtle.Position = new SKPoint(siz.Width / 2, siz.Height / 2);
            Turtle.Settings.Turtle.DefaultPosition = Turtle.Settings.Turtle.Position;
            Turtle.Settings.Turtle.DefaultAngle = 0;

            Turtle.Settings.Turtle.SetupDisplay((int)siz.Width, (int)siz.Height);


            Turtle.Settings.RefreshCanvas();
            this.RefreshSpeed();
            Turtle.Settings.Turtle.Reset(true);
            Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
        }

        private void CanvasView_PaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            if(Turtle.Settings.CanvasColor.HasValue)
                canvas.Clear(Turtle.Settings.CanvasColor.Value);

            if (this.doRunTur)
            {
                if (!Turtle.Settings.Turtle.RunCommands())
                {
                    this.btnStart.IsEnabled = true;
                    this.doRunTur = false;
                    this.update_button_image(true, this.doRunTur);
                }
            }

            if(Turtle.Settings.Turtle.Bitmap != null)
            {
                canvas.DrawBitmap(Turtle.Settings.Turtle.Bitmap, 0, 0);
            }

        }
    }
}