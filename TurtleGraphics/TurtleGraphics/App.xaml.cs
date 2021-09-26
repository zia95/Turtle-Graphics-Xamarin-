using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TurtleGraphics.Misc;

namespace TurtleGraphics
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            CrashReporting.Enable();

            string dev_id = DependencyService.Get<Services.IFileSystemHelper>().GetDeviceId();

            if (dev_id != null)
                CrashReporting.UserId = dev_id;


            this.MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
