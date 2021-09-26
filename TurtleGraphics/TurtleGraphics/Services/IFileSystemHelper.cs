using System;
using System.Collections.Generic;
using System.Text;

namespace TurtleGraphics.Services
{
    public interface IFileSystemHelper
    {
        string GetAppExternalStorage();
        string GetDeviceId();
    }
}
