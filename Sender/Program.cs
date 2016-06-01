using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CQRS;
using Newtonsoft.Json;
using UsersService;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ms = new RabbitMqEsbMessageService())
            {

                while (true)
                {
                    var arg = DateTime.Now.ToLongTimeString();
                    Console.WriteLine("Send: " + arg);

                    var msg = new GetUsersWithPermissionsQuery
                    {
                        Permissions = new List<string> {"1", "2", "3"}.ToArray()
                    };

                    var x =
                        ms.SendAndGetResult(new EsbMessage(JsonConvert.SerializeObject(msg), "demo", TimeSpan.FromSeconds(5))).Result;

                    if (!x.IsError)
                    {
                        Console.WriteLine("answer is: " + x.Body);
                    }
                    else
                    {
                        Console.WriteLine("Error: " + x.ErrorText);
                    }

                    Thread.Sleep(1000);
                }
            }
        }
    }
}
