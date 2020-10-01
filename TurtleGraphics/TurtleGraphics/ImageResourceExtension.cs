using System;
using System.Collections.Generic;
using System.Text;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TurtleGraphics
{
    [ContentProperty(nameof(Source))]
    class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this.Source != null ? ImageSource.FromResource(this.Source, typeof(ImageResourceExtension).Assembly) : null;
        }
    }
}
