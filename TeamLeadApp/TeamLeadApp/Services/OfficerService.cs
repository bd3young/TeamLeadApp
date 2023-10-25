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
		public OfficerService(string dbPath) 
		{
			_database = new SQLiteAsyncConnection(dbPath);
			_database.CreateTableAsync<Officer>().Wait();
		}
		public Task<bool> AddProductAsync(Officer officer)
		{
			throw new NotImplementedException();
		}

		public Task<bool> DeleteProductAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<Officer> GetProductAsync(int id)
		{
			throw new NotImplementedException();
		}

		public Task<IEnumerable<Officer>> GetProductsAsync()
		{
			throw new NotImplementedException();
		}

		public Task<bool> UpdateProductAsync(Officer officer)
		{
			throw new NotImplementedException();
		}
	}
}
