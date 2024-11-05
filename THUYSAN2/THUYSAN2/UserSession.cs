using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THUYSAN2
{
    public static class UserSession
    {
        public static string CurrentUser { get; set; }
        public static string CurrentRole { get; set; }
        public static string CurrentSessionID { get; set; }
        public static string UserRole { get; set; }
        public static string SessionID { get; set; }

    }
}
