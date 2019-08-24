using MagickConverter.Events;
using System;
using System.Collections.Generic;

namespace MagickConverter
{
    public class MagickService
    {
        private readonly MagickQueue _queue;

        public MagickService()
        {
            _queue = new MagickQueue();
        }

        public void Convert(TaskSetup setup)
        {
            var task = new MagickTask(setup);
            task.ConvertFinishedEvent += ConvertFinished;
            task.ConvertProgressEvent += ConvertProgress;
            _queue.EnqueueTask(task);
        }

        protected void ConvertFinished(object sender, ConvertFinishedEventArgs args)
        {
            var setup = args.Setup;

        }

        protected void ConvertProgress(object sender, ConvertProgressEventArgs args)
        {
            //if (MagickTask.ProgressCompleted(sender, args, out var setup)) { }
            //if (MagickTask.GainedDuration(sender, args, out var setup)) { }
        }
    }
}
