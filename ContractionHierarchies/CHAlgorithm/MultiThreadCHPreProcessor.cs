using ContractionHierarchies.GraphImpl;
using ContractionHierarchies.Io;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace ContractionHierarchies.CHAlgorithm
{
    class MultiThreadCHPreProcessor : ICHPreProcessor
    {

        public static int LockTimeout { get; } = 10000;

        const int _maxTaskCount = 3;

        public ContractionOrder PreProcess(IContractionOrder contractionOrder, IContractor contractor, StreetGraph graph)
        {
            bool[] contracted = new bool[graph.VertexCount];
            ConsoleProgressBar progressBar = new();
            int currentTaskCount = 0;
            Task[] tasks = new Task[_maxTaskCount];
            int finishedTasks = 0;
            for (int i = 0; i < graph.VertexCount; i++)
            {
                
                if(currentTaskCount >= _maxTaskCount)
                {
                    int finishedTaskId = Task.WaitAny(tasks);
                    tasks[finishedTaskId] = tasks[currentTaskCount - 1];
                    currentTaskCount--;
                }
                progressBar.ReportProgress((float) finishedTasks / graph.VertexCount);
                int contractionId = contractionOrder.NextVertex(graph, contracted);
                tasks[currentTaskCount] = Task.Run(() =>
                {
                    contractor.Contract(graph, contractionId, contracted);
                    contracted[contractionId] = true;
                    Interlocked.Increment(ref finishedTasks);
                });
                currentTaskCount++;
            }
            progressBar.ReportProgress(1);
            return contractionOrder.ContractionOrder;
        }
    }
}
