using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public class ChoreService: IChoreRepository
	{
		public SQLiteAsyncConnection _cdatabase;
		public ChoreService(string cdbPath)
		{

			_cdatabase = new SQLiteAsyncConnection(cdbPath);
			_cdatabase.CreateTableAsync<Chore>().Wait();

		}

		public async Task<bool> AddProductAsync(Chore chore)
		{
			if (chore.Id > 0)
			{
				await _cdatabase.UpdateAsync(chore);
			}
			else
			{
				await _cdatabase.InsertAsync(chore);
			}
			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			await _cdatabase.DeleteAsync<Chore>(id);
			return await Task.FromResult(true);
		}

		public async Task<Chore> GetProductAsync(int id)
		{
			return await _cdatabase.Table<Chore>().Where(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Chore>> GetProductsAsync()
		{
			return await Task.FromResult(await _cdatabase.Table<Chore>().ToListAsync());
		}

		public Task<bool> UpdateProductAsync(Chore chore)
		{
			throw new NotImplementedException();
		}
	}
}
