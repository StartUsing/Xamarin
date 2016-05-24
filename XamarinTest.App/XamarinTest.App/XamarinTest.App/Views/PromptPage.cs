using Xamarin.Forms;

namespace XamarinTest.App.Views
{
    public class PromptPage : ContentPage
    {
        //Notifications下次更新
        private static PromptPage _inse;

        public static PromptPage GetInse()
        {
            return _inse ?? (_inse = new PromptPage());
        }


        public PromptPage()
        {
            Title = "提示";
            ToolbarItems.AddBackButtion();
            Content = new StackLayout
            {
                Children =
                {
                    NewMakeTextBtn()
                }
            };
        }

        public View NewMakeTextBtn()
        {
            var btn = new Button
            {
                Text = "点我提示"
            };

            btn.Clicked += (s, e) =>
            {
                DependencyService.Get<IMakeTextShow>().MakeText_Short("提示");
            };

            return btn;
        }
    }
}
