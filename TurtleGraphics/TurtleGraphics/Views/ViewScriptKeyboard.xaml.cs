using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SkiaSharp;

namespace TurtleGraphics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ViewScriptKeyboard : ContentView
    {

        public readonly SKColor[] Colors =
        {
            SKColors.White,
            SKColors.Yellow,
            SKColors.Blue,
            SKColors.Purple,
            SKColors.Green,

            SKColors.Brown,
            SKColors.Pink,
            SKColors.Black,
            SKColors.Red,
            SKColors.Orange,
        };

        private Button[] m_num_btns;
        public ViewScriptKeyboard()
        {
            InitializeComponent();

            //this.m_num_btns = {  };
        }
    }
}