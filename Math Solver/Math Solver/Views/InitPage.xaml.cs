using MarcTron.Plugin;
using Math_Solver.DAO;
using Math_Solver.Models;
using Math_Solver.Resources;
using Math_Solver.Services;
using Math_Solver.ViewModels;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Math_Solver.Views
{
    public partial class InitPage : ContentPage
    {
        public static string formulaDetail { get; set; }

        public InitPage()
        {
            Task.Delay(500);
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            DisplayInfo displaySize = DeviceDisplay.MainDisplayInfo;
            ad.WidthRequest = displaySize.Width;
            ad.HeightRequest = 150;
        }

        protected override void OnAppearing()
        {
            ShowFavorites(stackLayoutFavorites);
            ShowRecents();
            base.OnAppearing();
        }

        public void ShowFavorites(StackLayout stackLayout, bool isToShowRecents = false)
        {
            DisplayInfo screenSize = DeviceDisplay.MainDisplayInfo;

            stackLayout.Children.Clear();
            stackLayout.BackgroundColor = Color.WhiteSmoke;

            BoxView line = new BoxView() { BackgroundColor = Color.DarkGray, HeightRequest = 1, VerticalOptions = LayoutOptions.StartAndExpand, Margin = new Thickness(0,3,0,20) };

            Thickness marginFav = new Thickness(0,10,0,0);
            Label lblMain = new Label()
            {
                FontFamily = "SFUIDisplay-Regular",
                TextColor = Color.FromHex("444"),
                FontAttributes = FontAttributes.Bold,
                FontSize = 26,
                Padding = marginFav,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                LineBreakMode = LineBreakMode.WordWrap
            };
            if (isToShowRecents)
            {
                lblMain.Text = AppResources.recents;
                stackLayoutRecent.Children.Add(lblMain);
                stackLayoutRecent.Children.Add(line);
                ProcessList(true);
            }
            else
            {
                lblMain.Text = AppResources.favorites;
                stackLayoutFavorites.Children.Add(lblMain);
                stackLayoutFavorites.Children.Add(line);
                ProcessList();
            }
        }

        private void CreateCardButton(int id, string idName, string name, string area, StackLayout stack)
        {
            Thickness marginSubFrame = new Thickness(0, 0, 0, 0);
            Thickness paddingSubFrame = new Thickness(0);
            string categoryToShow = string.Empty;
            DatabaseAccess DAO = new DatabaseAccess();
            
            Frame frameMain = new Frame()
            {
                CornerRadius = 10,
                IsClippedToBounds = true,
                HasShadow = true,
                Padding = 0,
                BackgroundColor = Color.WhiteSmoke,
                Margin = 0
            };
            stack.Children.Add(frameMain);

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
                DAO.UpdateRecent(new Recents() {FormulaId = id, IsRecent = 1 });
                Navigation.PushAsync(new FormulaDetailPage(idName, name));
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

            StackLayout stackText = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };

            StackLayout stackTitle = new StackLayout() { Orientation = StackOrientation.Horizontal };
            Label lblTitle = new Label() { Text = name, TextColor = Color.DarkGray, FontSize = 30, FontFamily = "Helvetica", VerticalOptions = LayoutOptions.CenterAndExpand };
            stackTitle.Children.Add(lblTitle);
            stackText.Children.Add(stackTitle);
            gridFrame.Children.Add(stackText, 1, 0);
        }

        private void ProcessList(bool isToShowRecent = false)
        {
            DatabaseAccess DAO = new DatabaseAccess();

            if (!isToShowRecent)
            {
                List<Favorites> favoritesList = DAO.GetFavorites();

                if (favoritesList == null || favoritesList.Count == 0)
                    return;
                else
                {
                    foreach (var favorite in favoritesList)
                    {
                        var info = App.mathList.Where(x => x.Id == favorite.FormulaId).Select(fav => new { fav.Id, fav.IdName, fav.Name, fav.Area }).First();    
                        CreateCardButton(info.Id, info.IdName, info.Name, info.Area, stackLayoutFavorites);
                    }
                }
            }
            else
            {
                List<Recents> recentsList = DAO.GetRecents();

                if (recentsList == null || recentsList.Count == 0)
                    return;
                else
                {
                    if (recentsList.Count == 6)
                    {
                        DAO.LimitRecentsTable(recentsList);
                        recentsList = DAO.GetRecents();
                    }

                    foreach (var recent in recentsList)
                    {
                        var info = App.mathList.Where(x => x.Id == recent.FormulaId).Select(fav => new { fav.Id, fav.IdName, fav.Name, fav.Area }).First();    
                        CreateCardButton(info.Id, info.IdName, info.Name, info.Area, stackLayoutRecent);
                    }
                }
            }
        }

        private void ShowRecents()
        {
            ShowFavorites(stackLayoutRecent, true);
        }
    }
}