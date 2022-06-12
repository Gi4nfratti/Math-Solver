using FFImageLoading.Forms;
using MarcTron.Plugin;
using Math_Solver.Models;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static Math_Solver.App;

namespace Math_Solver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FormulaDetailPage : ContentPage
    {
        string desc = null;
        string idName = string.Empty;
        string img, imgExample;
        Utils.Utils utils = new Utils.Utils();

        private string _toolbarText;
        public string ToolbarText { get => _toolbarText; set { _toolbarText = value; OnPropertyChanged(); } }

        void GoToPage()
        {
            Navigation.PushAsync(new SolveBase(idName, ToolbarText));
        }

        public FormulaDetailPage(string IDName, string title)
        {
            InitializeComponent();
            Utils.Utils utils = new Utils.Utils();
            idName = IDName;
            ToolbarText = title;

            GetDetailFormula();

            if (desc.Length <= 100)
            {
                lblFullDesc.IsEnabled = false;
                lblFullDesc.IsVisible = false;
                lblShortDesc.Text = desc;
            }
            else
            {
                lblFullDesc.IsEnabled = true;
                lblFullDesc.IsVisible = true;
                imgFullDesc.Opacity = 100;
                imgFullDesc.Source = utils.GetBase64Image("down_arrow");
                lblShortDesc.MaxLines = 3;
                lblShortDesc.LineBreakMode = LineBreakMode.TailTruncation;
                lblShortDesc.Text = desc;
            }

            imgFormula.HeightRequest = 100;
            imgFormulaExample.HeightRequest = 100;
            
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += (s, e) => {
                Navigation.PushAsync(new PopupPage(img));
            };

            imgFormula.GestureRecognizers.Add(tapGesture);

            var tapGesture2 = new TapGestureRecognizer();
            tapGesture2.Tapped += (s, e) => {
                Navigation.PushAsync(new PopupPage(imgExample));
            };

            imgFormulaExample.GestureRecognizers.Add(tapGesture2);

            if (CrossConnectivity.Current.IsConnected)
            {
                CrossMTAdmob.Current.OnInterstitialLoaded += (s, e) =>
                {
                    CrossMTAdmob.Current.ShowInterstitial();
                };

                CrossMTAdmob.Current.OnInterstitialOpened += (s, e) =>
                {
                    GoToPage();
                };

                CrossMTAdmob.Current.OnRewardedVideoAdFailedToLoad += (s, e) =>
                {
                    GoToPage();
                };
            }
        }

        public void GetDetailFormula()
        {
            Formula detailFormula = CategoryListPage.filteredFormulas.Where(formula => formula.IdName == idName).First();
            
            lblTitle.Text = detailFormula.Name;
            desc = detailFormula.Desc;
            hashtagArea.Text = String.Concat("#", detailFormula.Hashtag.ToLower());
            img = SetProperties(imgFormula, detailFormula.IdName);
            imgExample = SetProperties(imgFormulaExample, detailFormula.IdName, true);
        }

        private void ButtonSolve_Clicked(object sender, EventArgs e)
        {
            if (CrossConnectivity.Current.IsConnected)
                CrossMTAdmob.Current.LoadInterstitial("ca-app-pub-5802669771020789/2921386310");
            else
                GoToPage();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await imgFullDesc.FadeTo(0, 100);
            await lblShortDesc.FadeTo(0, 80);
            imgFullDesc.IsEnabled = false;
            imgFullDesc.IsVisible = false;
            lblShortDesc.IsVisible = false;
            lblFullDesc.FontSize = 18;
            lblFullDesc.Text = desc;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

        private string SetProperties(CachedImage img, string source, bool getExampleImage = false)
        {
            img.CacheDuration = TimeSpan.FromDays(1);
            img.DownsampleToViewSize = true;
            img.RetryCount = 2;
            img.RetryDelay = 250;
            img.BitmapOptimizations = false;
            img.LoadingPlaceholder = utils.GetBase64Image("loading");
            img.ErrorPlaceholder = utils.GetBase64Image("not_found");
            img.Source = utils.GetBase64Image(source, getExampleImage);
            return utils.GetBase64Image(source, getExampleImage);
        }
    }
}