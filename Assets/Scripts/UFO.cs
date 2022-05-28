using System;
using UnityEngine;
using UnityEngine.Pool;

public class UFO : OutOfScreenTeleporter
{
    private readonly Weapon weapon;
    private readonly GameObjectGraphics graphics;
    private readonly ObjectPool<UFO> pool;
    private readonly Transform target;
    private readonly float startShootDelay;

    private Vector2 currentPos;
    private Vector2 velocity;
    private float speed;
    private float currentShootDelay;

    public event Action<UFO> OnDestroy;

    private protected override Vector2 CurrentPosition { get => currentPos; set => SetPosition(value); }


    public UFO(ObjectPool<UFO> pool, GameObjectGraphics graphics, Transform target, Weapon weapon, IHitSender hitSender, float startShootDelay)
    {
        this.pool = pool;
        this.graphics = graphics;
        this.weapon = weapon;
        this.target = target;
        this.startShootDelay = startShootDelay;
        hitSender.OnHit += OnHit;
    }

    private void OnHit()
    {
        pool.Release(this);
        OnDestroy?.Invoke(this);
    }

    private void SetPosition(Vector2 value)
    {
        currentPos = value;
        graphics.SetPosition(value);
    }

    public void Launch(Vector2 startPos, float speed)
    {
        this.speed = speed;
        currentShootDelay = 0f;
        SetPosition(startPos);
        SetUpdateActive(true);
    }

    public override void Update(float deltaTime)
    {
        velocity = speed * deltaTime * ((Vector2)target.position - currentPos).normalized;

        currentPos += velocity;
        SetPosition(currentPos);

        weapon.Transform.right = ((Vector2)target.position - currentPos).normalized;

        if (currentShootDelay < startShootDelay)
        {
            currentShootDelay += deltaTime;
        }
        if (currentShootDelay > startShootDelay && weapon.CanShootNow)
        {
            weapon.Shoot();
        }

        base.Update(deltaTime);
    }

    public void PoolOnGet()
    {
        graphics.SetActive(true);
    }

    public void PoolOnRelease()
    {
        SetUpdateActive(false);
        graphics.SetActive(false);
    } 
}