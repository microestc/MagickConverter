using MagickConverter.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MagickConverter
{
    public class MagickQueue
    {
        private Queue<MagickTask> _tasks = new Queue<MagickTask>();
        private bool _isEnabled = true;
        private readonly object _lockObj = new object();
        private int _queuedTasksCount;
        private int _completedTasksCount;

        public QueueStatus CurrrentStatus { get; set; } = QueueStatus.NoTasks;

        public event EventHandler<QueueFinishedEventArgs> QueueFinishedEvent;

        public event EventHandler QueuePausedEvent;

        public void Play()
        {
            if (_isEnabled || CurrrentStatus != QueueStatus.Paused)
                return;
            _isEnabled = true;
            NextTask();
        }

        public void GracefullyPause()
        {
            if (!_isEnabled || CurrrentStatus != QueueStatus.Running)
                return;
            _isEnabled = false;
        }

        public void EnqueueTask(MagickTask task)
        {
            lock (_lockObj)
            {
                task.TaskFinishedCallback += new Action(NextTask);
                task.TaskFinishedCallback += new Action(OnTaskCompleted);
                _tasks.Enqueue(task);
                ++_queuedTasksCount;
            }
            if (!_isEnabled || CurrrentStatus != QueueStatus.NoTasks)
                return;
            NextTask();
        }

        public void EnqueueMultiple(IEnumerable<MagickTask> tasks)
        {
            lock (_lockObj)
            {
                foreach (MagickTask task in tasks)
                {
                    task.TaskFinishedCallback += new Action(NextTask);
                    task.TaskFinishedCallback += new Action(OnTaskCompleted);
                    _tasks.Enqueue(task);
                    ++_queuedTasksCount;
                }
                if (!_isEnabled || CurrrentStatus != QueueStatus.NoTasks)
                    return;
                NextTask();
            }
        }

        private async void NextTask()
        {
            if (_isEnabled)
            {
                MagickTask task;
                lock (_lockObj)
                {
                    if (!_tasks.Any())
                    {
                        NotifyQueueFinished();
                        return;
                    }
                    task = _tasks.Dequeue();
                    if (task == null)
                        return;
                }
                if (task != null)
                {
                    CurrrentStatus = QueueStatus.Running;
                    await Task.Factory.StartNew(() => task.Convert());
                }
                else
                    NotifyQueuePaused();
            }
            else
                NotifyQueuePaused();
        }

        private void OnTaskCompleted()
        {
            ++_completedTasksCount;
        }

        private void NotifyQueueFinished()
        {
            Console.WriteLine("MagickQueue: all tasks completed.");
            CurrrentStatus = QueueStatus.NoTasks;
            EventHandler<QueueFinishedEventArgs> queueFinishedEvent = QueueFinishedEvent;
            if (queueFinishedEvent == null)
                return;
            queueFinishedEvent(this, new QueueFinishedEventArgs(_queuedTasksCount, _completedTasksCount));
        }

        private void NotifyQueuePaused()
        {
            Console.WriteLine("MagickQueue: tasks has been paused...");
            CurrrentStatus = QueueStatus.Paused;
            EventHandler queuePausedEvent = QueuePausedEvent;
            if (queuePausedEvent == null)
                return;
            queuePausedEvent(this, EventArgs.Empty);
        }
    }
}
