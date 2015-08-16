using System;
using Nancy.Hosting.Self;
using Nancy.ModelBinding;
using Nancy.Extensions;

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
    // Add a REST interface that allows a Docker daemon to be registered with the plugin
    // TODO: is this a swarmAPI?
    // How to store it - is it only for the time this app is running, or should it persist
    public class DockerDaemon : Nancy.NancyModule
    {
        public DockerDaemon()
        {
            Post["/DockerDaemon.Add"] = _ =>
            {
                // Need the tcp://ip:port address, TLS setting, and a cert to use
                // really should use
                Console.WriteLine("/DockerDaemon.Add");
                var response = (Nancy.Response)@"{""err"": null}";
                return response;
            };
        }
    }
    // IDEA: `net share test$="C:\Users\sven\src\mon-test" 
    // will create a 'hidden' share.
    // as we're doing volume id's sometime, make the sharename <uid>$ /GRANT:Everyone,FULL
    // and then mount.cifs //10.10.10.20/qwerty$ /mnt -o user=sven,uid=1000,gid=50 (or whatever uid&gid the final container needs)
    // I still don't like the global permissions of it, but maybe it'll work?
    // Once the windows share is created, the plugin needs to talk to the docker daemon, and create a volume container to mount that share
    // and then use that to find out the path to the daemon side volume.
    public class DockerVolumePlugin : Nancy.NancyModule
    {
        public class Vol
        {
            public string Name;
        };

        public DockerVolumePlugin()
        {
        Post["/VolumeDriver.Create"] = _ =>
            // {"Name": "volume_name"}
            {
                Console.WriteLine("agent "+Request.Headers.UserAgent);
                Console.WriteLine("request from " + Request.UserHostAddress);
                // ouch. is the Docker Daemon isn't setting a content type for the data its sending?
                // TODO: make PR
                Request.Headers.ContentType = @"application/json";
                var vol = this.Bind<Vol>();
                Console.WriteLine("/VolumeDriver.Create " + vol.Name);
                return @"{""Err"": null}"; // or string error
            };
            Post["/VolumeDriver.Remove"] = _ =>
            // {"Name": "volume_name"}
            {
                Request.Headers.ContentType = @"application/json";
                var vol = this.Bind<Vol>();
                Console.WriteLine("/VolumeDriver.Remove " + vol.Name);
                return @"{""Err"": null}"; // or string error
            };
            Post["/VolumeDriver.Unmount"] = _ =>
            // {"Name": "volume_name"}
            {
                Request.Headers.ContentType = @"application/json";
                var vol = this.Bind<Vol>();
                Console.WriteLine("/VolumeDriver.Unmount " + vol.Name);
                return @"{""Err"": null}"; // or string error
            };
            Post["/VolumeDriver.Mount"] = _ =>
            // {"Name": "volume_name"}
            {
                Request.Headers.ContentType = @"application/json";
                var vol = this.Bind<Vol>();
                Console.WriteLine("/VolumeDriver.Mount " + vol.Name);
                return @"{""Mountpoint"": ""/etc"", ""Err"": null}"; // or string error
            };
            Post["/VolumeDriver.Path"] = _ =>
            // {"Name": "volume_name"}
            {
                Request.Headers.ContentType = @"application/json";
                var vol = this.Bind<Vol>();
                Console.WriteLine("/VolumeDriver.Path " + vol.Name);
                return @"{""Mountpoint"": ""/etc"", ""Err"": null}"; // or string error
            };

        }
    }

}
