using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TeamLeadApp.Models;
using Xamarin.Forms;

namespace TeamLeadApp.ViewModels
{
    class AddTimeViewModel : BaseOfficerViewModel
    {
		public Command SaveCommand { get; }

		public AddTimeViewModel()
		{
			SaveCommand = new Command(OnSave);

			this.PropertyChanged +=
				(_, __) => SaveCommand.ChangeCanExecute();
		}

		private async void OnSave()
		{
			var officer = Officer;
			var day = DateTime.Now.DayOfWeek.ToString().ToUpper();

			if (officer.EhsBegin > officer.EhsEnd)
			{
				await App.Current.MainPage.DisplayAlert("Incorrect Extra Hours", "The begin time cannot be more than the end time.", "Ok");

				return;
			}
			if (officer.RdoOne.ToUpper() != day
				&& officer.RdoTwo.ToUpper() != day
				&& officer.RdoThree.ToUpper() != day) 
			{
				if (officer.EhsBegin < officer.ShiftEnd && officer.EhsBegin >= officer.ShiftBegin
					|| officer.EhsEnd < officer.ShiftEnd && officer.EhsEnd > officer.ShiftBegin
					|| officer.EhsBegin < officer.ShiftBegin && officer.EhsEnd > officer.ShiftEnd)
				{
					await App.Current.MainPage.DisplayAlert("Incorrect Extra Hours", "The Officer can not be on extra hours during their work shift.", "Ok");

					return;
				}
				else
				{
					officer.Ehs = true;

					await App.OfficerService.AddProductAsync(officer);
				}
			}
			
			else 
			{
				officer.Ehs = true;

				await App.OfficerService.AddProductAsync(officer);
			}			

			await Shell.Current.GoToAsync("..");
		}
	}
}
