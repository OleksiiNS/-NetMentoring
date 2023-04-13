using System;
using System.Threading.Tasks;

namespace MultiThreading.Task1._100Tasks
{
    internal class Worker
    {
        private readonly int _taskAmount;
        private readonly int _maxIterationsCount;
        private readonly Action<int, int> _outputAction;
        public Worker(int taskAmount, int maxIterationsCount, Action<int,int> outputAction)
        {
            _taskAmount = taskAmount;
            _maxIterationsCount = maxIterationsCount;
            _outputAction = outputAction;
        }

        public void HundredTasks()
        {
            var tasks = new Task[_taskAmount];
            for (var i = 0; i < _taskAmount; i++)
            {
                var param = i;
                tasks[i] = Task.Run(() => IrritatedOutput(param));
            }

            Task.WaitAll(tasks);
        }

        private void IrritatedOutput(int taskNumber) 
        {
            for (var i = 0; i < _maxIterationsCount; i++)
            { 
                _outputAction(taskNumber,i);
            }
        }
    }
}
