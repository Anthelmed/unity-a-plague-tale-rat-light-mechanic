using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct  Destination : IComponentData
{
    public float3 Value;
}
