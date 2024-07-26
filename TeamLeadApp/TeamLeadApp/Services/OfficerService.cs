using SQLite;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TeamLeadApp.Models;

namespace TeamLeadApp.Services
{
	public class OfficerService : IOfficerRepository
	{
		public SQLiteAsyncConnection _database;
		List<Officer> initOfficers;
		public OfficerService(string dbPath) 
		{

			_database = new SQLiteAsyncConnection(dbPath);
			_database.CreateTableAsync<Officer>().Wait();

			//AddInitOfficers();

		}

		public async void AddInitOfficers()
		{
			int initOfficerCount = 0;

			var officerList = await App.OfficerService.GetProductsAsync();

			initOfficers = new List<Officer>()
			{
				//PM
new Officer { FirstName = "Jeremy", LastName = "Wiley", RdoOne = "Friday", RdoTwo = "Saturday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "M", Rank = "SUP", Shift = "PM", FullTime = true },
new Officer { FirstName = "Lynette", LastName = "Zimmerman", RdoOne = "Sunday", RdoTwo = "Monday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "F", Rank = "SUP", Shift = "PM", FullTime = true },
new Officer { FirstName = "Beau", LastName = "DeYoung", RdoOne = "Sunday", RdoTwo = "Monday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "M", Rank = "LEAD", Shift = "PM", FullTime = true },
new Officer { FirstName = "Rueben", LastName = "Torres", RdoOne = "Friday", RdoTwo = "Saturday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "M", Rank = "LEAD", Shift = "PM", FullTime = true },
new Officer { FirstName = "Martha", LastName = "Fluelling", RdoOne = "Friday", RdoTwo = "Saturday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "F", Rank = "OFFICER", Shift = "PM", FullTime = true },
new Officer { FirstName = "Missy", LastName = "Buskirk", RdoOne = "Sunday", RdoTwo = "Monday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "F", Rank = "OFFICER", Shift = "PM", FullTime = true },
new Officer { FirstName = "Loraine", LastName = "Russell", RdoOne = "Sunday", RdoTwo = "Monday", RdoThree = "Tuesday", ShiftBegin = new System.TimeSpan(13,00,00), ShiftEnd = new System.TimeSpan(18,00,00), Gender = "F", Rank = "OFFICER", Shift = "PM", FullTime = false },
new Officer { FirstName = "Marla", LastName = "Weston", RdoOne = "Tuesday",RdoTwo = "Wednesday", RdoThree = "Thursday", ShiftBegin = new System.TimeSpan(13,00,00), ShiftEnd = new System.TimeSpan(18,00,00), Gender = "F", Rank = "OFFICER", Shift = "PM", FullTime = false },
new Officer { FirstName = "Andrew", LastName = "Steele", RdoOne = "Sunday", RdoTwo = "Monday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "M", Rank = "OFFICER", Shift = "PM", FullTime = true },
new Officer { FirstName = "Penny", LastName = "Nitelet", RdoOne = "Tuesday", RdoTwo = "Wednesday", RdoThree = "Thursday", ShiftBegin = new System.TimeSpan(13,00,00), ShiftEnd = new System.TimeSpan(18,00,00), Gender = "F", Rank = "OFFICER", Shift = "PM", FullTime = false },
new Officer { FirstName = "Derek", LastName = "Knoll", RdoOne = "Wednesday", RdoTwo = "Thursday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "M", Rank = "OFFICER", Shift = "PM", FullTime = true },
new Officer { FirstName = "Tom", LastName = "Oleniacz", RdoOne = "Tuesday", RdoTwo = "Wednesday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "M", Rank = "OFFICER", Shift = "PM", FullTime = true },
new Officer { FirstName = "Dallas", LastName = "Duberg", RdoOne = "Thursday", RdoTwo = "Friday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "M", Rank = "OFFICER", Shift = "PM", FullTime = true },
new Officer { FirstName = "Thomas", LastName = "Hutchins", RdoOne = "Friday", RdoTwo = "Saturday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "M", Rank = "OFFICER", Shift = "PM", FullTime = true },
new Officer { FirstName = "Nate", LastName = "Blessing", RdoOne = "Monday", RdoTwo = "Tuesday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "M", Rank = "OFFICER", Shift = "PM", FullTime = true },
new Officer { FirstName = "Joan", LastName = "Nell", RdoOne = "Wednesday", RdoTwo = "Thursday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "F", Rank = "OFFICER", Shift = "PM", FullTime = true },
new Officer { FirstName = "Allyson", LastName = "Osga", RdoOne = "Thursday", RdoTwo = "Friday", ShiftBegin = new System.TimeSpan(11,00,00), ShiftEnd = new System.TimeSpan(19,30,00), Gender = "F", Rank = "OFFICER", Shift = "PM", FullTime = true },
//AM
new Officer { FirstName = "Mariane", LastName = "Ingersoll", RdoOne = "Friday", RdoTwo = "Saturday", ShiftBegin = new System.TimeSpan(03,45,00), ShiftEnd = new System.TimeSpan(12,15,00), Gender = "F", Rank = "SUP", Shift = "AM", FullTime = true },
new Officer { FirstName = "Josh", LastName = "Laporte", RdoOne = "Friday", RdoTwo = "Saturday", ShiftBegin = new System.TimeSpan(03,45,00), ShiftEnd = new System.TimeSpan(12,15,00), Gender = "M", Rank = "SUP", Shift = "PM", FullTime = true },
new Officer { FirstName = "Michelle", LastName = "Postal", RdoOne = "Sunday", RdoTwo = "Monday", ShiftBegin = new System.TimeSpan(03,45,00), ShiftEnd = new System.TimeSpan(12,15,00), Gender = "F", Rank = "SUP", Shift = "AM", FullTime = true },
new Officer { FirstName = "Lindsay", LastName = "Peaslee", RdoOne = "Sunday", RdoTwo = "Monday", ShiftBegin = new System.TimeSpan(03,45,00), ShiftEnd = new System.TimeSpan(12,15,00), Gender = "F", Rank = "LEAD", Shift = "AM", FullTime = true },
new Officer { FirstName = "Amy", LastName = "Vanslembrouck", RdoOne = "Friday", RdoTwo = "Saturday", ShiftBegin = new System.TimeSpan(03,45,00), ShiftEnd = new System.TimeSpan(12,15,00), Gender = "F", Rank = "LEAD", Shift = "AM", FullTime = true },
new Officer { FirstName = "James", LastName = "Nuckels", RdoOne = "Friday", RdoTwo = "Saturday", ShiftBegin = new System.TimeSpan(03,45,00), ShiftEnd = new System.TimeSpan(12,15,00), Gender = "M", Rank = "LEAD", Shift = "AM", FullTime = true },
new Officer { FirstName = "Karl", LastName = "Hauer", RdoOne = "Friday", RdoTwo = "Saturday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "M", Rank = "OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Julie", LastName = "Smythe", RdoOne = "Friday", RdoTwo = "Saturday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "F", Rank ="OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Richard", LastName = "Sparks", RdoOne = "Sunday", RdoTwo = "Monday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "M", Rank ="OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Sean", LastName = "Seekins", RdoOne = "Tuesday", RdoTwo = "Wednesday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "M", Rank = "OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Jessica", LastName = "Wilson", RdoOne = "Thursday", RdoTwo = "Friday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "F", Rank = "OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Mike", LastName = "Simkunas", RdoOne = "Sunday", RdoTwo = "Monday", RdoThree = "Tuesday", ShiftBegin = new System.TimeSpan(05,00,00), ShiftEnd = new System.TimeSpan(10,00,00), Gender = "M", Rank = "OFFICER", Shift = "AM", FullTime = false },
new Officer { FirstName = "Zii", LastName = "Rios", RdoOne = "Thursday", RdoTwo = "Friday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "M", Rank = "OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Erika", LastName = "Kolatski", RdoOne = "Sunday", RdoTwo = "Monday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "F", Rank = "OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Dave", LastName = "Sheppard", RdoOne = "Monday", RdoTwo = "Tuesday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "M", Rank = "OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Amy", LastName = "Taylor", RdoOne = "Tuesday", RdoTwo = "Wednesday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "F", Rank = "OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Mark", LastName = "Kissling", RdoOne = "Tuesday", RdoTwo = "Wednesday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "M", Rank = "OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Stacy", LastName = "Munjoy", RdoOne = "Wednesday", RdoTwo = "Thursday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "F", Rank = "OFFICER", Shift = "AM", FullTime = true },
new Officer { FirstName = "Reggie", LastName = "Torres", RdoOne = "Wednesday", RdoTwo = "Thursday", ShiftBegin = new System.TimeSpan(04,00,00), ShiftEnd = new System.TimeSpan(12,30,00), Gender = "M", Rank = "OFFICER", Shift = "AM", FullTime = true },
//MID
new Officer { FirstName = "Craig", LastName = "Feese", RdoOne = "Saturday", RdoTwo = "Sunday", ShiftBegin = new System.TimeSpan(07,00,00), ShiftEnd = new System.TimeSpan(15,30,00), Gender = "M", Rank = "SUP", Shift = "MID", FullTime = true, Admin = true },
new Officer { FirstName = "Tim", LastName = "Elmer", RdoOne = "Saturday", RdoTwo = "Sunday", ShiftBegin = new System.TimeSpan(08,00,00), ShiftEnd = new System.TimeSpan(16,30,00), Gender = "M", Rank = "LEAD", Shift = "MID", FullTime = true, Admin = true },
new Officer { FirstName = "Nancy", LastName = "Farkas", RdoOne = "Saturday", RdoTwo = "Sunday", ShiftBegin = new System.TimeSpan(08,00,00), ShiftEnd = new System.TimeSpan(16,30,00), Gender = "F", Rank = "LEAD", Shift = "MID", FullTime = true, Admin = true },
new Officer { FirstName = "Conrad", LastName = "Stein", RdoOne = "Wednesday", RdoTwo = "Thursday", ShiftBegin = new System.TimeSpan(09,30,00), ShiftEnd = new System.TimeSpan(15,30,00), Gender = "M", Rank = "OFFICER", Shift = "MID", FullTime = true },
new Officer { FirstName = "Hector", LastName = "Lopez", RdoOne = "Tuesday", RdoTwo = "Wednesday", ShiftBegin = new System.TimeSpan(09,30,00), ShiftEnd = new System.TimeSpan(18,00,00), Gender = "M", Rank = "OFFICER", Shift = "MID", FullTime = true },
new Officer { FirstName = "Antoinette", LastName = "Robertson", RdoOne = "Tuesday", RdoTwo = "Wednesday", ShiftBegin = new System.TimeSpan(09,30,00), ShiftEnd = new System.TimeSpan(18,00,00), Gender = "F", Rank = "LEAD", Shift = "MID", FullTime = true },
new Officer { FirstName = "Zachary", LastName = "Johnson", RdoOne = "Tuesday", RdoTwo = "Wednesday", ShiftBegin = new System.TimeSpan(09,30,00), ShiftEnd = new System.TimeSpan(18,00,00), Gender = "M", Rank = "OFFICER", Shift = "MID", FullTime = true },
new Officer { FirstName = "Hatika", LastName = "James", RdoOne = "Wednesday", RdoTwo = "Thursday", ShiftBegin = new System.TimeSpan(09,30,00), ShiftEnd = new System.TimeSpan(18,00,00), Gender = "F", Rank = "OFFICER", Shift = "MID", FullTime = true },
new Officer { FirstName = "John", LastName = "Hammell", RdoOne = "Wednesday", RdoTwo = "Thursday", ShiftBegin = new System.TimeSpan(09,30,00), ShiftEnd = new System.TimeSpan(18,00,00), Gender = "M", Rank = "LEAD", Shift = "MID", FullTime = true },
new Officer { FirstName = "Sylvia", LastName = "CruzSalazar", RdoOne = "Wednesday", RdoTwo = "Thursday", ShiftBegin = new System.TimeSpan(09,30,00), ShiftEnd = new System.TimeSpan(18,00,00), Gender = "F", Rank = "OFFICER", Shift = "MID", FullTime = true },
new Officer { FirstName = "Rafael", LastName = "Quinones", RdoOne = "Wednesday", RdoTwo = "Thursday", ShiftBegin = new System.TimeSpan(09,30,00), ShiftEnd = new System.TimeSpan(18,00,00), Gender = "M", Rank = "OFFICER", Shift = "MID", FullTime = true },
new Officer { FirstName = "Zack", LastName = "Meyer", RdoOne = "Tuesday", RdoTwo = "Wednesday", ShiftBegin = new System.TimeSpan(05,00,00), ShiftEnd = new System.TimeSpan(13,30,00), Gender = "M", Rank = "OFFICER", Shift = "MID", FullTime = true },
			};

			foreach (var initOfficer in initOfficers)
			{

				foreach (var exOfficer in officerList)
				{
					if (initOfficer.LastName == exOfficer.LastName )
					{
						initOfficerCount++;
					}
					
				}

				if (initOfficerCount == 0) 
				{
					await _database.InsertAsync(initOfficer);
					initOfficerCount = 0;
				}
				else
				{
					continue;
				}
			}

		}

		//Insert & Update
		public async Task<bool> AddProductAsync(Officer officer)
		{
			if (officer.Id > 0) 
			{
				await _database.UpdateAsync(officer);
			}
			else
			{
				await _database.InsertAsync(officer);
			}
			return await Task.FromResult(true);
		}

		public async Task<bool> DeleteProductAsync(int id)
		{
			await _database.DeleteAsync<Officer>(id);
			return await Task.FromResult(true);
		}

		public async Task<Officer> GetProductAsync(int id)
		{
			return await _database.Table<Officer>().Where(p => p.Id == id).FirstOrDefaultAsync();
		}

		public async Task<IEnumerable<Officer>> GetProductsAsync()
		{
			return await Task.FromResult(await _database.Table<Officer>().ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetRankOfficersAsync(string rank) 
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Rank == rank).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetShiftOfficersAsync(string shift, string day)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Shift == shift && o.Lv == false && o.Ehs == false && o.Admin == false && day != o.RdoOne.ToUpper() && day != o.RdoTwo.ToUpper() && day != o.RdoThree.ToUpper()).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetAdminOfficersAsync(bool admin)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Admin == admin && o.Ehs == true).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetShiftRankOfficersAsync(string shift, string rank, bool fullTime, string day)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Shift == shift && o.Rank == rank && o.FullTime == fullTime && o.Ehs == false && o.Lv == false && o.Admin == false && day != o.RdoOne.ToUpper() && day != o.RdoTwo.ToUpper() && day != o.RdoThree.ToUpper()
			).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetEhsOfficersAsync(string shift)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Shift == shift && o.Ehs == true && o.Admin == false).ToListAsync());
		}

		public async Task<IEnumerable<Officer>> GetAdminRankOfficersAsync(bool admin, string rank)
		{
			return await Task.FromResult(await _database.Table<Officer>().Where(o => o.Admin == admin && o.Rank == rank && o.Ehs == true).ToListAsync());
		}

		public async Task<Officer> GetOfficersByNameAsync(string firstName, string lastName)
		{
			return await _database.Table<Officer>().Where(o => o.FirstName == firstName && o.LastName == lastName).FirstOrDefaultAsync();
		}

		public Task<bool> UpdateProductAsync(Officer officer)
		{
			throw new NotImplementedException();
		}

		public async Task ResetOfficers()
		{
			var rotations = await App.RotationService.GetProductsAsync();
			foreach (var rotation in rotations)
			{
				rotation.IsComplete = false;

				await App.RotationService.AddProductAsync(rotation);
			}

			var officerList = await App.OfficerService.GetProductsAsync();
			foreach (var officer in officerList)
			{
				officer.BreakOne = false;
				officer.BreakTwo = false;
				officer.Lunch = false;
				officer.Notes = "";
				officer.Lv = false;
				officer.Ehs = false;
				officer.BreakNumber = "";
				if (officer.ShiftEnd - officer.ShiftBegin == new System.TimeSpan(08,30,00))
				{
					officer.FullTime = true;
				}
				else
				{
					officer.FullTime = false;
				}
				if (officer.ShiftBegin >= new System.TimeSpan(03, 00, 00) && officer.ShiftEnd <= new System.TimeSpan(12, 30, 00) && officer.Admin == false)
				{
					officer.Shift = "AM";

				}
				else if (officer.ShiftBegin >= new System.TimeSpan(10, 30, 00) && officer.ShiftEnd <= new System.TimeSpan(20, 00, 00) && officer.Admin == false)
				{
					officer.Shift = "PM";
				}
				else 
				{
					officer.Shift = "MID";
				}

				await App.OfficerService.AddProductAsync(officer);
			}
			var choreList = await App.ChoreService.GetProductsAsync();
			foreach (var chore in choreList)
			{
				chore.IsCompleted = false;
				chore.Time = new System.TimeSpan();

				await App.ChoreService.AddProductAsync(chore);
			}
			var positions = await App.RotationPositionService.GetProductsAsync();
			foreach (var position in positions) 
			{
				position.OfficerOne = "";
				position.OfficerTwo = "";
				position.OfficerOneGender = "";
				position.OfficerTwoGender = "";

				await App.RotationPositionService.AddProductAsync(position);
			}
			
		}

	}
}
