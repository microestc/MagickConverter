using System;

namespace MagickConverter.Events
{
    public class ConvertFinishedEventArgs : EventArgs
    {
        public TimeSpan ElapsedTime { get; set; }

        public TaskSetup Setup { get; set; }

        public ConvertFinishedEventArgs(TimeSpan elapsedTime, TaskSetup setup)
        {
            ElapsedTime = elapsedTime;
            Setup = setup;
        }
    }
}
