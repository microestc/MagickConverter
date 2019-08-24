using System.Runtime.InteropServices;

namespace MagickConverter
{
    public class EnvironmentCmd
    {
        public string Executable { get; }

        public EnvironmentCmd()
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
