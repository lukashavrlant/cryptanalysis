using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ExtensionMethods
{
    public class ParallelAction<ArrSource, ResArr>
    {
        public delegate ResArr ThreadAction(ArrSource threadIndex);
        public delegate void ProgressAction(int count);
        public delegate void FinishAction(ResArr[] resArray);

        public ThreadAction threadAction
        { get; set; }

        public ProgressAction progressAction
        { get; set; }

        public FinishAction finishAction
        { get; set; }

        private object synchroObject;

        private int threadsCount;

        private bool[] synchroArray;

        public ArrSource[] ArraySourceItems
        { get; set; }

        public ResArr[] ResultArray
        { get; set; }

        private int progressCounter;


        public ParallelAction(ArrSource[] arrSource, ThreadAction threadAction)
            : this(arrSource, threadAction, Environment.ProcessorCount)
        { }

        public ParallelAction(ArrSource[] arrSource, ThreadAction threadAction, int threadsCount)
        {
            this.ArraySourceItems = arrSource;
            this.threadAction = threadAction;
            this.threadsCount = threadsCount;
        }

        public void Start()
        {
            synchroObject = new object();
            synchroArray = new bool[threadsCount].Fill(false);
            ResultArray = new ResArr[ArraySourceItems.Length];
            progressCounter = 0;
            threadsCount.Times(i => StartThread(i));
        }

        private void StartThread(int i)
        {
            Thread thread = new Thread(() => SynchroThreadAction(i));
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Start();
        }

        private void SynchroThreadAction(int indexThread)
        {
            int count = ArraySourceItems.Length / threadsCount;

            for (int i = 0; i < count; i++)
            {
                int currIndex = indexThread * threadsCount + i;
                ResultArray[currIndex] = threadAction(ArraySourceItems[currIndex]);

                if (progressAction != null)
                {
                    lock (synchroObject)
                    {
                        progressCounter++;
                        progressAction(progressCounter);
                    }
                }
            }

            SyncThread(indexThread);
        }

        private void SyncThread(int indexThread)
        {
            lock (synchroObject)
            {
                synchroArray[indexThread] = true;
                if (synchroArray.All(x => x))
                {
                    if (finishAction != null)
                    {
                        finishAction(ResultArray);
                    }
                }
            }
        }
    }
}
