using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Newtonsoft.Json;

namespace FamilyTreeAPI
{
	public class PersonService : IPersonService
	{
		private readonly IGoogleSheetsService _googleSheetsService;
		private readonly IMarriageService _marriageService;
        public PersonService(IGoogleSheetsService googleSheetsService, IMarriageService marriageService)
		{
			_googleSheetsService = googleSheetsService;
			_marriageService = marriageService;

		}

        public async Task<List<PersonSimplifiedDTO>> GetPeople()
		{
			List<Person> allPeople = await GetAllPeopleFromSpreadsheet();
			return allPeople.Select(x => ConvertPersonToPersonSimplifiedDTO(x)).ToList();
		}

		private Person GetPerson(string personId, List<Person> people)
        {
			return people.Where(x => x.PersonID == personId).FirstOrDefault();
        }

		private async Task<List<Person>> GetAllPeopleFromSpreadsheet()
		{
			List<List<string>> allSpreadsheetValues = await GetAllSpreadsheetValues();
			return allSpreadsheetValues.Select(x => ConvertValuesToPerson(x)).ToList();
		}


		public async Task<PersonDTO> GetPersonDTO(string personId)
		{
			List<Person> allPeople = await GetAllPeopleFromSpreadsheet();
			Person person = GetPerson(personId, allPeople);
			PersonDTO personDTO = ConvertPersonToPersonDTO(person);
			personDTO.Spouses = await GetSpouses(person, allPeople);
			personDTO.Parents = GetParentsSimplified(person, allPeople);
			personDTO.Children = GetChildren(person, allPeople);
			personDTO.Siblings = GetSiblings(person, allPeople);
			return personDTO;
		}
		public async Task<CreatePersonResponse> CreatePerson(Person person)
		{
			person.PersonID = Guid.NewGuid().ToString();
			SetYearOfBirthAndYearOfDeath(person);
			List<string> personAsValues = ConvertPersonToValues(person);
			AddPersonToSpreadsheet(personAsValues);
			return new CreatePersonResponse() { Id = person.PersonID };

		}
		public async Task UpdatePerson(Person person, string personId)
		{
			person.PersonID = personId;
			SetYearOfBirthAndYearOfDeath(person);
			List<string> personAsValues = ConvertPersonToValues(person);
            int rowIndex = GetRowIndexFromId(personId);
			UpdatePersonValuesInSpreadsheet(rowIndex, personAsValues);
        }
		public async Task DeletePerson(string personId)
		{
			int rowIndex = GetRowIndexFromId(personId);
			DeletePersonFromSpreadsheet(rowIndex);
			List<Marriage> marriages = await GetMarriages(personId);
			foreach (Marriage marriage in marriages)
			{
				await _marriageService.DeleteMarriage(marriage.MarriageID);
			}
		}

		private void SetYearOfBirthAndYearOfDeath(Person person)
		{
			if (person.DateOfBirth != null)
			{
				person.YearOfBirth = person.DateOfBirth.Value.Year;
			}
			if (person.DateOfDeath != null)
			{
				person.YearOfDeath = person.DateOfDeath.Value.Year;
			}
		}

		private List<string> GetIds()
		{
			return _googleSheetsService.GetValues("People", "A2:A").Select(x => x.First()).ToList();
		}

