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

            任务实例化();

            Console.ReadKey();

        }

        private static void 任务实例化()
        {
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
    }
}
