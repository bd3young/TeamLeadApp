using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeamLeadApp.Models
{
	[Table("Positions")]
	public class Rotation
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public System.TimeSpan RotationTime { get; set; }

		[OneToMany]
		public List<Position> Positions { get; set; }
	}
}
