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
	public class MarriageService : IMarriageService
	{
		private readonly IGoogleSheetsService _googleSheetsService;
		public MarriageService(IGoogleSheetsService googleSheetsService)
		{
			_googleSheetsService = googleSheetsService;
		}

		public async Task<List<Marriage>> GetMarriages()
		{
			List<List<string>> allSpreadsheetValues = await GetAllSpreadsheetValues();
			return allSpreadsheetValues.Select(x => ConvertValuesToMarriage(x)).ToList();
		}
		private async Task<Marriage> GetMarriage(string marriageId)
		{
			int rowIndex = GetRowIndexFromId(marriageId);
			List<string> marriageValues = GetValuesById(marriageId);

			return ConvertValuesToMarriage(marriageValues);
		}

		
		public async Task CreateMarriage(Marriage marriage)
		{
			marriage.MarriageID = Guid.NewGuid().ToString();
			SetStartYear(marriage);
			List<string> marriageAsValues = ConvertMarriageToValues(marriage);
			AddMarriageToSpreadsheet(marriageAsValues);

		}
		public async Task UpdateMarriage(Marriage marriage, string marriageId)
		{
			marriage.MarriageID = marriageId;
			SetStartYear(marriage);
			List<string> marriageAsValues = ConvertMarriageToValues(marriage);
			int rowIndex = GetRowIndexFromId(marriageId);
			UpdateMarriageValuesInSpreadsheet(rowIndex, marriageAsValues);
		}
		public async Task DeleteMarriage(string marriageId)
		{
			int rowIndex = GetRowIndexFromId(marriageId);
			DeleteMarriageFromSpreadsheet(rowIndex);
		}

		private void SetStartYear(Marriage marriage)
		{
			if (marriage.StartDate != null)
			{
				marriage.StartYear = marriage.StartDate.Value.Year;
			}
		}

		private List<string> GetIds()
		{
			return _googleSheetsService.GetValues("Marriages", "A2:A").Select(x => x.First()).ToList();
		}

		private List<(string,string,string)> GetMarriageAndPersonIds()
		{
			return _googleSheetsService.GetValues("Marriages", "A2:C").Select(x => (x[0],x[1],x[2])).ToList();
		}
		private void AddMarriageToSpreadsheet(List<string> person)
		{
			_googleSheetsService.AddRow("Marriages", person, "A:E");
		}
		private void UpdateMarriageValuesInSpreadsheet(int rowIndex, List<string> marriage)
		{
			_googleSheetsService.UpdateRow("Marriages", marriage, $"A{rowIndex}:E{rowIndex}");
		}
		private void DeleteMarriageFromSpreadsheet(int rowIndex)
		{
			_googleSheetsService.DeleteRow("Marriages", rowIndex);
		}
		private async Task<List<List<string>>> GetAllSpreadsheetValues()
		{
			return _googleSheetsService.GetValues("Marriages", "A2:E");
		}
		private List<string> GetValuesById(string marriageId)
		{
			int rowIndex = GetRowIndexFromId(marriageId);
			return rowIndex != -1 ? _googleSheetsService.GetValues("Marriages", $"A{rowIndex}:E{rowIndex}").First() : null;
		}
		private Marriage ConvertValuesToMarriage(List<string> marriageValues)
		{
			int marriageValuesCount = marriageValues.Count;
			DateTime startDateResult = new DateTime();
			int startYearResult = 0;
			bool startDateIncluded = marriageValuesCount >= 4 && DateTime.TryParse(marriageValues[3], out startDateResult);
			bool startYearIncluded = marriageValuesCount >= 5 && int.TryParse(marriageValues[4], out startYearResult);

			return new Marriage()
			{
				MarriageID = marriageValuesCount >= 1 && marriageValues[0] != "" ? marriageValues[0] : null,
				Person1ID = marriageValuesCount >= 2 && marriageValues[1] != "" ? marriageValues[1] : null,
				Person2ID = marriageValuesCount >= 3 && marriageValues[2] != "" ? marriageValues[2] : null,
				StartDate = startDateIncluded ? startDateResult : null,
				StartYear = startDateIncluded ? startDateResult.Year : marriageValuesCount >= 4 && startYearIncluded ? startYearResult : null,
			};
		}

		private List<string> ConvertMarriageToValues(Marriage marriage)
		{
			return new List<string>()
			{
				marriage.MarriageID,
				marriage.Person1ID,
				marriage.Person2ID,
				marriage.StartDate.ToString(),
				marriage.StartYear.ToString(),
			};
		}

		private int GetRowIndexFromId(string marriageId)
		{
			List<string> idList = GetIds();
			return idList.Contains(marriageId) ? idList.IndexOf(marriageId) + 2 : -1;
		}

		private List<string> GetMarriageIdsFromPersonId(string personId)
		{
			List<(string,string,string)> idList = GetMarriageAndPersonIds();
			return idList.Where(x => x.Item2 == personId || x.Item3 == personId).Select(x => x.Item1).ToList();
		}
	}
}
