﻿using Math_Solver.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Math_Solver.ViewModels
{
    public class LoginViewModel
    {
        public Command LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked(object obj)
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            await Shell.Current.GoToAsync($"//{nameof(InitPage)}");
        }
    }
}
