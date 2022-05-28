using UnityEngine;
using UnityEngine.Pool;


public class CannonBullet : OutOfScreenTeleporter, IAmmo
{
    private readonly GameObjectGraphics graphics;
    private readonly ObjectPool<CannonBullet> pool;
    private readonly float speed;
    private readonly float lifeTime;

    private readonly RaycastHit2D[] hitResult = new RaycastHit2D[1];

    private Vector2 lastPos;
    private Vector2 currentPos;
    private Vector2 direction;
    private int hitMask;
    private float currentLifeTime;

    private protected override Vector2 CurrentPosition { get => currentPos; set => SetPosition(value); }


    public CannonBullet(ObjectPool<CannonBullet> pool, GameObjectGraphics graphics, float speed, float lifeTime)
    {
        this.pool = pool;
        this.graphics = graphics;
        this.speed = speed;
        this.lifeTime = lifeTime;
    }

    private void SetPosition(Vector2 value)
    {
        currentPos = value;
        graphics.SetPosition(value);
    }

    public void Launch(Vector2 startPos, Vector2 direction, int hitMask)
    {
        SetPosition(startPos);
        lastPos = startPos;
        this.direction = direction;
        this.hitMask = hitMask;

        SetUpdateActive(true);
    }

    public override void Update(float deltaTime)
    {
        currentPos = lastPos + speed * deltaTime * direction;
        graphics.SetPosition(currentPos);

        if (Physics2D.LinecastNonAlloc(lastPos, currentPos, hitResult, hitMask) > 0)
        {
            hitResult[0].transform.GetComponent<IHitable>().Hit();
            pool.Release(this);
            return;
        }

        lastPos = currentPos;

        currentLifeTime -= deltaTime;
        if (currentLifeTime <= 0f)
        {
            pool.Release(this);
        }

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
