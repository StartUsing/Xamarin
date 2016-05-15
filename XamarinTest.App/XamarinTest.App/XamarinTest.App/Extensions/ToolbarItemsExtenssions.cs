using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace XamarinTest.App
{
    public static class ToolbarItemsExtenssions
    {
        public static void AddBackButtion(this IList<ToolbarItem> items)
        {
            items.Add(new ToolbarItem("关闭", "back.png", () =>
            {
                Application.Current.MainPage.Navigation.PopModalAsync();
            }));
        }
    }
}
