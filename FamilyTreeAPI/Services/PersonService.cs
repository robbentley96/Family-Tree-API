using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FamilyTreeAPI
{
	public class PersonService : IPersonService
	{
		private readonly FamilyTreeContext _context;
		public PersonService(FamilyTreeContext context)
		{
			_context = context;
		}
		public async Task<List<Person>> GetPeople()
		{
			return await _context.People.ToListAsync();
		}
		public async Task CreatePerson(Person person)
		{
			_context.People.Add(person);
			await _context.SaveChangesAsync();
		}
		public async Task UpdatePerson(Person person, int personId)
		{
			Person personToUpdate = await _context.People.FirstOrDefaultAsync(x => x.PersonID == personId);
			personToUpdate.DateOfBirth = person.DateOfBirth ?? personToUpdate.DateOfBirth;
			personToUpdate.DateOfDeath = person.DateOfDeath ?? personToUpdate.DateOfDeath;
			personToUpdate.YearOfBirth = person.YearOfBirth ?? personToUpdate.YearOfBirth;
			personToUpdate.YearOfDeath = person.YearOfDeath ?? personToUpdate.YearOfDeath;
			personToUpdate.Parent1ID = person.Parent1ID ?? personToUpdate.Parent1ID;
			personToUpdate.Parent2ID = person.Parent2ID ?? personToUpdate.Parent2ID;
			personToUpdate.FirstName = !string.IsNullOrWhiteSpace(person.FirstName) ? person.FirstName : personToUpdate.FirstName;
			personToUpdate.MiddleName = !string.IsNullOrWhiteSpace(person.MiddleName) ? person.MiddleName : personToUpdate.MiddleName;
			personToUpdate.LastName = !string.IsNullOrWhiteSpace(person.LastName) ? person.LastName : personToUpdate.LastName;
			personToUpdate.MaidenName = !string.IsNullOrWhiteSpace(person.MaidenName) ? person.MaidenName : personToUpdate.MaidenName;
			await _context.SaveChangesAsync();
		}
		public async Task DeletePerson(int personID)
		{
			Person person = _context.People.Where(x => x.PersonID == personID).FirstOrDefault();
			_context.People.Remove(person);
			await _context.SaveChangesAsync();
		}
	}
}
