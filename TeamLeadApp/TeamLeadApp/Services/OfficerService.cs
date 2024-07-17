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

		public async Task<IEnumerable<Officer>> GetShiftOfficersAsync(string shift, string day)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Shift == shift && o.Lv == false && o.Ehs == false && o.Admin == false && day != o.RdoOne.ToUpper() && day != o.RdoTwo.ToUpper() && day != o.RdoThree.ToUpper()).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetAdminOfficersAsync(bool admin)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Admin == admin && o.Ehs == true).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetShiftRankOfficersAsync(string shift, string rank, bool fullTime, string day)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Shift == shift && o.Rank == rank && o.FullTime == fullTime && o.Ehs == false && o.Lv == false && o.Admin == false && day != o.RdoOne.ToUpper() && day != o.RdoTwo.ToUpper() && day != o.RdoThree.ToUpper()
			).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetEhsOfficersAsync(string shift)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Shift == shift && o.Ehs == true && o.Admin == false).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetAdminRankOfficersAsync(bool admin, string rank)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Admin == admin && o.Rank == rank && o.Ehs == true).ToListAsync());
		}

		public async Task<Officer> GetOfficersByNameAsync(string firstName, string lastName)
		{
			return await _database.Table<Officer>().Where(o => o.FirstName == firstName && o.LastName == lastName).FirstOrDefaultAsync();
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
				officer.BreakNumber = "";
				if (officer.ShiftEnd - officer.ShiftBegin == new System.TimeSpan(08,30,00))
				{
					officer.FullTime = true;
				}
				else
				{
					officer.FullTime = false;
				}
				if (officer.ShiftBegin >= new System.TimeSpan(03, 00, 00) && officer.ShiftEnd <= new System.TimeSpan(12, 30, 00) && officer.Admin == false)
				{
					officer.Shift = "AM";

				}
				else if (officer.ShiftBegin >= new System.TimeSpan(10, 30, 00) && officer.ShiftEnd <= new System.TimeSpan(20, 00, 00) && officer.Admin == false)
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
				position.OfficerOneGender = "";
				position.OfficerTwoGender = "";

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
