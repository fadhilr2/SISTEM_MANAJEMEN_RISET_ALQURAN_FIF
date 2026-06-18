using Lib.common;
using Lib.models;
using Lib.services;
using System;
using System.Collections.Generic;
using System.Text;
using static System.Collections.Specialized.BitVector32;

namespace CLI
{
    public class Views
    {
        public static void PrintHeader()
        {
            Console.WriteLine("====================================");
            Console.WriteLine("SISTEM MANAJEMEN HASIL RISET AL-QURAN FIF");
            Console.WriteLine("====================================");
            Console.WriteLine();
        }

        public static void PrintSubheader(string title)
        {
            Console.WriteLine(title);
            Console.WriteLine("------------------------------------");
        }

        public static void PrintWelcomeView()
        {
            PrintSubheader(Messages.Get("welcome.title"));

            Console.WriteLine(Messages.Get("menu.login"));
            Console.WriteLine(Messages.Get("menu.register"));
            Console.WriteLine(Messages.Get("menu.exit"));

            Session.Menu = InputReader.ReadInput<int>();

            Console.WriteLine();
        }

        public static void PrintLoginView()
        {
            PrintSubheader("LOGIN");

            Console.WriteLine(Messages.Get("prompt.enter_email"));
            string email = InputReader.ReadInput<string>();
            Console.WriteLine(Messages.Get("prompt.enter_password"));
            string password = InputReader.ReadInput<string>();

            bool isCorrect = AuthService.Login(email, password);
            if (isCorrect) Console.WriteLine(Messages.Get("login.success"));
            else Console.WriteLine(Messages.Get("login.failed"));

            Console.WriteLine();
        }

        public static void PrintRegisterView()
        {
            PrintSubheader("REGISTER");

            Console.WriteLine(Messages.Get("prompt.enter_email"));
            string email = InputReader.ReadInput<string>();
            Console.WriteLine(Messages.Get("prompt.enter_password"));
            string password = InputReader.ReadInput<string>();
            Console.WriteLine(Messages.Get("prompt.confirm_password"));
            string confirmPassword = InputReader.ReadInput<string>();

            if (!password.Equals(confirmPassword))
            {
                Console.WriteLine(Messages.Get("register.password_mismatch"));
                return;
            }

            Console.WriteLine();

            AuthService.Register(email, password);

            Console.WriteLine();
        }

        public static void PrintHomeView()
        {
            PrintSubheader("HOME PAGE");

            Console.WriteLine("1. View All Research");
            Console.WriteLine("2. View My Profile");
            if (Session.Account != null && Session.Account.Role.Equals("researcher", StringComparison.OrdinalIgnoreCase)) Console.WriteLine("3. Add New Research");
            Console.WriteLine("9. Log out");

            Session.Menu = InputReader.ReadInput<int>();

            Console.WriteLine();
        }

        public static void PrintAllResearch()
        {
            PrintSubheader("ALL RESEARCH");

            foreach (Paper paper in DataContext.Papers.GetAll())
            {
                Console.WriteLine(paper.Title);
                Console.WriteLine($"By: {paper.Author}");
                Console.WriteLine("-----");
            }

            Console.WriteLine();

            ShowAllResearchAction();

            Console.WriteLine();
        }

        public static void OpenResearch(Paper paper)
        {
            PrintSubheader(paper.Title);

            Console.WriteLine($"By: {paper.Author}");
            Console.WriteLine($"Published on: {paper.Date}");

            Console.WriteLine();

            Console.WriteLine("Abstract:");
            Console.WriteLine(paper.Paper_Abstract);

            Console.WriteLine();

            ShowResearchAction(paper);

            Console.WriteLine();
        }

        public static void ShowAllResearchAction()
        {
            Console.WriteLine("1. Open Research");
            Console.WriteLine("0. Back");
            int input = InputReader.ReadInput<int>();

            switch (input)
            {
                case 1:
                    Console.WriteLine(Messages.Get("prompt.enter_title"));
                    string title = InputReader.ReadInput<string>();

                    Console.WriteLine();

                    Paper? paper = PaperService.SearchPaper(title);
                    if (paper == null)
                    {
                        Console.WriteLine(Messages.Get("paper.not_found"));
                        break;
                    }

                    OpenResearch(paper);
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine("Invalid selection. Please choose 0 or 1.");
                    break;
            }
        }

