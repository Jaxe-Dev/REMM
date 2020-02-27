using System;
using System.IO;
using REMM.Common;

namespace REMM
{
    public partial class App
    {
        public const string Id = "REMM";

        public static DirectoryInfo Directory { get; private set; }

        public App()
        {
            Directory = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            if (!Static.Init()) { throw new AppException("Could not initialize"); }
        }
    }
}
