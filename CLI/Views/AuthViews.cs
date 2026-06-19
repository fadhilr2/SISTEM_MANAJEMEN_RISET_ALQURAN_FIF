using System;
using Lib.common;
using Lib.services;

namespace CLI.Views
{
    public static class AuthViews
    {
        public static void PrintWelcomeView()
        {
            BaseViews.PrintSubheader(Messages.Get("welcome.title"));

            Console.WriteLine(Messages.Get("menu.login"));
            Console.WriteLine(Messages.Get("menu.register"));
            Console.WriteLine(Messages.Get("menu.exit"));

            Session.Menu = InputReader.ReadInput<int>();

            Console.WriteLine();
        }

        public static void PrintLoginView()
        {
            BaseViews.PrintSubheader("LOGIN");

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
            BaseViews.PrintSubheader("REGISTER");

            Console.WriteLine(Messages.Get("prompt.enter_name"));
            string name = InputReader.ReadInput<string>();
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

            AuthService.Register(name, email, password);

            Console.WriteLine();
        }
    }
}
