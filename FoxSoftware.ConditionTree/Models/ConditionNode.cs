using FoxSoftware.ConditionTree.Models.Enums;

namespace FoxSoftware.ConditionTree.Models
{
	public class ConditionNode : BaseNode
	{
		public string PropertyName { get; set; }
		public object Value { get; set; }
		public ConditionOperator Condition { get; set; }
	}
}
