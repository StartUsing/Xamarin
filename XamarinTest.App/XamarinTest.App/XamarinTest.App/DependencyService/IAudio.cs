using System;

namespace XamarinTest.App
{
    //https://github.com/tkowalczyk/SimpleAudioForms/blob/cdca7727a16dd49cd7edca6321bd916e6a2eb199/SimpleAudioForms/Droid/Services/AudioService.cs
    public interface IAudio
    {
        void Play(string uri);

        /// <summary>
        /// 播放音频，并在播放完成时回调
        /// </summary>
        /// <param name="uri">地址</param>
        /// <param name="onCompletion">回调方法</param>
        /// <param name="onError"></param>
        void Play(string uri, Action onCompletion,Action onError);
        void Stop();

        bool IsPlaying { get; }
    }
}