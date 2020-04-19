using FoxSoftware.ConditionTree.Models.Enums;
using FoxSoftware.ConditionTree.Sql;
using NUnit.Framework;

namespace FoxSoftware.ConditionTree.Tests.Sql
{
	[TestFixture]
	public class SqlOperatorProviderTests
	{
		private SqlOperatorProvider _sqlOperatorProvider;

		[SetUp]
		public void Setup()
		{
			_sqlOperatorProvider = new SqlOperatorProvider();
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_StartsWith()
		{
			TestConditionOperator(ConditionOperator.StartsWith, null, "LIKE");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_EndsWith()
		{
			TestConditionOperator(ConditionOperator.EndsWith, null, "LIKE");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_Contains()
		{
			TestConditionOperator(ConditionOperator.Contains, null, "LIKE");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_Equal_NullValue()
		{
			TestConditionOperator(ConditionOperator.Equal, null, "IS");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_Equal_NonNullValue()
		{
			TestConditionOperator(ConditionOperator.Equal, 1, "=");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_GreaterThan()
		{
			TestConditionOperator(ConditionOperator.GreaterThan, null, ">");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_GreaterThanOrEqual()
		{
			TestConditionOperator(ConditionOperator.GreaterThanOrEqual, null, ">=");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_LessThan()
		{
			TestConditionOperator(ConditionOperator.LessThan, null, "<");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_LessThanOrEqual()
		{
			TestConditionOperator(ConditionOperator.LessThanOrEqual, null, "<=");
		}

		[Test]
		public void GetConditionOperator_GetConditionOperator_In()
		{
			TestConditionOperator(ConditionOperator.In, null, "IN");
		}

		[Test]
		public void GetConditionOperator_GetLogicalOperator_And()
		{
			TestLogical(LogicalOperator.And, "AND");
		}

		[Test]
		public void GetConditionOperator_GetLogicalOperator_Or()
		{
			TestLogical(LogicalOperator.Or, "OR");
		}

		[Test]
		public void GetConditionOperator_GetNegateOperator()
		{
			TestNegate("NOT");
		}

		private void TestConditionOperator(ConditionOperator @operator, object value, string expectedOperator)
		{
			var sqlOperator = _sqlOperatorProvider.GetConditionOperator(@operator, value);
			Assert.AreEqual(expectedOperator, sqlOperator);
		}

		private void TestLogical(LogicalOperator @operator, string expectedOperator)
		{
			var sqlOperator = _sqlOperatorProvider.GetLogicalOperator(@operator);
			Assert.AreEqual(expectedOperator, sqlOperator);
		}
		private void TestNegate(string expectedOperator)
		{
			var sqlOperator = _sqlOperatorProvider.GetNegateOperator();
			Assert.AreEqual(expectedOperator, sqlOperator);
		}
	}
}
