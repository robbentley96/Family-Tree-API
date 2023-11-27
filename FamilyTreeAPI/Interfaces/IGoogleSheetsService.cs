using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeAPI
{
    public interface IGoogleSheetsService
    {
        public List<List<string>> GetValues(string sheetName, string dataRange);
        public void AddRow(string sheetName, List<string> valueList, string dataRange);
        public void DeleteRow(string sheetName, int rowIndex);
        public void UpdateRow(string sheetName, List<string> valueList, string dataRange);
    }
}
