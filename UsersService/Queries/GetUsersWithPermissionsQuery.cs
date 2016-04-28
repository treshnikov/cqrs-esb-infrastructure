using CQRS;

namespace UsersService
{
    public class GetUsersWithPermissionsQuery : IQuery<User[]>
    {
        public string[] Permissions { get; set; }
        public string ServiceName => "rpc";
    }
}