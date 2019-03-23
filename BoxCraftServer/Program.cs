using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace BoxCraftServer
{
    class Program
    {
        static Program program;

        private LogServer server;
        private ChatClient client;

        static void Main(string[] args)
        {
            program = new Program();
            program.Run();
        }

        static void OnProcessExit(object sender, EventArgs e)
        {
            program.Stop();
        }

        private void Stop()
        {
            if (server != null)
            {
                server.Stop();
            }
            if (client != null)
            {
                client.Close();
            }
        }

        private void Run()
        {
            RunServer();
            //RunClient();
        }

        private void RunClient()
        {
            var connection = new SocketConnection(null, 11001);
            client = new ChatClient();
        }


        private void RunServer()
        {
            server = new LogServer();
            var connection = new SocketConnection(null, 11020);
            Console.WriteLine($"Server running on {connection.ip.ToString()}:{connection.port}");
            server.Run(connection);
        }
        
    }
}
