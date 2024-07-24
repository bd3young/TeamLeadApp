using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public class RotationPositionService : IRotationPositionRepository
	{
		public SQLiteAsyncConnection _rpdatabase;
		public RotationPositionService(string rpdbPath)
		{

			_rpdatabase = new SQLiteAsyncConnection(rpdbPath);
			_rpdatabase.CreateTableAsync<RotationPosition>().Wait();

		}
		public async Task<bool> AddProductAsync(RotationPosition rotationPosition)
		{
			if (rotationPosition.Id > 0)
			{
				await _rpdatabase.UpdateAsync(rotationPosition);
			}
			else
			{
				await _rpdatabase.InsertAsync(rotationPosition);
			}
			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			await _rpdatabase.DeleteAsync<RotationPosition>(id);
			return await Task.FromResult(true);
		}

		public async Task<RotationPosition> GetProductAsync(int id)
		{
			return await _rpdatabase.Table<RotationPosition>().Where(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<RotationPosition>> GetProductsAsync()
		{
			return await Task.FromResult(await _rpdatabase.Table<RotationPosition>().ToListAsync());
		}
		public async Task<IEnumerable<RotationPosition>> GetProductsRPAsync(int id)
		{
			return await Task.FromResult(await _rpdatabase.Table<RotationPosition>().Where(p => p.RotationId == id).ToListAsync());
		}

		public async Task<RotationPosition> GetProductRNAsync(int id, string name)
		{
			return await _rpdatabase.Table<RotationPosition>().Where(p => p.RotationId == id && p.Name == name).FirstOrDefaultAsync();
		}
	}
}
