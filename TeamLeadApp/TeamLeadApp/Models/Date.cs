using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeamLeadApp.Models
{
	public class Date
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Day { get; set; }
	}
}
