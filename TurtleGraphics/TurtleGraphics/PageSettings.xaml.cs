using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSettings : ContentPage
    {
        public static readonly string S_KEY_TURTLE_SPEED = "tspeed";
        public static readonly string S_KEY_LINE_SIZE = "tpsiz";
        public static readonly string S_KEY_BOARD_COLOR = "bcolor";
        public static readonly string S_KEY_LINE_COLOR = "lcolor";

        public static object GetPropertlyOrDefault(string key, object def)
        {
            return Application.Current.Properties.ContainsKey(key) ? Application.Current.Properties[key] : def;
        }
        public static void SetPropertly(string key, object val)
        {
            Application.Current.Properties[key] = val;
        }

        
        public PageSettings()
        {
            InitializeComponent();

            //"TurtleGraphics.graphics.fonts.Pixeled.ttf"
            //var assembly = typeof(PageSettings).GetTypeInfo().Assembly;
            //foreach (var res in assembly.GetManifestResourceNames()) {
            //    System.Diagnostics.Debug.WriteLine("found resource: " + res);
            //}

            /*
            this.btnTurtle1.ImageSource = ImageSource.FromResource(Settings.TurtleImageResourceIds[0], typeof(PageSettings).GetTypeInfo().Assembly);
            this.btnTurtle2.ImageSource = ImageSource.FromResource(Settings.TurtleImageResourceIds[1], typeof(PageSettings).GetTypeInfo().Assembly);
            this.btnTurtle3.ImageSource = ImageSource.FromResource(Settings.TurtleImageResourceIds[2], typeof(PageSettings).GetTypeInfo().Assembly);
            this.btnTurtle4.ImageSource = ImageSource.FromResource(Settings.TurtleImageResourceIds[3], typeof(PageSettings).GetTypeInfo().Assembly);
            this.btnTurtle5.ImageSource = ImageSource.FromResource(Settings.TurtleImageResourceIds[4], typeof(PageSettings).GetTypeInfo().Assembly);
            this.btnTurtle6.ImageSource = ImageSource.FromResource(Settings.TurtleImageResourceIds[5], typeof(PageSettings).GetTypeInfo().Assembly);
            this.btnTurtle7.ImageSource = ImageSource.FromResource(Settings.TurtleImageResourceIds[6], typeof(PageSettings).GetTypeInfo().Assembly);
            */

            var xclr_can = Color.FromHex(GetPropertlyOrDefault(S_KEY_BOARD_COLOR, Color.Black.ToHex()) as string);
            var xclr_pen = Color.FromHex(GetPropertlyOrDefault(S_KEY_LINE_COLOR, Color.Red.ToHex()) as string);
            Settings.TurtleSpeed = int.Parse(GetPropertlyOrDefault(S_KEY_TURTLE_SPEED, "0") as string);
            Settings.PenSize = int.Parse(GetPropertlyOrDefault(S_KEY_LINE_SIZE, "1") as string);
            Settings.CanvasColor = xclr_can.ToSKColor();
            Settings.PenColor = xclr_pen.ToSKColor();


            if(Settings.TurtleSpeed == 0)
            {
                this.chkTurtleSpeed.IsChecked = false;
                this.isldTurtleSpeed.Value = 0;
            }
            else
            {
                this.chkTurtleSpeed.IsChecked = true;
                this.isldTurtleSpeed.Value = (Settings.TurtleSpeed / 10) - 1;
            }

            this.stpLineSize.Value = Settings.PenSize;


            this.clrBackground.Color = Settings.CanvasColor.Value;
            this.clrLine.Color = Settings.PenColor.Value;

            this.isldTurtleSpeed.ValueChanged += (s, e) =>
            {
                Settings.TurtleSpeed = this.isldTurtleSpeed.IsEnabled ? (float)Math.Round((this.isldTurtleSpeed.Value + 1) * 10) : 0.0f;

                SetPropertly(S_KEY_TURTLE_SPEED, Settings.TurtleSpeed.ToString());

                Settings.Refresh(false);
            };
            this.chkTurtleSpeed.CheckedChanged += (s, e) =>
            {
                Settings.TurtleSpeed = 0.0f;

                SetPropertly(S_KEY_TURTLE_SPEED, Settings.TurtleSpeed.ToString());

                Settings.Refresh(false);
            };

            this.stpLineSize.ValueChanged += (s, e) =>
            {
                Settings.PenSize = (int)this.stpLineSize.Value;

                SetPropertly(S_KEY_LINE_SIZE, Settings.PenSize.ToString());
            };

            this.clrBackground.ValueChanged += (s, e) => 
            {
                Settings.CanvasColor = this.clrBackground.Color;

                SetPropertly(S_KEY_BOARD_COLOR, Settings.CanvasColor.ToString());

                Settings.Refresh(true);
            };
            this.clrLine.ValueChanged += (s, e) =>
            {
                Settings.PenColor = this.clrLine.Color;

                SetPropertly(S_KEY_LINE_COLOR, Settings.PenColor.ToString());


                Settings.Refresh(false);
            };
        }
    }
}