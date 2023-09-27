using System;
using System.Linq;
using System.Threading.Tasks;
using FamilyTreeAPI;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace FamilyTreeAPITests
{
	public class PersonServiceTests
	{
		private FamilyTreeContext _context;
		private IPersonService _personService;
		[OneTimeSetUp]
		public void OneTimeSetup()
		{
			var options = new DbContextOptionsBuilder<FamilyTreeContext>()
				.UseInMemoryDatabase(databaseName: "Test_DB")
				.Options;
			_context = new FamilyTreeContext(options);
			_personService = new PersonService(_context);
		}

		[TearDown]
		public void TearDown()
		{
			var peopleToRemove = _context.People;
			_context.RemoveRange(peopleToRemove);
			_context.SaveChanges();
		}

		[Test]
		public async Task CreatePersonIncreasesTheNumberOfPeopleInTheDatabaseByOne()
		{
			// Arrange
			Person person = new Person() {
				FirstName = "Robert",
				MiddleName = "William",
				LastName = "Bentley",
				DateOfBirth = new DateTime(2000, 1, 31),
				DateOfDeath = new DateTime(2012, 1, 31)
			};
			var numberOfPeopleBeforeCallingMethod = _context.People.ToList().Count();
			// Act
			await _personService.CreatePerson(person);
			// Assert
			var numberOfPeopleAfterCallingMethod = _context.People.ToList().Count();
			Assert.That(numberOfPeopleAfterCallingMethod, Is.EqualTo(numberOfPeopleBeforeCallingMethod + 1));
		}

		[Test]
		public async Task CreatePersonAddsAPersonWithTheCorrectDetailsToTheDatabase()
		{
			// Arrange
			Person person = new Person()
			{
				FirstName = "Robert",
				MiddleName = "William",
				LastName = "Bentley",
				DateOfBirth = new DateTime(2000, 1, 31),
				DateOfDeath = new DateTime(2012, 1, 31)
			};
			// Act
			await _personService.CreatePerson(person);
			// Assert
			Person personAddedToDb = _context.People.FirstOrDefault();
			Assert.That(personAddedToDb.FirstName, Is.EqualTo(person.FirstName));
			Assert.That(personAddedToDb.MiddleName, Is.EqualTo(person.MiddleName));
			Assert.That(personAddedToDb.LastName, Is.EqualTo(person.LastName));
			Assert.That(personAddedToDb.DateOfBirth, Is.EqualTo(person.DateOfBirth));
			Assert.That(personAddedToDb.DateOfDeath, Is.EqualTo(person.DateOfDeath));
		}

		[Test]
		public async Task DeletePersonRemovesThePersonWithTheSpecifiedID()
		{
			// Arrange
			Person person = new Person()
			{
				FirstName = "Robert",
				MiddleName = "William",
				LastName = "Bentley",
				DateOfBirth = new DateTime(2000, 1, 31),
				DateOfDeath = new DateTime(2012, 1, 31)
			};
			_context.People.Add(person);
			_context.SaveChanges();
			int idToDelete = _context.People.FirstOrDefault().PersonID;
			// Act
			await _personService.DeletePerson(idToDelete);
			// Assert
			Assert.That(_context.People.Any(x => x.PersonID == idToDelete), Is.False);
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		public async Task GetPeopleReturnsTheCorrectNumberOfpeople(int numberOfPeople)
		{
			// Arrange
			for (int i = 0; i < numberOfPeople; i++)
			{
				Person person = new Person()
				{
					FirstName = "Robert",
					MiddleName = "William",
					LastName = "Bentley",
					DateOfBirth = new DateTime(2000, 1, 31),
					DateOfDeath = new DateTime(2012, 1, 31)
				};
				_context.People.Add(person);
			}
			_context.SaveChanges();
			// Act
			var personList = await _personService.GetPeople();
			// Assert
			Assert.That(personList.Count(), Is.EqualTo(numberOfPeople));
		}

		[Test]
		public async Task UpdatePersonUpdatesTheDetailsOfTheSpecifiedPerson()
		{
			// Arrange
			Person person = new Person()
			{
				FirstName = "Robert",
				MiddleName = "William",
				LastName = "Bentley",
				DateOfBirth = new DateTime(2000, 1, 31),
				DateOfDeath = new DateTime(2012, 1, 31)
			};
			_context.People.Add(person);
			_context.SaveChanges();
			int idToUpdate = _context.People.FirstOrDefault().PersonID;
			Person newPersonDetails = new Person()
			{
				FirstName = "Thomas",
				MiddleName = "Andrew",
				LastName = "Bentley",
				DateOfBirth = new DateTime(2000, 6, 24),
				DateOfDeath = new DateTime(2013, 9, 11)
			};
			// Act
			await _personService.UpdatePerson(newPersonDetails,idToUpdate);
			// Assert
			Person personUpdatedInDb = _context.People.FirstOrDefault(x => x.PersonID == idToUpdate);
			Assert.That(personUpdatedInDb.FirstName, Is.EqualTo(newPersonDetails.FirstName));
			Assert.That(personUpdatedInDb.MiddleName, Is.EqualTo(newPersonDetails.MiddleName));
			Assert.That(personUpdatedInDb.LastName, Is.EqualTo(newPersonDetails.LastName));
			Assert.That(personUpdatedInDb.DateOfBirth, Is.EqualTo(newPersonDetails.DateOfBirth));
			Assert.That(personUpdatedInDb.DateOfDeath, Is.EqualTo(newPersonDetails.DateOfDeath));
		}
	}
}