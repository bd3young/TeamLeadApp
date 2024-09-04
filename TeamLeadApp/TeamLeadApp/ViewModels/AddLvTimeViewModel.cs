using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using TeamLeadApp.Models;
using Xamarin.Forms;

namespace TeamLeadApp.ViewModels
{
    class AddLvTimeViewModel : BaseOfficerViewModel
	{
		public Command SaveCommand { get; }

		public AddLvTimeViewModel()
		{
			SaveCommand = new Command(OnSave);

			this.PropertyChanged +=
				(_, __) => SaveCommand.ChangeCanExecute();
		}

		private async void OnSave()
		{
			var officer = Officer;

			if (officer.LvBegin > officer.LvEnd)
			{
				await App.Current.MainPage.DisplayAlert("Incorrect Leave Hours", "The begin time cannot be more than the end time.", "Ok");

				return;
			}
			if (officer.LvBegin < officer.ShiftBegin || officer.LvEnd > officer.ShiftEnd)
			{
				await App.Current.MainPage.DisplayAlert("Incorrect Leave", "Leave time must be within the Officers work shift", "Ok");

				return;
			}
			else
			{
				officer.Lv = true;

				await App.OfficerService.AddProductAsync(officer);
			}

			await Shell.Current.GoToAsync("..");
		}
	}
}
