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
		public Command LoadPmRotationCommand { get; }
		public Command LoadAmRotationCommand { get; }
		public Command UpdateBreakOneCommand { get; }
		public Command UpdateBreakTwoCommand { get; }
		public Command UpdateLunchCommand { get; }
		public Command UpdateLvCommand { get; }
		public Command UpdateNotesCommand { get; }
		public ObservableCollection<Officer> PmOfficers { get; }
		public ObservableCollection<Officer> AmOfficers { get; }
		public RotationViewModel(INavigation _navigation) 
		{
			LoadPmRotationCommand = new Command(async () => await ExecuteLoadPmRotationCommand());
			LoadAmRotationCommand = new Command(async () => await ExecuteLoadAmRotationCommand());
			PmOfficers = new ObservableCollection<Officer>();
			AmOfficers = new ObservableCollection<Officer>();
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
			if (officer != null) 
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
			
		}
		private async void OnUpdateBreakTwo(Officer officer)
		{
			if (officer != null)
			{

				var offList = await App.OfficerService.GetProductsAsync();
				foreach (var product in offList)
				{
					if (product.Id == officer.Id && product.BreakTwo != officer.BreakTwo)
					{
						await App.OfficerService.AddProductAsync(officer);
					}
				}
			}
		}
		private async void OnUpdateNotes(Officer officer)
		{

			await App.OfficerService.AddProductAsync(officer);
		}
		private async void OnUpdateLunch(Officer officer)
		{

			if (officer != null)
			{

				var offList = await App.OfficerService.GetProductsAsync();
				foreach (var product in offList)
				{
					if (product.Id == officer.Id && product.Lunch != officer.Lunch)
					{
						await App.OfficerService.AddProductAsync(officer);
					}
				}
			}
		}
		private async void OnUpdateLv(Officer officer)
		{
			officer.Lv = true;
			
			await App.OfficerService.AddProductAsync(officer);
		}
		private async Task ExecuteLoadPmRotationCommand()
		{
			IsBusy = true;

			try
			{
				PmOfficers.Clear();
				var officerList = await App.OfficerService.GetProductsAsync();
				foreach (var stsofficer in officerList)
				{
					if (stsofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && stsofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && stsofficer.Lv != true && stsofficer.Shift == "PM" && stsofficer.Rank == "STSO")
					{
						if (stsofficer.RdoOne.ToUpper().Trim() == "MONDAY" && stsofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY"
							|| stsofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && stsofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY"
							|| stsofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && stsofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY"
							|| stsofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && stsofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY"
							|| stsofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && stsofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY")
						{
							continue;
						}
						else 
						{
							PmOfficers.Add(stsofficer);
						}
						
					}
					else
					{
						continue;
					}

				}
				foreach (var ltsofficer in officerList)
				{
					if (ltsofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ltsofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ltsofficer.Lv != true && ltsofficer.Shift == "PM" && ltsofficer.Rank == "LTSO")
					{
						if (ltsofficer.RdoOne.ToUpper().Trim() == "MONDAY" && ltsofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY"
							|| ltsofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && ltsofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY"
							|| ltsofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && ltsofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY"
							|| ltsofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && ltsofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY"
							|| ltsofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && ltsofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY")
						{
							continue;
						}
						else
						{
							PmOfficers.Add(ltsofficer);
						}

					}
					else
					{
						continue;
					}

				}
				foreach (var ftofficer in officerList)
				{
					if (ftofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.Lv != true && ftofficer.Shift == "PM" && ftofficer.Rank == "TSO" && ftofficer.FullTime == true)
					{
						if (ftofficer.RdoOne.ToUpper().Trim() == "MONDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY"
							|| ftofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY"
							|| ftofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY"
							|| ftofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY"
							|| ftofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY")
						{
							continue;
						}
						else
						{
							PmOfficers.Add(ftofficer);
						}

					}
					else
					{
						continue;
					}

				}
				foreach (var ptofficer in officerList)
				{
					if (ptofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.Lv != true && ptofficer.Shift == "PM" && ptofficer.Rank == "TSO" && ptofficer.FullTime == false)
					{
						if (ptofficer.RdoOne.ToUpper().Trim() == "MONDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY"
							|| ptofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY"
							|| ptofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY"
							|| ptofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY"
							|| ptofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY")
						{
							continue;
						}
						else
						{
							PmOfficers.Add(ptofficer);
						}

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
		private async Task ExecuteLoadAmRotationCommand()
		{
			IsBusy = true;

			try
			{
				AmOfficers.Clear();
				var officerList = await App.OfficerService.GetProductsAsync();
				foreach (var stsofficer in officerList)
				{
					if (stsofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && stsofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && stsofficer.Lv != true && stsofficer.Shift == "AM" && stsofficer.Rank == "STSO")
					{
						if (stsofficer.RdoOne.ToUpper().Trim() == "MONDAY" && stsofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY"
							|| stsofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && stsofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY"
							|| stsofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && stsofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY"
							|| stsofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && stsofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY"
							|| stsofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && stsofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY")
						{
							continue;
						}
						else
						{
							AmOfficers.Add(stsofficer);
						}

					}
					else
					{
						continue;
					}

				}
				foreach (var ltsofficer in officerList)
				{
					if (ltsofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ltsofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ltsofficer.Lv != true && ltsofficer.Shift == "AM" && ltsofficer.Rank == "LTSO")
					{
						if (ltsofficer.RdoOne.ToUpper().Trim() == "MONDAY" && ltsofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY"
							|| ltsofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && ltsofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY"
							|| ltsofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && ltsofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY"
							|| ltsofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && ltsofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY"
							|| ltsofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && ltsofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY")
						{
							continue;
						}
						else
						{
							AmOfficers.Add(ltsofficer);
						}

					}
					else
					{
						continue;
					}

				}
				foreach (var ftofficer in officerList)
				{
					if (ftofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.Lv != true && ftofficer.Shift == "AM" && ftofficer.Rank == "TSO" && ftofficer.FullTime == true)
					{
						if (ftofficer.RdoOne.ToUpper().Trim() == "MONDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY"
							|| ftofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY"
							|| ftofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY"
							|| ftofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY"
							|| ftofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY")
						{
							continue;
						}
						else
						{
							AmOfficers.Add(ftofficer);
						}

					}
					else
					{
						continue;
					}

				}
				foreach (var ptofficer in officerList)
				{
					if (ptofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.Lv != true && ptofficer.Shift == "AM" && ptofficer.Rank == "TSO" && ptofficer.FullTime == false)
					{
						if (ptofficer.RdoOne.ToUpper().Trim() == "MONDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY"
							|| ptofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY"
							|| ptofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY"
							|| ptofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY"
							|| ptofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY")
						{
							continue;
						}
						else
						{
							AmOfficers.Add(ptofficer);
						}

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
