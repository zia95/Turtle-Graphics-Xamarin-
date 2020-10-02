using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TurtleGraphics.Turtle;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NavBar : ContentView
    {
        public enum TurtlePages
        {
            None,
            Main,
            Logic,
            Settings,
        };

        //public Page GetParentPage()
        //{
        //    var parent = this.Parent;
        //    while (parent != null)
        //    {
        //        if (parent is Page)
        //        {
        //            return parent as Page;
        //        }
        //        parent = parent.Parent;
        //    }
        //    return null;
        //}
        public TurtlePages CurrentPage
        {
            get
            {
                var p = this.GetParentPage();
                if (p is Pages.Main) return TurtlePages.Main;
                return (p is Pages.Logic) ? TurtlePages.Logic : TurtlePages.Settings;
            }
        }


        private void update_button_image(TurtlePages turtlePage)
        {
            this.btnTurtle.Source = ImageSource.FromResource(
                    turtlePage == TurtlePages.Main ? "TurtleGraphics.Resources.images.nav_turtle_on.png" : "TurtleGraphics.Resources.images.nav_turtle_off.png",
                    typeof(NavBar).Assembly);

            this.btnLogic.Source = ImageSource.FromResource(
                    turtlePage == TurtlePages.Logic ? "TurtleGraphics.Resources.images.nav_logic_on.png" : "TurtleGraphics.Resources.images.nav_logic_off.png",
                    typeof(NavBar).Assembly);

            this.btnSettings.Source = ImageSource.FromResource(
                    turtlePage == TurtlePages.Settings ? "TurtleGraphics.Resources.images.nav_settings_on.png" : "TurtleGraphics.Resources.images.nav_settings_off.png",
                    typeof(NavBar).Assembly);
        }



        public class PageChangeEventArgs : EventArgs
        {
            public TurtlePages CurrentPage { get; set; }
            public TurtlePages Page { get; set; }
            public bool Animate { get; set; } = false;
            public bool Block { get; set; } = false;
        }

        public event EventHandler<PageChangeEventArgs> PageChanging;


        


        public NavBar()
        {
            InitializeComponent();

            this.btnTurtle.Clicked += async (s, e) =>
            {
                var cp = this.CurrentPage;
                if (cp != TurtlePages.Main)
                {
                    var pce = new PageChangeEventArgs()
                    {
                        CurrentPage = cp,
                        Page = TurtlePages.Main
                    };
                    this.PageChanging?.Invoke(this, pce);

                    if (pce.Block)
                        return;

                    SoundManager.Play(SoundManager.SND_CLICK);
                    await Shell.Current.GoToAsync("//" + nameof(Pages.Main), pce.Animate);
                    this.update_button_image(cp);
                }
                else
                {
                    SoundManager.Play(SoundManager.SND_ERROR);
                }
            };


            this.btnLogic.Clicked += async (s, e) => 
            {
                var cp = this.CurrentPage;
                if (cp != TurtlePages.Logic)
                {
                    var pce = new PageChangeEventArgs()
                    {
                        CurrentPage = cp,
                        Page = TurtlePages.Main
                    };
                    this.PageChanging?.Invoke(this, pce);

                    if (pce.Block)
                        return;

                    SoundManager.Play(SoundManager.SND_CLICK);
                    await Shell.Current.GoToAsync("//" + nameof(Pages.Logic), pce.Animate);

                    this.update_button_image(cp);
                }
                else
                {
                    SoundManager.Play(SoundManager.SND_ERROR);
                }    
                
            };
            this.btnSettings.Clicked += async (s, e) => 
            {
                var cp = this.CurrentPage;
                if (cp != TurtlePages.Settings)
                {
                    var pce = new PageChangeEventArgs()
                    {
                        CurrentPage = cp,
                        Page = TurtlePages.Main
                    };
                    this.PageChanging?.Invoke(this, pce);

                    if (pce.Block)
                        return;

                    SoundManager.Play(SoundManager.SND_CLICK);
                    await Shell.Current.GoToAsync("//" + nameof(Pages.Settings), pce.Animate);

                    this.update_button_image(cp);
                }
                else
                {
                    SoundManager.Play(SoundManager.SND_ERROR);
                }
                
            };

            this.SizeChanged += NavBar_SizeChanged;
            
        }

        private void NavBar_SizeChanged(object sender, EventArgs e)
        {
            this.update_button_image(this.CurrentPage);
        }
    }
}