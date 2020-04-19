using System.Linq.Expressions;

namespace FoxSoftware.ConditionTree.ExpressionTree.Models
{
	public class ExpressionTreeResult
	{
		public Expression Expression { get; internal set; }
		public ParameterExpression Parameter { get; internal set; }
	}
}
