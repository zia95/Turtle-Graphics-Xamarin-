using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PagesNavigation : CarouselPage
    {
        public PagesNavigation()
        {
            InitializeComponent();

            this.Children.Add(new PageCommands());
            this.Children.Add(new MainPage());
            this.Children.Add(new PageSettings());

            this.CurrentPage = this.Children[1];

        }
    }
}