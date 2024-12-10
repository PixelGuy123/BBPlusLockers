

using UnityEngine;

namespace BBPlusLockers.Lockers.DecoyLockers
{
	public class DecoyAquaLocker : AcceptorDecoyLocker
	{
		protected override void ScammedPlayer(PlayerManager pm)
		{
			base.ScammedPlayer(pm);
			int maPoints = Singleton<CoreGameManager>.Instance.GetPoints(pm.playerNumber);
			Singleton<CoreGameManager>.Instance.AddPoints(-Mathf.Min(25, maPoints), pm.playerNumber, true);
		}
	}
}
