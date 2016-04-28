using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CQRS;
using UsersService;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ms = new MessageService())
            {

                while (true)
                {
                    var arg = DateTime.Now.ToLongTimeString();
                    Console.WriteLine("Send: " + arg);
                    //var x = ms.SendQueryAsync(new QueryMessage(arg, "rpc", TimeSpan.FromSeconds(10))).Result;
                    var x =
                        ms.SendQueryAsync(new GetUsersWithPermissionsQuery()
                        {
                            Permissions = new List<string>() {"1", "2", "3"}.ToArray()
                        }).Result;

                    /*
                    if (!x.IsError)
                    {
                        Console.WriteLine("answer is: " + x.Body);
                    }
                    else
                    {
                        Console.WriteLine("Timeout expired");
                    }
                    */
                    Thread.Sleep(1000);
                }
            }
        }
    }
}
