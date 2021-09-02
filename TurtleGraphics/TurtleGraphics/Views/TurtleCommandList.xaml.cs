using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TurtleCommandList : ContentView
    {
        private struct CommandItem
        {
            public int _id;

            private Turtle.SkiaTurtleE.CommandTypes _cmnd_type;
            public Turtle.SkiaTurtleE.CommandTypes CommandType
            {
                get => _cmnd_type;
                set
                {
                    this._cmnd_type = value;
                    ImageSource icn;
                    string txt;
                    if(!Turtle.SkiaTurtleE.GetCommandTypeInfo(this._cmnd_type, out icn, out txt))
                        throw new NotSupportedException($"Type: {this._cmnd_type} is not supported.");

                    this.Icon = icn;
                    this.Text = txt;
                }
            }
            //units acts as a color index value for color view....
            public int Units { get; set; }
            public bool ShouldShowUnits {
                get =>
                    !this.ShouldShowColor && Turtle.SkiaTurtleE.DoCommandNeedExtra(this.CommandType);
            }
            public bool ShouldShowColor { get => this.CommandType == Turtle.SkiaTurtleE.CommandTypes.PenColor; }
            public Color PenColor { get => (this.Units >= 0 && this.Units < ColorPicker.ViewColors.Length) ? ColorPicker.ViewColors[this.Units].ToFormsColor() : Color.Yellow; }

            public ImageSource Icon { get; private set; }
            public string Text { get; private set; }
            
        }
        private ObservableCollection<CommandItem> mListItems = new ObservableCollection<CommandItem>();


        private int mNextUniqueId = 0;
        public int Add(Turtle.SkiaTurtleE.CommandTypes type, int units)
        {
            CommandItem itm = new CommandItem()
            {
                _id = this.mNextUniqueId++,
                CommandType = type,
                Units = units
            };

            if (string.IsNullOrWhiteSpace(itm.Text))
                throw new ArgumentException("enter valid type.", nameof(type));


            int idx = mListItems.Count;

            mListItems.Add(itm);
            return idx;
        }
        public void RemoveAt(int index) => this.mListItems.RemoveAt(index);
        public void Clear()
        {
            this.mListItems.Clear();
            this.mNextUniqueId = 0;
        }

        public int Count { get => this.mListItems.Count; }
        public KeyValuePair<Turtle.SkiaTurtleE.CommandTypes, int> this[int index] 
        {
            get
            {
                var inf = this.mListItems[index];
                return new KeyValuePair<Turtle.SkiaTurtleE.CommandTypes, int>(inf.CommandType, inf.Units);
            }
            set
            {
                var inf = this.mListItems[index];
                inf.CommandType = value.Key;
                inf.Units = value.Value;
                this.mListItems[index] = inf;
            }
        }
        
        public IEnumerable<KeyValuePair<Turtle.SkiaTurtleE.CommandTypes, int>> ToEnumerable()
        {
            for(int i = 0; i < this.Count; i++)
            {
                yield return this[i];
            }
        }

        public TurtleCommandList()
        {
            InitializeComponent();
            
            this.lstCommands.ItemsSource = mListItems;

            this.lstCommands.ItemTapped += async (s, e) =>
            {
                string[] options = new string[] { "Move up", "Move down", "Remove" };
                /*
                PopupPicker.Show(new string[] { "Move up", "Move down", "Remove" }, 
                    (s, e_) => 
                    {
                        
                        this.mListItems.RemoveAt(e.SelectedItemIndex);
                    });
                */
                //Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
                string opt = await this.GetParentPage().DisplayActionSheet("Actions", "Close", null, options);
                if(opt == options[0])
                {
                    if(e.ItemIndex > 0)
                    {
                        
                        this.mListItems.Move(e.ItemIndex, e.ItemIndex-1);
                    }
                }
                else if (opt == options[1])
                {
                    if(e.ItemIndex < (this.mListItems.Count -1))
                    {
                        this.mListItems.Move(e.ItemIndex, e.ItemIndex + 1);
                    }
                }
                else if (opt == options[2])
                {
                    this.mListItems.RemoveAt(e.ItemIndex);
                }
            };
        }
    }
}