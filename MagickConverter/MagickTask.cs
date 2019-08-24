using MagickConverter.Events;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace MagickConverter
{
    public class MagickTask
    {
        private readonly MagickProgress _progress;
        private readonly string _workingDir;
        private readonly EnvironmentConfig _envConfig;
        private readonly TaskSetup _taskSetup;
        private DateTime _startTime;

        public Guid Id { get; set; }

        public string MediaId { get; private set; }

        public bool IsFinished { get; private set; } = false;
        internal Action TaskFinishedCallback { get; set; }

        public event EventHandler<ConvertProgressEventArgs> ConvertProgressEvent;

        public event EventHandler<ConvertFinishedEventArgs> ConvertFinishedEvent;

        public MagickTask(TaskSetup setup)
        {
            Id = Guid.NewGuid();
            _workingDir = new FileInfo(Assembly.GetEntryAssembly().Location).Directory.FullName;
            _progress = new MagickProgress(args => ConvertProgressEvent(this, args));
            _envConfig = new EnvironmentConfig();
        }

        public void Convert()
        {
            _startTime = DateTime.Now;
            if (IsFinished)
                throw new Exception("Cannot start task when it's finished");
            try
            {
                Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }

        private void Start()
        {
            var startInfo = new ProcessStartInfo
            {
                FileName = _envConfig.Executable,
                Arguments = _taskSetup.GetCmdLineParams(),
                WorkingDirectory = _workingDir,
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };
            using (var process = new Process())
            {
                process.StartInfo = startInfo;
                process.ErrorDataReceived += new DataReceivedEventHandler(OnErrorDataReceived);
                process.OutputDataReceived += new DataReceivedEventHandler(OnOutputDataReceived);
                process.Start();
                process.BeginErrorReadLine();
                process.BeginOutputReadLine();
                process.WaitForExit();
                IsFinished = true;
                if (TaskFinishedCallback == null) return;
                TaskFinishedCallback();
            }
        }

        private void OnOutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            _progress.ParseLine(e.Data);
        }

        private void OnErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            _progress.ParseLine(e.Data);
        }

        private void OnProgress(object sender, ConvertProgressEventArgs eargs)
        {
            if (ConvertProgressEvent == null) return;
            ConvertProgressEvent(sender, eargs);
        }

        private void OnFinished(object sender, ConvertFinishedEventArgs eargs)
        {
            if (ConvertFinishedEvent == null) return;
            ConvertFinishedEvent(sender, eargs);
        }

        public static bool ProgressCompleted(object sender, ConvertProgressEventArgs eargs, out string mediaId)
        {
            if (eargs.IsCompleted)
            {
                if (sender is MagickTask)
                {
                    mediaId = (sender as MagickTask).MediaId;
                    return true;
                }
                mediaId = null;
                return true;
            }
            mediaId = null;
            return false;
        }

        public static bool GainedDuration(object sender, ConvertProgressEventArgs eargs, out string mediaId)
        {
            if (eargs.IsGainedDuration)
            {
                if (sender is MagickTask)
                {
                    mediaId = (sender as MagickTask).MediaId;
                    return true;
                }
                mediaId = null;
                return true;
            }
            mediaId = null;
            return false;
        }
    }
}
