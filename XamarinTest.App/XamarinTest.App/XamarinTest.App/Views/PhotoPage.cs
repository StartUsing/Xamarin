using System.IO;
using Xamarin.Forms;
using XLabs.Platform.Services.Media;

namespace XamarinTest.App.Views
{
    public class PhotoPage : ContentPage
    {
        private static PhotoPage _inse;

        public static PhotoPage GetInse()
        {
            return _inse ?? (_inse = new PhotoPage());
        }

        private static readonly StackLayout Images = new StackLayout
        {
            Orientation = StackOrientation.Horizontal,
            HeightRequest = 80,
            Children = { }
        };

        private static readonly ScrollView Imgs = new ScrollView
        {
            Orientation = ScrollOrientation.Horizontal,
            Content = Images
        };

        public PhotoPage()
        {
            Title = "获得照片";
            ToolbarItems.AddBackButtion();
            Content = new StackLayout
            {
                Children =
                {
                    Imgs,
                    NewPhotographBtn(),
                    GetPhotographBtn(),
                }
            };
        }

        public View NewPhotographBtn()
        {
            var viewModel = new CameraViewModel();

            var btn = new Button { Text = "拍照" };

            btn.Clicked += async (s, e) =>
            {
                if (Device.OS == TargetPlatform.iOS)
                {
                    viewModel.TakePictureCommand.Execute(null);
                }
                else
                {
                    await DependencyService.Get<XLabs.Platform.Services.Media.IMediaPicker>().TakePhotoAsync(new CameraMediaStorageOptions()
                    {
                        DefaultCamera = CameraDevice.Front,
                        MaxPixelDimension = 400
                    }).ContinueWith(t =>
                    {
                        if (t.IsCanceled)
                        {

                        }
                        else
                        {
                            //重点是调用UI线程,不然没法显示图片
                            Device.BeginInvokeOnMainThread(() =>
                          {
                              Images.Children.Add(new Image
                              {
                                  Source = ImageSource.FromStream(() => t.Result.Source),
                                  Aspect = Aspect.AspectFit
                              });
                          });

                            #region UpLoad
                            //以下转换方便上传服务器
                            //byte[] bytes;
                            //using (var stream = t.Result.Source)
                            //{
                            //    stream.Seek(0, SeekOrigin.Begin);
                            //    bytes = new byte[stream.Length];
                            //    stream.Read(bytes, 0, (int)stream.Length);
                            //}
                            //var newBytes = DependencyService.Get<IImageService>().ResizeImage(bytes, 1024, 768);
                            #endregion
                        }
                    });
                }
            };

            return btn;
        }

        public View GetPhotographBtn()
        {
            var btn = new Button { Text = "从本机获取照片" };

            btn.Clicked += async (s, e) =>
            {
                await DependencyService.Get<IMediaPicker>().SelectPhotoAsync(new CameraMediaStorageOptions
                    {
                        DefaultCamera = CameraDevice.Front,
                        MaxPixelDimension = 400
                    }).ContinueWith(t =>
                    {
                        if (t.IsCanceled)
                        {

                        }
                        else
                        {
                            Device.BeginInvokeOnMainThread(() =>
                            {
                                Images.Children.Add(new Image
                                {
                                    Source = ImageSource.FromStream(() => t.Result.Source),
                                });
                            });
                        }
                    });
            };
            return btn;
        }
    }
}
