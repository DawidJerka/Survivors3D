using System;
using UnityEngine;

[Serializable]
public class EnemySpawnEntry
{
    [SerializeField] private GameObject prefab;

    [Min(0f)]
    [SerializeField] private float weight = 1f;

    public GameObject Prefab => prefab;
    public float Weight => weight;
}