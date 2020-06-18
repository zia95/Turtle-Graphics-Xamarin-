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
    public partial class ViewStepInt : ContentView
    {
        //TODO: ADD XAML SUPPORT...............



        public static readonly BindableProperty ValueProperty = BindableProperty.Create("Value", typeof(int), typeof(ViewStepInt), 1);
        public int Value
        {
            get
            {
                return (int)this.GetValue(ValueProperty);
            }
            set
            {
                if(this.m_btns != null && this.m_btns.Length > 0 &&
                    value > 0 && value < 7)
                {
                    this.SetValue(ValueProperty, value);
                    if (this.m_last_selected != null)
                        this.m_last_selected.CornerRadius = 0;
                    
                    
                    this.m_last_selected = this.m_btns[(value - 1)];
                    this.m_last_selected.CornerRadius = 50;

                    this.ValueChanged?.Invoke(this, new EventArgs());
                }
                
            }
        }


        public EventHandler ValueChanged;

        private Button m_last_selected;
        private readonly Button[] m_btns;

        public ViewStepInt()
        {
            InitializeComponent();

            this.m_btns = new Button[] { this.btn1, this.btn2, this.btn3, this.btn4, this.btn5, this.btn6 };

            this.m_btns[0].Clicked += (s, e) => { this.Value = 1; };
            this.m_btns[1].Clicked += (s, e) => { this.Value = 2; };
            this.m_btns[2].Clicked += (s, e) => { this.Value = 3; };
            this.m_btns[3].Clicked += (s, e) => { this.Value = 4; };
            this.m_btns[4].Clicked += (s, e) => { this.Value = 5; };
            this.m_btns[5].Clicked += (s, e) => { this.Value = 6; };



            this.btnL.Clicked += (s, e) => { this.Value--; };
            this.btnG.Clicked += (s, e) => { this.Value++; };
        }
    }
}