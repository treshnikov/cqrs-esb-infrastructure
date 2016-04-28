using System;
using CQRS;

namespace UsersService
{
    public class GetUsersWithPermissionsQueryHandler : IQueryHandler<GetUsersWithPermissionsQuery, User[]>
    {
        public User[] Handle(GetUsersWithPermissionsQuery query)
        {
            return new User[0] {};
        }
    }
}