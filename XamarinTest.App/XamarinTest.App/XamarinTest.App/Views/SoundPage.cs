using Xamarin.Forms;

namespace XamarinTest.App.Views
{
    //声音
    public class SoundPage : ContentPage
    {

        private static SoundPage _inse;

        public static SoundPage GetInse()
        {
            return _inse ?? (_inse = new SoundPage());
        }
    }
}
