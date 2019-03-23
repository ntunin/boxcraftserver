using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoxCraftServer
{
    public abstract class Client
    {
        private Pipe pipe;
        public void Authenticate(SocketConnection connection)
        {
            var sender = new Socket(connection.ip.AddressFamily,
                                SocketType.Stream,
                                ProtocolType.Tcp);
            sender.Connect(connection.endpoint);
            pipe = new Pipe(sender, (message) =>
            {
                Handle(message);
            });
        }

        public void Send(string message)
        {
            pipe.SendMessage(message);
        }

        public abstract void Handle(string message);

        public virtual void Close()
        {
            pipe.Close();
        }
    }
}
