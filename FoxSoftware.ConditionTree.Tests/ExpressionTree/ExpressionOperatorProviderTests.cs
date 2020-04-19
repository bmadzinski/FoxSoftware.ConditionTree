using FoxSoftware.ConditionTree.ExpressionTree;
using FoxSoftware.ConditionTree.Models;
using FoxSoftware.ConditionTree.Models.Enums;
using FoxSoftware.ConditionTree.Tests.ExpressionTree.Models;
using NUnit.Framework;
using System;
using System.Linq.Expressions;

namespace FoxSoftware.ConditionTree.Tests.ExpressionTree
{
	[TestFixture]
	public class ExpressionOperatorProviderTests
	{
		private ExpressionOperatorProvider _expressionOperatorProvider;
		private ParameterExpression _parameter;

		[SetUp]
		public void Setup()
		{
			_expressionOperatorProvider = new ExpressionOperatorProvider();
			_parameter = Expression.Parameter(typeof(Person), "x");
		}

		[TestCase(ConditionOperator.StartsWith)]
		[TestCase(ConditionOperator.EndsWith)]
		[TestCase(ConditionOperator.Contains)]
		public void GetConditionOperator_GetConditionOperator_Text_InvalidProperty(ConditionOperator conditionOperator)
		{
			var value = "Jo";
			var propName = nameof(Person.Age);
			Assert.Throws<ArgumentException>(() =>
			{
				GetConditionOperator(conditionOperator, propName, value);
			});
		}

		[TestCase(ConditionOperator.StartsWith)]
		[TestCase(ConditionOperator.EndsWith)]
		[TestCase(ConditionOperator.Contains)]
		public void GetConditionOperator_GetConditionOperator_Text_InvalidValue(ConditionOperator conditionOperator)
		{
			var value = 1;
			var propName = nameof(Person.Name);
			Assert.Throws<ArgumentException>(() =>
			{
				GetConditionOperator(conditionOperator, propName, value);
			});
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_StartsWith()
		{
			var value = "Jo";
			var propName = nameof(Person.Name);

			var expression = GetConditionOperator(ConditionOperator.StartsWith, propName, value);
			TestExpression(expression, $"x.{propName}.StartsWith(\"{value}\")");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_EndsWith()
		{
			var value = "Jo";
			var propName = nameof(Person.Name);
			var expression = GetConditionOperator(ConditionOperator.EndsWith, propName, value);
			TestExpression(expression, $"x.{propName}.EndsWith(\"{value}\")");

		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_Contains()
		{
			var value = "Jo";
			var propName = nameof(Person.Name);
			var expression = GetConditionOperator(ConditionOperator.Contains, propName, value);
			TestExpression(expression, $"x.{propName}.Contains(\"{value}\")");
		}

		[TestCase(ConditionOperator.Equal)]
		[TestCase(ConditionOperator.GreaterThan)]
		[TestCase(ConditionOperator.GreaterThanOrEqual)]
		[TestCase(ConditionOperator.LessThan)]
		[TestCase(ConditionOperator.LessThanOrEqual)]
		public void GetConditionOperator_GetConditionOperator_Comparasion_InvalidParameter(ConditionOperator conditionOperator)
		{
			var value = 1;
			var propName = nameof(Person.Name);
			Assert.Throws<InvalidOperationException>(() =>
			{
				GetConditionOperator(conditionOperator, propName, value);
			});
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_Equal()
		{
			var value = "Jo";
			var propName = nameof(Person.Name);
			var expression = GetConditionOperator(ConditionOperator.Equal, propName, value);
			TestExpression(expression, $"(x.{propName} == \"{value}\")");
		}


		[Test]
		public void GetConditionOperator_GetConditionOperator_GreaterThan()
		{
			var value = 12;
			var propName = nameof(Person.Age);
			var expression = GetConditionOperator(ConditionOperator.GreaterThan, propName, value);
			TestExpression(expression, $"(x.{propName} > {value})");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_GreaterThanOrEqual()
		{
			var value = 12;
			var propName = nameof(Person.Age);
			var expression = GetConditionOperator(ConditionOperator.GreaterThanOrEqual, propName, value);
			TestExpression(expression, $"(x.{propName} >= {value})");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_LessThan()
		{
			var value = 12;
			var propName = nameof(Person.Age);
			var expression = GetConditionOperator(ConditionOperator.LessThan, propName, value);
			TestExpression(expression, $"(x.{propName} < {value})");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_LessThanOrEqual()
		{
			var value = 12;
			var propName = nameof(Person.Age);
			var expression = GetConditionOperator(ConditionOperator.LessThanOrEqual, propName, value);
			TestExpression(expression, $"(x.{propName} <= {value})");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_In()
		{
			var value = new int[0];
			var propName = nameof(Person.Age);
			var expression = GetConditionOperator(ConditionOperator.In, propName, value);
			TestExpression(expression, $"value({value.GetType()}).Contains(x.{propName})");
		}

		[TestCase(LogicalOperator.And, "AndAlso")]
		[TestCase(LogicalOperator.Or, "OrElse")]
		public void GetConditionOperator_GetLogicalOperator_Logic_Consts(LogicalOperator @operator, string expressionOperator)
		{
			var node = new LogicalOperatorNode()
			{
				Negate = false,
				Operator = @operator,
				Nodes = null
			};
			var left = Expression.Constant(true);
			var right = Expression.Constant(false);

			var expression = _expressionOperatorProvider.GetLogicalOperator(node, left, right);
			TestExpression(expression, $"(True {expressionOperator} False)");
		}

		[TestCase(LogicalOperator.And, "AndAlso")]
		[TestCase(LogicalOperator.Or, "OrElse")]
		public void GetConditionOperator_GetLogicalOperator_Logic_Expressions(LogicalOperator @operator, string expressionOperator)
		{
			var node = new LogicalOperatorNode()
			{
				Negate = false,
				Operator = @operator,
				Nodes = null
			};
			var left = Expression.GreaterThan(Expression.Property(_parameter, nameof(Person.Age)), Expression.Constant(10));
			var right = Expression.LessThan(Expression.Property(_parameter, nameof(Person.Age)), Expression.Constant(20));

			var expression = _expressionOperatorProvider.GetLogicalOperator(node, left, right);
			TestExpression(expression, $"((x.Age > 10) {expressionOperator} (x.Age < 20))");
		}

		[Test]
		public void GetConditionOperator_GetNegateOperator()
		{
			var expression = _expressionOperatorProvider.GetNegateOperator(Expression.Constant(10));
			TestExpression(expression, "Not(10)");
		}

		public Expression GetConditionOperator(ConditionOperator @operator, string propertyName, object value)
		{
			var node = new ConditionNode
			{
				Condition = @operator,
				Negate = false,
				PropertyName = propertyName,
				Value = value,
			};

			return _expressionOperatorProvider.GetConditionOperator(node, _parameter);
		}

		public void TestExpression(Expression expression, string expected)
		{
			Assert.AreEqual(expected, expression.ToString());
		}
	}
}
