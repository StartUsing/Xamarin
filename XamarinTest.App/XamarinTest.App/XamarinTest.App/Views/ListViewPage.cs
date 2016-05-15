using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace XamarinTest.App.Views
{
    public class ListViewPage : ContentPage
    {
        private static ListViewPage _inse;

        public static ListViewPage GetInse()
        {
            return _inse ?? (_inse = new ListViewPage());
        }

        private ObservableCollection<string> Items { get; set; } = new ObservableCollection<string>();

        private bool IsLoading { get; set; }

        private ListView _lv;

        private const int IncreaseNumber = 15;


        public ListViewPage()
        {
            InitializationListViewSource();
            AddToolbarItems(NewToolbarItems());
            Title = "ListViewPage";
            Content = NewListView();
            _inse = this;
        }

        public View NewListView()
        {
            //ToDO 为了使用下拉加载数据ListView的数据源必须为ObservableCollection<T>有且只能实例化一次
            //虽然没有List方便但是能这样实例化var Items=new ObservableCollection<T>(new List<T>());
            //使用这种数据源的好处就是不需要重新绑定.但是添加数据少了List中的AddRange();只能一个个Add();
            _lv = new ListView()
            {
                IsPullToRefreshEnabled = true, //是否启用下拉刷新
                BackgroundColor = Color.White,
                ItemsSource = Items
            };

            _lv.ItemSelected += (s, e) =>
           {
               if (!_lv.IsEnabled) return;
               _lv.IsEnabled = false;
               var item = e.SelectedItem as string;
               DependencyService.Get<IMakeTextShow>().MakeText_Short($"您选中了:{item}");
               _lv.IsEnabled = true;
           };

            _lv.ItemAppearing += (s, e) =>
           {
               if (_lv == null || Items.Count <= 1 || IsLoading) return;
               var item = e.Item as string;
               if (!string.IsNullOrWhiteSpace(item) && item == Items[Items.Count - 1])
               {
                   Task.Run(() =>
                   {
                       IsLoading = true;
                       Device.BeginInvokeOnMainThread(() =>
                       {
                           _inse.Title = "Loading...";
                       });
                       //模拟网络加载延时
                       Device.StartTimer(TimeSpan.FromSeconds(2), () =>
                       {
                           for (int i = 0; i < IncreaseNumber; i++)
                           {
                               Items.Add($"Item {Items.Count}");
                           }
                           Device.BeginInvokeOnMainThread(() =>
                           {
                               _inse.Title = "ListViewPage";
                           });
                           IsLoading = false;
                           return false;
                       });

                   });
               }
           };

            _lv.RefreshCommand = new Command(() =>
           {
               _lv.IsRefreshing = true;
               InitializationListViewSource();
               _lv.IsRefreshing = false;
               DependencyService.Get<IMakeTextShow>().MakeText_Short("刷新完成！");
           });

            return _lv;
        }

        public void InitializationListViewSource()
        {
            Items.Clear();
            var list = new List<string>();
            for (var i = 1; i < 16; i++)
            {
                list.Add($"Item{i}");
            }
            foreach (var val in list)
            {
                Items.Add(val);
            }
        }

        public ToolbarItem[] NewToolbarItems()
        {
            var topBtn = new ToolbarItem("回到顶层", "Top.png", () =>
            {
                if (_lv == null) return;
                if (Items.Count <= 1) return;
                _lv.ScrollTo(Items[0], ScrollToPosition.Start, true);
            }, ToolbarItemOrder.Primary);
            //因为IOS没有back键的原因,所以基本上所有的二级页面都需要手动添加返回按钮.为了更方便可以写为拓展方法详细见：Extensions->ToolbarItemsExtenssions
            var closeBtn = new ToolbarItem("关闭页面", "back.png", ClosePage, ToolbarItemOrder.Primary);
            return new[] { topBtn, closeBtn };
        }

        public void AddToolbarItems(ToolbarItem[] items)
        {
            foreach (var item in items)
            {
                ToolbarItems.Add(item);
            }
        }

        public void ClosePage()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();//关闭页面
        }
    }
}