		private List<List<string>> GetParentIds()
		{
			return _googleSheetsService.GetValues("People", "J2:K").ToList();
		}
		private void AddPersonToSpreadsheet(List<string> person)
		{
			_googleSheetsService.AddRow("People", person, "A:M");
		}
		private void UpdatePersonValuesInSpreadsheet(int rowIndex, List<string> person)
		{
			_googleSheetsService.UpdateRow("People", person, $"A{rowIndex}:M{rowIndex}");
		}
		private void DeletePersonFromSpreadsheet(int rowIndex)
		{
			_googleSheetsService.DeleteRow("People", rowIndex);
		}
        private async Task<List<List<string>>> GetAllSpreadsheetValues()
        {
            return _googleSheetsService.GetValues("People", "A2:M");
        }
        private List<string> GetValuesById(string personId)
        {
			int rowIndex = GetRowIndexFromId(personId);
			return rowIndex != -1 ? _googleSheetsService.GetValues("People", $"A{rowIndex}:M{rowIndex}").First() : null;
        }
		private Person ConvertValuesToPerson(List<string> personValues)
		{
			int personValuesCount = personValues.Count;
			DateTime dobResult = new DateTime();
            DateTime dodResult = new DateTime();
			int yobResult = 0;
			int yodResult = 0;
            bool dobIncluded = personValuesCount >= 6 && DateTime.TryParse(personValues[5], out dobResult);
            bool dodIncluded = personValuesCount >= 7 && DateTime.TryParse(personValues[6], out dodResult);
			bool yobIncluded = personValuesCount >= 8 && int.TryParse(personValues[7], out yobResult);
			bool yodIncluded = personValuesCount >= 9 && int.TryParse(personValues[8], out yodResult);
			Person person = new Person()
			{
				PersonID = personValuesCount >= 1 && personValues[0] != "" ? personValues[0] : null,
				FirstName = personValuesCount >= 2 && personValues[1] != "" ? personValues[1] : null,
				MiddleName = personValuesCount >= 3 && personValues[2] != "" ? personValues[2] : null,
				LastName = personValuesCount >= 4 && personValues[3] != "" ? personValues[3] : null,
				MaidenName = personValuesCount >= 5 && personValues[4] != "" ? personValues[4] : null,
				DateOfBirth = dobIncluded ? dobResult : null,
				DateOfDeath = dodIncluded ? dodResult : null,
				YearOfBirth = dobIncluded ? dobResult.Year : personValuesCount >= 8 && yobIncluded ? yobResult : null,
				YearOfDeath = dodIncluded ? dodResult.Year : personValuesCount >= 9 && yodIncluded ? yodResult : null,
				Parent1ID = personValuesCount >= 10 && personValues[9] != "" ? personValues[9] : null,
				Parent2ID = personValuesCount >= 11 && personValues[10] != "" ? personValues[10] : null,
				Notes = personValuesCount >= 12 && personValues[11] != "" ? personValues[11] : null,
				IsAlive = personValuesCount >= 13 && personValues[12] != "" ? bool.Parse(personValues[12]) : false,
			};
			RemoveDateOfDeathIfAlive(person);
			return person;
		}

		private PersonSimplifiedDTO ConvertPersonToPersonSimplifiedDTO(Person person)
		{
			string deathDates = person.IsAlive ? "present" : person.YearOfDeath?.ToString() ?? "????";
			return new PersonSimplifiedDTO()
			{
				PersonID = person.PersonID,
				Name = $"{person.FirstName} {person.LastName}",
				Dates = $"{person.YearOfBirth?.ToString() ?? "????"} - {deathDates}"
			};
		}

		private PersonDTO ConvertPersonToPersonDTO(Person person)
		{
			PersonDTO personDTO = new PersonDTO()
			{
				FirstName = person.FirstName,
				MiddleName = person.MiddleName,
				LastName = person.LastName,
				MaidenName = person.MaidenName,
				DateOfBirth = person.DateOfBirth,
				DateOfDeath = person.DateOfDeath,
				YearOfBirth = person.YearOfBirth,
				YearOfDeath = person.YearOfDeath,
				Notes = person.Notes,
				IsAlive=person.IsAlive
			};
			return personDTO;
		}

        private List<string> ConvertPersonToValues(Person person)
		{
			RemoveDateOfDeathIfAlive(person);
			return new List<string>()
			{
				person.PersonID,
				person.FirstName,
				person.MiddleName,
				person.LastName,
				person.MaidenName,
				person.DateOfBirth.ToString(),
				person.DateOfDeath.ToString(),
				person.YearOfBirth.ToString(),
				person.YearOfDeath.ToString(),
				person.Parent1ID,
				person.Parent2ID,
				person.Notes,
				person.IsAlive.ToString()
			};
		}

