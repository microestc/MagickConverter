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

    public class ConvertProgressEventArgs : EventArgs
    {
        public TimeSpan TotalDuration { get; private set; }

        public TimeSpan Processed { get; private set; }

        public bool IsCompleted { get; private set; }

        public bool IsGainedDuration { get; private set; }

        public ConvertProgressEventArgs(TimeSpan totalDuration, TimeSpan processed, bool isGainedDuration, bool isCompleted = false)
        {
            TotalDuration = totalDuration;
            Processed = processed;
            IsGainedDuration = isGainedDuration;
            IsCompleted = isCompleted;
        }

        public int GetPctCompleted()
        {
            double num = Processed.TotalMilliseconds * 100.0;
            double totalMilliseconds = TotalDuration.TotalMilliseconds;
            return Convert.ToInt32(num / totalMilliseconds);
        }
    }

    public class ConvertTaskFinishedEventArgs : EventArgs
    {
        public MagickTask FinishedTask { get; set; }

        public ConvertTaskFinishedEventArgs(MagickTask task)
        {
            FinishedTask = task;
        }
    }

    public class ConvertFinishedEventArgs : EventArgs
    {
        public TimeSpan ElapsedTime { get; set; }

        public MagickTaskInfo TaskInfo { get; set; }

        public ConvertFinishedEventArgs(TimeSpan elapsedTime, MagickTaskInfo taskInfo)
        {
            ElapsedTime = elapsedTime;
            TaskInfo = taskInfo;
        }
    }
}
