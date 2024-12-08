using UnityEngine;

namespace BBPlusLockers.Lockers.HideableLockerVariants
{
	public class BrownLocker : HideableLocker // Reminder: open texture is broken one, closed texture is the usable one
	{
		protected override Sprite HudImage() => sprForLocker;
		protected override void LockerUsed() 
		{
			Close(false, false);
			broken = true;
		}
		protected override bool CanLockerBeUsed() =>
			!broken;

		bool broken = false;
		internal static Sprite sprForLocker;
	}
}
