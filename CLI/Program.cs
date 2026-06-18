using System;
using Lib.core;
using Lib.services;
using Lib.models;

namespace CLI
{
    internal static class Program
    {
        static void Main()
        {
            StateMachine stateMachine = new StateMachine();
            Views.PrintHeader();

            while (stateMachine.Current != State.Exit)
            {
                switch (stateMachine.Current)
                {
                    case State.Welcome:
                        Views.PrintWelcomeView();
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
                        Views.PrintLoginView();
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
                        Views.PrintRegisterView();
                        stateMachine.Set(State.Welcome);
                        break;

                    case State.Home:
                        Views.PrintHomeView();
                        if (Session.Menu == 1)
                        {
                            Views.PrintAllResearch();
                        }
                        else if (Session.Menu == 2)
                        {
                            Views.PrintProfileView();
                        }
                        else if (Session.Menu == 3 && Session.Account != null && Session.Account.Role.Equals("researcher", StringComparison.OrdinalIgnoreCase))
                        {
                            Views.PrintAddResearchView();
                        }
                        else if (Session.Menu == 9)
                        {
                            Session.Account = null;
                            stateMachine.Set(State.Welcome);
                        }
                        break;

                    case State.Admin:
                        Views.PrintAdminView();
                        if (Session.Menu == 1)
                        {
                            Views.PrintAllUsers();
                        }
                        else if (Session.Menu == 2)
                        {
                            Views.PrintRequestView();
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
