using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ControlBehaviour : MonoBehaviour
{
    [Header("Elements")] public GameObject[] spawnerPrefabs;
    public float spawnTimeMin = 2f;
    public float spawnTimeMax = 5f;

    [Header("Numbers")] public GameObject numberPrefab;
    public int maxNumber = 30;


    public bool spawnNumberNext = false;
    private float nextSpawnTime = 5f;

    public Queue<GameObject> ToSpawn = new Queue<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        var args = System.Environment.GetCommandLineArgs();

        if (args.Length >= 1)
        {
            try
            {
                maxNumber = int.Parse(args[1]);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"CLI Argument wrong. was: {args[1]}");
                //not a proper CLI Argument, just ignore.
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            Spawn();
            nextSpawnTime = CalculateNextSpawnTime(Time.time);
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit(0);
        }

        if (Input.GetKey(KeyCode.Space))
        {
            spawnNumberNext = true;
        }
    }

    public void Spawn()
    {
        if (spawnNumberNext)
        {
            SpawnNumber(Random.Range(1, maxNumber));
            spawnNumberNext = false;
        }
        else
        {
            SpawnElement();
        }
    }

    public void SpawnElement()
    {
        Debug.Log("Spawning Element");

        var spawnIndex = Random.Range(0, spawnerPrefabs.Length);
        ToSpawn.Enqueue(Instantiate(spawnerPrefabs[spawnIndex]));
    }

    public void SpawnNumber(int number)
    {
        Debug.Log("Spawning Number");
        var newInstance = Instantiate(numberPrefab);
        ToSpawn.Enqueue(newInstance);

        var numberBehaviour = newInstance.GetComponent<NumberBehaviour>();
        numberBehaviour.Number = number;
        newInstance.name = number.ToString();
    }

    private float CalculateNextSpawnTime(float time)
    {
        return time + Random.Range(spawnTimeMin, spawnTimeMax);
    }
}