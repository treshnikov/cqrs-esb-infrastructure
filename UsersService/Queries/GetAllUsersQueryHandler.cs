using System.Collections.Generic;
using CQRS;

namespace UsersService
{
    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, User[]>
    {
        public User[] Handle(GetAllUsersQuery query)
        {
            return new List<User>
            {
                new User()
                {
                    Id = "1",
                    Name = "Ivanov",
                    Permissions = new List<Permission>() {new Permission() {Id = "1", Name = "p1"}}.ToArray()
                },

                new User()
                {
                    Id = "2",
                    Name = "Petrov",
                    Permissions = new List<Permission>() {new Permission() {Id = "2", Name = "p2"}}.ToArray()
                }
            }.ToArray();
        }
    }
}