using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using XLabs;
using XLabs.Ioc;
using XLabs.Platform.Device;
using XLabs.Platform.Mvvm;
using XLabs.Platform.Services.IO;
using XLabs.Platform.Services.Media;

namespace XamarinTest.App
{
    public class WaveRecorderViewModel : XLabs.Forms.Mvvm.ViewModel
    {
        /// <summary>
        /// The file name（文件名称）
        /// </summary>
        private string _fileName;
        /// <summary>
        /// The sample rate（）
        /// </summary>
        private int _sampleRate;
        /// <summary>
        // Indicates if it is recording（表名如果是录音）
        /// </summary>
        private bool _isRecording;
        /// <summary>
        /// The audio stream(音频流)
        /// </summary>
        private readonly IAudioStream _audioStream;
        /// <summary>
        /// The recorder(录音)
        /// </summary>
        private readonly WaveRecorder _recorder;


        private Stream _stream;
        /// <summary>
        /// Initializes a new instance of the <see cref="WaveRecorderViewModel"/> class.
        /// </summary>
        public WaveRecorderViewModel()
        {


            var app = Resolver.Resolve<IXFormsApp>();

            //this.FileName = System.IO.Path.Combine(app.AppDataDirectory, "audiosample.wav");
            FileName = Device.OnPlatform(
               System.IO.Path.GetRandomFileName(),
                System.IO.Path.GetRandomFileName(),
                System.IO.Path.Combine(app.AppDataDirectory, System.IO.Path.GetRandomFileName())
                );

            var device = Resolver.Resolve<IDevice>();

            if (device != null)
            {
                _audioStream = device.Microphone;
                _recorder = new WaveRecorder();
            }
            SampleRate = _audioStream.SupportedSampleRates.Min();
            Record = new Command(
                () =>
                {
                    _audioStream.OnBroadcast += audioStream_OnBroadcast;
                    //_stream = new MemoryStream();
                    //this.audioStream.Start.Execute(this.SampleRate);
                    _recorder.StartRecorder(
                        _audioStream,
                        _stream = device.FileManager.OpenFile(FileName, FileMode.Create, FileAccess.Write),
                        SampleRate).ContinueWith(t =>
                        {
                            if (t.IsCompleted)
                            {
                                IsRecording = t.Result;
                                System.Diagnostics.Debug.WriteLine("Microphone recorder {0}.", IsRecording ? "was started" : "failed to start.");
                                Record.ChangeCanExecute();
                                Stop.ChangeCanExecute();
                            }
                            else if (t.IsFaulted)
                            {
                                _audioStream.OnBroadcast -= audioStream_OnBroadcast;
                            }
                        });
                },
                () => RecordingEnabled &&
                      _audioStream.SupportedSampleRates.Contains(SampleRate) &&
                      !IsRecording &&
                      device.FileManager != null
                );

            Stop = new Command(
                async () =>
                {
                    try
                    {
                        _audioStream.OnBroadcast -= audioStream_OnBroadcast;


                        await _recorder.StopRecorder();
                        //this.audioStream.Stop.Execute(this);   
                        if (OnStopped != null)
                            OnStopped(device.FileManager.OpenFile(FileName, FileMode.Open,
                                FileAccess.Read));
                        System.Diagnostics.Debug.WriteLine("Microphone recorder was stopped.");
                        Record.ChangeCanExecute();
                        Stop.ChangeCanExecute();
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                },
                () =>
                {
                    return IsRecording;
                }
                );
        }
        public Action<Stream> OnStopped { get; set; }
        /// <summary>
        /// Audioes the stream_ on broadcast.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The e.</param>
        private void audioStream_OnBroadcast(object sender, EventArgs<byte[]> e)
        {
            System.Diagnostics.Debug.WriteLine("Microphone recorded {0} bytes.", e.Value.Length);
        }

        /// <summary>
        /// Gets a value indicating whether [recording enabled].
        /// </summary>
        /// <value><c>true</c> if [recording enabled]; otherwise, <c>false</c>.</value>
        public bool RecordingEnabled
        {
            get
            {
                return (_audioStream != null);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is recording.
        /// </summary>
        /// <value><c>true</c> if this instance is recording; otherwise, <c>false</c>.</value>
        public bool IsRecording
        {
            get { return _isRecording; }
            set { SetProperty(ref _isRecording, value); }
        }

        /// <summary>
        /// Gets or sets the sample rate.
        /// </summary>
        /// <value>The sample rate.</value>
        public int SampleRate
        {
            get { return _sampleRate; }
            set { SetProperty(ref _sampleRate, value); }
        }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>The name of the file.</value>
        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        /// <summary>
        /// Gets the record.
        /// </summary>
        /// <value>The record.</value>
        public Command Record
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the stop.
        /// </summary>
        /// <value>The stop.</value>
        public Command Stop
        {
            get;
            private set;
        }
    }
}