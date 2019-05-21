using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class RatMovementSystem : JobComponentSystem
{
    [BurstCompile]
    private struct MovementJob : IJobForEach<Translation, Rotation, Destination, MoveSpeed, RotationSpeed>
    {
        public float DeltaTime;
        [ReadOnly]
        [DeallocateOnJobCompletion]
        public NativeArray<float3> LightSourcesPositions;
        [ReadOnly]
        [DeallocateOnJobCompletion]
        public NativeArray<float> LightSourcesRanges;
        
        public void Execute(
            ref Translation translation, 
            ref Rotation rotation, 
            [ReadOnly] ref Destination destination, 
            [ReadOnly] ref MoveSpeed moveSpeed, 
            [ReadOnly] ref RotationSpeed rotationSpeed)
        {
            var headingToDestination = destination.Value - translation.Value;
            var distanceToDestination = math.length(headingToDestination);
            var finalDirection = headingToDestination / distanceToDestination;

            for (var i = 0; i < LightSourcesPositions.Length; i++)
            {
                var headingToLight = LightSourcesPositions[i] - translation.Value;
                var distanceToLight = math.length(headingToLight);
                var directionToLight = headingToLight / distanceToLight;

                if (distanceToLight <= LightSourcesRanges[i])
                {
                    finalDirection = directionToLight * -1;
                    break;
                } 
            }
            
            finalDirection = new float3(finalDirection.x, 0, finalDirection.z);
            math.normalize(finalDirection);
                                                
            rotation.Value = math.slerp(
                rotation.Value, 
                quaternion.LookRotation(finalDirection, Vector3.up), 
                rotationSpeed.Value * DeltaTime);
            
            translation.Value = math.lerp(
                translation.Value, 
                translation.Value + math.forward(rotation.Value), 
                moveSpeed.Value * DeltaTime);
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var lightSourcesCount = LightSourcesController.Instance.LightSources.Length;
        var lightSourcesPositions = new NativeArray<float3>(lightSourcesCount, Allocator.TempJob);
        var lightSourcesRanges = new NativeArray<float>(lightSourcesCount, Allocator.TempJob);
        
        LightSourcesController.Instance.GetLightSourcesPositions(ref lightSourcesPositions);
        LightSourcesController.Instance.GetLightSourcesRanges(ref lightSourcesRanges);
        
        var job = new MovementJob
        {
            DeltaTime = Time.deltaTime,
            LightSourcesPositions = lightSourcesPositions,            
            LightSourcesRanges = lightSourcesRanges
        };
                
        return job.Schedule(this, inputDeps);
    }
}
