using Math_Solver.Models;
using Math_Solver.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Math_Solver.ViewModels
{
    public class OnboardingViewModel : MvvmHelpers.BaseViewModel
    {
        private ObservableCollection<Onboarding> items;
        private int position;
        private string nextButtonText;
        private string skipButtonText;

        public OnboardingViewModel()
        {
            SetNextButtonText(AppResources.next);
            SetSkipButtonText(AppResources.skip);
            OnBoarding();
            LaunchNextCommand();
            LaunchSkipCommand();
        }

        private void SetNextButtonText(string nextButtonText) => NextButtonText = nextButtonText;
        private void SetSkipButtonText(string skipButtonText) => SkipButtonText = skipButtonText;

        private void OnBoarding()
        {
            Items = new ObservableCollection<Onboarding>
            {
                new Onboarding
                {
                    Title = AppResources.welcome,
                    Content = AppResources.content,
                    ImageUrl = "lampada.png"
                },
                new Onboarding
                {
                    Title = AppResources.titleONB,
                    Content = AppResources.contentONB,
                    ImageUrl = "matematica.png"
                },
                new Onboarding
                {
                    Title = AppResources.titleONB2,
                    Content = AppResources.contentONB2,
                    ImageUrl = "trofeu.png"
                }
            };
        }

        private void LaunchNextCommand()
        {

            NextCommand = new Command(() =>
            {
                if (LastPositionReached())
                {
                    ExitOnBoarding();
                }
                else
                {
                    MoveToNextPosition();
                }
            });
        }
        private void LaunchSkipCommand()
        {
            SkipCommand = new Command(() =>
            {
                ExitOnBoarding();

            });
        }

        private static void ExitOnBoarding()
        {
            Application.Current.MainPage.Navigation.PopModalAsync();
            Application.Current.MainPage = new AppShell();
        }

        private void MoveToNextPosition()
        {
            if (Position == Items.Count - 1) { }
            else
            {
                var nextPosition = ++Position;
                Position = nextPosition;
            }
        }

        private bool LastPositionReached()
            => Position == Items.Count - 1;

        public ObservableCollection<Onboarding> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public string NextButtonText
        {
            get => nextButtonText;
            set => SetProperty(ref nextButtonText, value);
        }
        public string SkipButtonText
        {
            get => skipButtonText;
            set => SetProperty(ref skipButtonText, value);
        }

        public int Position
        {
            get => position;
            set
            {
                if (SetProperty(ref position, value))
                {
                    UpdateNextButtonText();
                }
            }
        }

        private void UpdateNextButtonText()
        {
            if (LastPositionReached())
            {
                SetNextButtonText(AppResources.letsGo);
            }
            else
            {
                SetNextButtonText(AppResources.next);
            }
        }

        public ICommand NextCommand { get; private set; }
        public ICommand SkipCommand { get; private set; }
    }
}
