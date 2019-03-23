using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoxCraftServer
{
    public class Pipe
    {
        Socket handler;
        bool running;
        Action<string> handleMessage;

        public Pipe(Socket handler, Action<string> handleMessage)
        {
            this.handler = handler;
            this.handleMessage = handleMessage;
            var thread = new Thread(new ThreadStart(WaitMessages));
            thread.Start();
        }

        public void SendMessage(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);
            handler.Send(bytes);
        }

        private void WaitMessages()
        {
            running = true;
            byte[] bytes = new byte[1024];
            while (running)
            {
                int bytesRecieved = handler.Receive(bytes);
                var message = Encoding.UTF8.GetString(bytes, 0, bytesRecieved);
                handleMessage(message);
            }
        }

        public void Close()
        {
            running = false;
            handler.Shutdown(SocketShutdown.Both);
            handler.Close();
        }
    }
}
