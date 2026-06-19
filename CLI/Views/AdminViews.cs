using System;
using System.Linq;
using Lib.common;
using Lib.models;
using Lib.services;

namespace CLI.Views
{
    public static class AdminViews
    {
        public static void PrintAdminView()
        {
            BaseViews.PrintSubheader("ADMIN PAGE");
            Console.WriteLine("1. View All Users");
            Console.WriteLine("2. View Registration Requests");
            Console.WriteLine("9. Log Out");

            Session.Instance.Menu = InputReader.ReadInput<int>();

            Console.WriteLine();
        }

        public static void PrintAllUsers()
        {
            BaseViews.PrintSubheader("ACTIVE USERS");

            foreach (User user in DataContext.Users.GetAll())
            {
                if (!user.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"-- [{user.Role}] {user.Name ?? user.Email}");
                }
            }

            Console.WriteLine();

            UserService.ShowAllUsersAction();

            Console.WriteLine();
        }

        public static void PrintRequestView()
        {
            BaseViews.PrintSubheader("REGISTRATION REQUESTS");

            if (!DataContext.Requests.GetAll().Any()) Console.WriteLine(Messages.Get("admin.no_requests"));

            foreach (User user in DataContext.Requests.GetAll())
            {
                Console.WriteLine(user.Email);
                Console.WriteLine("-----");
            }

            Console.WriteLine();

            UserService.ShowRequestsAction();

            Console.WriteLine();
        }
    }
}
