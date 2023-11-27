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
        Dictionary<int, string> ColumnIdentifierDict = new Dictionary<int, string>() { { 1, "A" }, { 2, "B" }, { 3, "C" }, { 4, "D" }, { 5, "E" }, { 6, "F" }, { 7, "G" }, { 8, "H" }, { 9, "I" }, { 10, "J" }, { 11, "K" }, { 12, "L" }, { 13, "M" }, { 14, "N" }, { 15, "O" }, { 16, "P" }, { 17, "Q" }, { 18, "R" }, { 19, "S" }, { 20, "T" }, { 21, "U" }, { 22, "V" }, { 23, "W" }, { 24, "X" }, { 25, "Y" }, { 26, "Z" }, };
		private readonly IGoogleSheetsService _googleSheetsService;
        public PersonService(IGoogleSheetsService googleSheetsService)
		{
			_googleSheetsService = googleSheetsService;
        }

        public async Task<List<Person>> GetPeople()
		{
			List<List<string>> allSpreadsheetValues = GetAllSpreadsheetValues();
			return allSpreadsheetValues.Select(x => ConvertValuesToPerson(x)).ToList();
		}
        public async Task<Person> GetPerson(string personId)
        {
            int rowIndex = GetRowIndexFromId(personId);
			List<string> personValues = GetValuesById(personId);

			return ConvertValuesToPerson(personValues);
        }
        public async Task CreatePerson(Person person)
		{
			person.PersonID = Guid.NewGuid().ToString();
			List<string> personAsValues = ConvertPersonToValues(person);
			AddPersonToSpreadsheet(personAsValues);

		}
		public async Task UpdatePerson(Person person, string personId)
		{
			person.PersonID = personId;
			List<string> personAsValues = ConvertPersonToValues(person);
            int rowIndex = GetRowIndexFromId(personId);
			UpdatePersonValuesInSpreadsheet(rowIndex, personAsValues);
        }
		public async Task DeletePerson(string personId)
		{
			int rowIndex = GetRowIndexFromId(personId);
			DeletePersonFromSpreadsheet(rowIndex);
		}

		private List<string> GetIds()
		{
			return _googleSheetsService.GetValues("People", "A2:A").Select(x => x.First()).ToList();
		}
		private void AddPersonToSpreadsheet(List<string> person)
		{
			_googleSheetsService.AddRow("People", person, "A:L");
		}
		private void UpdatePersonValuesInSpreadsheet(int rowIndex, List<string> person)
		{
			_googleSheetsService.UpdateRow("People", person, $"A{rowIndex}:L{rowIndex}");
		}
		private void DeletePersonFromSpreadsheet(int rowIndex)
		{
			_googleSheetsService.DeleteRow("People", rowIndex);
		}
        private List<List<string>> GetAllSpreadsheetValues()
        {
            return _googleSheetsService.GetValues("People", "A2:L");
        }
        private List<string> GetValuesById(string personId)
        {
			int rowIndex = GetRowIndexFromId(personId);
			return rowIndex != -1 ? _googleSheetsService.GetValues("People", $"A{rowIndex}:L{rowIndex}").First() : null;
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

            return new Person()
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
				Parent2ID = personValuesCount >= 11 && personValues[10] != "" ? personValues[10] : null
            };
		}

        private List<string> ConvertPersonToValues(Person person)
		{
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
				person.Parent2ID
			};
		}

		private int GetRowIndexFromId(string personId)
		{
            List<string> idList = GetIds();
			return idList.Contains(personId) ? idList.IndexOf(personId) + 2 : -1;
        }
    }
}
