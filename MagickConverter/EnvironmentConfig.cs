using System.Runtime.InteropServices;

namespace MagickConverter
{
    public class EnvironmentConfig
    {
        public string Executable { get; }

        public EnvironmentConfig()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Executable = "magick convert";
            }
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                return;
            Executable = "convert";
        }
    }
}
