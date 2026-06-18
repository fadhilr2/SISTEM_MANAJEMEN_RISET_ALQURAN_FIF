using Lib.common;
using Lib.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.services
{
    public static class AuthService
    {
        public static bool Login(string email, string password)
        {
            User? user = UserService.SearchUser("email", email);

            if (user == null) return false;

            if (!user.Password.Equals(password)) return false;

            Session.Account = user;
            return true;
        }

        public static void Register(string email, string password)
        {
            User requester = new User
            {
                Email = email,
                Password = password,
                Role = "visitor",
            };

            DataContext.Requests.Add(requester);

            Console.WriteLine(Messages.Get("register.success"));
        }
    }
}
