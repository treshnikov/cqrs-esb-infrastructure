using System;
using System.Collections.Generic;
using System.Linq;
using CQRS;

namespace UsersService
{
    public class GetUsersWithPermissionsQueryHandler : IQueryHandler<GetUsersWithPermissionsQuery, User[]>
    {
        public User[] Handle(GetUsersWithPermissionsQuery query)
        {
            var res = new List<User>();
            var users = new GetAllUsersQueryHandler().Handle(new GetAllUsersQuery());
            foreach (var user in users)
            {
                foreach (var permission in query.Permissions)
                {
                    if (user.Permissions.Any(p => p.Name == permission))
                    {
                        if (!res.Contains(user))
                            res.Add(user);
                    }
                }
            }
            return res.ToArray();
        }
    }
}