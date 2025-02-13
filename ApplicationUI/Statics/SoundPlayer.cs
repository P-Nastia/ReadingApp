using System.Windows.Media;
using System.Windows.Threading;

namespace ApplicationUI.Statics;

public static class SoundPlayer
{
    private static readonly MediaPlayer _player = new MediaPlayer();
    public static async Task PlayButtonSoundAsync()
    {
        await Dispatcher.CurrentDispatcher.InvokeAsync(async () =>
        {
            _player.Open(new Uri(SoundConstants.ButtonClick, UriKind.Absolute));
            _player.Volume = 1;
            _player.Play();
            await Task.Delay(100);
        });
    }
}
