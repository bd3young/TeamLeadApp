using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeamLeadApp.Models
{
    public class Position
    {
		[PrimaryKey, AutoIncrement]

		public int Id { get; set; }
		public string Name { get; set; }
		public string OfficerOne { get; set; }
		public string OfficerTwo { get; set; }
		public bool TwoOfficers { get; set; }
		public string Row { get; set; }
		public string Column { get; set; }
	}
}
