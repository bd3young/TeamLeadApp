using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public class DateService : IDateRepository
	{
		public SQLiteAsyncConnection _ddatabase;
		List<Date> initDate;
		public DateService(string ddbPath)
		{

			_ddatabase = new SQLiteAsyncConnection(ddbPath);
			_ddatabase.CreateTableAsync<Date>().Wait();

		}
		public async void AddInitDates()
		{

			var dayList = await App.DateService.GetProductsAsync();

			initDate = new List<Date>()
			{
				new Date{ Day = "" },
			};

			if (dayList.Count() == 0) 
			{
				foreach (var day in initDate) 
				{
					await _ddatabase.InsertAsync(day);
				}
				
			}

		}

		public async Task<bool> AddProductAsync(Date date)
		{
			if (date.Id > 0)
			{
				await _ddatabase.UpdateAsync(date);
			}
			else
			{
				await _ddatabase.InsertAsync(date);
			}
			return await Task.FromResult(true);
		}

		public async Task<Date> GetProductAsync(int id)
		{
			return await _ddatabase.Table<Date>().Where(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Date>> GetProductsAsync()
		{
			return await Task.FromResult(await _ddatabase.Table<Date>().ToListAsync());
		}

		public Task<bool> UpdateProductAsync(Date date)
		{
			throw new NotImplementedException();
		}
	}
}
