using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyTreeAPI
{
	public interface IMarriageService
	{
		public Task<List<Marriage>> GetMarriages();
		public Task CreateMarriage(Marriage marriage);
		public Task UpdateMarriage(Marriage marriage, string marriageId);
		public Task DeleteMarriage(string marriageId);
	}
}
