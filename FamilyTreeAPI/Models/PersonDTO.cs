using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeAPI
{
	public class PersonDTO
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string MaidenName { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public DateTime? DateOfDeath { get; set; }
		public int? YearOfBirth { get; set; }
		public int? YearOfDeath { get; set; }
		public string Notes { get; set; }
		public bool IsAlive { get; set; }
		public List<Marriage> Marriages { get; set; }
		public List<PersonSimplifiedDTO> Children { get; set; }
		public List<PersonSimplifiedDTO> Parents { get; set; }
		public List<Spouse> Spouses { get; set; }
		public List<PersonSimplifiedDTO> Siblings { get; set; }
	}
}
