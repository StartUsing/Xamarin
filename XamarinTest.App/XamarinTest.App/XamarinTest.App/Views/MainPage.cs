using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Xamarin.Forms;
using XLabs.Forms.Controls;

namespace XamarinTest.App.Views
{
    public class MainPage : ContentPage
    {

        public MainPage()
        {
            Title = "Hello Xamarin";

            Content = new PopupLayout
            {
                BackgroundColor = Color.White,
                Content = new StackLayout
                {
                    Children =
                    {
                        BtnFactory(new FactoryViewModel<ListViewPage>("ListView")),
                        //BtnFactory(new FactoryViewModel("SaveAndLoad",async (s,e)=>{await Application.Current.MainPage.Navigation.PushModalAsync( new NavigationPage(ListViewPage.GetInse()));})),
                    }
                }
            };
        }

        public View BtnFactory<T>(FactoryViewModel<T> model) where T : ContentPage
        {
            var btn = new Button() { Text = model.Title };
            btn.Clicked += model.Event;
            return btn;
        }
    }

    public class FactoryViewModel<T>where T: ContentPage
    { 
        public string Title { get; set; }

        public EventHandler Event { get; set; }

        public FactoryViewModel(string title)
        {
            Title = title;
            Event +=  async(s, e) =>
            {
                var tx = typeof(T);
                var test = tx.GetTypeInfo().GetDeclaredMethod("GetInse");
                var x = (T)test.Invoke(null, null);
                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(x));
            };
        }
    }
}
