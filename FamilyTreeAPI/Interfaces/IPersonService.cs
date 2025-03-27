using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeAPI
{
	public interface IPersonService
	{
		public Task<List<PersonSimplifiedDTO>> GetPeople();
		public Task<PersonDTO> GetPersonDTO(string personId);

        public Task<CreatePersonResponse> CreatePerson(Person person);
		public Task UpdatePerson(Person person, string personId);
		public Task DeletePerson(string personID);
	}
}
