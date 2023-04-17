using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sat.Recruitment.Domain.Models;
using Sat.Recruitment.Infrastructure.Helpers;
using Sat.Recruitment.Infrastructure.Interfaces;

namespace Sat.Recruitment.Infrastructure.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(IUserDataReader userDataReader) : base(userDataReader)
        {
        }

        public async Task<User> Add(User user)
        {
            var users = await GetAll();

            if (user is null) throw new ArgumentException(Messages.UserShouldNotBeNull);

            var isUserDuplicated = IsUserDuplicated(users, user);

            if (isUserDuplicated) throw new ArgumentException($"{Messages.DuplicatedUser}{user.Name}");

           var createdUser = await base.Add(user);

            return createdUser;
        }

        private bool IsUserDuplicated(IEnumerable<User> users, User user) => users.Any(u =>
            u.Email == user.Email || u.Phone == user.Phone || u.Name == user.Name &&
            u.Address == user.Address);
    }
}