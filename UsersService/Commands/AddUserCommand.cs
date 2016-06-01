using System.Collections.Generic;
using System.Linq;
using CQRS;
using CQRS.DAL;

namespace UsersService.Commands
{
    public class AddUserCommand : ICommand
    {
        public string ServiceName => "userService";
        public string Name { get; set; }
        public string[] Permissions { get; set; }
    }

    public class AddUserCommandHandler : ICommandHandler<AddUserCommand>
    {
        private readonly IRepository _repository;

        public AddUserCommandHandler(IRepository repository)
        {
            _repository = repository;
        }

        public void Handle(AddUserCommand command)
        {
            var users = new List<User>(_repository.Get<User>());
            var newUser = new User
            {
                Id = (users.Count + 1).ToString(),
                Name = command.Name,
                Permissions = command.Permissions.Select(i => new Permission {Name = i}).ToArray()
            };
            users.Add(newUser);
            _repository.Set(users.ToArray());

        }
    }
}