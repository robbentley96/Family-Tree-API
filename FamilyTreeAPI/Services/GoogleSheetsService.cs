using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.IO;
using System.Threading;

namespace FamilyTreeAPI
{
    public class GoogleSheetsService
    {
        string[] Scopes = { SheetsService.Scope.Spreadsheets };
        string ApplicationName = "Family Tree";
        string SheetId = "1CAlOfU5lrj8WeSjbR82oFxCbvJjI40vB24HyQeoeLqI";

        private SheetsService AuthorizeGoogleApp()
        {
            string credentialString = Environment.GetEnvironmentVariable("GoogleSheetsCredentials");
            GoogleCredential credential = GoogleCredential.FromJson(credentialString)
                    .CreateScoped(Scopes);

            // Create Google Sheets API service.
            var service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

            return service;
        }

        public void GetValues()
        {
            SheetsService service = AuthorizeGoogleApp();

            var range = $"People!A:K";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SheetId, range);
            // Ecexuting Read Operation...
            var response = request.Execute();
            // Getting all records from Column A to E...
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    // Writing Data on Console...
                    Console.WriteLine("{0} | {1} | {2} | {3} | {4} ", row[0], row[1], row[2], row[3], row[4]);
                }
            }
            else
            {
                Console.WriteLine("No data found.");
            }
        }

        public void AddRow()
        {
            SheetsService service = AuthorizeGoogleApp();
            // Specifying Column Range for reading...
            var range = $"People!A:K";
            var valueRange = new ValueRange();
            var oblist = new List<object>() { "1", "2", "3", "5", "4", "1", "2", "3", "5", "4", "no" };
            valueRange.Values = new List<IList<object>> { oblist };
            // Append the above record...
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
        }
    }



}
