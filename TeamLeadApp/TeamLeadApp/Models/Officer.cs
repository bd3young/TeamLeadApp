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
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string RdoOne { get; set; }
		public string RdoTwo { get; set; }
		public string RdoThree { get; set; } = "";
		public string ShiftBegin { get; set; }
		public string ShiftEnd { get; set; }
		public string Gender { get; set; }
		public string Rank { get; set; }
		public string Shift { get; set; }
		public bool ThirdRdo { get; set; } = false;
		public bool FullTime { get; set; } = true;
		public bool BreakOne { get; set; } = false;
		public bool BreakTwo { get; set; } = false;
		public bool Lunch { get; set; } = false;
		public bool Admin { get; set; } = false;
		public string Notes { get; set; } = "";
		public bool Lv { get; set; } = false;
		public bool Ehs { get; set; } = false;
		public string Position { get; set; } = "";
		public string BreakNumber { get; set; } = "";
		//public List<Position> PositionsList { get; set; }
	}
}
