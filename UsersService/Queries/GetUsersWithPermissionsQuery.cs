using System.Collections.Generic;
using System.Linq;
using CQRS;
using CQRS.DAL;

namespace UsersService
{
    public class GetUsersWithPermissionsQuery : IQuery<User[]>
    {
        public string[] Permissions { get; set; }
        public string ServiceName => "rpc";
    }

    public class GetUsersWithPermissionsQueryHandler : IQueryHandler<GetUsersWithPermissionsQuery, User[]>
    {
        private readonly IRepository _repository;

        public GetUsersWithPermissionsQueryHandler(IRepository repository)
        {
            _repository = repository;
        }

        public User[] Handle(GetUsersWithPermissionsQuery query)
        {
            var res = new List<User>();
            var users = new GetAllUsersQueryHandler(_repository).Handle(new GetAllUsersQuery());
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