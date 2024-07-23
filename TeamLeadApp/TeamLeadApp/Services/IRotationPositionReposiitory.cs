using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public interface IRotationPositionRepository
	{
		Task<bool> AddProductAsync(RotationPosition rotationPositon);
		Task<bool> DeleteProductAsync(int id);
		Task<RotationPosition> GetProductAsync(int id);
		Task<IEnumerable<RotationPosition>> GetProductsAsync();
	}
}

