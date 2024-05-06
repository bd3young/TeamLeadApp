using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public class OfficerService : IOfficerRepository
	{
		public SQLiteAsyncConnection _database;
		List<Officer> initOfficers;
		public OfficerService(string dbPath) 
		{

			_database = new SQLiteAsyncConnection(dbPath);
			_database.CreateTableAsync<Officer>().Wait();

			//AddInitOfficers();

		}

		public async void AddInitOfficers()
		{
			int initOfficerCount = 0;

			var officerList = await App.OfficerService.GetProductsAsync();

			initOfficers = new List<Officer>()
			{

			};

			foreach (var initOfficer in initOfficers)
			{

				foreach (var exOfficer in officerList)
				{
					if (initOfficer.LastName == exOfficer.LastName )
					{
						initOfficerCount++;
					}
					
				}

				if (initOfficerCount == 0) 
				{
					await _database.InsertAsync(initOfficer);
					initOfficerCount = 0;
				}
				else
				{
					continue;
				}
			}

		}

		//Insert & Update
		public async Task<bool> AddProductAsync(Officer officer)
		{
			if (officer.Id > 0) 
			{
				await _database.UpdateAsync(officer);
			}
			else
			{
				await _database.InsertAsync(officer);
			}
			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			await _database.DeleteAsync<Officer>(id);
			return await Task.FromResult(true);
		}

		public async Task<Officer> GetProductAsync(int id)
		{
			return await _database.Table<Officer>().Where(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Officer>> GetProductsAsync()
		{
			return await Task.FromResult(await _database.Table<Officer>().ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetRankOfficersAsync(string rank)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Rank == rank).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetShiftRankOfficersAsync(string shift, string rank, bool fullTime, string day)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Shift == shift && o.Rank == rank && o.FullTime == fullTime && o.Ehs == false && o.Lv == false && day != o.RdoOne.ToUpper() && day != o.RdoTwo.ToUpper() && day != o.RdoThree.ToUpper()
			).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetEhsOfficersAsync(string shift, bool ehs)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Shift == shift && o.Ehs == ehs).ToListAsync());
		}

		public Task<bool> UpdateProductAsync(Officer officer)
		{
			throw new NotImplementedException();
		}

		public async Task ResetOfficers()
		{

			//if (await App.Current.MainPage.DisplayAlert("New Day", "Are you sure you would like to start a New Day", "Yes", "No"))
			//{
			var officerList = await App.OfficerService.GetProductsAsync();
			foreach (var officer in officerList)
			{
				officer.BreakOne = false;
				officer.BreakTwo = false;
				officer.Lunch = false;
				officer.Notes = "";
				officer.Lv = false;
				officer.Ehs = false;
				officer.Position = "";
				officer.BreakNumber = "";
				int shiftBegin = Convert.ToInt32(officer.ShiftBegin);
				int shiftEnd = Convert.ToInt32(officer.ShiftEnd);
				if (shiftEnd - shiftBegin == 830 || shiftEnd - shiftBegin == 870 || shiftEnd - shiftBegin == 1030 || shiftEnd - shiftBegin == 1070)
				{
					officer.FullTime = true;
				}
				else
				{
					officer.FullTime = false;
				}
				if (shiftBegin >= 300 && shiftEnd <= 1400 && officer.Admin == false)
				{
					officer.Shift = "AM";

				}
				else if (shiftBegin >= 1030 && shiftEnd <= 2000 && officer.Admin == false)
				{
					officer.Shift = "PM";
				}
				else 
				{
					officer.Shift = "MID";
				}

				await App.OfficerService.AddProductAsync(officer);
			}
			var choreList = await App.ChoreService.GetProductsAsync();
			foreach (var chore in choreList)
			{
				chore.IsCompleted = false;
				chore.Time = new System.TimeSpan();

				await App.ChoreService.AddProductAsync(chore);
			}
			var postionList = await App.PositionService.GetProductsAsync();
			foreach (var position in postionList) 
			{
				position.OfficerOne = "";
				position.OfficerTwo = "";

				await App.PositionService.AddProductAsync(position);
			}
			//await ExecuteLoadOfficerCommand();
			//}
			//else
			//{
			//	return;
			//}
		}

	}
}
