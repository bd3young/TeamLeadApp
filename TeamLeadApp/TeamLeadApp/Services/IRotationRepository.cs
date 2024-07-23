using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public interface IRotationRepository
	{
		Task<bool> AddProductAsync(Rotation rotation);
		Task<bool> DeleteProductAsync(int id);
		Task<Rotation> GetProductAsync(int id);
		Task<IEnumerable<Rotation>> GetProductsAsync();
	}
}
