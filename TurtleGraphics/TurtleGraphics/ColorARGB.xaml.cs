using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace TurtleGraphics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorARGB : ContentView
    {
        public static readonly BindableProperty ShowAlphaProperty = BindableProperty.Create("ShowAlpha", typeof(bool), typeof(ColorARGB), default(bool));
        //public static readonly BindableProperty ColorProperty = BindableProperty.Create("Color", typeof(Color), typeof(ColorARGB), default(Color));
        public bool ShowAlpha 
        { 
            get 
            { 
                return (bool)this.GetValue(ShowAlphaProperty); 
            } 
            set 
            { 
                this.stkColorA.IsVisible = value; 
                this.SetValue(ShowAlphaProperty, value); 
            } 
        }

        public byte A
        {
            get
            {
                if (this.ShowAlpha)
                    this.isldColorA.Value = 255;

                return (byte)this.isldColorA.Value;
            }
            set
            {
                this.isldColorA.Value = this.ShowAlpha ? value : 255;
            }
        }
        public byte R
        {
            get
            {
                return (byte)this.isldColorR.Value;
            }
            set
            {
                this.isldColorR.Value = value;
            }
        }
        public byte G
        {
            get
            {
                return (byte)this.isldColorG.Value;
            }
            set
            {
                this.isldColorG.Value = value;
            }
        }
        public byte B
        {
            get
            {
                return (byte)this.isldColorB.Value;
            }
            set
            {
                this.isldColorB.Value = value;
            }
        }

        public SKColor Color
        {
            get
            {
                return new SKColor(this.R, this.G, this.B, this.A);
            }
            set
            {
                this.A = value.Alpha;
                this.R = value.Red;
                this.G = value.Green;
                this.B = value.Blue;
            }
        }

        public event EventHandler ValueChanged;
        
        public ColorARGB()
        {
            InitializeComponent();
            
            this.stkColorA.SetBinding(StackLayout.IsVisibleProperty, new Binding("ShowAlpha", source: this));
            
            /*
            this.imgColor.SetBinding(Image.BackgroundColorProperty, new Binding("Color", source: this));
            this.imgColor.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(Image.BackgroundColor))
                {
                    this.Color = imgColor.BackgroundColor;
                }
            };
            */

            this.imgColor.BackgroundColor = this.Color.ToFormsColor();

            this.isldColorA.ValueChanged += IsldColor_ValueChanged;
            this.isldColorR.ValueChanged += IsldColor_ValueChanged;
            this.isldColorG.ValueChanged += IsldColor_ValueChanged;
            this.isldColorB.ValueChanged += IsldColor_ValueChanged;
        }

        private void IsldColor_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            this.imgColor.BackgroundColor = this.Color.ToFormsColor();
            this.ValueChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}