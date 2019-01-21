using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTaskExample
{
    class Program
    {
        static void Main(string[] args)
        {
            //任务实例化();

            //创建和执行任务();

            //分离任务创建和执行
            //等待一个或多个任务完成
            //等待一个任务完成();

            //等待一个任务完成超时了();

            //等待执行任务的一系列中的第一个任务以完成();

            //创建十个任务,等待十个要完成,然后显示其状态

            创建十个任务_等待十个要完成_然后显示其状态();


            Console.WriteLine("main thread");
            Console.ReadKey();

        }

        /// <summary>
        /// 任务实例化
        /// </summary>
        private static void 任务实例化()
        {
            /*
                创建并执行4个任务。
                3个任务执行Action<T>名为的委托action。
                第4个任务执行lambda表达式（Action委托），它是以内联方式定义的任务创建方法。
                每个任务是实例化的，并以不同方式运行：
                任务t1，通过调用任务类构造函数实例化，但在任务t2后面调用其Start()方法。
                任务t2，实例化并启动。
                任务t3，实例化并通过Run方法启动。
                任务t4,是以同步方式执行，在主线程上调用RunSynchronously方法。

                任务t4以同步方式，在主应用程序线程上执行。
                其它任务通常在一个或多个线程池线程上异步执行。
             */

            Action<object> action = (object obj) =>
            {
                Console.WriteLine("Task={0}, obj={1}, Thread={2}",
                        Task.CurrentId,
                        obj,
                        Thread.CurrentThread.ManagedThreadId);
            };

            //Task表示单个没有返回值的操作，通常以异步方式执行。
            //Task对象的中心思想是基于任务的异步模式。
            //执行工作的Task对象通常以异步方式执行线程池线程上，而不是以同步方式在主应用程序线程中，可以使用Status属性确定任务的状态。
            //大多数情况下，lambda表达式用于指定该任务所执行的工作量。

            // Create a task but do not start it.
            Task t1 = new Task(action, "alpha");

            // Construct a  started task
            Task t2 = Task.Factory.StartNew(action, "beta");

            // Block the main thread to demonstrate that t2 is executing
            t2.Wait();
            //等待 Task 完成执行过程。
            //它是一种同步方法，将导致调用线程等待当前的任务实例。

            // Launch t1
            t1.Start();
            Console.WriteLine("t1 has been launched. (Main Thread={0})", Thread.CurrentThread.ManagedThreadId);
            // Wait for the task to finish.
            t1.Wait(); //等待 Task 完成执行过程。

            // Construct a started task using Task.Run.
            String taskData = "delta";
            Task t3 = Task.Run(() => {
                Console.WriteLine("Task={0}, obj={1}, Thread={2}",
                                    Task.CurrentId,
                                    taskData,
                                    Thread.CurrentThread.ManagedThreadId);
            });
            // Wait for the task to finish
            t3.Wait();

            // Construct an unstarted task
            Task t4 = new Task(action, "gamma");
            // Run it synchronously
            t4.RunSynchronously();
            //Although the task was run synchronously, it is a good practice
            // to wait for it in the event exceptions were thrown by the task.
            t4.Wait();
        }

        private static void 创建和执行任务()
        {
            Task t = Task.Factory.StartNew(() => {
                // Just loop.
                int ctr = 0;
                for (ctr = 0; ctr <= 1000000; ctr++)
                {
                }
                Console.WriteLine("Finished {0} loop iterations", ctr);
            });

            t.Wait();
        }

        private static void 等待一个任务完成()
        {
            // Wait on a single task with no timeout specified.
            Task taskA = Task.Run(() => Thread.Sleep(2000));
            Console.WriteLine("taskA Status: {0}", taskA.Status);
            try
            {
                taskA.Wait(); //阻止调用线程，直到任务完成或超时间隔结束
                Console.WriteLine("taskA Status: {0}", taskA.Status);
            }
            catch (AggregateException)
            {
                Console.WriteLine("Exception in taskA");
            }
        }

        private static void 等待一个任务完成超时了()
        {
            // Wait on a single task with a timeout specified.
            Task taskA = Task.Run(() => Thread.Sleep(2000));
            try
            {
                taskA.Wait(1000); // wait for 1 second
                bool completed = taskA.IsCompleted;
                Console.WriteLine("Task A completed: {0}, Status: {1}", completed, taskA.Status);

                if (!completed)
                {
                    Console.WriteLine("Timed out before task A completed. 任务执行超时了");
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Exception in taskA.");
            }
        }

        private static void 等待执行任务的一系列中的第一个任务以完成()
        {
            var tasks = new Task[3];
            var rnd = new Random();
            for (int ctr = 0; ctr <= 2; ctr++)
            {
                tasks[ctr] = Task.Run(() => Thread.Sleep(rnd.Next(500, 3000)));
            }

            try
            {
                int index = Task.WaitAny(tasks); // 等待提供的任一Task对象完成执行过程
                Console.WriteLine("Task #{0} completed first.\n", tasks[index].Id);
                Console.WriteLine("Status of all tasks:");
                foreach (var t in tasks)
                {
                    Console.WriteLine("     Task #{0}: {1}", t.Id, t.Status);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("An exception occurred.");
            }
        }

        private static void 创建十个任务_等待十个要完成_然后显示其状态()
        {
            // Wait for all tasks to complete.
            Task[] tasks = new Task[10];
            for (int i = 0; i < 10; i++)
            {
                tasks[i] = Task.Run(() => Thread.Sleep(5000));
            }
            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("One or more exceptions occurred: ");
                foreach (var ex in ae.Flatten().InnerExceptions)
                {
                    Console.WriteLine("   {0}", ex.Message);
                }
            }

            Console.WriteLine("Status of completed tasks:");
            foreach (var t in tasks)
            {
                Console.WriteLine("   Task #{0}: {1}", t.Id, t.Status);
            }
        }

    }
}
