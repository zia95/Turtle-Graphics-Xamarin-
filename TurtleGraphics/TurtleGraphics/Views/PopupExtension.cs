using System;
using System.Collections.Generic;
using System.Text;
using Rg.Plugins.Popup;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace TurtleGraphics.Views
{
    public static class PopupExtension
    {

        public static void Close(this PopupPage popup, bool animate = true)
        {
            if(PopupNavigation.Instance.PopupStack.Count > 0)
                PopupNavigation.Instance.PopAsync(animate);
        }
    }
}
