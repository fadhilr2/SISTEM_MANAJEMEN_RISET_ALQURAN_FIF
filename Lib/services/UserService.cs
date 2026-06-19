using Lib.common;
using Lib.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.services
{
    public static class UserService
    {
        public static User? SearchUser(string field, string value)
        {
            return field switch
            {
                "email" => DataContext.Users.Find(user => value.Equals(user.Email, StringComparison.OrdinalIgnoreCase)),
                "name" => DataContext.Users.Find(user => value.Equals(user.Name, StringComparison.OrdinalIgnoreCase)),
                _ => null,
            };
        }

        public static void ShowProfileAction()
        {
            Console.WriteLine("1. Edit Name");
            Console.WriteLine("2. Edit Email");
            Console.WriteLine("0. Back");

            int input = InputReader.ReadInput<int>();

            switch (input)
            {
                case 1:
                    Console.WriteLine("Enter new name:");
                    string name = InputReader.ReadInput<string>();

                    User? user = SearchUser("email", Session.Instance.Account.Email);
                    if (user == null)
                    {
                        Console.WriteLine("Current user not found.");
                        break;
                    }

                    string oldName = user.Name;
                    user.Name = name;
                    Session.Instance.Account.Name = name;

                    if (oldName != null && !oldName.Equals(name, StringComparison.OrdinalIgnoreCase))
                    {
                        foreach (var paper in DataContext.Papers.GetAll())
                        {
                            if (paper.Author.Equals(oldName, StringComparison.OrdinalIgnoreCase))
                            {
                                paper.Author = name;
                            }
                        }
                    }

                    Console.WriteLine("~Name edited");
                    break;

                case 2:
                    Console.WriteLine("Enter new email:");
                    string email = InputReader.ReadInput<string>();

                    User? user2 = SearchUser("email", Session.Instance.Account.Email);
                    if (user2 == null)
                    {
                        Console.WriteLine("Current user not found.");
                        break;
                    }

                    user2.Email = email;
                    Session.Instance.Account.Email = email;

                    Console.WriteLine("~Email edited");
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine(Messages.Get("error.invalid_selection"));
                    break;
            }
        }

        public static void ShowAllUsersAction()
        {
            Console.WriteLine("1. Change Role");
            Console.WriteLine("0. Back");

            int input = InputReader.ReadInput<int>();

            switch (input)
            {
                case 1:
                    Console.WriteLine("Enter User Name:");
                    string name = InputReader.ReadInput<string>();

                    User? user = SearchUser("name", name);
                    if (user == null)
                    {
                        Console.WriteLine("User not found.");
                        break;
                    }

                    if (user.Role.Equals("visitor", StringComparison.OrdinalIgnoreCase)) user.Role = "researcher";
                    else if (user.Role.Equals("researcher", StringComparison.OrdinalIgnoreCase)) user.Role = "visitor";

                    Console.WriteLine("~Role updated");
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine(Messages.Get("error.invalid_selection"));
                    break;
            }
        }

        public static void ShowRequestsAction()
        {
            Console.WriteLine("1. Accept Request");
            Console.WriteLine("0. Back");

            int input = InputReader.ReadInput<int>();

            switch (input)
            {
                case 1:
                    Console.WriteLine("Enter User Email:");
                    string email = InputReader.ReadInput<string>();

                    User? requester = null;

                    foreach (User user in DataContext.Requests.GetAll())
                    {
                        if (user.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
                        {
                            requester = user;
                            break;
                        }
                    }

                    if (requester == null)
                    {
                        Console.WriteLine("Request not found.");
                        break;
                    }

                    DataContext.Users.Add(requester);
                    DataContext.Requests.Remove(requester);

                    Console.WriteLine("~Request accepted");
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine(Messages.Get("error.invalid_selection"));
                    break;
            }
        }
    }
}
