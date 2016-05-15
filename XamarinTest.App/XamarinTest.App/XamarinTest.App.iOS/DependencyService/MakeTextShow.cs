using XamarinTest.App.DependencyService;

namespace XamarinTest.App.iOS.DependencyService
{
    public class MakeTextShow : IMakeTextShow
    {
        public void MakeText_Long(string content)
        {
            try
            {
                ToastIOS.Toast.MakeText(content, 1500).Show();
            }
            catch
            {
            }
        }

        public void MakeText_Short(string content)
        {
            try
            {
                ToastIOS.Toast.MakeText(content, 500).Show();
            }
            catch
            {
            }
        }
    }
}
