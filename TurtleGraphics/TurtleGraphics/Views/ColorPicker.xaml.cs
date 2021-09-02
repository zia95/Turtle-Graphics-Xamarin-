using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;
using SkiaSharp.Views.Forms;
//using Xamarin.Forms.Markup;
using System.Diagnostics;

namespace TurtleGraphics.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColorPicker : ContentView
    {
        public static readonly BindableProperty SelectedProperty = BindableProperty.Create("Selected", typeof(int), typeof(ColorPicker), default(int));
        public static readonly BindableProperty CollapseColumnsProperty = BindableProperty.Create("CollapseColumns", typeof(bool), typeof(ColorPicker), false, BindingMode.TwoWay, 
            propertyChanged: (b, oldval, newval) => 
            {
                ((ColorPicker)b).CollapseColumns = (bool)newval;
            });


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
        public bool CollapseColumns
        {
            get => (bool)this.GetValue(CollapseColumnsProperty);
            set
            {
                //get all 4 elms in collection
                var c0 = colColorGrid.Children[0];
                var c1 = colColorGrid.Children[1];
                var c2 = colColorGrid.Children[2];
                var c3 = colColorGrid.Children[3];

                if (value)
                {
                    //set vis... we only want first two elements...
                    c0.IsVisible = true;
                    c1.IsVisible = true;
                    c2.IsVisible = false;
                    c3.IsVisible = false;


                    //move second element to 2nd col
                    Grid.SetRow(c0, 0);
                    Grid.SetColumn(c0, 0);

                    Grid.SetRow(c1, 0);
                    Grid.SetColumn(c1, 1);

                    Grid.SetRow(c2, 0);
                    Grid.SetColumn(c2, 0);

                    Grid.SetRow(c3, 0);
                    Grid.SetColumn(c3, 1);


                    //set up the col defs
                    colColorGrid.ColumnDefinitions.Clear();
                    colColorGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });
                    colColorGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                    colColorGrid.RowDefinitions.Clear();
                    colColorGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                }
                else
                {
                    //set vis... we want all elements...
                    c0.IsVisible = true;
                    c1.IsVisible = true;
                    c2.IsVisible = true;
                    c3.IsVisible = true;


                    //set all elements in a row
                    Grid.SetRow(c0, 0);
                    Grid.SetColumn(c0, 0);

                    Grid.SetRow(c1, 1);
                    Grid.SetColumn(c1, 0);

                    Grid.SetRow(c2, 2);
                    Grid.SetColumn(c2, 0);

                    Grid.SetRow(c3, 3);
                    Grid.SetColumn(c3, 0);


                    //set up the col&row defs
                    colColorGrid.ColumnDefinitions.Clear();
                    colColorGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                    colColorGrid.RowDefinitions.Clear();
                    colColorGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    colColorGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    colColorGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                    colColorGrid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(1, GridUnitType.Star) });
                }

                this.SetValue(CollapseColumnsProperty, value);
            }
        }
        public SKColor SelectedColor { get => ViewColors[this.Selected]; }


        public static readonly SKColor[] ViewColors =
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

        public static SKColor GetColorByIndex(int index) => ViewColors[index];

        public EventHandler SelectionChanged;



        private Dictionary<BoxView, int> m_color_views = new Dictionary<BoxView, int>();



        private int _old_selection = -1;
        private bool select_color(int clr)
        {
            if (this._old_selection == clr)
                return false;

            if (clr >= 0 && clr < this.m_color_views.Count)
            {
                if (_old_selection != -1)
                    this.m_color_views.ElementAt(_old_selection).Key.Scale = 1;

                this.m_color_views.ElementAt(clr).Key.Scale = .6;

                this._old_selection = clr;
                this.Selected = this._old_selection;
                return true;
            }
            return false;
        }


        public ColorPicker()
        {
            InitializeComponent();

            for(int i = 0; i < ViewColors.Length; i++)
            {
                var cbox = this.FindByName<BoxView>($"bxColor{i}");
                this.m_color_views.Add(cbox, i);
                
                cbox.Color = ViewColors[i].ToFormsColor();

                var tap_ges = new TapGestureRecognizer();
                tap_ges.Tapped += (s, e) =>
                {
                    int color_idx;
                    if(this.m_color_views.TryGetValue(s as BoxView, out color_idx))
                    {
                        if(this.select_color(color_idx))
                        {
                            Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
                            this.SelectionChanged?.Invoke(this, new EventArgs());
                        }
                        else
                        {
                            Turtle.SoundManager.Play(Turtle.SoundManager.SND_ERROR);
                        }
                        
                    }
                };
                cbox.GestureRecognizers.Add(tap_ges);
            }

            this.Selected = 0;
        }
    }
}