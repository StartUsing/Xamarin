using System;

namespace XamarinTest.App
{
    //https://github.com/tkowalczyk/SimpleAudioForms/blob/cdca7727a16dd49cd7edca6321bd916e6a2eb199/SimpleAudioForms/Droid/Services/AudioService.cs
    public interface IAudio
    {
        void Play(string uri);

        /// <summary>
        /// ������Ƶ�����ڲ������ʱ�ص�
        /// </summary>
        /// <param name="uri">��ַ</param>
        /// <param name="onCompletion">�ص�����</param>
        /// <param name="onError"></param>
        void Play(string uri, Action onCompletion,Action onError);
        void Stop();

        bool IsPlaying { get; }
    }
}