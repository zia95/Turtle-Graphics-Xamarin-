using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewNavBar : ContentView
    {
        public enum TurtlePages
        {
            None,
            Main,
            Logic,
            Settings,
        };


        public static readonly BindableProperty CurrentPageProperty = BindableProperty.Create("CurrentPage", typeof(TurtlePages), typeof(ViewNavBar), default(TurtlePages), 
            propertyChanged: OnCurrentPageChanged);

        private static void OnCurrentPageChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var nav = ((ViewNavBar)bindable);
            nav.CurrentPage = (ViewNavBar.TurtlePages)newValue;
        }
        public TurtlePages CurrentPage
        {
            get
            {
                return (TurtlePages)this.GetValue(CurrentPageProperty);
            }
            set
            {
                this.SetValue(CurrentPageProperty, value);
            }
        }




        public static PageMain MainPage { get; private set; } = null;
        public static PageLogic LogicPage { get; private set; } = null;
        public static PageSettings SettingsPage { get; private set; } = null;


        public ViewNavBar()
        {
            InitializeComponent();







            this.btnTurtle.Clicked += async (s, e) =>
            {

                if (this.CurrentPage != TurtlePages.Main)
                    await Shell.Current.GoToAsync("//" + nameof(PageMain), false);
                /*
                while (Navigation.ModalStack.Count != 0)
                    await Navigation.PopModalAsync(false);
                */
                /* old method
                if (MainPage != null)
                {
                    Navigation.PushAsync(MainPage, false);
                }
                */
            };


            this.btnLogic.Clicked += async (s, e) => 
            {
                


                if (LogicPage == null)
                    LogicPage = new PageLogic();

                if(this.CurrentPage != TurtlePages.Logic)
                {

                    await Shell.Current.GoToAsync("//" + nameof(PageLogic), false);
                    

                    /*
                    await Navigation.PushModalAsync(LogicPage, false);

                    if (this.CurrentPage != TurtlePages.Main)
                        Navigation.RemovePage(Navigation.ModalStack[0]);
                    */

                }
            };
            this.btnSettings.Clicked += async (s, e) => 
            {
                if (MainPage == null)
                    MainPage = (PageMain)this.Parent.Parent;


                if (SettingsPage == null)
                    SettingsPage = new PageSettings();

                if (this.CurrentPage != TurtlePages.Settings)
                {
                    await Shell.Current.GoToAsync("//" + nameof(PageSettings), false);

                    /*

                    await Navigation.PushModalAsync(SettingsPage, false);

                    if (this.CurrentPage != TurtlePages.Main)
                        Navigation.RemovePage(Navigation.ModalStack[0]);
                    */
                }
            };
        }
    }
}