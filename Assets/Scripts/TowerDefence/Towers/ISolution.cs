using Cysharp.Threading.Tasks;
using System.Threading;

namespace TowerDefence.Towers
{
    public interface ISolution
	{
		UniTask<bool> ExecuteAsync(CancellationToken cancellation);
	}
}