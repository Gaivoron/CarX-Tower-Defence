using Cysharp.Threading.Tasks;

namespace TowerDefence.Towers
{
    public interface ISolution
	{
		UniTask<bool> ExecuteAsync();
	}
}