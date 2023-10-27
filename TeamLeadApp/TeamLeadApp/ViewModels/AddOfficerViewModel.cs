using System;
using System.Collections.Generic;
using System.Text;
using TeamLeadApp.Models;
using Xamarin.Forms;

namespace TeamLeadApp.ViewModels
{
	public class AddOfficerViewModel: BaseOfficerViewModel
	{
		public Command SaveCommand { get; }
		public Command CancelCommand { get; }
		public AddOfficerViewModel() 
		{
			SaveCommand = new Command(OnSave);
			CancelCommand = new Command(OnCancel);
			Officer = new Officer();
		}

		private async void OnCancel()
		{
			await Shell.Current.GoToAsync("..");
		}

		private async void OnSave()
		{
			var officer = Officer;
			await App.OfficerService.AddProductAsync(officer);

			await Shell.Current.GoToAsync("..");
		}
	}
}
