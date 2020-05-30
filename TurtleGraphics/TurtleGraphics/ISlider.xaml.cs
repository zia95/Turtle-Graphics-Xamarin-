using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ISlider : Slider
    {
        public static readonly BindableProperty StepProperty = 
            BindableProperty.Create("Step", typeof(double), typeof(ISlider), (double)1.0f);
        public double Step { get { return (double)this.GetValue(StepProperty); } set { this.SetValue(StepProperty, value); } }
        public ISlider()
        {
            InitializeComponent();
            
            
            //StepValue = (double)this.GetValue(StepProperty);
            /*
            StepValue = 1.0f;
            SliderMain = new Slider
            {
                Minimum = 0.0f,
                Maximum = 5.0f,
                Value = 0.0f,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            */

            this.ValueChanged += (s, e) => 
            {
                var stp = this.Step;
                var newStep = Math.Round(e.NewValue / Step);

                this.Value = newStep * Step;
            };
            /*
            Content = new StackLayout
            {
                Children = { SliderMain },
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            */
        }
    }
}