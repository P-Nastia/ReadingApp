using BLL.ModelsDTO;

namespace ApplicationUI.Statics
{
    public static class StaticUser
    {
        public static UserDTO User { get; set; }
        public static bool IsLoggedIn { get; set; } = false;
        public static bool UserNeedsToSignUp { get; set; } = false;
    }
}
