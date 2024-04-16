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
		public Command UpdateIsCompletedCommand { get; }
		public Command UpdateTimeCommand { get; }
		public ObservableCollection<Chore> Chores { get; }
		public Command AddChoreCommand { get; }
		public Command EditChoreCommand { get; }
		public Command DeleteChoreCommand { get; }
		public string SelectedTime { get; set; }

		public ChoreViewModel(INavigation _navigation)
		{
			LoadChoreCommand = new Command(async () => await ExecuteLoadChoreCommand());
			UpdateIsCompletedCommand = new Command<Chore>(OnIsCompleted);
			UpdateTimeCommand = new Command<Chore>(OnUpdateTime);
			Chores = new ObservableCollection<Chore>();
			AddChoreCommand = new Command(OnAddChore);
			EditChoreCommand = new Command<Chore>(OnEditChore);
			DeleteChoreCommand = new Command<Chore>(OnDeleteChore);
			SelectedTime = "";
			Navigation = _navigation;
		}

		public async void OnUpdateTime(Chore chore)
		{
			
			if (chore != null)
			{
				var selectedChore = await App.ChoreService.GetProductAsync(chore.Id);
				if (chore.Time != selectedChore.Time)
				{
					await App.ChoreService.AddProductAsync(chore);
				}
			}
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}
		private async void OnIsCompleted(Chore chore)
		{
			if (chore != null)
			{

				var currentChore = await App.ChoreService.GetProductAsync(chore.Id);
				if (currentChore.IsCompleted != chore.IsCompleted) 
				{
					currentChore.IsCompleted = chore.IsCompleted;
					await App.ChoreService.AddProductAsync(currentChore);
				}
				

			}

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
