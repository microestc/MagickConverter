using System;

namespace MagickConverter.Events
{
    public class ConvertTaskFinishedEventArgs : EventArgs
    {
        public MagickTask FinishedTask { get; set; }

        public ConvertTaskFinishedEventArgs(MagickTask task)
        {
            FinishedTask = task;
        }
    }
}
