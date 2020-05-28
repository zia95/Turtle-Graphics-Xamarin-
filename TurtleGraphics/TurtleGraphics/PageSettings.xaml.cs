using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageSettings : ContentPage
    {
        public PageSettings()
        {
            InitializeComponent();

            /*
            var assembly = typeof(PageSettings).GetTypeInfo().Assembly;
            foreach (var res in assembly.GetManifestResourceNames())
            {
                System.Diagnostics.Debug.WriteLine("found resource: " + res);
            }
            */
            
            this.img1.Source = ImageSource.FromResource("TurtleGraphics.graphics.turtle.1.png", typeof(PageSettings).GetTypeInfo().Assembly);
            this.img2.Source = ImageSource.FromResource("TurtleGraphics.graphics.turtle.2.png", typeof(PageSettings).GetTypeInfo().Assembly);
            this.img3.Source = ImageSource.FromResource("TurtleGraphics.graphics.turtle.3.png", typeof(PageSettings).GetTypeInfo().Assembly);
            this.img4.Source = ImageSource.FromResource("TurtleGraphics.graphics.turtle.4.png", typeof(PageSettings).GetTypeInfo().Assembly);
            this.img5.Source = ImageSource.FromResource("TurtleGraphics.graphics.turtle.5.png", typeof(PageSettings).GetTypeInfo().Assembly);
            this.img6.Source = ImageSource.FromResource("TurtleGraphics.graphics.turtle.6.png", typeof(PageSettings).GetTypeInfo().Assembly);
            this.img7.Source = ImageSource.FromResource("TurtleGraphics.graphics.turtle.7.png", typeof(PageSettings).GetTypeInfo().Assembly);
        }
    }
}