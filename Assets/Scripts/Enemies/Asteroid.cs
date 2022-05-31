using System;
using UnityEngine;
using UnityEngine.Pool;


public class Asteroid : IOutOfScreenObject
{
    private readonly ObjectPool<Asteroid> pool;
    private readonly GameObjectGraphics graphics;
    private readonly float[] generationSizes;

    private readonly LinearMovement movement;

    private int generation;
    private Vector2 direction;
    private float speed;

    public Vector2 CurrentPosition { get => movement.CurrentPos; set => SetPosition(value); }

    public event Action<Asteroid, Vector2, int> OnDestroy;
    public event Action<IUpdatable, bool> SetUpdateActive;

    public Asteroid(ObjectPool<Asteroid> pool, GameObjectGraphics graphics, float[] generationSizes, IHitSender hitSender)
    {
        this.pool = pool;
        this.graphics = graphics;
        this.generationSizes = generationSizes;
        hitSender.OnHit += OnHit;
        movement = new LinearMovement();
    }

    public void Launch(Vector2 startPos, Vector2 dir, float speed, int generation)
    {
        direction = dir;
        this.speed = speed;
        this.generation = generation;
        SetPosition(startPos);
        graphics.SetScale(generationSizes[generation]);

        SetUpdateActive?.Invoke(this, true);
    }

    private void SetPosition(Vector2 value)
    {
        movement.CurrentPos = value;
        graphics.SetPosition(value);
    }

    public void Update(float deltaTime)
    {
        movement.Move(speed * deltaTime, direction);
        graphics.SetPosition(movement.CurrentPos);
    }

    public void PoolOnGet()
    {
        graphics.SetActive(true);
    }

    public void PoolOnRelease()
    {
        SetUpdateActive?.Invoke(this, false);
        graphics.SetActive(false);
    }

    private void OnHit()
    {
        pool.Release(this);
        OnDestroy?.Invoke(this, movement.CurrentPos, generation);
    }
}