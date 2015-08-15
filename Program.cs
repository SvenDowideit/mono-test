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
            Get["/"] = _ =>
            {
                return "Hello mono World: asked for / ";
            };
            Get["/test/{req}"] = parameters =>
            {
                return "Hello mono World: asked for test "+ parameters.req;
            };
        }
    }
    public class DockerPlugin: Nancy.NancyModule
    {
        public DockerPlugin()
        {
            Get["/Plugin.Activate"] = _ =>
            {
                Console.WriteLine("/Plugin.Activate");
                return @"{""Implements"": [""VolumeDriver""]}";
            };
        }
    }
    public class DockerVolumePlugin : Nancy.NancyModule
    {
        public DockerVolumePlugin()
        {
            Get["/VolumeDriver.Create"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("/VolumeDriver.Create");
                return @"{""Err"": null}"; // or string error
            };
            Get["/VolumeDriver.Remove"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("/VolumeDriver.Remove");
                return @"{""Err"": null}"; // or string error
            };
            Get["/VolumeDriver.Unmount"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("/VolumeDriver.Unmount");
                return @"{""Err"": null}"; // or string error
            };
            Get["/VolumeDriver.Mount"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("/VolumeDriver.Mount");
                return @"{""Mountpoint"": "" / path / to / directory / on / host"", ""Err"": null}"; // or string error
            };
            Get["/VolumeDriver.Path"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("/VolumeDriver.Path");
                return @"{""Mountpoint"": "" / path / to / directory / on / host"", ""Err"": null}"; // or string error
            };

        }
    }

}
