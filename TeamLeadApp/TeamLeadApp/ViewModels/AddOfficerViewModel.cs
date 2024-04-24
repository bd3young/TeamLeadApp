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

			this.PropertyChanged += 
				(_, __) => SaveCommand.ChangeCanExecute();

			Officer = new Officer();
		}

		private async void OnCancel()
		{
			await Shell.Current.GoToAsync("..");
		}

		private async void OnSave()
		{
			var officer = Officer;
			if (officer.RdoThree.ToUpper() == "NONE") 
			{
				officer.RdoThree = "";
			}
			await App.OfficerService.AddProductAsync(officer);

			await Shell.Current.GoToAsync("..");
		}
	}
}
