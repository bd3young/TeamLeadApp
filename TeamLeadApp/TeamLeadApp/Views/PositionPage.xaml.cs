using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.ViewModels;
using TeamLeadApp.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TeamLeadApp.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PositionPage : ContentPage
	{
		PositionViewModel positionViewModel;
		public PositionPage ()
		{
			InitializeComponent ();
			BindingContext = positionViewModel = new PositionViewModel(Navigation);
		}
		protected override void OnAppearing()
		{
			base.OnAppearing();
			positionViewModel.OnAppearing();
		}
		private void Picker_SelectedOfficerOneChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;

			var viewModel = (PositionViewModel)BindingContext;

			if (viewModel.UpdateOfficerOneCommand.CanExecute(picker.BindingContext))
			{
				viewModel.UpdateOfficerOneCommand.Execute(picker.BindingContext);
			}
		}
		private void Picker_SelectedOfficerTwoChanged(object sender, EventArgs e)
		{
			var picker = (Picker)sender;

			var viewModel = (PositionViewModel)BindingContext;

			if (viewModel.UpdateOfficerTwoCommand.CanExecute(picker.BindingContext))
			{
				viewModel.UpdateOfficerTwoCommand.Execute(picker.BindingContext);
			}
		}
		private void Button_ResetPositionPressed(object sender, EventArgs e)
		{
			var button = (Button)sender;

			var viewModel = (PositionViewModel)BindingContext;

			if (viewModel.ResetPositionCommand.CanExecute(button.BindingContext))
			{
				viewModel.ResetPositionCommand.Execute(button.BindingContext);
			}
		}
	}
}