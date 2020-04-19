using FoxSoftware.ConditionTree.Models.Enums;
using FoxSoftware.ConditionTree.Sql.Interfaces;
using System;

namespace FoxSoftware.ConditionTree.Sql
{
	public class SqlOperatorProvider : ISqlOperatorProvider
	{
		public string GetConditionOperator(ConditionOperator conditionOperator, object value)
		{
			return conditionOperator switch
			{
				ConditionOperator.StartsWith => "LIKE",
				ConditionOperator.EndsWith => "LIKE",
				ConditionOperator.Contains => "LIKE",
				ConditionOperator.Equal => value is null ? "IS" : "=",
				ConditionOperator.GreaterThan => ">",
				ConditionOperator.GreaterThanOrEqual => ">=",
				ConditionOperator.LessThan => "<",
				ConditionOperator.LessThanOrEqual => "<=",
				ConditionOperator.In => "IN",
				_ => throw new NotSupportedException($"Condition operator operator not suported: {conditionOperator}")
			};
		}

		public string GetLogicalOperator(LogicalOperator logicalOperator)
		{
			return logicalOperator switch
			{
				LogicalOperator.And => "AND",
				LogicalOperator.Or => "OR",
				_ => throw new NotSupportedException($"Logical operator not suported: {logicalOperator}")
			};
		}

		public string GetNegateOperator()
		{
			return "NOT";
		}
	}
}
