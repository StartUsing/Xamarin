using System;
using System.Collections.Generic;
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
                        BtnFactory(new FactoryViewModel("ListView",async (s,e)=>{ await Application.Current.MainPage.Navigation.PushModalAsync( new NavigationPage(ListViewPage.GetInse()));})),
                        //BtnFactory(new FactoryViewModel("SaveAndLoad",async (s,e)=>{await Application.Current.MainPage.Navigation.PushModalAsync( new NavigationPage(ListViewPage.GetInse()));})),
                    }
                }
            };
        }

        public View BtnFactory(FactoryViewModel model)
        {
            var btn = new Button() { Text = model.Title };
            btn.Clicked += model.Event;
            return btn;
        }
    }

    public class FactoryViewModel<T>
    {
        public string Title { get; set; }

        public EventHandler Event { get; set; }

        public FactoryViewModel(string title)
        {
            Title = title;
            Event += async (s, e) =>
            {
                Type tx = typeof(T);
                var mf = tx.GetRuntimeMethod("GetInse", null);
                var x = (T)mf.Invoke(null, null);
                await Application.Current.MainPage.Navigation.PushModalAsync(new NavigationPage(x));
            };
        }
    }
}
