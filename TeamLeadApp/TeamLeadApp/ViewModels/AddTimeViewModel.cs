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

			if (officer.LvBegin != new TimeSpan() && officer.LvEnd != new TimeSpan()) 
			{
				if (officer.LvBegin >= officer.ShiftBegin && officer.LvEnd <= officer.ShiftEnd)
				{
					officer.Lv = true;

					await App.OfficerService.AddProductAsync(officer);
				}
				else 
				{
					await App.Current.MainPage.DisplayAlert("Incorrect Leave", "Leave time must be within the Officers work shift", "Ok");

					return;
				}
			}
			if (officer.EhsBegin != new TimeSpan() && officer.EhsEnd != new TimeSpan()) 
			{

				if (officer.EhsBegin > officer.EhsEnd)
				{
					await App.Current.MainPage.DisplayAlert("Incorrect Extra Hours", "The begin time cannot be more than the end time.", "Ok");

					return;
				}
				if (officer.RdoOne.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.EhsBegin < officer.ShiftEnd && officer.EhsBegin > officer.ShiftBegin
					|| officer.RdoTwo.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.EhsBegin < officer.ShiftEnd && officer.EhsBegin > officer.ShiftBegin
					|| officer.RdoThree.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.EhsBegin < officer.ShiftEnd && officer.EhsBegin > officer.ShiftBegin
					|| officer.RdoOne.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.EhsEnd < officer.ShiftEnd && officer.EhsEnd > officer.ShiftBegin
					|| officer.RdoTwo.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.EhsEnd < officer.ShiftEnd && officer.EhsEnd > officer.ShiftBegin
					|| officer.RdoThree.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.EhsEnd < officer.ShiftEnd && officer.EhsEnd > officer.ShiftBegin)
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

			

			await Shell.Current.GoToAsync("..");
		}
	}
}
