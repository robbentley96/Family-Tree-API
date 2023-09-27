using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeAPI
{
	public interface IPersonService
	{
		public Task<List<Person>> GetPeople();
		public Task CreatePerson(Person person);
		public Task UpdatePerson(Person person, int personId);
		public Task DeletePerson(int personID);
	}
}
