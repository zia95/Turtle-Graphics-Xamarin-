using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Internals;
using Rg.Plugins.Popup.Services;
using SkiaSharp.Views.Forms;

namespace TurtleGraphics.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupAddCommand : PopupPage
    {
        public Turtle.SkiaTurtleE.CommandTypes ResultCommand { get; private set; }
        public int ResultUnits { get; private set; }
        public Color ResultColor { get; private set; }

        public event EventHandler Success;

        private struct CommandTypeItem
        {
            public Turtle.SkiaTurtleE.CommandTypes _id;
            public Color StyleBackgroundColor { get; set; }
            public ImageSource Icon { get; set; }
            public string Text { get; set; }
        }

        private readonly Color style_color_selected = (Color)Application.Current.Resources["PrimaryColor"];
        private readonly Color style_color_normal = (Color)Application.Current.Resources["SecondaryColor2"];


        private static int LastSelectedIndex = -1;
        private ObservableCollection<CommandTypeItem> mListItems = new ObservableCollection<CommandTypeItem>();


        public PopupAddCommand(EventHandler on_success)
        {
            InitializeComponent();

            this.Success = on_success;

            int len = Enum.GetNames(typeof(Turtle.SkiaTurtleE.CommandTypes)).Length;
            for (int i = 0; i < len; i++)
            {
                ImageSource img;
                string txt;

                if (Turtle.SkiaTurtleE.GetCommandTypeInfo((Turtle.SkiaTurtleE.CommandTypes)i, out img, out txt))
                {
                    mListItems.Add(new CommandTypeItem() { _id = (Turtle.SkiaTurtleE.CommandTypes)i, StyleBackgroundColor = style_color_normal, Icon = img, Text = txt });
                }
                else throw new NotSupportedException($"Enum of type: {typeof(Turtle.SkiaTurtleE.CommandTypes)} is not supported. (it needs to be in order 0 1 2 ...)");
            }

            this.lstCommandTypes.ItemsSource = mListItems;

            if (LastSelectedIndex > -1)
            {
                this.lstCommandTypes.SelectedItem = mListItems[LastSelectedIndex];
                this.select_item(LastSelectedIndex);
            }
                
            

            this.lstCommandTypes.ItemSelected += (s, e) => this.select_item(e.SelectedItemIndex);


            this.btnAdd.Clicked += (s, e) =>
            {
                var curr = this.mListItems[LastSelectedIndex];


                this.ResultCommand = curr._id;

                
                if(Turtle.SkiaTurtleE.DoCommandNeedExtra(this.ResultCommand))
                {
                    if(this.ResultCommand == Turtle.SkiaTurtleE.CommandTypes.PenColor)
                    {
                        this.ResultUnits = this.clrColorSelector.Selected;
                        this.ResultColor = this.clrColorSelector.SelectedColor.ToFormsColor();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(this.txtUnits.Text) || int.TryParse(this.txtUnits.Text, out int units) == false)
                        {
                            Turtle.SoundManager.Play(Turtle.SoundManager.SND_ERROR);
                            PopupMessage popupMessage = new PopupMessage("Invalid Units", "Please Input valid units.");
                            PopupNavigation.Instance.PushAsync(popupMessage);
                            //DisplayAlert("Invalid Units", "Please Input valid units.", "OK");
                            return;
                        }

                        this.ResultUnits = units;
                    }
                }
                Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
                this.Success?.Invoke(this, EventArgs.Empty);
                PopupNavigation.Instance.PopAsync();
            };
        }

        private void select_item(int new_selected_index)
        {
            if (LastSelectedIndex > -1)
            {
                var prev = this.mListItems[LastSelectedIndex];
                prev.StyleBackgroundColor = style_color_normal;
                this.mListItems[LastSelectedIndex] = prev;
            }

            //set last as curr
            LastSelectedIndex = new_selected_index;

            var curr = this.mListItems[LastSelectedIndex];
            curr.StyleBackgroundColor = style_color_selected;
            this.mListItems[LastSelectedIndex] = curr;

            var needamnt = Turtle.SkiaTurtleE.DoCommandNeedExtra(curr._id);
            this.txtUnits.IsVisible = needamnt && curr._id != Turtle.SkiaTurtleE.CommandTypes.PenColor;
            this.clrColorSelector.IsVisible = needamnt && curr._id == Turtle.SkiaTurtleE.CommandTypes.PenColor;
        }
    }
}