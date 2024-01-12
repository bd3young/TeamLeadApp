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

		public Task<bool> UpdateProductAsync(Officer officer)
		{
			throw new NotImplementedException();
		}

	}
}
