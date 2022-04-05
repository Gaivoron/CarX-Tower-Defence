using Cysharp.Threading.Tasks;

namespace TowerDefence
{
    public interface ISolution
	{
		UniTask<bool> ExecuteAsync();
	}
}