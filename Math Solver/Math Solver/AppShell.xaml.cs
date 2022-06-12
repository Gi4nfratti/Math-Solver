 using Math_Solver.DAO;
using Math_Solver.ViewModels;
using Math_Solver.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;

namespace Math_Solver
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CategoryListPage), typeof(CategoryListPage));
            Routing.RegisterRoute(nameof(FormulaDetailPage), typeof(FormulaDetailPage));
            Routing.RegisterRoute(nameof(OnboardingPage), typeof(OnboardingPage));
            Routing.RegisterRoute(nameof(SolveBase), typeof(SolveBase));
            Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
        }
    }
}
