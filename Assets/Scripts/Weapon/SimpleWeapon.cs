using UnityEngine;

public class SimpleWeapon : Weapon
{
    public override bool CanShootNow => true;

    public SimpleWeapon(AmmoFactory ammoFactory, AmmoType ammoType, int hitMask) : base(ammoFactory, ammoType, hitMask) { }


    public override void Shoot()
    {
        ammoFactory.Create(ammoType).Launch(Transform.position, Transform.right, hitMask);
    }
}
