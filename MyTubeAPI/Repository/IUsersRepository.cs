using System;
using System.Collections.Generic;
using TestProject.Models;

namespace MyTube.Repository
{
    interface IUsersRepository : IDisposable
    {
        IEnumerable<User> GetNTopSubbedUsers(int n);
        IEnumerable<User> GetUsersSubscribedBy(string username);
        IEnumerable<User> GetUsersSubscribedTo(string username);
        IEnumerable<User> SearchAndSortUsers(string searchString, string sortOrder);
        User GetUserByUsername(string username);
        void InsertUser(User user);
        void UpdateUser(User user);
        void BlockUser(string username);
        void UnblockUser(string username);
        void DeleteUser(string username);
        bool Login(User user);
        bool UsernameTaken(string username);
    }
}
