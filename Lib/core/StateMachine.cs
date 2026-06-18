using System;

namespace Lib.core
{
    public enum State
    {
        Welcome,
        Login,
        Register,
        Home,
        Admin,
        ViewPaper,
        Exit
    }

    public class StateMachine
    {
        public State Current { get; private set; } = State.Welcome;

        public void Set(State s) => Current = s;
    }
}
