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
		private void Picker_SelectedOfficerOneChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;

			var viewModel = (RotationPositionsViewModel)BindingContext;


			if (viewModel.UpdateOfficerOneCommand.CanExecute(picker.BindingContext))
			{
				viewModel.UpdateOfficerOneCommand.Execute(picker.BindingContext);
			}


		}
		private void Picker_SelectedOfficerTwoChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;

			var viewModel = (RotationPositionsViewModel)BindingContext;

			if (viewModel.UpdateOfficerTwoCommand.CanExecute(picker.BindingContext))
			{
				viewModel.UpdateOfficerTwoCommand.Execute(picker.BindingContext);
			}
		}
		private void Button_ResetPositionPressed(object sender, EventArgs e)
		{
			var button = (Button)sender;

			var viewModel = (RotationPositionsViewModel)BindingContext;

			if (viewModel.ResetPositionCommand.CanExecute(button.BindingContext))
			{
				viewModel.ResetPositionCommand.Execute(button.BindingContext);
			}

		}
	}
}