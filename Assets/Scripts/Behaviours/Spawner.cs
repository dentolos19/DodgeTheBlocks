﻿using UnityEngine;
using Random = System.Random;

public class Spawner : MonoBehaviour
{

    private float _time;
    private readonly Random _randomizer = new Random();
    
    public GameObject goalPrefab;
    public GameObject obstaclePrefab;
    public Transform[] points;

    private void Update()
    {
        if (!(Time.time >= _time))
            return;
        Spawn();
        _time = Time.time + 1;
    }
    
    private void Spawn()
    {
        var random = _randomizer.Next(points.Length);
        for (var index = 0; index < points.Length; index++)
            Instantiate(random != index ? obstaclePrefab : goalPrefab, points[index].position, Quaternion.identity);
    }

}