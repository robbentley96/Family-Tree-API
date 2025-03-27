using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeAPI
{
	public class Marriage
	{
		public string MarriageID { get; set; }
		public string Person1ID { get; set; }
		public string Person2ID { get; set; }
		public DateTime? StartDate { get; set; }
		public int? StartYear { get; set; }
	}
}
