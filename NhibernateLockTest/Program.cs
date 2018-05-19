using System;
using System.Linq;
using System.Threading;

namespace NhibernateLockTest
{
    class Program
    {
        static Random Random = new Random();

        static bool Running = true;

        static void Main(string[] args)
        {
            using (var session = DataService.OpenSession())
            {
                var items = session.QueryOver<TestClass>().RowCount();
                //Console.WriteLine("There are now " + items + " items");
            }

            int cnt = 1;

            StartRandomAccesser();

            StartThread(cnt++);
            //StartThread(cnt++);

            Console.WriteLine("After statring threads");

            string command = Console.ReadLine();
            while (String.IsNullOrWhiteSpace(command))
            {
                command = Console.ReadLine();
                StartThread(cnt);
                cnt++;
            }
            Running = false;
            Console.WriteLine("Done...");
        }

        static void Work1()
        {
            Console.WriteLine("Inside Thread: " + Thread.CurrentThread.Name);
            using (var session = DataService.OpenStatelessSession())
            using (var transaction = session.BeginTransaction())
            {
                for (var i = 0; i < 100; i++)
                {
                    var tmpItem = session.QueryOver<TestClass>().Where(x => x.Number == i).List().ToList();

                    Thread.Sleep(100);
                    //Console.WriteLine("Found " + tmpItem.Count + " items matching number " + i);
                }
            }

            using (var session = DataService.OpenSession())
            //using (var transaction = session.BeginTransaction())
            {
                Console.WriteLine("Starting for loop inside Thread: " + Thread.CurrentThread.Name);
                for (var i = 0; i < 100; i++)
                {
                    var number = Random.Next();

                    var tmp = new TestClass()
                    {
                        Date = DateTime.UtcNow,
                        Number = number,
                        Title = "Item #" + number
                    };
                    //session.Insert(tmp);
                    session.Save(tmp);

                    Thread.Sleep(100);

                    //Console.WriteLine("Sleeping " + i + " Thread: " + Thread.CurrentThread.Name);
                }

                Console.WriteLine("Commiting Thread: " + Thread.CurrentThread.Name);
                //transaction.Commit();
            }
        }

        static void Work2()
        {
            Console.WriteLine("Inside Thread: " + Thread.CurrentThread.Name);
            using (var session = DataService.OpenStatelessSession())
            using (var transaction = session.BeginTransaction())
            {
                for (var i = 0; i < 100; i++)
                {
                    var tmpItem = session.QueryOver<TestClass>().Where(x => x.Number == i).List().ToList();

                    Thread.Sleep(100);
                    //Console.WriteLine("Found " + tmpItem.Count + " items matching number " + i);
                }
            }

            using (var session = DataService.OpenSession())
            {
                Console.WriteLine("Starting for loop inside Thread: " + Thread.CurrentThread.Name);
                for (var i = 0; i < 100; i++)
                {
                    var number = Random.Next();

                    var tmp = new TestClass()
                    {
                        Date = DateTime.UtcNow,
                        Number = number,
                        Title = "Item #" + number
                    };
                    session.Save(tmp);

                    Thread.Sleep(100);

                    //Console.WriteLine("Sleeping " + i + " Thread: " + Thread.CurrentThread.Name);
                }

                Console.WriteLine("Flusing Thread: " + Thread.CurrentThread.Name);
                session.Flush();
            }
        }

        static void StartThread(int number)
        {
            // The old way of using ParameterizedThreadStart. This requires a
            // method which takes ONE object as the parameter so you need to
            // encapsulate the parameters inside one object.
            if (number % 2 == 0)
            {
                var thread = new Thread(Work1);
                thread.Name = "Runner #" + number;
                thread.Start();
            }
            else
            {
                var thread = new Thread(Work1);
                thread.Name = "Runner #" + number;
                thread.Start();
            }

            //var task1 = Task.Factory.StartNew(Work1);
            //var task2 = Task.Factory.StartNew(Work2);

            //Task.WaitAll(task1, task2);
        }

        static void OpenAndCloseConnections()
        {
            while (Running == true)
            {
                Console.WriteLine("Randomly accessing session");
                using (var session = DataService.OpenStatelessSession())
                {
                    var firstItem = session.QueryOver<TestClass>().List().FirstOrDefault();

                    Console.WriteLine("At end of random session");
                }
                var randomSleepCount = Random.Next(10) * 100; // 0.1 to 1 second
                Console.WriteLine("random runner going to sleep");
                //Thread.Sleep(randomSleepCount);
                Thread.Sleep(5000);
            }
        }

        static void StartRandomAccesser()
        {
            var thread = new Thread(OpenAndCloseConnections);
            thread.Name = "Random Running Thread";
            thread.Start();
        }
    }
}