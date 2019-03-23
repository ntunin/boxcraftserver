using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace BoxCraftServer
{
    class Program: ConsoleProgram
    {
        private LogServer server;
        private ChatClient client;

        public Program(string[] args): base(args)
        {
        }

        static void Main(string[] args)
        {
            program = new Program(args);
        }

        protected override void Run()
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

        protected override void OnTerminate()
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

    }
}
