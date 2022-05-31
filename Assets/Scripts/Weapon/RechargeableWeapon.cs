using System;
using UnityEngine;


public class RechargeableWeapon : Weapon, IUpdatable
{
    private readonly int chargesNum;
    private readonly float rechargeDuration;

    private int currentCharges;
    private float currentRechargeTimer;

    public event Action<int> ChargesChanged;
    public event Action<float> RechargeTimerChanged;
    public event Action<IUpdatable, bool> SetUpdateActive;

    public override bool CanShootNow => currentCharges > 0;


    public RechargeableWeapon(AmmoFactory ammoFactory, AmmoType ammoType, int hitMask, int chargesNum, float rechargeDuration) : base(ammoFactory, ammoType, hitMask)
    {
        this.chargesNum = chargesNum;
        this.rechargeDuration = rechargeDuration;
        currentCharges = chargesNum;
        currentRechargeTimer = rechargeDuration;
    }

    public override void Shoot()
    {
        currentCharges--;
        ChargesChanged?.Invoke(currentCharges);

        ammoFactory.Create(ammoType).Launch(Transform.position, Transform.right, hitMask);
    }

    public void Update(float deltaTime)
    {
        if (currentCharges < chargesNum)
        {
            currentRechargeTimer -= deltaTime;
            RechargeTimerChanged?.Invoke(currentRechargeTimer);

            if (currentRechargeTimer <= 0f)
            {
                currentRechargeTimer = rechargeDuration;
                RechargeTimerChanged?.Invoke(currentRechargeTimer);

                currentCharges++;
                ChargesChanged?.Invoke(currentCharges);
            }
        }
    }
}