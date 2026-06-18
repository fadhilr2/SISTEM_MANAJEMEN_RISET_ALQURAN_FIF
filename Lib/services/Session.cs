using Lib.models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lib.services
{
    public static class Session
    {
        public static int Menu { get; set; }
        public static User? Account { get; set; }
    }
}
