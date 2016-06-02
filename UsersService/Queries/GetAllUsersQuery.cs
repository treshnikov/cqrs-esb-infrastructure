using System;
using CQRS;
using CQRS.DAL;

namespace UsersService
{
    public class GetAllUsersQuery : IQuery
    {
        public string ServiceName => "userService";
    }

    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, User[]>
    {
        private readonly IRepository _repository;

        public GetAllUsersQueryHandler(IRepository repository)
        {
            _repository = repository;
        }

        public User[] Handle(GetAllUsersQuery query)
        {
            return _repository.Get<User>();
        }
    }
}