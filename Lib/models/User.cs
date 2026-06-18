using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.models
{
    public class User
    {
        public string? Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }

        public User() { }
    }
}
