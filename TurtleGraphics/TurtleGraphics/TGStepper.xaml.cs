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
    public partial class TGStepper : ContentView
    {
        public static readonly BindableProperty MinProperty = BindableProperty.Create("Min", typeof(int), typeof(TGStepper), 1);
        public static readonly BindableProperty MaxProperty = BindableProperty.Create("Max", typeof(int), typeof(TGStepper), 10);
        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(int), typeof(TGStepper), 1);


        public int Min
        {
            get => (int)this.GetValue(MinProperty);
            set => this.SetValue(MinProperty, value);
        }
        public int Max
        {
            get => (int)this.GetValue(MaxProperty);
            set => this.SetValue(MaxProperty, value);
        }
        public int Value
        {
            get => (int)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }


        public event EventHandler ValueChanged;

        public TGStepper()
        {
            InitializeComponent();


            stp.SetBinding(Stepper.MinimumProperty, new Binding("Min", source: this));
            stp.SetBinding(Stepper.MaximumProperty, new Binding("Max", source: this));
            stp.SetBinding(Stepper.ValueProperty, new Binding("Value", source: this));

            stp.ValueChanged += (s, e) => { ValueChanged?.Invoke(this, e); };
        }
    }
}