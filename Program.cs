using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var p = new Program();

            Task.Run(async () =>
            {
                var tasks = new List<ServiceBase>();

                var t1 = new ServiceOne();
                var t2 = new ServiceTwo();

                tasks.Add(t1);
                tasks.Add(t2);

                var result = new List<double>();

                await Task.WhenAll(
                    tasks.Select(async t =>
                    {
                        var singleTaskResutlt = await t.GetSomeData();

                        result.AddRange(singleTaskResutlt);

                        if (t.Succeed)
                            Console.WriteLine($"{t.Id} completed with {singleTaskResutlt.Count} items");
                        else
                            Console.WriteLine($"{t.Id} not completed");
                    })
                );
            }).Wait();
        }
    }

    public abstract class ServiceBase
    {
        public int Id { get; set; }
        public bool Succeed { get; set; }
        public abstract Task<List<double>> GetSomeData();
    }

    public class ServiceOne : ServiceBase
    {
        public ServiceOne()
        {
            Id = 1;
        }

        public override Task<List<double>> GetSomeData()
        {
            return Task.Run(
                async () =>
                {
                    await Task.Delay(1000);

                    var result = new List<double> { 1, 2, 3 };

                    try
                    {
                        var i = 0;
                        result.Add(1 / i);
                        Succeed = true;
                    }
                    catch (System.DivideByZeroException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Succeed = false;
                    }

                    return result;
                });
        }
    }

    public class ServiceTwo : ServiceBase
    {
        public ServiceTwo()
        {
            Id = 2;
        }

        public override Task<List<double>> GetSomeData()
        {
            return Task.Run(
                async () =>
                {
                    await Task.Delay(2000);

                    Succeed = true;

                    return new List<double> { 4, 5, 6, 7 };
                });
        }
    }
}