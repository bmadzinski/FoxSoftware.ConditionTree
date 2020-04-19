using System.Text;

namespace FoxSoftware.ConditionTree.Sql.Extensions
{
	public static class StringBuilderExtensions
	{
		public static StringBuilder AppendWithIndent(this StringBuilder stringBuilder, int depth)
		{
			return stringBuilder.Append("".PadRight(depth, '\t'));
		}
	}
}
