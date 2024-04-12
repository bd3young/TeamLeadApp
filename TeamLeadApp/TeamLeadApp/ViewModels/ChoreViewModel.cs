using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;
using TeamLeadApp.Views;
using Xamarin.Forms;

namespace TeamLeadApp.ViewModels
{
	public class ChoreViewModel : BaseChoreViewModel
	{
		public Command LoadChoreCommand { get; }
		public ObservableCollection<Chore> Chores { get; }
		public Command AddChoreCommand { get; }
		public Command EditChoreCommand { get; }
		public Command DeleteChoreCommand { get; }

		public ChoreViewModel(INavigation _navigation)
		{
			LoadChoreCommand = new Command(async () => await ExecuteLoadChoreCommand());
			Chores = new ObservableCollection<Chore>();
			AddChoreCommand = new Command(OnAddChore);
			EditChoreCommand = new Command<Chore>(OnEditChore);
			DeleteChoreCommand = new Command<Chore>(OnDeleteChore);
			Navigation = _navigation;
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		private async void OnDeleteChore(Chore chore)
		{


			if (chore == null)
			{
				return;
			}

			if (await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you would like to Delete this Chore", "Yes", "No"))
			{
				await App.ChoreService.DeleteProductAsync(chore.Id);
				await ExecuteLoadChoreCommand();
			}
			else
			{
				return;
			}
		}

		private async void OnEditChore(Chore chore)
		{
			await Navigation.PushAsync(new AddChorePage(chore));
		}

		private async void OnAddChore(object obj)
		{
			await Shell.Current.GoToAsync(nameof(AddChorePage));
		}

		async Task ExecuteLoadChoreCommand()
		{

			IsBusy = true;
			var choreList = await App.ChoreService.GetProductsAsync();

			try
			{
				Chores.Clear();
				foreach (var chore in choreList)
				{
					Chores.Add(chore);
				}
			}
			catch (Exception)
			{
				throw;
			}
			finally
			{
				IsBusy = false;
			}
		}
	}
}
