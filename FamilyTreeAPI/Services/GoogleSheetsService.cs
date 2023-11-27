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
using System.Security.Cryptography.X509Certificates;

namespace FamilyTreeAPI
{
    public class GoogleSheetsService : IGoogleSheetsService
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

        public List<List<string>> GetValues(string sheetName, string dataRange)
        {
            SheetsService service = AuthorizeGoogleApp();

            var range = $"{sheetName}!{dataRange}";
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(SheetId, range);
            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                List<List<string>> valuesArray = new List<List<string>>();
                foreach (var row in values)
                {
                    List<string> rowStrings = new List<string>();
                    foreach(var entry in row)
                    {
                        rowStrings.Add(entry.ToString());
                    }
                    valuesArray.Add(rowStrings);
                }
                return valuesArray;
            }
            else
            {
                return new List<List<string>>();
            }
        }

        public void AddRow(string sheetName, List<string> valueList, string dataRange)
        {
            SheetsService service = AuthorizeGoogleApp();
            // Specifying Column Range for reading...
            var range = $"{sheetName}!{dataRange}";
            var valueRange = new ValueRange();
            List<object> oblist = valueList.ToList<object>();
            valueRange.Values = new List<IList<object>> { oblist };
            // Append the above record...
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, SheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
        }
        public void UpdateRow(string sheetName, List<string> valueList, string dataRange)
        {
            SheetsService service = AuthorizeGoogleApp();
            // Specifying Column Range for reading...
            var range = $"{sheetName}!{dataRange}";
            var valueRange = new ValueRange();
            List<object> oblist = valueList.ToList<object>();
            valueRange.Values = new List<IList<object>> { oblist };
            // Append the above record...
            var appendRequest = service.Spreadsheets.Values.Update(valueRange, SheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
        }

        public void DeleteRow(string sheetName, int rowIndex)
        {
            SheetsService service = AuthorizeGoogleApp();

            var deleteRequest = new BatchUpdateSpreadsheetRequest()
            {
                Requests = new List<Request>()
                {
                    new Request()
                    {
                        DeleteDimension = new DeleteDimensionRequest()
                        {
                            Range = new DimensionRange()
                            {
                                SheetId = 0,
                                Dimension="ROWS",
                                StartIndex=rowIndex - 1,
                                EndIndex=rowIndex
                            }

                        }
                    }


                }
            };

            service.Spreadsheets.BatchUpdate(deleteRequest, SheetId).Execute();



        }
    }



}
