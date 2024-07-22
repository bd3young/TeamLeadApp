using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;
using TeamLeadApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamLeadApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RotationPositionsPage : ContentPage
	{
		RotationPositionsViewModel rotationPositionsViewModel;
		public RotationPositionsPage(Rotation rotation)
		{
			InitializeComponent();
			BindingContext = rotationPositionsViewModel = new RotationPositionsViewModel(Navigation);

			if (rotation != null)
			{
				((RotationPositionsViewModel)BindingContext).Rotation = rotation;
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			rotationPositionsViewModel.OnAppearing();
		}
	}
}