using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
    public interface IPositionRepository
    {
		Task<bool> AddProductAsync(Position positon);
		Task<bool> UpdateProductAsync(Position position);
		Task<bool> DeleteProductAsync(int id);
		Task<Position> GetProductAsync(int id);
		Task<IEnumerable<Position>> GetProductsAsync();
	}
}
