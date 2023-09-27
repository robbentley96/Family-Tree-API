using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeAPI
{
	public class Marriage
	{
		public int MarriageID { get; set; }
		public int Person1ID { get; set; }
		public int Person2ID { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public int StartYear { get; set; }
		public int EndYear { get; set; }
	}
}
