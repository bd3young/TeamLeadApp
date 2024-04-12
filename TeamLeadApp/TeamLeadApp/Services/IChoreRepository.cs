using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public interface IChoreRepository
	{
		Task<bool> AddProductAsync(Chore chore);
		Task<bool> UpdateProductAsync(Chore chore);
		Task<bool> DeleteProductAsync(int id);
		Task<Chore> GetProductAsync(int id);
		Task<IEnumerable<Chore>> GetProductsAsync();
	}
}
