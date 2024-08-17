using SQLite;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeamLeadApp.Models
{
	[Table("Positions")]
	public class RotationPosition
	{
		[PrimaryKey, AutoIncrement]

		public int Id { get; set; }
		public string Name { get; set; }
		public string OfficerOne { get; set; } = "";
		public string OfficerTwo { get; set; } = "";
		public bool TwoOfficers { get; set; }
		public string OfficerOneGender { get; set; } = "";
		public string OfficerTwoGender { get; set; } = "";

		[ForeignKey(typeof(Rotation))]
		public int RotationId { get; set; }
	}
}
