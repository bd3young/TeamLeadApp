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
    public class PositionViewModel : BasePositionViewModel
    {
		public Command LoadPositionCommand { get; }
		public ObservableCollection<Position> Positions { get; }
		public Command AddPositionCommand { get; }
		public Command EditPositionCommand { get; }
		public Command DeletePositionCommand { get; }

		public PositionViewModel(INavigation _navigation)
		{
			LoadPositionCommand = new Command(async () => await ExecuteLoadPositionCommand());
			Positions = new ObservableCollection<Position>();
			AddPositionCommand = new Command(OnAddPosition);
			EditPositionCommand = new Command<Position>(OnEditPosition);
			DeletePositionCommand = new Command<Position>(OnDeletePosition);
			Navigation = _navigation;
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		private async void OnDeletePosition(Position position)
		{
			var officerList = await App.OfficerService.GetProductsAsync();
			

			if (position == null)
			{
				return;
			}

			if (await App.Current.MainPage.DisplayAlert("Delete", "Are you sure you would like to Delete this Position", "Yes", "No"))
			{
				await App.PositionService.DeleteProductAsync(position.Id);
				var positionList = await App.PositionService.GetProductsAsync();
				foreach (var officer in officerList) 
				{
					foreach (var pos in positionList) 
					{
						//officer.PositionsList.Add(pos.Name);
					}
				}
			}
			else
			{
				return;
			}
		}

		private async void OnEditPosition(Position position)
		{
			await Navigation.PushAsync(new AddPositionPage(position));
			var officerList = await App.OfficerService.GetProductsAsync();
			var positionList = await App.PositionService.GetProductsAsync();
			foreach (var officer in officerList)
			{
				foreach (var pos in positionList)
				{
					//officer.PositionsList.Add(pos.Name);
				}
			}
		}

		private async void OnAddPosition(object obj)
		{
			await Shell.Current.GoToAsync(nameof(AddPositionPage));
			var officerList = await App.OfficerService.GetProductsAsync();
			var positionList = await App.PositionService.GetProductsAsync();
			foreach (var officer in officerList)
			{
				foreach (var pos in positionList)
				{
					//officer.PositionsList.Add(pos.Name);
				}
			}
		}

		async Task ExecuteLoadPositionCommand()
		{
			
			IsBusy = true;
			var positionList = await App.PositionService.GetProductsAsync();

			try 
			{
				Positions.Clear();
				foreach (var position in positionList) 
				{
					Positions.Add(position);
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
