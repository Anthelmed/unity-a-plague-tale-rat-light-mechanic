using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[RequiresEntityConversion]
public class RotationSpeedAuthoring : MonoBehaviour, IConvertGameObjectToEntity
{
    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var data = new RotationSpeed();
        dstManager.AddComponentData(entity, data);
    }
}
