using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TurtleGraphics.Turtle;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();

            this.clrBackground.Selected = SaveManager.GetCanvasColorIndex();
            Turtle.Settings.CanvasColor = this.clrBackground.SelectedColor;


            Turtle.Settings.TurtleSpeed = SaveManager.GetTurtleSpeed();
            Turtle.Settings.PenSize = SaveManager.GetLineSize(1);




            this.tstpTurtleSpeed.Value = Turtle.Settings.TurtleSpeed;//(Turtle.Settings.TurtleSpeed == 0) ? 6 : ((int)Turtle.Settings.TurtleSpeed / 10) - 1;
            this.tstpLineSize.Value = (int)Turtle.Settings.PenSize;

            Turtle.Settings.RefreshCanvas();
            this.tstpTurtleSpeed.ValueChanged += (s, e) =>
            {
                Turtle.Settings.TurtleSpeed = (float)(this.tstpTurtleSpeed.Value);//new....


                //float spd_val = (float)(this.tstpTurtleSpeed.Value) * 10;

                //Turtle.Settings.TurtleSpeed = (spd_val <= 60) ? spd_val : 0.0f;
                SaveManager.SetTurtleSpeed((int)Turtle.Settings.TurtleSpeed);


                //Settings.RefreshSpeed(); ************************************************** DO REF SPEED???????
            };
            
            this.tstpLineSize.ValueChanged += (s, e) =>
            {
                Turtle.Settings.PenSize = (float)this.tstpLineSize.Value;

                SaveManager.SetLineSize((int)Turtle.Settings.PenSize);
            };

            this.clrBackground.SelectionChanged += (s, e) =>
            {
                Turtle.Settings.CanvasColor = this.clrBackground.SelectedColor;

                SaveManager.SetCanvasColorIndex(this.clrBackground.Selected);

                Turtle.Settings.RefreshCanvas();
            };

            this.btnContact.Clicked += (s, e) =>
            {
                Xamarin.Essentials.Launcher.TryOpenAsync(new Uri("https://play.google.com/store/apps/details?id=com.x2byteapps.turtlegraphics2"));
            };
        }
    }
}