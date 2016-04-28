using System;
using CQRS;

namespace UsersService
{
    public class GetAllUsersQuery : IQuery<User[]>
    {
        public string ServiceName => "userService";
    }
}