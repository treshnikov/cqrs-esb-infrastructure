using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CQRS;
using Newtonsoft.Json;

namespace UsersService
{
    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, User[]>
    {
        public static string UsersDataFilePath
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().CodeBase;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);
                var res = Path.GetDirectoryName(path);

                return $"{res}\\..\\App_Data\\{@"users.json"}";
            }
        }


        public User[] Handle(GetAllUsersQuery query)
        {
            return JsonConvert.DeserializeObject<User[]>(File.ReadAllText(UsersDataFilePath));
        }
    }
}