namespace BBPlusLockers.Lockers.DecoyLockers
{
	public class DecoyLightOrangeLocker : AcceptorDecoyLocker
	{
		protected override void AfterTrollAndClose(PlayerManager pm)
		{
			base.AfterTrollAndClose(pm);
			ec.RemoveTimeScale(timeScale);
		}

		protected override void ScammedPlayer(PlayerManager pm)
		{
			base.ScammedPlayer(pm);
			ec.AddTimeScale(timeScale);
		}

		readonly TimeScaleModifier timeScale = new(0f, 1f, 1f);
	}
}
