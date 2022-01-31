using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BombManager : MonoBehaviour
{
    public static BombManager Instance;

    [Header("Map")]
    public Tilemap ObstaclesMap;
    
    [Header("Object")]
    public GameObject ExplosionMiddleObject;
    public GameObject ExplosionLineObject;
    public GameObject ExplosionSideObject;
    [SerializeField] private GameObject BombObject;

    public delegate void ExplodeDelegate();
    public static event ExplodeDelegate OnExplode;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void CreateBomb(Vector3Int cellPos)
    {
        Vector3 gridPos = ObstaclesMap.GetCellCenterLocal(cellPos);
        Bomb.Create(BombObject, gridPos);
        SoundManager.Play("BombPlace");
    }

    public void Explode()
    {
        OnExplode?.Invoke();
    }
}