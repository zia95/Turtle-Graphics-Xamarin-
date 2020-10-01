using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TurtleStepper : ContentView
    {
        public event EventHandler<ValueChangedEventArgs> ValueChanged;


        private ImageButton[] _btns = null;

        private int _val;
        public double Value 
        {
            get => _val; 
            set {
                
                if(value > 0 && value <= _btns.Length)
                {
                    this._btns[(int)value - 1].Source = ImageSource.FromResource($"TurtleGraphics.Resources.images.btn_num{value}_on.png");

                    if((_val > 0 && _val <= _btns.Length))
                    {
                        this._btns[(_val - 1)].Source = ImageSource.FromResource($"TurtleGraphics.Resources.images.btn_num{this.Value}_off.png");
                    }

                    this._val = (int)value;
                }
                
            } 
        }

        public TurtleStepper()
        {
            InitializeComponent();

            this._btns = new ImageButton[] { this.btnNum1, this.btnNum2, this.btnNum3, this.btnNum4, this.btnNum5, this.btnNum6 };


            var img_sou_off = new List<ImageSource>();
            var img_sou_on = new List<ImageSource>();


            _btns.ForEach(
                (b) => 
                {
                    b.Pressed += (sender, e) => 
                    {
                        ImageButton btn_sender = sender as ImageButton;

                        for(int i = 0; i < _btns.Length; i++)
                        {
                            ImageButton btn_i = _btns[i];

                            if(btn_i == btn_sender)
                            {
                                if (i + 1 == this.Value)
                                    break;

                                var old_val = _val;
                                this.Value = i + 1;
                                this.ValueChanged?.Invoke(this, new ValueChangedEventArgs(old_val, this.Value));
                                break;
                            }
                        }    
                    };
                }
                );
        }
    }
}