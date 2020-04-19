using System.Collections.Generic;

namespace FoxSoftware.ConditionTree.Sql.Models
{
	public class SqlQueryData
	{
		public SqlQueryData(string query, IDictionary<string, object> parameters)
		{
			Query = query;
			Parameters = parameters;
		}

		public string Query { get; }
		public IDictionary<string, object> Parameters { get; }
	}
}
