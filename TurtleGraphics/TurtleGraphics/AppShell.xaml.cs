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
    public partial class AppShell : Shell
    {
        public delegate bool BlockBackButton(object sender, EventArgs e);

        public static BlockBackButton DoBlockBackButton { get; set; }

        public AppShell()
        {
            InitializeComponent();

            
        }
    }
}