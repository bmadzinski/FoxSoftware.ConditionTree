using FoxSoftware.ConditionTree.ExpressionTree.Interfaces;
using FoxSoftware.ConditionTree.Models;
using FoxSoftware.ConditionTree.Models.Enums;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace FoxSoftware.ConditionTree.ExpressionTree
{
	public class ExpressionOperatorProvider : IExpressionOperatorProvider
	{
		public Expression GetConditionOperator(ConditionNode node, Expression parameter)
		{
			return node.Condition switch
			{
				ConditionOperator.StartsWith => StartsWithExpression(node, parameter),
				ConditionOperator.EndsWith => EndsWithExpression(node, parameter),
				ConditionOperator.Equal => EqualExpression(node, parameter),
				ConditionOperator.GreaterThan => GreaterThanExpression(node, parameter),
				ConditionOperator.GreaterThanOrEqual => GreaterThanOrEqualExpression(node, parameter),
				ConditionOperator.LessThan => LessThanExpression(node, parameter),
				ConditionOperator.LessThanOrEqual => LessThanOrEqualExpression(node, parameter),
				ConditionOperator.In => InExpression(node, parameter),
				ConditionOperator.Contains => ContainsExpression(node, parameter),
				_ => throw new NotSupportedException($"Condition: {node.Condition} is not supported {nameof(ConditionOperator)}")
			};
		}

		public Expression GetLogicalOperator(LogicalOperatorNode node, Expression left, Expression right)
		{
			return node.Operator switch
			{
				LogicalOperator.And => Expression.AndAlso(left, right),
				LogicalOperator.Or => Expression.OrElse(left, right),
				_ => throw new NotSupportedException($"Logical operator not suported: {node.Operator}")
			};
		}

		public Expression GetNegateOperator(Expression expression)
		{
			return Expression.Not(expression);
		}

		private static Expression StartsWithExpression(ConditionNode conditionNode, Expression parameter)
		{
			var startsWithMethod = typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) });
			return Expression.Call(Expression.Property(parameter, conditionNode.PropertyName), startsWithMethod, Expression.Constant(conditionNode.Value));
		}
		private static Expression EndsWithExpression(ConditionNode conditionNode, Expression parameter)
		{
			var endsWithMethod = typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) });
			return Expression.Call(Expression.Property(parameter, conditionNode.PropertyName), endsWithMethod, Expression.Constant(conditionNode.Value));
		}
		private static Expression ContainsExpression(ConditionNode conditionNode, Expression parameter)
		{
			var endsWithMethod = typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) });
			return Expression.Call(Expression.Property(parameter, conditionNode.PropertyName), endsWithMethod, Expression.Constant(conditionNode.Value));
		}

		private static Expression EqualExpression(ConditionNode conditionNode, Expression parameter)
		{
			return Expression.Equal(
				Expression.Property(parameter, conditionNode.PropertyName),
				Expression.Constant(conditionNode.Value)
			);
		}

		private static Expression GreaterThanExpression(ConditionNode conditionNode, Expression parameter)
		{
			return Expression.GreaterThan(
				Expression.Property(parameter, conditionNode.PropertyName),
				Expression.Constant(conditionNode.Value)
			);
		}
		private static Expression GreaterThanOrEqualExpression(ConditionNode conditionNode, Expression parameter)
		{
			return Expression.GreaterThanOrEqual(
				Expression.Property(parameter, conditionNode.PropertyName),
				Expression.Constant(conditionNode.Value)
			);
		}
		private static Expression LessThanExpression(ConditionNode conditionNode, Expression parameter)
		{
			return Expression.LessThan(
				Expression.Property(parameter, conditionNode.PropertyName),
				Expression.Constant(conditionNode.Value)
			);
		}
		private static Expression LessThanOrEqualExpression(ConditionNode conditionNode, Expression parameter)
		{
			return Expression.LessThanOrEqual(
				Expression.Property(parameter, conditionNode.PropertyName),
				Expression.Constant(conditionNode.Value)
			);
		}

		private static Expression InExpression(ConditionNode conditionNode, Expression parameter)
		{
			var propertyType = parameter.Type.GetProperty(conditionNode.PropertyName).PropertyType;

			var property = Expression.Property(parameter, conditionNode.PropertyName);

			var method = typeof(Enumerable)
				.GetMethods()
				.Where(x => x.Name == nameof(Enumerable.Contains))
				.Single(x => x.GetParameters().Length == 2)
				.MakeGenericMethod(propertyType);

			return Expression.Call(method, Expression.Constant(conditionNode.Value), property);
		}

	}
}
