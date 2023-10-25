using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace TeamLeadApp.Models
{
	public class Officer
	{
		[PrimaryKey, AutoIncrement]
		public int OfficerId { get; set; }	
		public String OfficerFirstName { get; set; }
		public String OfficerLastName { get; set; }
		public String OfficerRdo { get; set; }
		public int OfficerShiftBegin { get; set; }
		public int OfficerShiftEnd { get; set; }
		public string OfficerGender { get; set; }
		public string OfficerRank { get; set; }

	}
}
