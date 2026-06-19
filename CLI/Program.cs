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
                        if (Session.Menu == 1)
                        {
                            stateMachine.Set(State.Login);
                        }
                        else if (Session.Menu == 2)
                        {
                            stateMachine.Set(State.Register);
                        }
                        else if (Session.Menu == 0)
                        {
                            stateMachine.Set(State.Exit);
                        }
                        break;

                    case State.Login:
                        AuthViews.PrintLoginView();
                        if (Session.Account != null)
                        {
                            if (Session.Account.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
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
                        if (Session.Menu == 1)
                        {
                            ResearchViews.PrintAllResearch();
                        }
                        else if (Session.Menu == 2)
                        {
                            ProfileViews.PrintProfileView();
                        }
                        else if (Session.Menu == 3 && Session.Account != null && Session.Account.Role.Equals("researcher", StringComparison.OrdinalIgnoreCase))
                        {
                            ProfileViews.PrintAddResearchView();
                        }
                        else if (Session.Menu == 9)
                        {
                            Session.Account = null;
                            stateMachine.Set(State.Welcome);
                        }
                        break;

                    case State.Admin:
                        AdminViews.PrintAdminView();
                        if (Session.Menu == 1)
                        {
                            AdminViews.PrintAllUsers();
                        }
                        else if (Session.Menu == 2)
                        {
                            AdminViews.PrintRequestView();
                        }
                        else if (Session.Menu == 9)
                        {
                            Session.Account = null;
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
