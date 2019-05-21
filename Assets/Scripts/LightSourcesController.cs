using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class LightSourcesController : MonoBehaviour
{
    [SerializeField] public Light[] LightSources; 
    
    public static LightSourcesController Instance;
    
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void GetLightSourcesPositions(ref NativeArray<float3> positions)
    {
        for (var i = 0; i < LightSources.Length; i++)
        {
            positions[i] = LightSources[i].transform.position;
        }
    }
    
    public void GetLightSourcesRanges(ref NativeArray<float> ranges)
    {
        for (var i = 0; i < LightSources.Length; i++)
        {
            ranges[i] = LightSources[i].range;
        }
    }
    
}
