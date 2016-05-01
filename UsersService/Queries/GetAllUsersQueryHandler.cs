using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CQRS;
using CQRS.DAL;
using Newtonsoft.Json;

namespace UsersService
{
    public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, User[]>
    {
        private readonly IRepository _repository;

        public GetAllUsersQueryHandler(IRepository  repository)
        {
            _repository = repository;
        }

        public User[] Handle(GetAllUsersQuery query)
        {
            return _repository.Get<User>();
        }
    }
}