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

    private readonly LinearMovement movement;

    private float speed;
    private float currentShootDelay;

    public event Action<UFO> OnDestroy;

    protected override Vector2 CurrentPosition { get => movement.CurrentPos; set => SetPosition(value); }


    public UFO(ObjectPool<UFO> pool, GameObjectGraphics graphics, Transform target, Weapon weapon, IHitSender hitSender, float startShootDelay, Vector2 sceneDimension) 
        : base(sceneDimension)
    {
        this.pool = pool;
        this.graphics = graphics;
        this.weapon = weapon;
        this.target = target;
        this.startShootDelay = startShootDelay;
        hitSender.OnHit += OnHit;
        movement = new LinearMovement();
    }

    private void OnHit()
    {
        pool.Release(this);
        OnDestroy?.Invoke(this);
    }

    private void SetPosition(Vector2 value)
    {
        movement.CurrentPos = value;
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
        movement.Move(speed * deltaTime, ((Vector2)target.position - movement.CurrentPos).normalized);
        graphics.SetPosition(movement.CurrentPos);

        weapon.Transform.right = ((Vector2)target.position - movement.CurrentPos).normalized;

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