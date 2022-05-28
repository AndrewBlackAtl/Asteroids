using UnityEngine;
using UnityEngine.Pool;


public class LaserRay : UpdatableBase, IAmmo
{
    private readonly GameObjectGraphics graphics;
    private readonly ObjectPool<LaserRay> pool;
    private readonly float lifeTime;

    private float currentLifeTime;


    public LaserRay(ObjectPool<LaserRay> pool, GameObjectGraphics graphics, float lifeTime)
    {
        this.pool = pool;
        this.graphics = graphics;
        this.lifeTime = lifeTime;
    }

    public void Launch(Vector2 startPos, Vector2 direction, int hitMask)
    {
        graphics.SetPosition(startPos);

        var z = Vector2.SignedAngle(Vector2.right, direction);
        graphics.SetRotation(Quaternion.Euler(0f, 0f, z));

        RaycastHit2D[] result = Physics2D.RaycastAll(startPos, direction, float.PositiveInfinity, hitMask);
        foreach (var item in result)
        {
            item.transform.GetComponent<IHitable>().Hit();
        }

        SetUpdateActive(true);
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

    public override void Update(float deltaTime)
    {
        currentLifeTime -= deltaTime;
        if (currentLifeTime <= 0f)
        {
            pool.Release(this);
        }
    }
}