		private void RemoveDateOfDeathIfAlive(Person person)
		{
			if (person.IsAlive)
			{
				person.DateOfDeath = null;
				person.YearOfDeath = null;
			}
		}

		private int GetRowIndexFromId(string personId)
		{
            List<string> idList = GetIds();
			return idList.Contains(personId) ? idList.IndexOf(personId) + 2 : -1;
        }

		private List<int> GetChildrenRowIndicesFromId(string personId)
		{
			List<List<string>> idList = GetParentIds();
			List<int> ids = new List<int>();
			foreach (List<string> id in idList)
			{
				if (id.Contains(personId))
				{
					ids.Add(idList.IndexOf(id) + 2);
				}
				
			}
			return ids;
		}

		private async Task<List<Spouse>> GetSpouses(Person person, List<Person> allPeople)
		{

			List<Marriage> marriages = await GetMarriages(person.PersonID);
			List<Spouse> spouses = new List<Spouse>();
			foreach (Marriage marriage in marriages)
			{
				Person personSpouse = GetPerson(marriage.Person1ID == person.PersonID ? marriage.Person2ID : marriage.Person1ID, allPeople);
				PersonSimplifiedDTO personSDTO = ConvertPersonToPersonSimplifiedDTO(personSpouse);
				Spouse spouse = new Spouse()
				{
					PersonID = personSDTO.PersonID,
					Name = personSDTO.Name,
					Dates = personSDTO.Dates,
					MarriageDate = marriage.StartDate,
					MarriageYear = marriage.StartYear,
					MarriageID = marriage.MarriageID
				};
				spouses.Add(spouse);
			}
			return spouses.OrderBy(x => x.MarriageDate).ToList();
		}

		private async Task<List<Marriage>> GetMarriages(string personId)
		{
			List<Marriage> allMarriages = await _marriageService.GetMarriages();
			return allMarriages.Where(x => (!string.IsNullOrEmpty(x.Person1ID) && x.Person1ID == personId) || (!string.IsNullOrEmpty(x.Person2ID) && x.Person2ID == personId)).ToList();
		}

		private List<PersonSimplifiedDTO> GetParentsSimplified(Person person, List<Person> allPeople)
		{
			List<Person> parents = GetParents(person, allPeople);
			return parents.Select(x => ConvertPersonToPersonSimplifiedDTO(x)).ToList();
		}

		private List<Person> GetParents(Person person, List<Person> allPeople)
		{
			List<Person> parents = new List<Person>();
			if (person.Parent1ID != null) { parents.Add(GetPerson(person.Parent1ID, allPeople)); }
			if (person.Parent2ID != null) { parents.Add(GetPerson(person.Parent2ID, allPeople)); }
			return parents.Where(x => x != null).ToList();
		}

		private List<PersonSimplifiedDTO> GetChildren(Person person, List<Person> allPeople)
		{
			List<Person> children = allPeople.Where(x => (!string.IsNullOrEmpty(x.Parent1ID) && x.Parent1ID == person.PersonID) || (!string.IsNullOrEmpty(x.Parent2ID) && x.Parent2ID == person.PersonID)).ToList();
			return children.Select(x => ConvertPersonToPersonSimplifiedDTO(x)).ToList();
		}
		private List<PersonSimplifiedDTO> GetSiblings(Person person, List<Person> allPeople)
		{
			List<Person> parents = GetParents(person, allPeople);
			List<PersonSimplifiedDTO> siblings = new List<PersonSimplifiedDTO>();
            foreach (Person parent in parents)
            {
				List<PersonSimplifiedDTO> children = GetChildren(parent, allPeople);
				foreach (PersonSimplifiedDTO child in children)
				{
					if (child.PersonID != person.PersonID && siblings.Where(x => x.PersonID == child.PersonID).Count() == 0)
					{
						siblings.Add(child);
					}
				}
            }
			return siblings;
		}
	}
}
