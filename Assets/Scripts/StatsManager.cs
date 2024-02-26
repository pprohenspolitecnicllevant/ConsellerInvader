using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.Networking;

/// <summary>
/// Classe estática con los objetos 'invader', 'player' y 'tower'
/// que contienen las 'stats' o propiedades configurables por un fichero
/// de configuración externo en formato JSON y que los diseñadores podrán así
/// modificar sin abrir el editor de o modificar código.
/// </summary>
public static class StatsManager
{
    /// <summary>
    /// Esta són las tres propiedades estáticas que se podran consultar
    /// en qualquier momento desde qualquier componenete para consutlar
    /// las 'stats' o propiedades establecidas por el diseñador
    /// </summary>
    public static Invader invader;
    public static Player player;
    public static Tower tower;
    public static Terrain terrain;

    /// <summary>
    /// Método que se tendrá que llamar una sola vez al principio de la ejecución
    /// del juego para cargar la configuracón externa del fichero JSON.
    /// Se aconseja que este método sea llamado de forma estática por el objeto
    /// 'GameManager' o similar.
    /// </summary>
    /// 
    public static void Init()
    {
        string jsonInfo;
        string filePath = Path.Combine(Application.streamingAssetsPath, "InvadersData.json");
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        www.SendWebRequest();
        while (!www.isDone)
        {

        }
        jsonInfo = www.downloadHandler.text;

        CreateStatsObject(jsonInfo);
    }

    public static IEnumerator InitCo()
    {
        string jsonInfo;
        string filePath = Path.Combine(Application.streamingAssetsPath, "InvadersData.json");
        UnityWebRequest www = UnityWebRequest.Get(filePath);
        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
        {
            // Handle error
            Debug.LogError(www.error);
            yield break;
        }

        jsonInfo = www.downloadHandler.text;
        CreateStatsObject(jsonInfo);
    }

    private static void CreateStatsObject(string _jsonInfo)
    {
        GameConfiguration gameConfiguration = JsonUtility.FromJson<GameConfiguration>(_jsonInfo);

        if (gameConfiguration is not null)
        {
            invader = gameConfiguration.invader;
            player = gameConfiguration.player;
            tower = gameConfiguration.tower;
            terrain = gameConfiguration.terrain;
        }
    }

    /// <summary>
    /// Classe serializable que recogerá la información proviniente del 
    /// fichero JSON donde el diseñador del juego establecerá la configuración
    /// de las propiedades que definen el funcionamiento de
    /// el jugador, los invaders y la base
    /// </summary>
    [Serializable]
    public class GameConfiguration 
    {
        public Invader invader = new();
        public Player player = new();
        public Tower tower = new();
        public Terrain terrain = new();
    }

    /// <summary>
    /// Classe que representa las 'stats' o propiedades de un invader 
    /// en el juego. Se compone de varias classes para encapsular información
    /// relacionada.
    /// </summary>
    [Serializable]
    public class Invader
    {
        public Motion motion = new();
        public Shooting shooting = new();
        public InvaderLife life = new();
        public Spawning spawning = new();
    }

    [Serializable]
    public class Motion
    {
        public string type;
        public float minSpeed;
        public float maxSpeed;
    }
    [Serializable]
    public class Shooting
    {
        public int minShots;
        public int maxShots;
        public float minShootTimer;
        public float maxShootTimer;
        public int distanceStopAttack;
    }
    [Serializable]
    public class InvaderLife
    {
        public int minHitsToDead;
        public int maxHitsToDead;
        public float minHealingPower;
        public float maxHealingPower;
    }
    [Serializable]
    public class Spawning
    {
        public float minSpawnTime;
        public float maxSpawnTime;
    }

    /// <summary>
    /// Classe que representa las 'stats' o propiedades del jugador 
    /// en el juego. Se compone de varias classes para encapsular información
    /// relacionada.
    /// </summary>
    [Serializable]
    public class Player
    {
        public PlayerLife life = new();
        public LaserGun laserGun = new();
        public MagnetGun magnetGun = new();
    }

    [Serializable]
    public class PlayerLife
    {
        public int numHitsToDead;
        public float energyLossOnHit;
        public float healingAbsorb;
    }

    [Serializable]
    public class LaserGun
    {
        public float timeCounterDuration;
        public float coolSpeed;
        public float heatSpeed;
        public float bulletSpeed;
    }

    [Serializable]
    public class MagnetGun
    {
        public float timeCounterDuration;
        public float coolSpeed;
        public float heatSpeed;
        public float magnetStrength;
        public float magnetStopDistance;
        public float magnetRange;
    }

    /// <summary>
    /// Classe que representa las 'stats' o propiedades de la torre o base 
    /// en el juego. Se compone de varias classes para encapsular información
    /// relacionada.
    /// </summary>
    [Serializable]
    public class Tower
    {
        public int numCovers;
        public Cover cover = new();
    }

    [Serializable]
    public class Cover
    {
        public float startEnergyLevel;
        public int numHitsToExplode;
        public float energyLossOnHit;
        public float healingPowerAbs;

        public float minIntensGlow;
        public float maxIntensGlow;
        public float glowSpeed;
        public float maxLife;
    }

    /// <summary>
    /// Classe que representa la configuración de parpadeo glow 
    /// de la cuadrícula del terreno.
    /// </summary>
    [Serializable]
    public class Terrain
    {
        public float minIntensGlow;
        public float maxIntensGlow;
        public float glowSpeed;
    }
}
