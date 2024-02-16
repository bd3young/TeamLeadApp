using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
    public class PositionService : IPositionRepository
	{
		public SQLiteAsyncConnection _pdatabase;
		List<Position> initPositions;
		public PositionService(string pdbPath)
		{

			_pdatabase = new SQLiteAsyncConnection(pdbPath);
			_pdatabase.CreateTableAsync<Position>().Wait();

		}

		public async Task<bool> AddProductAsync(Position position)
		{
			if (position.Id > 0)
			{
				await _pdatabase.UpdateAsync(position);
			}
			else
			{
				await _pdatabase.InsertAsync(position);
			}
			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			await _pdatabase.DeleteAsync<Position>(id);
			return await Task.FromResult(true);
		}

		public async Task<Position> GetProductAsync(int id)
		{
			return await _pdatabase.Table<Position>().Where(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Position>> GetProductsAsync()
		{
			return await Task.FromResult(await _pdatabase.Table<Position>().ToListAsync());
		}

		public Task<bool> UpdateProductAsync(Position position)
		{
			throw new NotImplementedException();
		}
	}
}
