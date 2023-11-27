using System;
using System.Collections.Generic;
using System.Text;

namespace FamilyTreeAPI
{
	public class Person
	{
		public string PersonID { get; set; }
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string MaidenName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public DateTime? DateOfDeath { get; set; }
		public int? YearOfBirth { get; set; }
		public int? YearOfDeath { get; set; }
		public string Parent1ID { get; set; }
		public string Parent2ID { get; set; }

	}
}
