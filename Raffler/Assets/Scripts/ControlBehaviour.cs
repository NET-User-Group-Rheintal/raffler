using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.Cinemachine;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class ControlBehaviour : MonoBehaviour
{
    [Header("Elements")] public GameObject[] spawnerPrefabs;
    public float spawnTimeMin = 2f;
    public float spawnTimeMax = 5f;

    [Header("Numbers")] public GameObject numberPrefab;
    public int maxNumber = 25;
    public bool spawnNumberNext = false;

    [Header("Cammeras")] public CinemachineVirtualCamera overViewCam;
    public CinemachineVirtualCamera detailCam;


    private float nextSpawnTime = 5f;

    public Queue<GameObject> ToSpawn = new Queue<GameObject>();

    public HashSet<int> UsedNumbers = new HashSet<int>();

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
            SpawnNumber();
            spawnNumberNext = false;
            Invoke(nameof(SwitchToDetail), 2f);
            nextSpawnTime = CalculateNextSpawnTime(Time.time, 3f);
        }
        else
        {
            SpawnElement();
            SwitchToOverview();
        }
    }

    public void SpawnElement()
    {
        Debug.Log("Spawning Element");

        var spawnIndex = Random.Range(0, spawnerPrefabs.Length);
        var element = Instantiate(spawnerPrefabs[spawnIndex]);
        element.SetActive(false);
        ToSpawn.Enqueue(element);
    }

    public void SpawnNumber()
    {
        Debug.Log("Spawning Number");
        int number;
        do
        {
            number = Random.Range(1, maxNumber);
        } while (UsedNumbers.Contains(number) && UsedNumbers.Count < maxNumber);

        UsedNumbers.Add(number);

        var newInstance = Instantiate(numberPrefab);
        newInstance.SetActive(false);
        ToSpawn.Enqueue(newInstance);

        var numberBehaviour = newInstance.GetComponent<NumberBehaviour>();
        numberBehaviour.Number = number;
        newInstance.name = number.ToString();

        this.detailCam.Follow = newInstance.transform;
    }

    private float CalculateNextSpawnTime(float time, float addedDelay = 0f)
    {
        return time + addedDelay + Random.Range(spawnTimeMin, spawnTimeMax);
    }

    public void SwitchToOverview()
    {
        this.detailCam.Priority = 5;
    }

    public void SwitchToDetail()
    {
        this.detailCam.Priority = 20;
    }
}