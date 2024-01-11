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
		public String RdoOne { get; set; }
		public String RdoTwo { get; set; }
		public int ShiftBegin { get; set; }
		public int ShiftEnd { get; set; }
		public string Gender { get; set; }
		public string Rank { get; set; }
		public string Shift { get; set; }
		public bool FullTime { get; set; } = true;
		public bool BreakOne { get; set; } = false;
		public bool BreakTwo { get; set; } = false;
		public bool Lunch { get; set; } = false;
		public string Notes { get; set; } = "";
		public bool Lv { get; set; } = false;
		public bool Ehs { get; set; } = false;
	}
}
