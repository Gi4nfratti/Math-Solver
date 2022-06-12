using Math_Solver.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Math_Solver.App;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace Math_Solver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchPage : ContentPage
    {
        IEnumerable<string> categories;
        public SearchPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            styleListView();
            listViewSearch.ItemsSource = getList();
            DisplayInfo displaySize = DeviceDisplay.MainDisplayInfo;
            ad.WidthRequest = displaySize.Width;
            ad.HeightRequest = 150;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }


        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = searchBar.Text;
            if (keyword.Length >= 1)
            {
                var itemSearched = categories.Where(item => item.ToLower().Contains(keyword.ToLower()));
                listViewSearch.ItemsSource = itemSearched;
                listViewSearch.IsVisible = true;
            }
            else
            {
                listViewSearch.ItemsSource = getList();
            }
        }

        private void listViewSearch_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if(e.Item as string == null)
            {
                return;
            }
            else
            {
                var formulaDetail = getIdName(e.Item as string);
                Navigation.PushAsync(new FormulaDetailPage(formulaDetail.idName, formulaDetail.name));
            }
        }

        private IEnumerable<string> getList()
        {
            categories = mathList.Where(category => category.Tag == "Calc")
                .OrderBy(x => x.Name)
                .Select(names => names.Name);
            return categories;
        }

        private (string idName, string name) getIdName(string item)
        {
            (string idName, string name) itemTapped = mathList.Where(items => items.Name == item)
                .Select(x => (x.IdName, x.Name)).First();
            
            
            return itemTapped;
        }

        private void styleListView()
        {
            var template = new DataTemplate(typeof(TextCell));
            template.SetValue(TextCell.TextColorProperty, Color.Black);
            template.SetValue(TextCell.TextProperty, TextAlignment.Start);
            template.SetBinding(TextCell.TextProperty, ".");
            listViewSearch.ItemTemplate = template;
        }
    }
}