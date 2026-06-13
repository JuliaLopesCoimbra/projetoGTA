using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieApocalypseSpawner : MonoBehaviour
{
    [Header("Prefabs")]
    public GameObject zombiePrefab;
    public GameObject personPrefab;

    [Header("Quantidades")]
    public int zombieCount = 10;
    public int personCount = 6;

    [Header("Debug")]
    public bool showGizmos = true;

    [Header("Sons")]
    public AudioClip somMorteZumbi;

    private static readonly Vector3[][] personRoutes = new Vector3[][]
    {
        new Vector3[] { new Vector3( 50f, 0f, 125f), new Vector3(120f, 0f, 125f), new Vector3(190f, 0f, 125f) },
        new Vector3[] { new Vector3(190f, 0f,  55f), new Vector3(120f, 0f,  55f), new Vector3( 50f, 0f,  55f) },
        new Vector3[] { new Vector3( 45f, 0f, 120f), new Vector3( 45f, 0f,  90f), new Vector3( 45f, 0f,  60f) },
        new Vector3[] { new Vector3(195f, 0f,  60f), new Vector3(195f, 0f,  90f), new Vector3(195f, 0f, 120f) },
        new Vector3[] { new Vector3( 80f, 0f,  90f), new Vector3(120f, 0f,  90f), new Vector3(160f, 0f,  90f) },
        new Vector3[] { new Vector3(100f, 0f,  75f), new Vector3(140f, 0f,  75f), new Vector3(180f, 0f,  75f) },
    };

    private static readonly Vector3[] personSpawns = new Vector3[]
    {
        new Vector3( 50f, 0f, 125f),
        new Vector3(190f, 0f,  55f),
        new Vector3( 45f, 0f, 120f),
        new Vector3(195f, 0f,  60f),
        new Vector3( 80f, 0f,  90f),
        new Vector3(100f, 0f,  75f),
    };

    private static readonly Vector3[] zombieSpawns = new Vector3[]
    {
        new Vector3( 60f, 0f, 120f),
        new Vector3(100f, 0f, 120f),
        new Vector3(150f, 0f, 120f),
        new Vector3(190f, 0f, 110f),
        new Vector3(190f, 0f,  80f),
        new Vector3(160f, 0f,  60f),
        new Vector3(120f, 0f,  60f),
        new Vector3( 80f, 0f,  60f),
        new Vector3( 50f, 0f,  80f),
        new Vector3(120f, 0f,  90f),
    };

    private static readonly Vector3[][] zombiePatrols = new Vector3[][]
    {
        new Vector3[] { new Vector3( 60f,0f,120f), new Vector3( 80f,0f,120f), new Vector3( 80f,0f,105f) },
        new Vector3[] { new Vector3(100f,0f,120f), new Vector3(120f,0f,120f), new Vector3(120f,0f,105f) },
        new Vector3[] { new Vector3(150f,0f,120f), new Vector3(170f,0f,120f), new Vector3(170f,0f,105f) },
        new Vector3[] { new Vector3(190f,0f,110f), new Vector3(175f,0f,110f), new Vector3(175f,0f, 90f) },
        new Vector3[] { new Vector3(190f,0f, 80f), new Vector3(175f,0f, 80f), new Vector3(175f,0f, 65f) },
        new Vector3[] { new Vector3(160f,0f, 60f), new Vector3(140f,0f, 60f), new Vector3(140f,0f, 75f) },
        new Vector3[] { new Vector3(120f,0f, 60f), new Vector3(100f,0f, 60f), new Vector3(100f,0f, 75f) },
        new Vector3[] { new Vector3( 80f,0f, 60f), new Vector3( 60f,0f, 60f), new Vector3( 60f,0f, 75f) },
        new Vector3[] { new Vector3( 50f,0f, 80f), new Vector3( 50f,0f, 95f), new Vector3( 65f,0f, 95f) },
        new Vector3[] { new Vector3(120f,0f, 90f), new Vector3(140f,0f, 90f), new Vector3(140f,0f,105f) },
    };

    void Start()
    {
        SpawnPersonagens();
        SpawnZumbis();
    }

    void SpawnPersonagens()
    {
        int count = Mathf.Min(personCount, personSpawns.Length);
        for (int i = 0; i < count; i++)
        {
            GameObject person = Instantiate(personPrefab, personSpawns[i], Quaternion.identity);
            person.name = "Personagem_" + i;

            followPath fp = person.GetComponent<followPath>();
            if (fp != null)
                fp.waypoints = personRoutes[i % personRoutes.Length];
        }
        Debug.Log($"[Spawner] {count} personagens spawnados.");
    }

    void SpawnZumbis()
    {
        int count = Mathf.Min(zombieCount, zombieSpawns.Length);
        for (int i = 0; i < count; i++)
        {
            GameObject zombie = Instantiate(zombiePrefab, zombieSpawns[i], Quaternion.identity);
            zombie.name = "Zumbi_" + i;
            zombie.tag = "Zumbi";

            if (zombie.GetComponent<ZombieVida>() == null)
                zombie.AddComponent<ZombieVida>();

            ZombieVida zVida = zombie.GetComponent<ZombieVida>();
            if (zVida != null && somMorteZumbi != null)
                zVida.somMorte = somMorteZumbi;

            ZombiePatrol patrol = zombie.GetComponent<ZombiePatrol>();
            if (patrol != null)
                patrol.waypoints = zombiePatrols[i % zombiePatrols.Length];
        }
        Debug.Log($"[Spawner] {count} zumbis spawnados.");
    }

    void OnDrawGizmos()
    {
        if (!showGizmos) return;

        Gizmos.color = Color.cyan;
        foreach (var route in personRoutes)
            for (int i = 0; i < route.Length; i++)
            {
                Gizmos.DrawSphere(route[i], 1f);
                if (i < route.Length - 1)
                    Gizmos.DrawLine(route[i], route[i + 1]);
            }

        Gizmos.color = Color.green;
        foreach (var sp in personSpawns)
            Gizmos.DrawCube(sp, Vector3.one * 1.5f);

        Gizmos.color = Color.red;
        foreach (var patrol in zombiePatrols)
            for (int i = 0; i < patrol.Length; i++)
            {
                Gizmos.DrawSphere(patrol[i], 1f);
                if (i < patrol.Length - 1)
                    Gizmos.DrawLine(patrol[i], patrol[i + 1]);
            }

        Gizmos.color = new Color(1f, 0.5f, 0f);
        foreach (var sp in zombieSpawns)
            Gizmos.DrawCube(sp, Vector3.one * 1.5f);
    }
}   