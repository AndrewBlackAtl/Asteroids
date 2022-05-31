using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class UFOSpawner : Spawner, IUpdatable
{
    private readonly UpdatableController updatableController;
    private readonly float minSpawnTime, maxSpawnTime;
    private readonly Weapon weapon;
    private readonly ObjectPool<UFO> UFOPool;
    private readonly float minSpeed;
    private readonly float maxSpeed;

    private float currentSpawnTime;
    private float currentTime;

    public event Action UFODestroyed;
    public event Action<IUpdatable, bool> SetUpdateActive;

    public UFOSpawner(UpdatableController updatableController, Transform target, GameObject UFOPrefab, Weapon weapon, float minSpeed, float maxSpeed, float startShootDelay, 
        Vector2 sceneDimension, float horizontalOffset, float verticalOffset, float minSpawnTime, float maxSpawnTime) 
        : base(sceneDimension, horizontalOffset, verticalOffset)
    {
        this.updatableController = updatableController;
        this.weapon = weapon;
        this.minSpawnTime = minSpawnTime;
        this.maxSpawnTime = maxSpawnTime;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;

        UFOPool = new ObjectPool<UFO>(() => CreateUFO(UFOPrefab, target, startShootDelay), (obj) => obj.PoolOnGet(), (obj) => obj.PoolOnRelease(), null, false, 1);
    }

    private UFO CreateUFO(GameObject prefab, Transform target, float startShootDelay)
    {
        var UFOGO = Object.Instantiate(prefab);
        var graphics = new GameObjectGraphics(UFOGO);
        var hitSender = UFOGO.GetComponent<IHitSender>();
        var weaponTransform = UFOGO.transform.Find("Weapon");
        weapon.Transform = weaponTransform;
        var UFO = new UFO(UFOPool, graphics, target, weapon, hitSender, startShootDelay);
        updatableController.Add(new OutOfScreenObject(UFO, sceneDimension));
        return UFO;
    }

    public void Start()
    {
        currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }

    private void SpawnUFO()
    {
        var UFO = UFOPool.Get();
        UFO.OnDestroy += UFOOnDestroy;
        UFO.Launch(GetStartPos(), Random.Range(minSpeed, maxSpeed));
    }

    private void UFOOnDestroy(UFO sender)
    {
        sender.OnDestroy -= UFOOnDestroy;
        UFODestroyed?.Invoke();
    }

    public void Update(float deltaTime)
    {
        currentTime += Time.deltaTime;
        if (currentTime >= currentSpawnTime)
        {
            SpawnUFO();
            currentTime = 0f;
            currentSpawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        }
    }
}