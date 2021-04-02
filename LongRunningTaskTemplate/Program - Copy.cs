//using System;
//using System.Threading;
//using System.Threading.Tasks;

//namespace LongRunningTaskTemplate
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//            while (true)
//            {
//                JobService service = new JobService();
//                service.Run1();
//                service.Run2();
//                Task.Delay(3000).GetAwaiter().GetResult();
//            }
//            Console.WriteLine("Hello World!");
//        }
//    }

//    internal class JobService
//    {
//        private static readonly SemaphoreSlim _collectionLock1 = new SemaphoreSlim(1, 1);
//        private static readonly SemaphoreSlim _collectionLock2 = new SemaphoreSlim(1, 1);

//        public JobService()
//        {

//        }

//        /// <summary>
//        /// Use for Job Run
//        /// </summary>
//        /// <returns></returns>
//        public async Task Run1()
//        {
//            if (_collectionLock1.Wait(100))
//            {
//                try
//                {

//                    Console.WriteLine($"1111>>>> LOCKED _collectionLock1 - {Task.CurrentId}");
//                    // Process Notification Queue

//                    await Function1();

//                    _collectionLock1.Release();
//                    await System.Threading.Tasks.Task.Delay(100);

//                    Console.WriteLine($"1111>>>> RELEASED _collectionLock1 - {Task.CurrentId}");


//                }
//                catch (Exception ex)
//                {
//                    _collectionLock1.Release();
//                    Console.WriteLine($"{ex.Message} - {ex.StackTrace}");
//                }
//            }
//            else
//            {
//                Console.WriteLine($"1111>>>> FAILED _collectionLock1- {Task.CurrentId}");
//            }
//        }

//        public void Run2()
//        {
//            if (_collectionLock2.Wait(100))
//            {
//                Task.Run(async () =>
//                            {
//                                try
//                                {

//                                    Console.WriteLine($"2222>>>> LOCKED _collectionLock2- { Task.CurrentId}");
//                                    // Process Notification Queue

//                                    await Function2();

//                                    _collectionLock2.Release();
//                                    await System.Threading.Tasks.Task.Delay(100);

//                                    Console.WriteLine($"2222>>>> RELEASED _collectionLock2 - {Task.CurrentId}");

//                                }
//                                catch (Exception ex)
//                                {
//                                    _collectionLock2.Release();
//                                    Console.WriteLine($"{ex.Message} - {ex.StackTrace}");
//                                }
//                            });
//            }
//            else
//            {
//                Console.WriteLine($"2222<<< FAILED _collectionLock2 - {Task.CurrentId}");
//            }
//        }

//        async Task Function1()
//        {
//            for (var i = 0; i < 1000; i++)
//            {
//                await System.Threading.Tasks.Task.Delay(10);
//            }
//        }

//        async Task Function2()
//        {
//            for (var i = 0; i < 10; i++)
//            {
//                await System.Threading.Tasks.Task.Delay(100);
//            }
//        }
//    }
//}
