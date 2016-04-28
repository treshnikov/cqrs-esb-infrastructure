using System.Security.Policy;
using System.Windows.Input;

namespace UsersService
{
    public class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Permission[] Permissions { get; set; }
    }
}