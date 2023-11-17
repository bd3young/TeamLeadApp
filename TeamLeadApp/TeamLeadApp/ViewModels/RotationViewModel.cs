using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;
using Xamarin.Forms;

namespace TeamLeadApp.ViewModels
{
    public class RotationViewModel : BaseOfficerViewModel
    {
		public Command LoadRotationCommand { get; }
		public ObservableCollection<Officer> Officers { get; }
		public RotationViewModel(INavigation _navigation) 
		{
			LoadRotationCommand = new Command(async () => await ExecuteLoadRotationCommand());
			Officers = new ObservableCollection<Officer>();
			Navigation = _navigation;
		}

		public void OnAppearing()
		{
			IsBusy = true;
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
					if (officer.ShiftBegin < Convert.ToInt16(DateTime.Now.ToShortTimeString()) && officer.ShiftEnd > Convert.ToInt16(DateTime.Now.ToShortTimeString()) && officer.Rdo.ToUpper() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper())
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
