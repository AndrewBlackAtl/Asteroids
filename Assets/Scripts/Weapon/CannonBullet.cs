using UnityEngine;
using UnityEngine.Pool;


public class CannonBullet : OutOfScreenTeleporter, IAmmo
{
    private readonly GameObjectGraphics graphics;
    private readonly ObjectPool<CannonBullet> pool;
    private readonly float speed;
    private readonly float lifeTime;

    private readonly LinearMovement movement;
    private readonly RaycastHit2D[] hitResult = new RaycastHit2D[1];

    private Vector2 lastPos;
    private Vector2 direction;
    private int hitMask;
    private float currentLifeTime;

    protected override Vector2 CurrentPosition { get => movement.CurrentPos; set => SetPosition(value); }


    public CannonBullet(ObjectPool<CannonBullet> pool, GameObjectGraphics graphics, float speed, float lifeTime, Vector2 sceneDimension) : base(sceneDimension)
    {
        this.pool = pool;
        this.graphics = graphics;
        this.speed = speed;
        this.lifeTime = lifeTime;
        movement = new LinearMovement();
    }

    private void SetPosition(Vector2 value)
    {
        lastPos = value;
        movement.CurrentPos = value;
        graphics.SetPosition(value);
    }

    public void Launch(Vector2 startPos, Vector2 direction, int hitMask)
    {
        SetPosition(startPos);
        this.direction = direction;
        this.hitMask = hitMask;

        SetUpdateActive(true);
    }

    public override void Update(float deltaTime)
    {
        movement.Move(speed * deltaTime, direction);
        
        if (Physics2D.LinecastNonAlloc(lastPos, movement.CurrentPos, hitResult, hitMask) > 0)
        {
            hitResult[0].transform.GetComponent<IHitable>().Hit();
            pool.Release(this);
            return;
        }

        currentLifeTime -= deltaTime;
        if (currentLifeTime <= 0f)
        {
            pool.Release(this);
        }

        graphics.SetPosition(movement.CurrentPos);
        lastPos = movement.CurrentPos;

        base.Update(deltaTime);
    }

    public void PoolOnGet()
    {
        currentLifeTime = lifeTime;

        graphics.SetActive(true);
    }

    public void PoolOnRelease()
    {
        SetUpdateActive(false);
        graphics.SetActive(false);
    }
}
