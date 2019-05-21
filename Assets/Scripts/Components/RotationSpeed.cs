using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct  RotationSpeed : IComponentData
{
    public float Value;
}
