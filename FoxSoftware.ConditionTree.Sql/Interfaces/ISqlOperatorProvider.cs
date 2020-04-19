using FoxSoftware.ConditionTree.Models.Enums;

namespace FoxSoftware.ConditionTree.Sql.Interfaces
{
	public interface ISqlOperatorProvider
	{
		string GetConditionOperator(ConditionOperator conditionOperator, object value);
		string GetLogicalOperator(LogicalOperator logicalOperator);
		string GetNegateOperator();
	}
}
