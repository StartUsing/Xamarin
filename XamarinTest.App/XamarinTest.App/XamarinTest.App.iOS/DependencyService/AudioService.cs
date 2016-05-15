using System;

namespace XamarinTest.App.iOS.DependencyService
{
    public class AudioService : IAudio
    {
        public AudioService() { }

        

        public void Play(string uri)
        {
          //  Play(uri, () => { });
        }

        public void Play(string uri, Action onCompletion, Action onError)
        {
            throw new NotImplementedException();
        }

        public  void Stop()
        {
       
        }

        public bool IsPlaying
        {
            get { return false; }
        }

 
    }
}