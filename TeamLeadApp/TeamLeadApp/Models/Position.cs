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
	}
}
