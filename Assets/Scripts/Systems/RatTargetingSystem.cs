using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using Random = Unity.Mathematics.Random;

public class RatTargetingSystem : JobComponentSystem
{
    [BurstCompile]
    private struct TargetingJob : IJobForEach<Translation, Destination>
    {
        public Random Random;
        
        public void Execute(
            [ReadOnly] ref Translation translation, 
            ref Destination destination)
        {
            var distance = math.distance(translation.Value, destination.Value);
            
            if (distance <= 1)
            {
                var randomPoint = Random.NextFloat2(new float2(-11f, -14f), new float2(11f, 14f));
                
                destination.Value = new float3(randomPoint.x, 0, randomPoint.y);
            }  
        }
    }

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        var job = new TargetingJob
        {
            Random = new Random((uint)UnityEngine.Random.Range(1, 100000))
        };

        return job.Schedule(this, inputDeps);
    }
}