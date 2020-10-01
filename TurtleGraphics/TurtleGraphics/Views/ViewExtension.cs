using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace TurtleGraphics.Views
{
    public static class ViewExtension
    {
        public static Page GetParentPage(this Element element)
        {
            var parent = element.Parent;
            while (parent != null)
            {
                if (parent is Page)
                {
                    return parent as Page;
                }
                parent = parent.Parent;
            }
            return null;
        }
    }
}
