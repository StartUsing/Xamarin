using System;
using System.Collections.Generic;
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

    public class FactoryViewModel
    {
        public string Title { get; set; }

        public EventHandler Event { get; set; }

        public FactoryViewModel(string title, EventHandler e)
        {
            Title = title;
            Event = e;
        }
    }
}
