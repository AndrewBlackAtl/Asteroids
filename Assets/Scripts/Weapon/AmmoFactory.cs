using UnityEngine;
using UnityEngine.Pool;


public class AmmoFactory
{
    private readonly ObjectPool<CannonBullet> bulletPool;
    private readonly ObjectPool<LaserRay> laserPool;
    private readonly UpdatableController updatableController;

    private const int defaultBulletCapacity = 30;
    private const int defaultLazerCapacity = 3;


    public AmmoFactory(GameObject bulletGraphicsPrefab, float bulletSpeed, float bulletLifeTime, 
        GameObject laserGraphicsPrefab, float laserLifeTime, UpdatableController updatableController, Vector2 sceneDimension)
    {
        this.updatableController = updatableController;

        bulletPool = new ObjectPool<CannonBullet>(() => CreateBullet(bulletGraphicsPrefab, bulletSpeed, bulletLifeTime, sceneDimension), 
        (obj) => obj.PoolOnGet(), (obj) => obj.PoolOnRelease(), null, false, defaultBulletCapacity);

        laserPool = new ObjectPool<LaserRay>(() => CreateLaserRay(laserGraphicsPrefab, laserLifeTime),
        (obj) => obj.PoolOnGet(), (obj) => obj.PoolOnRelease(), null, false, defaultLazerCapacity);
    }

    private CannonBullet CreateBullet(GameObject prefab, float speed, float lifeTime, Vector2 sceneDimension) 
    {
        var bullet = new CannonBullet(bulletPool, new GameObjectGraphics(Object.Instantiate(prefab)), speed, lifeTime);
        updatableController.Add(new OutOfScreenObject(bullet, sceneDimension));
        return bullet;
    }

    private LaserRay CreateLaserRay(GameObject prefab, float lifeTime) 
    {
        var laser = new LaserRay(laserPool, new GameObjectGraphics(Object.Instantiate(prefab)), lifeTime);
        updatableController.Add(laser);
        return laser;
    }


    public IAmmo Create(AmmoType type)
    {
        switch (type)
        {
            case AmmoType.CanonBullet:
                return bulletPool.Get();
            case AmmoType.LaserRay:
                return laserPool.Get();
            default:
                throw new System.NotImplementedException($"Ammo with type {type} is not implemented");
        }
    }
}
