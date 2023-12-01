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
		public ObservableCollection<Officer> Officers { get; }
		public RotationViewModel(INavigation _navigation) 
		{
			LoadRotationCommand = new Command(async () => await ExecuteLoadRotationCommand());
			Officers = new ObservableCollection<Officer>();
			//UpdateBreakOneCommand = new Command(OnUpdateBreakOne);
			//UpdateBreakTwoCommand = new Command(OnUpdateBreakTwo);
			//UpdateLunchCommand = new Command(OnUpdateLunch);
			Navigation = _navigation;
		}

		public void OnAppearing()
		{
			IsBusy = true;
		}

		//private async void OnUpdateBreakOne(Object officer)
		//{
		//	await Navigation.PushAsync(new AddOfficerPage(officer));
		//}
		//private async void OnUpdateBreakTwo(Officer officer)
		//{
		//	await Navigation.PushAsync(new AddOfficerPage(officer));
		//}
		//private async void OnUpdateLunch(Officer officer)
		//{
		//	await Navigation.PushAsync(new AddOfficerPage(officer));
		//}
		private async Task ExecuteLoadRotationCommand()
		{
			IsBusy = true;

			try
			{
				Officers.Clear();
				var officerList = await App.OfficerService.GetProductsAsync();
				foreach (var officer in officerList)
				{
					if (officer.RdoOne.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && officer.RdoTwo.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper())
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
