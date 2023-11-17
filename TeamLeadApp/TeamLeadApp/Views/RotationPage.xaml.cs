using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamLeadApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RotationPage : ContentPage
	{
		RotationViewModel rotationViewModel;
		public RotationPage ()
		{
			InitializeComponent ();
			BindingContext = rotationViewModel = new RotationViewModel(Navigation);
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			rotationViewModel.OnAppearing();
		}
	}
}