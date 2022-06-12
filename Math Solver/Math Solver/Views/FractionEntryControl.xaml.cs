using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Math_Solver.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FractionEntryControl : ContentView
    {
        public FractionEntryControl()
        {
            InitializeComponent();
        }

        public static BindableProperty NumeratorTextProperty = BindableProperty.Create(nameof(NumeratorText), typeof(string),
                typeof(FractionEntryControl), defaultValue: string.Empty);

        public string NumeratorText
        {
            get { return (string)GetValue(NumeratorTextProperty); }
            set { SetValue(NumeratorTextProperty, value); }
        }

        public static BindableProperty DenominatorTextProperty = BindableProperty.Create(nameof(DenominatorText), typeof(string),
                typeof(FractionEntryControl), defaultValue: string.Empty);

        public string DenominatorText
        {
            get { return (string)GetValue(DenominatorTextProperty); }
            set { SetValue(DenominatorTextProperty, value); }
        }

        public static BindableProperty PLNumeratorTextProperty = BindableProperty.Create(nameof(PLNumeratorText), typeof(string),
                typeof(FractionEntryControl), defaultValue: string.Empty);

        public string PLNumeratorText
        {
            get { return (string)GetValue(PLNumeratorTextProperty); }
            set { SetValue(PLNumeratorTextProperty, value); }
        }

        public static BindableProperty PLDenominatorTextProperty = BindableProperty.Create(nameof(PLDenominatorText), typeof(string),
                typeof(FractionEntryControl), defaultValue: string.Empty);

        public string PLDenominatorText
        {
            get { return (string)GetValue(PLDenominatorTextProperty); }
            set { SetValue(PLDenominatorTextProperty, value); }
        }
    }
}