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

    // TODO: compare outputs :)
//docker @boot2docker:~$ curl -X POST -I http://10.10.10.20:8000/Plugin.Activate
//HTTP/1.1 200 OK
//Transfer-Encoding: chunked
//Content-Type: text/html
//Server: Microsoft-HTTPAPI/2.0
//Date: Sun, 16 Aug 2015 06:08:00 GMT

//docker@boot2docker:~$ curl -X POST -I http://127.0.0.1:8888/Plugin.Activate
//HTTP/1.1 200 OK
//Content-Type: appplication/vnd.docker.plugins.v1+json
//Date: Sun, 16 Aug 2015 06:08:06 GMT
//Content-Length: 33

    public class DockerPlugin: Nancy.NancyModule
    {
        public DockerPlugin()
        {
            Post["/Plugin.Activate"] = _ =>
            {
                Console.WriteLine("/Plugin.Activate");
                var response = (Nancy.Response)@"{""Implements"": [""VolumeDriver""]}";
                response.ContentType = @"application/vnd.docker.plugins.v1+json";
                return response;
            };
        }
    }
    public class DockerVolumePlugin : Nancy.NancyModule
    {
        public DockerVolumePlugin()
        {
            Post["/VolumeDriver.Create"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("/VolumeDriver.Create");
                return @"{""Err"": null}"; // or string error
            };
            Post["/VolumeDriver.Remove"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("/VolumeDriver.Remove");
                return @"{""Err"": null}"; // or string error
            };
            Post["/VolumeDriver.Unmount"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("/VolumeDriver.Unmount");
                return @"{""Err"": null}"; // or string error
            };
            Post["/VolumeDriver.Mount"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("/VolumeDriver.Mount");
                return @"{""Mountpoint"": ""/etc"", ""Err"": null}"; // or string error
            };
            Post["/VolumeDriver.Path"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("/VolumeDriver.Path");
                return @"{""Mountpoint"": ""/etc"", ""Err"": null}"; // or string error
            };

        }
    }

}
