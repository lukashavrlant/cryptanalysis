using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ExtensionMethods
{
    public static class Parallel
    {
        /// <summary>
        /// Paralelní for
        /// </summary>
        /// <param name="repeat">Počet opakování</param>
        /// <param name="action">Funkce, která má běžet ve vláknu</param>
        public static void For(int repeat, Action<int> action)
        {
            Object lockDecrement = new object();
            ParallelStatus status = new ParallelStatus(repeat);

            for (int i = 0; i < repeat; i++)
                StartThread(i, action, lockDecrement, status);

            while (status.Alive)
            {
                Thread.Sleep(10);
            }
        }

        /// <summary>
        /// Paralelní for. Vytvoří se tolik vláken, kolik má procesor jader
        /// </summary>
        /// <param name="action">Funkce, která má běžet ve vláknu</param>
        public static void For(Action<int> action)
        {
            For(Environment.ProcessorCount, action);
        }

        /// <summary>
        /// Vytvoří vlákno s danou funkcí a spustí ho
        /// </summary>
        /// <param name="i">Parametr, který se předá funkci</param>
        /// <param name="action"></param>
        private static void StartThread(int i, Action<int> action, Object lockDecrement, ParallelStatus status)
        {
            Thread thread = new Thread(() => { 
                action(i);
                lock(lockDecrement)
                {
                    status.NumbersRunningThreads--;
                }
            });
            thread.IsBackground = true;
            thread.Priority = ThreadPriority.BelowNormal;
            thread.Start();
        }

        public static Thread InBackground(Action action)
        {
            return InBackground(action, ThreadPriority.BelowNormal);
        }

        public static Thread InBackground(Action action, ThreadPriority priority)
        {
            Thread thread = new Thread(() => action());
            thread.Priority = priority;
            thread.IsBackground = true;
            thread.Start();
            return thread;
        }
    }

    class ParallelStatus
    {
        public int NumbersRunningThreads { get; set; }
        public bool Alive
        {
            get
            {
                return NumbersRunningThreads != 0;
            }
        }

        public ParallelStatus(int threadNumbers)
        {
            NumbersRunningThreads = threadNumbers;
        }
    }
}
