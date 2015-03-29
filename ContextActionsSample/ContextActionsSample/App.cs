using System;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;

namespace ContextActionsSample
{
    public class App : Application
    {
        public App()
        {
            MainPage = new MyPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }

    class MyPage : ContentPage {
        private ObservableCollection<String> _ar = new ObservableCollection<string>(Enumerable.Range(0, 50).Select(n => "item-" + n));
        public MyPage()
        {
            var listView = new ListView{
                //ItemsSource = Enumerable.Range(0, 50).Select(n => "item-" + n),
                ItemsSource = _ar,
                ItemTemplate = new DataTemplate(() => new MyCell(this)),
            };
            Padding = new Thickness(0, Device.OnPlatform(20, 0, 0), 0, 0);
            Content = listView;
        }

        public async void Action(MenuItem item) {
            var text = item.CommandParameter.ToString();
            if (item.Text == "Add") {
                _ar.Insert(_ar.IndexOf(text) + 1, text + "-Add");
            }
            else if (item.Text == "Delete")
            {
                _ar.RemoveAt(_ar.IndexOf(text));
                
            }
        }

    }

    class MyCell : ViewCell
    {
        public MyCell(MyPage myPage)
        {

            //テキスト表示用のラベル
            var label = new Label{
                VerticalOptions = LayoutOptions.CenterAndExpand,
            };
            label.SetBinding(Label.TextProperty, new Binding("."));//ItemSourceのテキストを表示する

            //１つ目のメニュー
            var actionDelete = new MenuItem{
                Text = "Delete",
                //Command = new Command(p => myPage.DisplayAlert("Delete",p.ToString(),"OK")),//Commandにセットすることでイベントを取得
                IsDestructive = true, //背景赤色
            };
            actionDelete.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));//コマンド実行時のパラメータに表示テキストを使用する
            ContextActions.Add(actionDelete);

            //２つ目のメニュー
            var actionAdd = new MenuItem{
                Text = "Add",
            };
            actionAdd.SetBinding(MenuItem.CommandParameterProperty, new Binding("."));//コマンド実行時のパラメータに表示テキストを使用する
            //actionAdd.Clicked += async (s, e) => { //Clickedによりイベントを取得
            //    var itemMenu = ((MenuItem) s);
            //    await myPage.DisplayAlert(itemMenu.Text, (string)itemMenu.CommandParameter, "OK");
            //};
            ContextActions.Add(actionAdd);

            actionAdd.Clicked+=(s, a) => myPage.Action((MenuItem)s);
            actionDelete.Clicked += (s, a) => myPage.Action((MenuItem)s); 

            View = new StackLayout
            {
                Padding = 10,
                Children = { label }
            };
        }
    }
}
