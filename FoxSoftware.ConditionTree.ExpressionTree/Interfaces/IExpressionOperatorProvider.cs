using FoxSoftware.ConditionTree.Models;
using System.Linq.Expressions;

namespace FoxSoftware.ConditionTree.ExpressionTree.Interfaces
{
	public interface IExpressionOperatorProvider
	{
		Expression GetConditionOperator(ConditionNode conditionNode, Expression parameter);
		Expression GetLogicalOperator(LogicalOperatorNode node, Expression left, Expression right);
		Expression GetNegateOperator(Expression expression);
	}
}
