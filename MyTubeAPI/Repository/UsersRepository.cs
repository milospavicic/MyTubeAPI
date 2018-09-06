using MyTubeAPI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TestProject.Models;

namespace MyTube.Repository
{
    public class UsersRepository : IUsersRepository
    {
        private MyDBContext db;
        public UsersRepository(MyDBContext db)
        {
            this.db = db;
        }
        public IEnumerable<User> GetAllUsers()
        {
            var queryString = "SELECT * FROM Users WHERE Deleted = 0";
            return db.Users.SqlQuery(queryString);
        }

        public IEnumerable<User> GetNTopSubbedUsers(int n)
        {
            var sqlString = String.Format("SELECT TOP {0} * FROM Users WHERE Deleted = 0 ORDER BY SubscribersCount DESC", n);
            return db.Users.SqlQuery(sqlString).ToList();
        }

        public bool Login(User user)
        {
            bool userExists = db.Users.Any(u => u.Username == user.Username && u.Pass == user.Pass && u.Deleted == false);
            return userExists;

        }

        public bool UsernameTaken(string username)
        {
            bool userExists = db.Users.Any(u => u.Username == username);
            return userExists;

        }

        public IEnumerable<User> GetUsersSubscribedBy(string username)
        {
            var usernameParam = String.Format("'{0}'", username);
            var queryString = String.Format("SELECT U.* FROM Users AS U INNER JOIN Subscribers AS S ON U.Username = S.ChannelSubscribedUsername WHERE S.SubscriberUsername = {0}", usernameParam);
            return db.Users.SqlQuery(queryString).ToList();
        }

        public IEnumerable<User> GetUsersSubscribedTo(string username)
        {
            var usernameParam = String.Format("'{0}'", username);
            var queryString = String.Format("SELECT U.* FROM Users AS U INNER JOIN Subscribers AS S ON U.Username = S.SubscriberUsername WHERE S.ChannelSubscribedUsername = {0}", usernameParam);
            return db.Users.SqlQuery(queryString).ToList();
        }
        public IEnumerable<User> SearchAndSortUsers(string searchString, string sortOrder)
        {
            var searchParam = String.Format("'%{0}%'", searchString);
            var queryString = String.Format("SELECT * FROM Users WHERE Deleted = 0 AND ( Username LIKE {0} OR Firstname LIKE {0} OR Lastname LIKE {0} OR Email LIKE {0} ) {1}", searchParam, SortUsersString(sortOrder));
            return db.Users.SqlQuery(queryString).ToList();
        }
        public string SortUsersString(string sortOrder)
        {
            switch (sortOrder)
            {
                case "username_asc":
                    return "ORDER BY Username ASC";
                case "username_desc":
                    return "ORDER BY Username DESC";
                case "firstname_asc":
                    return "ORDER BY Firstname ASC";
                case "firstname_desc":
                    return "ORDER BY Firstname DESC";
                case "lastname_asc":
                    return "ORDER BY Lastname ASC";
                case "lastname_desc":
                    return "ORDER BY Lastname DESC";
                case "email_asc":
                    return "ORDER BY Email ASC";
                case "email_desc":
                    return "ORDER BY Email DESC";
                case "user_type_asc":
                    return "ORDER BY UserType ASC";
                case "user_type_desc":
                    return "ORDER BY UserType DESC";
                case "status_asc":
                    return "ORDER BY Blocked ASC";
                case "status_desc":
                    return "ORDER BY Blocked DESC";

                default:
                    return "ORDER BY Username ASC";
            }
        }

        public User GetUserByUsername(string username)
        {
            try
            {
                return db.Users.Single(x => x.Username == username && x.Deleted == false);
            }
            catch
            {
                return null;
            }
        }

        public void InsertUser(User user)
        {
            if (user != null)
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }

        public void UpdateUser(User user)
        {
            if (user != null)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
            }

        }

        public void BlockUser(string username)
        {
            var found_user = GetUserByUsername(username);
            if (found_user != null)
            {
                found_user.Blocked = true;
                db.SaveChanges();
            }
        }
        public void UnblockUser(string username)
        {
            var found_user = GetUserByUsername(username);
            if (found_user != null)
            {
                found_user.Blocked = false;
                db.SaveChanges();
            }
        }
        public void DeleteUser(string username)
        {
            var found_user = GetUserByUsername(username);
            if (found_user != null)
            {
                found_user.Deleted = true;
                db.SaveChanges();
            }
        }

        public void Dispose()
        {
            db.Dispose();
        }

    }
}