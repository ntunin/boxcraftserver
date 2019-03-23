using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BoxCraftServer
{
    public class ChatClient : Client
    {
        bool running = false;
        string name;

        public ChatClient()
        {
            Console.Write("Server IP: > ");
            string endpointString = Console.ReadLine();
            string[] endpointComponents = endpointString.Split(':');
            string hostName = endpointComponents[0].Trim();
            int port = int.Parse(endpointComponents[1]);

            Console.Write("Your nickname: > ");
            name = Console.ReadLine();

            Authenticate(new SocketConnection(hostName, port));
            Send($"<auth user = \"{name}\"/>");

            running = true;
            while(running)
            {
                Console.Write($"{name}: > ");
                string message = Console.ReadLine();
                Send($"<message user = \"{name}\">{message}</message>");
            }
        }

        public override void Handle(string message)
        {
            ClearCurrentConsoleLine();
            Console.WriteLine(message);
            Console.Write($"{name}: > ");
        }

        public override void Close()
        {
            running = false;
            base.Close();
        }

        private void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            for (int i = 0; i < Console.WindowWidth; i++)
                Console.Write(" ");
            Console.SetCursorPosition(0, currentLineCursor);
        }
    }
}
