using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BoxCraftServer
{
    public abstract class Server
    {
        Socket listener;
        public void Run(SocketConnection connection)
        {
            byte[] bytes = new byte[1024];
            listener = new Socket(connection.ip.AddressFamily, 
                                SocketType.Stream, 
                                ProtocolType.Tcp);
            var pipes = new Dictionary<string, Pipe>();
            try
            {
                listener.Bind(connection.endpoint);
                listener.Listen(10);

                while(true)
                {
                    Socket handler = listener.Accept();
                    var key = handler.LocalEndPoint.ToString();
                    var pipe = new Pipe(handler, (message) => {
                        HandleRequest(message, pipes[key]);
                    });
                    pipes[key] = pipe;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public abstract void HandleRequest(string request, Pipe pipe);

        public void Stop()
        {
            listener.Shutdown(SocketShutdown.Both);
            listener.Close();
        }
    }
}
