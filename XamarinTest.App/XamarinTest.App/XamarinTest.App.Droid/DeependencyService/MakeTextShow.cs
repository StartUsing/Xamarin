using Android.Widget;
using Xamarin.Forms;

namespace XamarinTest.App.Droid.DeependencyService
{
    public class MakeTextShow : IMakeTextShow
    {
        public void MakeText_Short(string content)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(Forms.Context, content, ToastLength.Short).Show();
            });
        }

        public void MakeText_Long(string content)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Toast.MakeText(Forms.Context, content, ToastLength.Long).Show();
            });
        }
    }
}