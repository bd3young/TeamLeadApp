using System;
using System.Collections.Generic;
using System.Text;
using TeamLeadApp.Models;
using Xamarin.Forms;
using static Android.Icu.Text.Transliterator;

namespace TeamLeadApp.ViewModels
{
	public class AddChoreViewModel : BaseChoreViewModel
	{
		public Command SaveCommand { get; }
		public Command CancelCommand { get; }
		public AddChoreViewModel()
		{
			SaveCommand = new Command(OnSave);
			CancelCommand = new Command(OnCancel);

			this.PropertyChanged +=
				(_, __) => SaveCommand.ChangeCanExecute();

			Chore = new Chore();
		}

		private async void OnCancel()
		{
			await Shell.Current.GoToAsync("..");
		}

		private async void OnSave()
		{
			var chore = Chore;

			if (chore.Name == null || chore.Name == "")
			{
				await App.Current.MainPage.DisplayAlert("Incorrect Name", "Task name cannot be blank", "Ok");

				return;
			}

			await App.ChoreService.AddProductAsync(chore);

			await Shell.Current.GoToAsync("..");
		}
	}
}
