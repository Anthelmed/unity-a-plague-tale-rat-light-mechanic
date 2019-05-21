using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct  MoveSpeed : IComponentData
{
    public float Value;
}
