using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public interface IDateRepository
	{
		Task<bool> AddProductAsync(Date date);
		Task<bool> UpdateProductAsync(Date date);
		Task<Date> GetProductAsync(int id);
		Task<IEnumerable<Date>> GetProductsAsync();
	}
}
