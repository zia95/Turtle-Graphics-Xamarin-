using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Pages;
using System.Collections.ObjectModel;
using TurtleGraphics.Turtle;
using Xamarin.Forms.Internals;

namespace TurtleGraphics.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PopupLoadCommands : PopupPage
    {
        public List<KeyValuePair<Turtle.SkiaTurtleE.CommandTypes, int>> Result { get; private set; }
        public EventHandler Success;


        private KeyValuePair<string, List<KeyValuePair<Turtle.SkiaTurtleE.CommandTypes, int>>>[] mSavedLists;

        private ObservableCollection<string> mLoadListItems;
        public PopupLoadCommands(EventHandler on_success)
        {
            InitializeComponent();
            this.Success = on_success;
            var lst = SaveManager.LoadCommandLists().ToList();
            if (SaveManager.PresetCommands != null)
                lst.AddRange(SaveManager.PresetCommands);

            this.mSavedLists = lst.ToArray();
            
            this.Appearing += (s, e) => {
                if (this.mSavedLists.Length <= 0)
                {
                    this.Close();
                }
            };
            

            this.mLoadListItems = new ObservableCollection<string>();
            this.mSavedLists.ForEach(elm => this.mLoadListItems.Add(elm.Key));
            
            
            this.pkrSaves.ItemsSource = this.mLoadListItems;

            this.pkrSaves.SelectedIndex = 0;


            this.btnLoad.Clicked += (s, e) =>
            {
                Turtle.SoundManager.Play(Turtle.SoundManager.SND_CLICK);
                this.Result = this.mSavedLists[this.pkrSaves.SelectedIndex].Value;
                this.Success?.Invoke(this, EventArgs.Empty);
                this.Close();
            };
        }
    }
}