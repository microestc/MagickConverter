using System;
using System.Runtime.InteropServices;

namespace MagickConverter
{
    public class TaskSetup
    {
        public TaskSetup(string id, string source, string dest, string mark = "")
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Executable = "magick.exe";
                Arg = "convert";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Executable = "convert";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Executable = "convert";
            Id = id;
            Source = source;
            Dest = dest;
            Mark = mark;
        }

        public TaskSetup(string id, string cmd)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Executable = "magick.exe";
                Arg = "convert";
            }
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Executable = "convert";
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                Executable = "convert";
            Id = id;
            Cmd = cmd;
        }

        public string Id { get; private set; }

        public string Source { get; private set; }

        public string Dest { get; private set; }

        public string Mark { get; private set; }

        private string Cmd { get; set; }

        public string Executable { get; }

        private string Arg { get; set; } = "";

        public Action<TaskSetup> CompletedCallBack { get; set; }

        internal string GetCmdLineParams()
        {
            if (!string.IsNullOrWhiteSpace(Cmd))
            {
                return Cmd;
            }
            return $"{Arg} -verbose -density 220 -quality 50 {Source} -append {Dest}";
        }
    }
}
