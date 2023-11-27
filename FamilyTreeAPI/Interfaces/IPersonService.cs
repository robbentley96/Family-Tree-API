using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeAPI
{
	public interface IPersonService
	{
		public Task<List<Person>> GetPeople();
		public Task<Person> GetPerson(string personId);

        public Task CreatePerson(Person person);
		public Task UpdatePerson(Person person, string personId);
		public Task DeletePerson(string personID);
	}
}
