using UnityEngine;


public abstract class Weapon
{
    private protected AmmoFactory ammoFactory;
    private protected AmmoType ammoType;
    private protected int hitMask;


    public Weapon(AmmoFactory ammoFactory, AmmoType ammoType, int hitMask)
    {
        this.ammoFactory = ammoFactory;
        this.ammoType = ammoType;
        this.hitMask = hitMask;
    }

    public Transform Transform { get; set; }
    public abstract bool CanShootNow { get; }
    public abstract void Shoot();
}