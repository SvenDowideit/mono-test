using System;
using Nancy.Hosting.Self;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var uri = "http://localhost:8000";
            var config = new HostConfiguration(); config.UrlReservations.CreateAutomatically = true;
            var host = new NancyHost(config, new Uri(uri));

            Console.WriteLine("Listening on port 8000");
            try
            {
                host.Start();  // start hosting
                Console.WriteLine("started!");
                const string escapeString = "q";

                do Console.Write("> "); while (Console.ReadLine() != escapeString);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception\n" + e.Message);
                Console.ReadKey(true);
            }
            finally
            {
                host.Stop();
            }
        }
    }
    public class HelloWorld : Nancy.NancyModule
    {
        public HelloWorld()
        {
            Get["/"] = _ => "Hello mono World!";
        }
    }
}
