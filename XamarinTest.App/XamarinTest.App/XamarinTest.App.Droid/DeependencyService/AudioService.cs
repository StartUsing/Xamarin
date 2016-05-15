using System;
using System.Threading.Tasks;
using Android.Media;
using Xamarin.Forms;

namespace XamarinTest.App.Droid.DeependencyService
{
    public class AudioService : IAudio
    {
        public AudioService() { }

        private MediaPlayer _mediaPlayer;

        public void Play(string uri)
        {
            Play(uri, () => { }, () => { });
        }

        public void Play(string uri, Action onCompletion,Action onError)
        {

            Device.BeginInvokeOnMainThread(() =>
            {
                try
                {
                    Android.Net.Uri url = Android.Net.Uri.Parse(uri);
                    Stop();
                    _mediaPlayer = MediaPlayer.Create(global::Android.App.Application.Context, url);
                    _mediaPlayer.Completion += (s, e) =>
                    {
                        if (onCompletion != null)
                            onCompletion();
                    };
                    _mediaPlayer.Error += (s, e) =>
                    {
                        if (onError != null)
                            onError();
                    };
                    _mediaPlayer.Start();

                }
                catch
                {
                    if (onError != null)
                        onError();
                }
            });
        }

        public  void Stop()
        {
           
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Stop();
            }
        }

        public bool IsPlaying
        {
            get { return _mediaPlayer!=null && _mediaPlayer.IsPlaying; }
        }

 
    }
}