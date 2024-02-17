using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
		public Command UpdatePositionCommand { get; }
		public ObservableCollection<Officer> PmOfficers { get; }
		public ObservableCollection<Officer> AmOfficers { get; }
		public List<string> Positions { get; }
		public string SelectedPosition { get; set; }
		
		public DateTime CurrentDate { get; set; }

		public RotationViewModel(INavigation _navigation)
		{
			LoadPmRotationCommand = new Command(async () => await ExecuteLoadPmRotationCommand());
			LoadAmRotationCommand = new Command(async () => await ExecuteLoadAmRotationCommand());
			PmOfficers = new ObservableCollection<Officer>();
			AmOfficers = new ObservableCollection<Officer>();
			SelectedPosition = "";
			Positions = new List<string>();
			CurrentDate = DateTime.Now.Date;
			UpdateBreakOneCommand = new Command<Officer>(OnUpdateBreakOne);
			UpdateBreakTwoCommand = new Command<Officer>(OnUpdateBreakTwo);
			UpdateLunchCommand = new Command<Officer>(OnUpdateLunch);
			UpdateLvCommand = new Command<Officer>(OnUpdateLv);
			UpdateNotesCommand = new Command<Officer>(OnUpdateNotes);
			UpdatePositionCommand = new Command<Officer>(OnUpdatePosition);
			Navigation = _navigation;
		}

		private async void OnUpdatePosition(Officer officer)
		{
			var offList = await App.OfficerService.GetProductsAsync();
			if (SelectedPosition != null)
			{

				
				foreach (var product in offList)
				{
					if (product.Id == officer.Id && product.Position != SelectedPosition)
					{
						officer.Position = SelectedPosition;
						await App.OfficerService.AddProductAsync(officer);
						SelectedPosition = null;
					}
				}
			}
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
			officer.Ehs = false;

			await App.OfficerService.AddProductAsync(officer);

			if (officer.Shift == "AM")
			{
				AmOfficers.Remove(officer);
			}
			if (officer.Shift == "PM")
			{
				PmOfficers.Remove(officer);
			}
			if (officer.Admin == true) 
			{
				AmOfficers.Remove(officer);
				PmOfficers.Remove(officer);
			}


		}
		private async Task ExecuteLoadPmRotationCommand()
		{
			
			PmOfficers.Clear();
			AmOfficers.Clear();
			CurrentDate = DateTime.Now.Date;
			IsBusy = true;
			var officerList = await App.OfficerService.GetProductsAsync();
			var positionList = await App.PositionService.GetProductsAsync();

			try
			{
				
				

				Positions.Clear();
				foreach (var position in positionList)
				{
					Positions.Add(position.Name);
				}

				foreach (var sofficer in officerList)
				{
					if (sofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.Lv != true && sofficer.Shift == "PM" && sofficer.Rank.ToUpper().Trim() == "SUP" && sofficer.Admin != true
						|| sofficer.Ehs == true && sofficer.Lv != true && sofficer.Shift == "PM" && sofficer.Rank.ToUpper().Trim() == "SUP"
						|| sofficer.Ehs == true && sofficer.Admin == true && sofficer.Rank.ToUpper().Trim() == "SUP")
					{
						if (sofficer.RdoOne.ToUpper().Trim() == "MONDAY" && sofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY" && sofficer.Ehs != true
							|| sofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && sofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY" && sofficer.Ehs != true
							|| sofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && sofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY" && sofficer.Ehs != true
							|| sofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && sofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY" && sofficer.Ehs != true
							|| sofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && sofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY" && sofficer.Ehs != true)
						{
							continue;
						}
						else 
						{
							PmOfficers.Add(sofficer);
						}
						
					}
					else
					{
						continue;
					}

				}
				foreach (var lofficer in officerList)
				{
					if (lofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.Lv != true && lofficer.Shift == "PM" && lofficer.Rank.ToUpper().Trim() == "LEAD" && lofficer.Admin != true
						|| lofficer.Ehs == true && lofficer.Lv != true && lofficer.Shift == "PM" && lofficer.Rank.ToUpper().Trim() == "LEAD"
						|| lofficer.Ehs == true && lofficer.Admin == true && lofficer.Rank.ToUpper().Trim() == "LEAD")
					{
						if (lofficer.RdoOne.ToUpper().Trim() == "MONDAY" && lofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY" && lofficer.Ehs != true
							|| lofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && lofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY" && lofficer.Ehs != true
							|| lofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && lofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY" && lofficer.Ehs != true
							|| lofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && lofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY" && lofficer.Ehs != true
							|| lofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && lofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY" && lofficer.Ehs != true)
						{
							continue;
						}
						else
						{
							PmOfficers.Add(lofficer);
						}

					}
					else
					{
						continue;
					}

				}
				foreach (var ftofficer in officerList)
				{
					if (ftofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.Lv != true && ftofficer.Shift == "PM" && ftofficer.Rank.ToUpper().Trim() == "OFFICER" && ftofficer.FullTime == true && ftofficer.Admin != true
						|| ftofficer.Ehs == true && ftofficer.Lv != true && ftofficer.Shift == "PM" && ftofficer.Rank.ToUpper().Trim() == "OFFICER" && ftofficer.FullTime == true
						|| ftofficer.Ehs == true && ftofficer.Admin == true && ftofficer.Rank.ToUpper().Trim() == "OFFICER")
					{
						if (ftofficer.RdoOne.ToUpper().Trim() == "MONDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY" && ftofficer.Ehs != true
							|| ftofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY" && ftofficer.Ehs != true
							|| ftofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY" && ftofficer.Ehs != true
							|| ftofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY" && ftofficer.Ehs != true
							|| ftofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY" && ftofficer.Ehs != true)
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
					if (ptofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.Lv != true && ptofficer.Shift == "PM" && ptofficer.Rank.ToUpper().Trim() == "OFFICER" && ptofficer.FullTime == false && ptofficer.Admin != true
						|| ptofficer.Ehs == true && ptofficer.Lv != true && ptofficer.Shift == "PM" && ptofficer.Rank.ToUpper().Trim() == "OFFICER" && ptofficer.FullTime == false 
						|| ptofficer.Ehs == true && ptofficer.Admin == true && ptofficer.Rank.ToUpper().Trim() == "OFFICER")
					{
						if (ptofficer.RdoOne.ToUpper().Trim() == "MONDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY" && ptofficer.Ehs != true
							|| ptofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY" && ptofficer.Ehs != true
							|| ptofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY" && ptofficer.Ehs != true
							|| ptofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY" && ptofficer.Ehs != true
							|| ptofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY" && ptofficer.Ehs != true
							|| ptofficer.RdoOne.ToUpper().Trim() == "SATURDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY" && ptofficer.Ehs != true
							|| ptofficer.RdoOne.ToUpper().Trim() == "SATURDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY" && ptofficer.Ehs != true)
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

			AmOfficers.Clear();
			PmOfficers.Clear();
			IsBusy = true;
			var officerList = await App.OfficerService.GetProductsAsync();
			var positionList = await App.PositionService.GetProductsAsync();


			try
			{
				

				Positions.Clear();
				foreach (var position in positionList)
				{
					Positions.Add(position.Name);
				}

				foreach (var sofficer in officerList)
				{
					if (sofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && sofficer.Lv != true && sofficer.Shift == "AM" && sofficer.Rank.ToUpper().Trim() == "SUP" && sofficer.Admin != true
						|| sofficer.Ehs == true && sofficer.Lv != true && sofficer.Shift == "AM" && sofficer.Rank.ToUpper().Trim() == "SUP"
						|| sofficer.Ehs == true && sofficer.Admin == true && sofficer.Rank.ToUpper().Trim() == "SUP")
					{
						if (sofficer.RdoOne.ToUpper().Trim() == "MONDAY" && sofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY" && sofficer.Ehs != true
							|| sofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && sofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY" && sofficer.Ehs != true
							|| sofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && sofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY" && sofficer.Ehs != true
							|| sofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && sofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY" && sofficer.Ehs != true
							|| sofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && sofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY" && sofficer.Ehs != true)
						{
							continue;
						}
						else
						{
							AmOfficers.Add(sofficer);
						}

					}
					else
					{
						continue;
					}

				}
				foreach (var lofficer in officerList)
				{
					if (lofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && lofficer.Lv != true && lofficer.Shift == "AM" && lofficer.Rank.ToUpper().Trim() == "LEAD" && lofficer.Admin != true
						|| lofficer.Ehs == true && lofficer.Lv != true && lofficer.Shift == "AM" && lofficer.Rank.ToUpper().Trim() == "LEAD"
						|| lofficer.Ehs == true && lofficer.Admin == true && lofficer.Rank.ToUpper().Trim() == "LEAD")
					{
						if (lofficer.RdoOne.ToUpper().Trim() == "MONDAY" && lofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY" && lofficer.Ehs != true
							|| lofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && lofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY" && lofficer.Ehs != true
							|| lofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && lofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY" && lofficer.Ehs != true
							|| lofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && lofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY" && lofficer.Ehs != true
							|| lofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && lofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY" && lofficer.Ehs != true)
						{
							continue;
						}
						else
						{
							AmOfficers.Add(lofficer);
						}

					}
					else
					{
						continue;
					}

				}
				foreach (var ftofficer in officerList)
				{
					if (ftofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ftofficer.Lv != true && ftofficer.Shift == "AM" && ftofficer.Rank.ToUpper().Trim() == "OFFICER" && ftofficer.FullTime == true && ftofficer.Admin != true
						|| ftofficer.Ehs == true && ftofficer.Lv != true && ftofficer.Shift == "AM" && ftofficer.Rank.ToUpper().Trim() == "OFFICER" && ftofficer.FullTime == true
						|| ftofficer.Ehs == true && ftofficer.Admin == true && ftofficer.Rank.ToUpper().Trim() == "OFFICER")
					{
						if (ftofficer.RdoOne.ToUpper().Trim() == "MONDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY" && ftofficer.Ehs != true
							|| ftofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY" && ftofficer.Ehs != true
							|| ftofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY" && ftofficer.Ehs != true
							|| ftofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY" && ftofficer.Ehs != true
							|| ftofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && ftofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY" && ftofficer.Ehs != true)
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
					if (ptofficer.RdoOne.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.RdoTwo.ToUpper().Trim() != Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() && ptofficer.Lv != true && ptofficer.Shift == "AM" && ptofficer.Rank.ToUpper().Trim() == "OFFICER" && ptofficer.FullTime == false && ptofficer.Admin != true
						|| ptofficer.Ehs == true && ptofficer.Lv != true && ptofficer.Shift == "AM" && ptofficer.Rank.ToUpper().Trim() == "OFFICER" && ptofficer.FullTime == false
						|| ptofficer.Ehs == true && ptofficer.Admin == true && ptofficer.Rank.ToUpper().Trim() == "OFFICER")
					{
						if (ptofficer.RdoOne.ToUpper().Trim() == "MONDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "WEDNESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "TUESDAY" && ptofficer.Ehs != true
							|| ptofficer.RdoOne.ToUpper().Trim() == "TUESDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "THURSDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "WEDNESDAY" && ptofficer.Ehs != true
							|| ptofficer.RdoOne.ToUpper().Trim() == "WEDNESDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "FRIDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "THURSDAY" && ptofficer.Ehs != true
							|| ptofficer.RdoOne.ToUpper().Trim() == "THURSDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "SATURDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "FRIDAY" && ptofficer.Ehs != true
							|| ptofficer.RdoOne.ToUpper().Trim() == "SUNDAY" && ptofficer.RdoTwo.ToUpper().Trim() == "TUESDAY" && Convert.ToString(DateTime.Now.DayOfWeek).ToUpper() == "MONDAY" && ptofficer.Ehs != true)
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
