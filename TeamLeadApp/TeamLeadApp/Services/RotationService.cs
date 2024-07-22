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
		public RotationService(string cdbPath)
		{

			_rdatabase = new SQLiteAsyncConnection(cdbPath);
			_rdatabase.CreateTableAsync<Rotation>().Wait();
			_rdatabase.CreateTableAsync<RotationPosition>().Wait();

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

		public async Task<IEnumerable<Rotation>> GetProductsAsync()
		{
			return await Task.FromResult(await _rdatabase.Table<Rotation>().ToListAsync());
		}

		public async Task<bool> AddProductPAsync(RotationPosition rotationPosition)
		{
			if (rotationPosition.Id > 0)
			{
				await _rdatabase.UpdateAsync(rotationPosition);
			}
			else
			{
				await _rdatabase.InsertAsync(rotationPosition);
			}
			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteProductPAsync(int id)
		{
			await _rdatabase.DeleteAsync<RotationPosition>(id);
			return await Task.FromResult(true);
		}

		public async Task<RotationPosition> GetProductPAsync(int id)
		{
			return await _rdatabase.Table<RotationPosition>().Where(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<RotationPosition>> GetProductsPAsync()
		{
			return await Task.FromResult(await _rdatabase.Table<RotationPosition>().ToListAsync());
		}
	}
}
