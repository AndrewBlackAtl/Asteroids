using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class UFOSpawner : Spawner, IUpdatable
{
    private readonly GameObject UFOPrefab;
    private readonly float minSpawnTime, maxSpawnTime;
    private readonly Transform target;
    private readonly Weapon weapon;
    private readonly ObjectPool<UFO> UFOPool;
    private readonly float startShootDelay;
    private readonly float minSpeed;
    private readonly float maxSpeed;

    private float currentSpawnTime;
    private float currentTime;

    public event Action UFODestroyed;


    public UFOSpawner(Transform target, GameObject UFOPrefab, Weapon weapon, float minSpeed, float maxSpeed, float startShootDelay, 
        Vector2 sceneDimension, float horizontalOffset, float verticalOffset, float minSpawnTime, float maxSpawnTime) 
        : base(sceneDimension, horizontalOffset, verticalOffset)
    {
        this.target = target;
        this.weapon = weapon;
        this.UFOPrefab = UFOPrefab;
        this.minSpawnTime = minSpawnTime;
        this.maxSpawnTime = maxSpawnTime;
        this.startShootDelay = startShootDelay;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;

        UFOPool = new ObjectPool<UFO>(UFOCreateFunc, (obj) => obj.PoolOnGet(), (obj) => obj.PoolOnRelease(), null, false, 1);

        UpdatableController.I.Add(this);
    }

    private UFO UFOCreateFunc()
    {
        var UFOGO = Object.Instantiate(UFOPrefab);
        var graphics = new GameObjectGraphics(UFOGO);
        var hitSender = UFOGO.GetComponent<IHitSender>();
        var weaponTransform = UFOGO.transform.Find("Weapon");
        weapon.Transform = weaponTransform;
        return new UFO(UFOPool, graphics, target, weapon, hitSender, startShootDelay, sceneDimension);
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