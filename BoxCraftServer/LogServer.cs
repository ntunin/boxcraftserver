using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BoxCraftServer
{
    public class LogServer : Server
    {
        private Dictionary<string, User> users = new Dictionary<string, User>();

        private class User
        {
            public Pipe pipe;
            public string name;

            public User(string name, Pipe pipe)
            {
                this.name = name;
                this.pipe = pipe;
            }
        }

        public override void HandleRequest(string request, Pipe pipe)
        {
            var element = XElement.Parse(request);
            foreach (var attribute in element.Attributes()) {
                if (attribute.Name.LocalName.Equals("type"))
                {
                    switch (attribute.Value)
                    {
                        case "user":
                            RegisterUser(element, pipe);
                            break;
                        case "message":
                            SendMessage(element, pipe);
                            break;
                    }
                }
            }
        }


        private void RegisterUser(XElement element, Pipe pipe)
        {
            var name = element.Nodes().First().ToString();
            users[pipe.ToString()] = new User(name, pipe);
            var message = $"User \'{name}\' joined to the channel";
            Console.WriteLine(message);
            foreach (var key in users.Keys)
            {
                var user = users[key];
                user.pipe.SendMessage((user.name != name) ? message : "You are joined to the channel");
            } 
        }

        private void SendMessage(XElement element, Pipe pipe)
        {
            var content = element.Nodes().First().ToString();
            var sender = users.Where((KeyValuePair<string, User> pair) => {
                return pair.Key.Equals(pipe.ToString());
            }).First().Value;

            var message = $"\'{sender.name}\' [{DateTime.Now}]: {content}";
            Console.WriteLine(message);
            foreach (var user in users.Values)
            {
                if (sender.name != user.name)
                {
                    user.pipe.SendMessage(message);
                }
            }
        }
    }
}
