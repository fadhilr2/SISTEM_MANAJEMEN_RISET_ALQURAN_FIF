using System;
using Lib.common;
using Lib.services;

namespace CLI.Views
{
    public static class ProfileViews
    {
        public static void PrintHomeView()
        {
            BaseViews.PrintSubheader("HOME PAGE");

            Console.WriteLine("1. View All Research");
            Console.WriteLine("2. View My Profile");
            if (Session.Instance.Account != null && Session.Instance.Account.Role.Equals("researcher", StringComparison.OrdinalIgnoreCase)) Console.WriteLine("3. Add New Research");
            Console.WriteLine("9. Log out");

            Session.Instance.Menu = InputReader.ReadInput<int>();

            Console.WriteLine();
        }

        public static void PrintProfileView()
        {
            BaseViews.PrintSubheader("MY PROFILE");

            if (Session.Instance.Account != null)
            {
                Console.WriteLine($"Name: {Session.Instance.Account.Name}");
                Console.WriteLine($"Email: {Session.Instance.Account.Email}");
                Console.WriteLine($"Role: {Session.Instance.Account.Role}");
            }

            Console.WriteLine();

            UserService.ShowProfileAction();

            Console.WriteLine();
        }

        public static void PrintAddResearchView()
        {
            BaseViews.PrintSubheader("ADD NEW RESEARCH");
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
    }
}
