using FoxSoftware.ConditionTree.ExpressionTree.Interfaces;
using FoxSoftware.ConditionTree.ExpressionTree.Models;
using FoxSoftware.ConditionTree.Models;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FoxSoftware.ConditionTree.ExpressionTree
{
	public class NodeToExpressionTreeParser<TModel> : INodeToExpressionTreeParser<TModel>
	{
		private static readonly Type _modelType = typeof(TModel);
		private readonly IExpressionOperatorProvider _operatorProvider;

		public NodeToExpressionTreeParser(IExpressionOperatorProvider operatorProvider)
		{
			_operatorProvider = operatorProvider;
		}

		public ExpressionTreeResult Parse(BaseNode node)
		{
			var parameter = Expression.Parameter(_modelType, "x");
			return new ExpressionTreeResult
			{
				Expression = Parse(node, parameter),
				Parameter = parameter
			};
		}

		private Expression Parse(BaseNode node, Expression parameter)
		{
			return node switch
			{
				ConditionNode conditionNode => ParseConditionNode(conditionNode, parameter),
				LogicalOperatorNode logicalOperatorNode => ParseLogicalNode(logicalOperatorNode, parameter),
				_ => throw new NotSupportedException($"Node type not supported: {node.GetType().FullName}")
			};
		}

		private Expression ParseConditionNode(ConditionNode mode, Expression parameter)
		{
			var expression = _operatorProvider.GetConditionOperator(mode, parameter);
			if (mode.Negate)
			{
				expression = _operatorProvider.GetNegateOperator(expression);
			}
			return expression;
		}

		private Expression ParseLogicalNode(LogicalOperatorNode node, Expression parameter)
		{
			var subExpressions = node.Nodes.Select(x => Parse(x, parameter)).ToArray();
			if (!subExpressions.Any())
			{
				throw new InvalidOperationException("LogicalOperatorNode cannot have empty nodes");
			}

			var logicalExpression = subExpressions.First();
			foreach (var expression in subExpressions.Skip(1))
			{
				var logicalExpession = _operatorProvider.GetLogicalOperator(node, logicalExpression, expression);
			}

			if (node.Negate)
			{
				logicalExpression = _operatorProvider.GetNegateOperator(logicalExpression);
			}

			return logicalExpression;

		}
	}
}
