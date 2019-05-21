using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using Random = Unity.Mathematics.Random;

public class RatSpawner : MonoBehaviour
{
    [Header("Spawn Properties")] 
    
    [SerializeField] private GameObject Prefab;
    [SerializeField] private int Count = 100;
    [SerializeField] private float Interval = 0.05f;
    
    [Header("Rat Properties")] 
    [SerializeField] private float moveSpeed = 0.95f;
    [SerializeField] private float rotationSpeed = 0.95f;

    private Entity _entity;
    private EntityManager _entityManager;

    private void Start()
    {
        _entity = GameObjectConversionUtility.ConvertGameObjectHierarchy(Prefab, World.Active);
        _entityManager = World.Active.EntityManager;

        for (var i = 0; i < Count; i++)
        {
            Invoke(nameof(Spawn), i * Interval);
        }
    }

    private void Spawn()
    {
        var instance = _entityManager.Instantiate(_entity);

        var random = new Random((uint) UnityEngine.Random.Range(1, 100000));
            
        var position = (float3)transform.position;
        var randomPoint = random.NextFloat2(new float2(-11f, -14f), new float2(11f, 14f));
        
        _entityManager.SetComponentData(instance, new Translation { Value = position });
        _entityManager.SetComponentData(instance, new Rotation { Value = quaternion.identity });
        _entityManager.SetComponentData(instance, new Destination { Value = new float3(randomPoint.x, 0, randomPoint.y) });
        
        _entityManager.SetComponentData(instance, new MoveSpeed { Value = moveSpeed });
        _entityManager.SetComponentData(instance, new RotationSpeed { Value = rotationSpeed });
    }
}
