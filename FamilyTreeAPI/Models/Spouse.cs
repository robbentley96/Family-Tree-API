using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeAPI
{
	public class Spouse : PersonSimplifiedDTO
	{
		public DateTime? MarriageDate { get; set; }
		public int? MarriageYear { get; set; }
		public string MarriageID { get; set; }
	}
}
