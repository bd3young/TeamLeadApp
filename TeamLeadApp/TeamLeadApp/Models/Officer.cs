using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeamLeadApp.Models
{
	public class Officer
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }	
		public String FirstName { get; set; }
		public String LastName { get; set; }
		public String Rdo { get; set; }
		public int ShiftBegin { get; set; }
		public int ShiftEnd { get; set; }
		public string Gender { get; set; }
		public string Rank { get; set; }
		public string Shift { get; set; }
	}
}
