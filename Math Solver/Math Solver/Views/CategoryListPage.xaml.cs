using Math_Solver.ViewModels;
using Math_Solver.DAO;
using System.ComponentModel;
using static Math_Solver.App;
using Xamarin.Forms;
using System.Linq;
using System;
using System.Collections.Generic;
using Math_Solver.Models;

namespace Math_Solver.Views
{
    public partial class CategoryListPage : ContentPage
    {
        public static IEnumerable<Formula> filteredFormulas = mathList;
        public static string formulaDetail { get; set; }
        private DatabaseAccess DAO = new DatabaseAccess();
        private List<Favorites> fav;

        private string _toolbarText;
        public string ToolbarText { get => _toolbarText; set { _toolbarText = value; OnPropertyChanged(); }}

        public CategoryListPage(string title)
        {
            InitializeComponent();
            fav = DAO.GetFavorites();
            InflateLabels();
            ToolbarText = title;
        }

        public void InflateLabels()
        {
            Utils.Utils utils = new Utils.Utils();
            Thickness gridMargin = new Thickness(0, 0 , 0, 10);
            filteredFormulas = mathList.Where(formula => formula.Area == CategoryPage.categoryToShow).Where(tag => tag.Tag == "Calc").OrderBy(x => x.Name);
            foreach (var formula in filteredFormulas)
            {
                Frame frameLines = new Frame() { CornerRadius = 5, Background = LinearGradientBrush.WhiteSmoke, HasShadow = true };
                StackLayout stackLines = new StackLayout() { BackgroundColor = Color.Transparent };
                
                var tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, e) => {
                    DAO.UpdateRecent(new Recents() {FormulaId = formula.Id, IsRecent = 1 });
                    Navigation.PushAsync(new FormulaDetailPage(formula.IdName, formula.Name));
                };

                frameLines.GestureRecognizers.Add(tapGesture);

                Frame frameTitle = new Frame() { Padding = 3, VerticalOptions = LayoutOptions.StartAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand, BackgroundColor = Color.Transparent, CornerRadius = 10 };
                Frame frameDesc = new Frame() { Padding = 3, VerticalOptions = LayoutOptions.EndAndExpand, HorizontalOptions = LayoutOptions.StartAndExpand, BackgroundColor = Color.Transparent, CornerRadius = 10 };
                Label lblTitle = new Label() { Text = formula.Name, FontFamily = "SFUIDisplay-Regular", TextColor = Color.FromHex("999999"), FontSize = 25, VerticalOptions = LayoutOptions.CenterAndExpand };
                ImageButton imgFavorite = new ImageButton() { BackgroundColor = Color.Transparent, VerticalOptions = LayoutOptions.CenterAndExpand, HorizontalOptions = LayoutOptions.End };
                Label lblDesc = new Label() { Text = formula.Desc, FontFamily = "Helvetica", TextColor = Color.FromHex("111111"), FontSize = 15, VerticalOptions = LayoutOptions.CenterAndExpand, LineBreakMode = LineBreakMode.TailTruncation, MaxLines = 3};
                
                List<int> isFavoritedList = fav.Where(f => f.FormulaId == formula.Id).Select(g => g.IsFavorited).ToList();
                int isFavorited = 0;
                if (isFavoritedList.Count != 0)
                {
                    isFavorited = isFavoritedList[0];

                    if (isFavorited == 1)
                    {
                        imgFavorite.Source = utils.GetBase64Image("favorited");
                    }
                    else
                    {
                        imgFavorite.Source = utils.GetBase64Image("unfavorited");
                    }
                }
                else
                {
                    imgFavorite.Source = utils.GetBase64Image("unfavorited");
                }

                imgFavorite.Clicked += (s, e) =>
                {
                    if (isFavorited == 1)
                    {
                        isFavorited = 0;
                        if (DAO.UpdateFavorite(new Favorites() { FormulaId = formula.Id, IsFavorited = isFavorited }))
                        {
                            imgFavorite.Source = utils.GetBase64Image("unfavorited");
                        }
                        else
                        {
                            throw new Exception("Erro ao atualizar favoritos"); 
                        }
                    }
                    else
                    {
                        isFavorited = 1;
                        if(DAO.UpdateFavorite(new Favorites() { FormulaId = formula.Id, IsFavorited = isFavorited })) { 

                            imgFavorite.Source = utils.GetBase64Image("favorited");
                        }
                        else
                        {
                            throw new Exception("Erro ao atualizar favoritos");
                        }
                    }
                };

                Grid gridTitle = new Grid() { FlowDirection = FlowDirection.LeftToRight };
                gridTitle.Children.Add(lblTitle, 0, 0);
                gridTitle.Children.Add(imgFavorite, 1, 0);
                frameDesc.Content = lblDesc;
                stackLines.Children.Add(gridTitle);
                stackLines.Children.Add(frameDesc);
                frameLines.Content = stackLines;
                stackMain.Children.Add(frameLines);
            }
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}