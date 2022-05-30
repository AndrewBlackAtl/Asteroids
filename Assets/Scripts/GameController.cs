using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] private PlayerInput input;

    [SerializeField] private GameUI gameUI;
    [SerializeField] private GameOverPopup gameOverPopup;

    [Header("SPACESHIP")]
    [SerializeField] private Transform spaceshipTransform;
    [SerializeField] private Transform cannonTransform;
    [SerializeField] private Transform laserTransform;
    [SerializeField] private SpriteRenderer spaceshipSR;
    [SerializeField] private Sprite activeMoveSprite, inactiveMoveSprite;
    [SerializeField] private Transform collisionSenderTransform;
    [SerializeField] private float acceleration;
    [SerializeField] private float deceleration;
    [SerializeField] private float rotateSpeed;

    [Header("BULLET CONFIG")]
    [SerializeField] private GameObject bulletGraphicsPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletLifeTime;
    
    [Header("LASER CONFIG")]
    [SerializeField] private GameObject laserGraphicsPrefab;
    [SerializeField] private float laserLifeTime;
    [SerializeField] private int laserCharges;
    [SerializeField] private float laserRechargeDuration;

    [Header("ASTEROIDS CONFIG")]
    [SerializeField] private GameObject asteroidPrefab;
    [SerializeField] private int maxAsteroidGen;
    [SerializeField] private int nextGenAsteroidsNum;
    [SerializeField] private float[] genSizes;
    [SerializeField] private float[] minGenSpeeds;
    [SerializeField] private float[] maxGenSpeeds;
    [SerializeField] private int startAsteroidsNum;
    [SerializeField] private int asteroidsPerWaveIncrease;
    [SerializeField] private float asteroidSpawnHorizontalOffset;
    [SerializeField] private float asteroidSpawnVerticalOffset;
    [SerializeField] private int[] genScore;

    [Header("UFO CONFIG")]
    [SerializeField] private GameObject UFOPrefab;
    [SerializeField] private int UFOCannonRechargeDuration;
    [SerializeField] private float UFOMinSpeed;
    [SerializeField] private float UFOMaxSpeed;
    [SerializeField] private float UFOMinSpawnTime;
    [SerializeField] private float UFOMaxSpawnTime;
    [SerializeField] private float UFOSpawnHorizontalOffset;
    [SerializeField] private float UFOSpawnVerticalOffset;
    [SerializeField] private float UFOStartShootDelay;
    [SerializeField] private int UFOScore;


    private ScoreCounter scoreCounter;


    private void Start()
    {
        Setup();
    }

    private void Setup()
    {
        var sceneDimension = (Vector2)Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        AmmoFactory ammoFactory = new AmmoFactory(bulletGraphicsPrefab, bulletSpeed, bulletLifeTime, laserGraphicsPrefab, laserLifeTime, sceneDimension);
        ProjectPhysics projPhycics = new ProjectPhysics();

        PlayerSetup(ammoFactory, projPhycics.PlayerHitMask, sceneDimension);
        SpawnersSetup(ammoFactory, sceneDimension, projPhycics.EnemyHitMask, out AsteroidSpawner asteroidSpawner, out UFOSpawner UFOSpawner);
        ScoreSetup(asteroidSpawner, UFOSpawner);

        asteroidSpawner.Start();
        UFOSpawner.Start();
    }

    private void PlayerSetup(AmmoFactory ammoFactory, int playerHitMask, Vector2 sceneDimension) 
    {
        var playerCannon = new SimpleWeapon(ammoFactory, AmmoType.CanonBullet, playerHitMask)
        {
            Transform = cannonTransform
        };
        var playerLaser = new RechargeableWeapon(ammoFactory, AmmoType.LaserRay, playerHitMask, laserCharges, laserRechargeDuration)
        {
            Transform = laserTransform
        };
        
        List<Weapon> weapons = new List<Weapon>() { playerCannon, playerLaser };
        ICollisionEventSender collisionSender = collisionSenderTransform.GetComponent<ICollisionEventSender>();
        IHitSender hitSender = collisionSenderTransform.GetComponent<IHitSender>();
        IGraphics moveGraphics = new SpriteGraphics(spaceshipSR, activeMoveSprite, inactiveMoveSprite);

        PlayerSpaceship spaceship = new PlayerSpaceship(
            spaceshipTransform,
            moveGraphics,
            new InputSystemPlayerInput(input),
            collisionSender,
            weapons,
            hitSender,
            acceleration,
            deceleration,
            rotateSpeed,
            sceneDimension);

        spaceship.SetActiveWeapon(0);

        playerLaser.ChargesChanged += gameUI.OnLaserChargesChanged;
        playerLaser.RechargeTimerChanged += gameUI.OnLaserCooldownChanged;
        spaceship.PositionChanged += gameUI.OnPositionChanged;
        spaceship.RotationChanged += gameUI.OnRotationChanged;
        spaceship.SpeedChanged += gameUI.OnSpeedChanged;

        spaceship.Destroyed += GameOver;
    }

    private void ScoreSetup(AsteroidSpawner asteroidSpawner, UFOSpawner UFOSpawner) 
    {
        scoreCounter = new ScoreCounter(genScore, UFOScore);
        scoreCounter.ScoreChanged += gameUI.OnScoreChanged;
        asteroidSpawner.AsteroidDestroyed += scoreCounter.OnAsteroidDestroyed;
        UFOSpawner.UFODestroyed += scoreCounter.OnUFODestroyed;
    }

    private void SpawnersSetup(AmmoFactory ammoFactory, Vector2 sceneDimension, int enemyHitMask, out AsteroidSpawner asteroidSpawner, out UFOSpawner UFOSpawner) 
    {
        asteroidSpawner = new AsteroidSpawner(startAsteroidsNum, asteroidsPerWaveIncrease, asteroidPrefab,
            genSizes, maxGenSpeeds, maxGenSpeeds, maxAsteroidGen, nextGenAsteroidsNum, sceneDimension, asteroidSpawnHorizontalOffset, asteroidSpawnVerticalOffset);

        var UFOCannon = new RechargeableWeapon(ammoFactory, AmmoType.CanonBullet, enemyHitMask, 1, UFOCannonRechargeDuration);
        UFOSpawner = new UFOSpawner(spaceshipTransform, UFOPrefab, UFOCannon, UFOMinSpeed, UFOMaxSpeed, UFOStartShootDelay,
            sceneDimension, UFOSpawnHorizontalOffset, UFOSpawnVerticalOffset, UFOMinSpawnTime, UFOMaxSpawnTime);
    }

    private void GameOver()
    {
        UpdatableController.I.RemoveAll();
        gameOverPopup.Show(scoreCounter.CurrentScore);
        gameOverPopup.TryAgainClick += RestartGame;
    }

    private void RestartGame() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}