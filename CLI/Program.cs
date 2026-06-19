using System;
using Lib.core;
using Lib.services;
using Lib.models;
using CLI.Views;

namespace CLI
{
    internal static class Program
    {
        static void Main()
        {
            StateMachine stateMachine = new StateMachine();
            BaseViews.PrintHeader();

            while (stateMachine.Current != State.Exit)
            {
                switch (stateMachine.Current)
                {
                    case State.Welcome:
                        AuthViews.PrintWelcomeView();
                        if (Session.Instance.Menu == 1)
                        {
                            stateMachine.Set(State.Login);
                        }
                        else if (Session.Instance.Menu == 2)
                        {
                            stateMachine.Set(State.Register);
                        }
                        else if (Session.Instance.Menu == 0)
                        {
                            stateMachine.Set(State.Exit);
                        }
                        break;

                    case State.Login:
                        AuthViews.PrintLoginView();
                        if (Session.Instance.Account != null)
                        {
                            if (Session.Instance.Account.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                            {
                                stateMachine.Set(State.Admin);
                            }
                            else
                            {
                                stateMachine.Set(State.Home);
                            }
                        }
                        else
                        {
                            stateMachine.Set(State.Welcome);
                        }
                        break;

                    case State.Register:
                        AuthViews.PrintRegisterView();
                        stateMachine.Set(State.Welcome);
                        break;

                    case State.Home:
                        ProfileViews.PrintHomeView();
                        if (Session.Instance.Menu == 1)
                        {
                            ResearchViews.PrintAllResearch();
                        }
                        else if (Session.Instance.Menu == 2)
                        {
                            ProfileViews.PrintProfileView();
                        }
                        else if (Session.Instance.Menu == 3 && Session.Instance.Account != null && Session.Instance.Account.Role.Equals("researcher", StringComparison.OrdinalIgnoreCase))
                        {
                            ProfileViews.PrintAddResearchView();
                        }
                        else if (Session.Instance.Menu == 9)
                        {
                            Session.Instance.Logout();
                            stateMachine.Set(State.Welcome);
                        }
                        break;

                    case State.Admin:
                        AdminViews.PrintAdminView();
                        if (Session.Instance.Menu == 1)
                        {
                            AdminViews.PrintAllUsers();
                        }
                        else if (Session.Instance.Menu == 2)
                        {
                            AdminViews.PrintRequestView();
                        }
                        else if (Session.Instance.Menu == 9)
                        {
                            Session.Instance.Logout();
                            stateMachine.Set(State.Welcome);
                        }
                        break;

                    default:
                        stateMachine.Set(State.Exit);
                        break;
                }
            }
        }
    }
}
