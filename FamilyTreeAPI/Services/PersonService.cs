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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FamilyTreeAPI
{
	public class PersonService : IPersonService
	{
		public PersonService()
		{
			
		}

        public async Task<List<Person>> GetPeople()
		{
			return new List<Person>();
		}
		public async Task CreatePerson(Person person)
		{
            GoogleSheetsService GSService = new GoogleSheetsService();
			GSService.AddRow();
		}
		public async Task UpdatePerson(Person person, int personId)
		{
			
		}
		public async Task DeletePerson(int personID)
		{
			
		}
	}
}
