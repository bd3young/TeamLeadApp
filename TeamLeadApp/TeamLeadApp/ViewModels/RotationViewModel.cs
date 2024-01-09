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
    public class RotationViewModel : BaseOfficerViewModel
    {
		public Command LoadRotationCommand { get; }
		public Command UpdateBreakOneCommand { get; }
		public Command UpdateBreakTwoCommand { get; }
		public Command UpdateLunchCommand { get; }
		public Command UpdateLvCommand { get; }
		public Command UpdateNotesCommand { get; }
		public ObservableCollection<Officer> Officers { get; }
		public RotationViewModel(INavigation _navigation) 
		{
			LoadRotationCommand = new Command(async () => await ExecuteLoadRotationCommand());
			Officers = new ObservableCollection<Officer>();
			UpdateBreakOneCommand = new Command<Officer>(OnUpdateBreakOne);
			UpdateBreakTwoCommand = new Command<Officer>(OnUpdateBreakTwo);
			UpdateLunchCommand = new Command<Officer>(OnUpdateLunch);
			UpdateLvCommand = new Command<Officer>(OnUpdateLv);
			UpdateNotesCommand = new Command<Officer>(OnUpdateNotes);
			Navigation = _navigation;
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		private async void OnUpdateBreakOne(Officer officer)
		{
			var offList = await App.OfficerService.GetProductsAsync();
			foreach (var product in offList) 
			{
				if (product.Id == officer.Id && product.BreakOne != officer.BreakOne) 
				{
					await App.OfficerService.AddProductAsync(officer);
				}
			}

			
		}
		private async void OnUpdateBreakTwo(Officer officer)
		{
			await App.OfficerService.AddProductAsync(officer);
		}
		private async void OnUpdateNotes(Officer officer)
		{

			await App.OfficerService.AddProductAsync(officer);
		}
		private async void OnUpdateLunch(Officer officer)
		{

			await App.OfficerService.AddProductAsync(officer);
		}
		private async void OnUpdateLv(Officer officer)
		{
			officer.Lv = true;
			
			await App.OfficerService.AddProductAsync(officer);
		}
		private async Task ExecuteLoadRotationCommand()
		{
			IsBusy = true;

			try
			{
				Officers.Clear();
				var officerList = await App.OfficerService.GetProductsAsync();
				foreach (var officer in officerList)
				{
					if (officer.RdoOne.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoTwo.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.Lv != true)
					{
						Officers.Add(officer);
					}
					else
					{
						continue;
					}

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
