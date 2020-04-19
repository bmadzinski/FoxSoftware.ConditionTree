using FoxSoftware.ConditionTree.Models;

namespace FoxSoftware.ConditionTree.Interfaces
{

	public interface INodeParser<TResult>
	{
		TResult Parse(BaseNode node);
	}
}
