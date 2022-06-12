using FFImageLoading.Forms;
using Math_Solver.Resources;
using Math_Solver.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Math_Solver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AboutPage : ContentPage
    {
        private readonly DisplayInfo screenSize = DeviceDisplay.MainDisplayInfo;

        public AboutPage()
        {
            InitializeComponent();
            Utils.Utils utils = new Utils.Utils();
            imgPhoto.Aspect = Aspect.AspectFill;
            stackPhoto.HorizontalOptions = LayoutOptions.Start;
            stackPhoto.VerticalOptions = LayoutOptions.Start;
            stackPhoto.WidthRequest = screenSize.Width;
            
            GetImg(frameBehance, "behance");
            GetImg(frameGithub, "github");
            GetImg(frameLinkedin, "linkedin");
        }
        
        private void GetImg(Frame frame, string name)
        {
            Utils.Utils utils = new Utils.Utils();
            CachedImage img = utils.CreateImage(name);
            img.GestureRecognizers.Add(GetTapGesture(name));
            frame.Content = img;
        }

        private GestureRecognizer GetTapGesture(string name)
        {
            var tapGesture = new TapGestureRecognizer();
            tapGesture.Tapped += async (s, e) => {
                switch (name)
                {
                    case "linkedin":
                        await Browser.OpenAsync(AppResources.linkedin);
                        break;
                    case "behance":
                        await Browser.OpenAsync(AppResources.behance);
                        break;
                    case "github":
                        await Browser.OpenAsync(AppResources.github);
                        break;
                    default:
                        break;
                }
            };
            return tapGesture;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }
    }
}