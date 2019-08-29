using MagickConverter.Events;
using System.IO;

namespace MagickConverter
{
    public class MagickService : IMagickService
    {
        private readonly MagickQueue _queue;

        public MagickService()
        {
            _queue = new MagickQueue();
        }

        public void Convert(TaskSetup setup)
        {
            var task = new MagickTask(setup);
            task.TaskFinishedCallback += () => TaskFinished(setup);
            task.ConvertFinishedEvent += ConvertFinished;
            task.ConvertProgressEvent += ConvertProgress;
            _queue.EnqueueTask(task);
        }

        public virtual void ConvertFinished(object sender, ConvertFinishedEventArgs args)
        {
            var setup = args.Setup;
        }

        public virtual void ConvertProgress(object sender, ConvertProgressEventArgs args)
        {
            //if (MagickTask.ProgressCompleted(sender, args, out var setup)) { }
            //if (MagickTask.GainedDuration(sender, args, out var setup)) { }
        }

        public virtual void TaskFinished(TaskSetup setup)
        {
            if (!string.IsNullOrWhiteSpace(setup.Source))
                File.Delete(setup.Source);
            if (setup.CompletedCallBack != null)
                setup.CompletedCallBack(setup);
        }
    }
}
