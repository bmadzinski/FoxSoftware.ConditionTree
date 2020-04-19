using FoxSoftware.ConditionTree.Models.Enums;
using System.Collections.Generic;

namespace FoxSoftware.ConditionTree.Models
{
	public class LogicalOperatorNode : BaseNode
	{
		public LogicalOperator Operator { get; set; }
		public IEnumerable<BaseNode> Nodes { get; set; }
	}
}
