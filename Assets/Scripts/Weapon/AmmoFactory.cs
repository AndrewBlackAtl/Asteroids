using UnityEngine;
using UnityEngine.Pool;


public class AmmoFactory
{
    private readonly ObjectPool<CannonBullet> bulletPool;
    private readonly ObjectPool<LaserRay> laserPool;

    private const int defaultBulletCapacity = 30;
    private const int defaultLazerCapacity = 3;


    public AmmoFactory(GameObject bulletGraphicsPrefab, float bulletSpeed, float bulletLifeTime, GameObject laserGraphicsPrefab, float laserLifeTime, Vector2 sceneDimension)
    {
        bulletPool = new ObjectPool<CannonBullet>(() =>
        new CannonBullet(bulletPool, new GameObjectGraphics(Object.Instantiate(bulletGraphicsPrefab)), bulletSpeed, bulletLifeTime, sceneDimension), 
        (obj) => obj.PoolOnGet(), (obj) => obj.PoolOnRelease(), null, false, defaultBulletCapacity);

        laserPool = new ObjectPool<LaserRay>(() =>
        new LaserRay(laserPool, new GameObjectGraphics(Object.Instantiate(laserGraphicsPrefab)), laserLifeTime), 
        (obj) => obj.PoolOnGet(), (obj) => obj.PoolOnRelease(), null, false, defaultLazerCapacity);
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
