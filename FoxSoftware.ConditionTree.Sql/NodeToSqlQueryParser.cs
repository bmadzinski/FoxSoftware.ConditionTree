
using FoxSoftware.ConditionTree.Models;
using FoxSoftware.ConditionTree.Sql.Extensions;
using FoxSoftware.ConditionTree.Sql.Interfaces;
using FoxSoftware.ConditionTree.Sql.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FoxSoftware.ConditionTree.Sql
{
	public class NodeToSqlQueryParser : INodeToSqlQueryParser
	{
		private readonly SqlOperatorProvider _sqlOperatorProvider;

		public NodeToSqlQueryParser(
			SqlOperatorProvider sqlOperatorProvider
		)
		{
			_sqlOperatorProvider = sqlOperatorProvider;
		}

		public SqlQueryData Parse(BaseNode node)
		{
			var queryBuilder = new StringBuilder();
			var sqlParameters = new Dictionary<string, object>();

			Parse(queryBuilder, node, 0, sqlParameters);

			return new SqlQueryData(queryBuilder.ToString(), sqlParameters);
		}


		private StringBuilder Parse(StringBuilder queryBuilder, BaseNode node, int depth, IDictionary<string, object> sqlParameters)
		{
			return node switch
			{
				ConditionNode conditionNode => ParseConditionNode(queryBuilder, depth, sqlParameters, conditionNode),
				LogicalOperatorNode logicalOperatorNode => ParseLogicalNode(queryBuilder, depth, sqlParameters, logicalOperatorNode),
				_ => throw new NotSupportedException($"Node type not supported: {node.GetType().FullName}")
			};
		}

		private StringBuilder ParseLogicalNode(StringBuilder queryBuilder, int depth, IDictionary<string, object> sqlParameters, LogicalOperatorNode logicalOperatorNode)
		{
			var count = logicalOperatorNode.Nodes.Count();
			if (count == 0)
			{
				return queryBuilder;
			}

			if (logicalOperatorNode.Negate)
			{
				queryBuilder.AppendWithIndent(depth).AppendLine(_sqlOperatorProvider.GetNegateOperator());
			}

			queryBuilder.AppendWithIndent(depth).AppendLine("(");

			Parse(queryBuilder, logicalOperatorNode.Nodes.First(), depth + 1, sqlParameters);

			foreach (var childNode in logicalOperatorNode.Nodes.Skip(1))
			{
				queryBuilder.AppendWithIndent(depth + 1).AppendLine(_sqlOperatorProvider.GetLogicalOperator(logicalOperatorNode.Operator));
				Parse(queryBuilder, childNode, depth + 1, sqlParameters);
			}

			return queryBuilder.AppendWithIndent(depth).AppendLine(")");
		}

		private StringBuilder ParseConditionNode(StringBuilder queryBuilder, int depth, IDictionary<string, object> sqlParameters, ConditionNode conditionNode)
		{
			queryBuilder.AppendWithIndent(depth);

			if (conditionNode.Negate)
			{
				queryBuilder.Append($"{_sqlOperatorProvider.GetNegateOperator()} ");
			}
			string parameter = AddParameters(sqlParameters, conditionNode);

			return queryBuilder
				.AppendLine($"[{conditionNode.PropertyName}] {_sqlOperatorProvider.GetConditionOperator(conditionNode.Condition, conditionNode.Value)} {parameter}");
		}

		private static string AddParameters(IDictionary<string, object> sqlParameters, ConditionNode conditionNode)
		{
			var value = conditionNode.Value;
			if (value is null)
			{
				return HandleNullValue(sqlParameters, conditionNode.PropertyName);
			}

			var enumerable = value as IEnumerable;
			if (!(enumerable is null))
			{
				return HandleArrayValue(sqlParameters, conditionNode.PropertyName, enumerable);
			}

			return HandleValue(sqlParameters, conditionNode.PropertyName, conditionNode.Value);
		}

		private static string HandleArrayValue(IDictionary<string, object> sqlParameters, string propertyName, IEnumerable enumerable)
		{
			var enumerator = enumerable.GetEnumerator();
			var stringBuilder = new StringBuilder();

			var any = enumerator.MoveNext();
			if (!any)
			{
				throw new InvalidOperationException("Array value cannot be empty");
			}

			stringBuilder.Append("( ");

			stringBuilder.Append(HandleValue(sqlParameters, propertyName, enumerator.Current));

			while (enumerator.MoveNext())
			{
				stringBuilder.Append(", ");
				stringBuilder.Append(HandleValue(sqlParameters, propertyName, enumerator.Current));
			}
			stringBuilder.Append(" )");
			return stringBuilder.ToString();
		}

		private static string HandleValue(IDictionary<string, object> sqlParameters, string propertyName, object value)
		{
			var parameterName = $"@{propertyName}{sqlParameters.Count}";
			sqlParameters.Add(parameterName, value);
			return parameterName;
		}

		private static string HandleNullValue(IDictionary<string, object> sqlParameters, string propertyName)
		{
			var parameterName = $"@{propertyName}{sqlParameters.Count}";
			sqlParameters.Add(parameterName, DBNull.Value);
			return parameterName;
		}
	}
}
