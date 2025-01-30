using BLL.ModelsDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationUI.Statics
{
    public static class StaticUser
    {
        public static UserDTO User { get; set; }
        public static bool IsLoggedIn { get; set; } = false;
    }
}
