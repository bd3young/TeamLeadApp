using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeamLeadApp.Models
{
	public class Chore
	{
		[PrimaryKey, AutoIncrement]

		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsCompleted { get; set; } = false;
		public string Time { get; set; }
	}
}
