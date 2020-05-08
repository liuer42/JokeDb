using Xamarin.Forms;

namespace JokeDb
{
    public class JokeListPageCS : ContentPage
    {
        ListView listView;

        public JokeListPageCS()
        {
            Title = "Todo";

            var toolbarItem = new ToolbarItem
            {
                Text = "+",
                IconImageSource = Device.RuntimePlatform == Device.iOS ? null : "plus.png"
            };
            toolbarItem.Clicked += async (sender, e) =>
            {
                await Navigation.PushAsync(new JokeItemPageCS
                {
                    BindingContext = new JokeItem()
                });
            };
            ToolbarItems.Add(toolbarItem);

            listView = new ListView
            {
                Margin = new Thickness(20),
                ItemTemplate = new DataTemplate(() =>
                {
                    var label = new Label
                    {
                        VerticalTextAlignment = TextAlignment.Center,
                        HorizontalOptions = LayoutOptions.StartAndExpand
                    };
                    label.SetBinding(Label.TextProperty, "Name");

                    var tick = new Image
                    {
                        Source = ImageSource.FromFile("check.png"),
                        HorizontalOptions = LayoutOptions.End
                    };
                    tick.SetBinding(VisualElement.IsVisibleProperty, "Done");

                    var stackLayout = new StackLayout
                    {
                        Margin = new Thickness(20, 0, 0, 0),
                        Orientation = StackOrientation.Horizontal,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        Children = { label, tick }
                    };

                    return new ViewCell { View = stackLayout };
                })
            };
            listView.ItemSelected += async (sender, e) =>
            {

                if (e.SelectedItem != null)
                {
                    await Navigation.PushAsync(new JokeItemPageCS
                    {
                        BindingContext = e.SelectedItem as JokeItem
                    });
                }
            };

            Content = listView;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            listView.ItemsSource = await App.Database.GetItemsAsync();
        }
    }
}
