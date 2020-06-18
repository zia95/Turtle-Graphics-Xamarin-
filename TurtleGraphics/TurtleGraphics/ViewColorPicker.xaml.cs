using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms.Markup;

namespace TurtleGraphics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewColorPicker : ContentView
    {
        //TODO: ADD XAML SUPPORT...............



        public static readonly BindableProperty SelectedProperty = BindableProperty.Create("Selected", typeof(int), typeof(ViewColorPicker), default(int));
        public int Selected
        {
            get
            {
                return (int)this.GetValue(SelectedProperty);
            }
            set
            {
                this.select_color(value);
                this.SetValue(SelectedProperty, value);
            }
        }

        public SKColor Color { get => this.Colors[this.Selected]; }

        public const int MaxColorInRow = 3;
        public readonly SKColor[] Colors =
        {
            SKColors.White,
            SKColors.Chocolate,
            SKColors.DarkBlue,
            SKColors.Purple,

            SKColors.Green,
            SKColors.Brown,
            SKColors.Teal,
            SKColors.Gray,

            SKColors.Red,
            SKColors.Orange,
            SKColors.Yellow,
            SKColors.LightGreen,

            SKColors.LightBlue,
            SKColors.LightCyan,
            SKColors.LightPink,
            SKColors.LightYellow,
        };


        public EventHandler SelectionChanged;



        private List<BoxView> m_color_views = new List<BoxView>();




        private BoxView ___prev_selected;
        private void select_color(int clr)
        {
            if (this.m_color_views != null)
            {
                this.select_color(this.m_color_views[clr]);
            }
        }
        private void select_color(BoxView clr)
        {
            if (this.___prev_selected == clr)
                return;

            if (this.___prev_selected != null)
                this.___prev_selected.CornerRadius = new CornerRadius(0);

            clr.CornerRadius = new CornerRadius(50);

            this.___prev_selected = clr;

            //update selected index
            for(int i = 0; i < this.m_color_views.Count; i++)
            {
                if(this.___prev_selected == this.m_color_views[i])
                {
                    this.Selected = i;
                    break;
                }
            }
        }


        public ViewColorPicker()
        {
            InitializeComponent();

            int curr_col = 0;
            int curr_row = 0;
            foreach(var clr in this.Colors)
            {
                var bx = new BoxView()
                {
                    Color = clr.ToFormsColor(),
                };
                var tap_ges = new TapGestureRecognizer();
                tap_ges.Tapped += (s, e) => 
                {
                    this.select_color(bx);
                    this.SelectionChanged?.Invoke(this, new EventArgs());
                };
                bx.GestureRecognizers.Add(tap_ges);

                


                this.grdColors.Children.Add(bx, curr_row, curr_col);
                this.m_color_views.Add(bx);
                if (curr_row == MaxColorInRow)
                {
                    curr_row = 0;
                    curr_col++;
                }
                else
                {
                    curr_row++;
                }
            }
        }
    }
}