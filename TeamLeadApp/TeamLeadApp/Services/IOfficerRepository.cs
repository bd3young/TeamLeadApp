using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public interface IOfficerRepository
	{
		Task<bool> AddProductAsync(Officer officer);
		Task<bool> DeleteProductAsync(int id);
		Task<Officer> GetProductAsync(int id);
		Task<IEnumerable<Officer>> GetProductsAsync();
	}
}
