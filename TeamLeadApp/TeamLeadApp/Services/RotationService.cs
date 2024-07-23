using SQLite;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public class RotationService : IRotationRepository
	{
		public SQLiteAsyncConnection _rdatabase;
		public RotationService(string rdbPath)
		{

			_rdatabase = new SQLiteAsyncConnection(rdbPath);
			_rdatabase.CreateTableAsync<Rotation>().Wait();

		}

		public async Task<bool> AddProductAsync(Rotation rotation)
		{
			if (rotation.Id > 0)
			{
				await _rdatabase.UpdateAsync(rotation);
			}
			else
			{
				await _rdatabase.InsertAsync(rotation);
			}
			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			await _rdatabase.DeleteAsync<Rotation>(id);
			return await Task.FromResult(true);
		}

		public async Task<Rotation> GetProductAsync(int id)
		{
			return await _rdatabase.Table<Rotation>().Where(p => p.Id == id).FirstOrDefaultAsync();
		}
		public async Task<Rotation> GetProductTAsync(TimeSpan time)
		{
			return await _rdatabase.Table<Rotation>().Where(p => p.RotationTime == time).FirstOrDefaultAsync();
		}
		public async Task<IEnumerable<Rotation>> GetProductsAsync()
		{
			return await Task.FromResult(await _rdatabase.Table<Rotation>().ToListAsync());
		}
	}
}
