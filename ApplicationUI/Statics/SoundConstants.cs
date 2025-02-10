using System.IO;

namespace ApplicationUI.Statics;

public static class SoundConstants
{
    public static readonly string ButtonClick = Path.Combine(
        GetApplicationRoot(), "Sounds", "Button.wav"
    );

    private static string GetApplicationRoot()
    {
        string currentDir = AppDomain.CurrentDomain.BaseDirectory;

        // Піднімаємося вгору до ApplicationUI
        DirectoryInfo dir = new DirectoryInfo(currentDir);
        while (dir != null && dir.Name != "ApplicationUI")
        {
            dir = dir.Parent;
        }

        return dir?.FullName;
    }

}

