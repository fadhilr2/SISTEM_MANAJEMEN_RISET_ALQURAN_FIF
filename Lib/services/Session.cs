using Lib.models;

namespace Lib.services
{

    public class Session
    {
        private static Session? _instance;
        private static readonly object _lock = new object();

        public static Session Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new Session();
                        }
                    }
                }
                return _instance;
            }
        }

        private Session() { }

        public int Menu { get; set; }

        public User? Account { get; set; }

        public void Logout()
        {
            Account = null;
            Menu = 0;
        }
    }
}