        public static void ShowResearchAction(Paper paper)
        {
            Console.WriteLine("1. View Researcher's Profile");

            bool canEdit = PaperService.CanEdit(paper);
            if (canEdit)
            {
                Console.WriteLine("2. Edit Title");
                Console.WriteLine("3. Edit Abstract");
                Console.WriteLine("4. Delete");
            }
            Console.WriteLine("0. Back");
            int input = InputReader.ReadInput<int>();

            Console.WriteLine();

            switch (input)
            {
                case 1:
                    OpenResearcherProfile(paper.Author);
                    break;

                case 2:
                    if (!canEdit)
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                        break;
                    }

                    Console.WriteLine(Messages.Get("prompt.enter_title"));
                    string newTitle = InputReader.ReadInput<string>();
                    if (PaperService.EditTitle(paper, newTitle))
                    {
                        Console.WriteLine("~Title changed successfully");
                    }
                    else
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                    }
                    break;

                case 3:
                    if (!canEdit)
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                        break;
                    }

                    Console.WriteLine(Messages.Get("prompt.enter_abstract"));
                    string newAbstract = InputReader.ReadInput<string>();
                    if (PaperService.EditAbstract(paper, newAbstract))
                    {
                        Console.WriteLine("~Abstract changed successfully");
                    }
                    else
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                    }
                    break;

                case 4:
                    if (!canEdit)
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                        break;
                    }

                    if (PaperService.DeletePaper(paper))
                    {
                        Console.WriteLine(Messages.Get("confirm.deleted"));
                    }
                    else
                    {
                        Console.WriteLine(Messages.Get("permission.denied"));
                    }
                    break;

                case 0:
                    break;

                default:
                    Console.WriteLine(Messages.Get("error.invalid_selection"));
                    break;
            }
        }

        public static void OpenResearcherProfile(string name)
        {
            User? researcher = UserService.SearchUser("name", name);
            if (researcher == null)
            {
                Console.WriteLine(Messages.Get("paper.not_found"));
                return;
            }

            PrintSubheader(researcher.Name);

            foreach (Paper paper in DataContext.Papers.GetAll())
            {
                if (paper.Author.Equals(researcher.Name, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine(paper.Title);
                    Console.WriteLine($"By: {paper.Author}");
                    Console.WriteLine("-----");
                }
            }

            Console.WriteLine();
        }

        public static void PrintAddResearchView()
        {
            PrintSubheader("ADD NEW RESEARCH");
            Console.WriteLine(Messages.Get("prompt.enter_title"));
            string title = InputReader.ReadInput<string>();
            Console.WriteLine(Messages.Get("prompt.enter_abstract"));
            string paperAbstract = InputReader.ReadInput<string>();

            bool success = PaperService.UploadPaper(title, paperAbstract);
            if (success)
            {
                Console.WriteLine(Messages.Get("confirm.uploaded"));
            }
            else
            {
                Console.WriteLine(Messages.Get("permission.denied"));
            }

            Console.WriteLine();
        }

        public static void PrintProfileView()
        {
            PrintSubheader("MY PROFILE");

            Console.WriteLine($"Name: {Session.Account.Name}");
            Console.WriteLine($"Email: {Session.Account.Email}");
            Console.WriteLine($"Role: {Session.Account.Role}");

            Console.WriteLine();

            UserService.ShowProfileAction();

            Console.WriteLine();
        }

        public static void PrintAdminView()
        {
            PrintSubheader("ADMIN PAGE");
            Console.WriteLine("1. View All Users");
            Console.WriteLine("2. View Registration Requests");
            Console.WriteLine("9. Log Out");

            Session.Menu = InputReader.ReadInput<int>();

            Console.WriteLine();
        }

        public static void PrintAllUsers()
        {
            PrintSubheader("ACTIVE USERS");

            foreach (User user in DataContext.Users.GetAll())
            {
                if (!user.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine($"-- [{user.Role}] {user.Name}");
                }
            }

            Console.WriteLine();

            UserService.ShowAllUsersAction();

            Console.WriteLine();
        }

        public static void PrintRequestView()
        {
            PrintSubheader("REGISTRATION REQUESTS");

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
