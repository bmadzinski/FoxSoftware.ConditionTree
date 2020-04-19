using FoxSoftware.ConditionTree.ExpressionTree.Models;
using FoxSoftware.ConditionTree.Interfaces;

namespace FoxSoftware.ConditionTree.ExpressionTree.Interfaces
{
	public interface INodeToExpressionTreeParser<TModel> : INodeParser<ExpressionTreeResult>
	{
	}
}
