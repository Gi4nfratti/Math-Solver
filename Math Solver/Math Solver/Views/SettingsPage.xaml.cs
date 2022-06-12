using Math_Solver.DAO;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Math_Solver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsPage : ContentPage
    {
        DatabaseAccess DAO = new DatabaseAccess();

        public SettingsPage()
        {
            InitializeComponent();

            var lblAbout_tap = new TapGestureRecognizer();
            lblAbout_tap.Tapped += (s, e) => {
                Shell.Current.GoToAsync(nameof(AboutPage));
            };
            lblAbout.GestureRecognizers.Add(lblAbout_tap);
        }
    }
}