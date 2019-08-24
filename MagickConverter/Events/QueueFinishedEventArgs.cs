using System;

namespace MagickConverter.Events
{
    public class QueueFinishedEventArgs : EventArgs
    {
        public int StartedTasksCount { get; set; }

        public int CompletedTasksCount { get; set; }

        public QueueFinishedEventArgs(int queuedTasksCount, int completedTasksCount)
        {
            StartedTasksCount = queuedTasksCount;
            CompletedTasksCount = completedTasksCount;
        }
    }
}
