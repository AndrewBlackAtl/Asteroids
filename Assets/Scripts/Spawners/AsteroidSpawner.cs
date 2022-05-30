﻿using System;
using UnityEngine;
using UnityEngine.Pool;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;


public class AsteroidSpawner : Spawner
{
    private readonly int startAsteroidsNum;
    private readonly int asteroidsPerWaveIncrease;
    private readonly GameObject asteroidPrefab;
    private readonly ObjectPool<Asteroid> asteroidsPool;
    private readonly float[] genSizes;
    private readonly float[] minGenSpeeds;
    private readonly float[] maxGenSpeeds;
    private readonly int maxGen;
    private readonly int nextGenNum;

    private const int defaultAsteroidCapacity = 30;

    private int currentAsteroidsPerWave;
    private int currentAsteroidsNum;

    public event Action<int> AsteroidDestroyed;


    public AsteroidSpawner(int startAsteroidsNum, int asteroidsPerWaveIncrease, GameObject asteroidPrefab, 
        float[] genSizes, float[] minGenSpeeds, float[] maxGenSpeeds, int maxGen, int nextGenNum, Vector2 sceneDimension, float horizontalOffset, float verticalOffset) 
        : base(sceneDimension, horizontalOffset, verticalOffset)
    {
        this.asteroidPrefab = asteroidPrefab;
        this.startAsteroidsNum = startAsteroidsNum;
        this.asteroidsPerWaveIncrease = asteroidsPerWaveIncrease;
        this.genSizes = genSizes;
        this.minGenSpeeds = minGenSpeeds;
        this.maxGenSpeeds = maxGenSpeeds;
        this.maxGen = maxGen;
        this.nextGenNum = nextGenNum;

        asteroidsPool = new ObjectPool<Asteroid>(AsteroidCreateFunc, (obj) => obj.PoolOnGet(), (obj) => obj.PoolOnRelease(), null, false, defaultAsteroidCapacity);
    }

    private Asteroid AsteroidCreateFunc()
    {
        var asteroidGO = Object.Instantiate(asteroidPrefab);
        var graphics = new GameObjectGraphics(asteroidGO);
        var hitSender = asteroidGO.GetComponent<IHitSender>();
        return new Asteroid(asteroidsPool, graphics, genSizes, hitSender, sceneDimension);
    }

    public void Start()
    {
        currentAsteroidsPerWave = startAsteroidsNum;
        SpawnAsteroids();
    }

    private float GetAsteroidSpeed(int generation)
    {
        return Random.Range(minGenSpeeds[generation], maxGenSpeeds[generation]);
    }

    private void SpawnAsteroids()
    {
        currentAsteroidsNum += currentAsteroidsPerWave;
        for (int i = 0; i < currentAsteroidsPerWave; i++)
        {
            var asteroid = asteroidsPool.Get();
            asteroid.OnDestroy += Asteroid_OnDestroy;
            asteroid.Launch(GetStartPos(), Random.insideUnitCircle.normalized, GetAsteroidSpeed(0), 0);
        }
    }

    private void Asteroid_OnDestroy(Asteroid sender, Vector2 pos, int generation)
    {
        sender.OnDestroy -= Asteroid_OnDestroy;

        AsteroidDestroyed?.Invoke(generation);

        currentAsteroidsNum--;
        if (generation < maxGen)
        {
            currentAsteroidsNum += nextGenNum;
            for (int i = 0; i < nextGenNum; i++)
            {
                var asteroid = asteroidsPool.Get();
                asteroid.OnDestroy += Asteroid_OnDestroy;
                asteroid.Launch(pos, Random.insideUnitCircle.normalized, GetAsteroidSpeed(generation + 1), generation + 1);
            }
        }
        else
        {
            if (currentAsteroidsNum == 0)
            {
                currentAsteroidsPerWave += asteroidsPerWaveIncrease;
                SpawnAsteroids();
            }
        }
    }
}