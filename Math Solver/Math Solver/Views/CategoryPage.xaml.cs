using FFImageLoading.Forms;
using Math_Solver.DAO;
using Math_Solver.Services;
using Math_Solver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Math_Solver.App;

namespace Math_Solver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage
    {
        public static string categoryToShow { get; set; }

        public CategoryPage()
        {
            InitializeComponent();
            InflateCardButton();
            NavigationPage.SetHasNavigationBar(this, false);
            DisplayInfo displaySize = DeviceDisplay.MainDisplayInfo;
            ad.WidthRequest = displaySize.Width;
            ad.HeightRequest = 150;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private void InflateCardButton()
        {
            var categories = mathList.Where(category => category.Tag == "Group").OrderBy(x => x.Name);
            foreach (var category in categories)
            {
                CreateCardButton(category.Area, category.Name, category.Desc, category.Id, category.IdName);
            }
        }

        private void CreateCardButton(string area, string title, string desc, int id, string idName)
        {
            Utils.Utils utils = new Utils.Utils();
            Thickness thickness = new Thickness(10);
            Thickness marginSubFrame = new Thickness(0, 0, 0, 0);
            Thickness paddingSubFrame = new Thickness(0);

            Frame frameMain = new Frame()
            {
                CornerRadius = 10,
                IsClippedToBounds = true,
                HasShadow = true,
                Padding = 0,
                BackgroundColor = Color.WhiteSmoke,
                Margin = 10
            };
            stackLayoutCategory.Children.Add(frameMain);

            StackLayout stackFrame = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal
            };
            frameMain.Content = stackFrame;

            Frame cardMainFrame = new Frame()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = marginSubFrame,
                HasShadow = false,
                Padding = paddingSubFrame,
                AutomationId = area
            };

            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) => {
                categoryToShow = cardMainFrame.AutomationId;
                Navigation.PushAsync(new CategoryListPage(title));
            };

            cardMainFrame.GestureRecognizers.Add(tapGesture);
            stackFrame.Children.Add(cardMainFrame);

            Grid gridFrame = new Grid()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Start,
                Padding = paddingSubFrame,
                Margin = marginSubFrame,
                BackgroundColor = Color.Transparent,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = GridLength.Auto },
                    new ColumnDefinition { Width = GridLength.Auto },
                }
            };
            cardMainFrame.Content = gridFrame;

            StackLayout stackImage = new StackLayout()
            {
                VerticalOptions = LayoutOptions.CenterAndExpand,
                Margin = marginSubFrame
            };

            Frame frameImage = new Frame() { HasShadow = false };
           
            var urlNotationList = mathList.Where(tag => tag.Tag == "Group").Select(URLs => new { URLs.Id, URLs.UrlImage });
            var specificUri = urlNotationList.Where(x => x.Id == id).Select(y => y.UrlImage).ToList();


            var img = new CachedImage()
            {
                Aspect = Aspect.Fill,
                WidthRequest = 90,
                HeightRequest = 90,
                CacheDuration = TimeSpan.FromDays(1),
                DownsampleToViewSize = true,
                RetryCount = 2,
                RetryDelay = 250,
                BitmapOptimizations = false,
                LoadingPlaceholder = utils.GetBase64Image("loading"),
                ErrorPlaceholder = utils.GetBase64Image("not_found"),
                Source = utils.GetBase64Image(idName)
            };
            frameImage.Content = img;
            stackImage.Children.Add(frameImage);
            gridFrame.Children.Add(stackImage, 0, 0);

            StackLayout stackText = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            StackLayout stackTitle = new StackLayout() { Orientation = StackOrientation.Horizontal };
            Label lblTitle = new Label() { Text = title, TextColor = Color.DarkGray, FontFamily = "SFUIDisplay-Regular", FontSize = 30, VerticalOptions = LayoutOptions.CenterAndExpand };
            stackTitle.Children.Add(lblTitle);
            stackText.Children.Add(stackTitle);

            gridFrame.Children.Add(stackText, 1, 0);
        }
    }
}