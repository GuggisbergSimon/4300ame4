using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpawnControlAsset : PlayableAsset
{
    public ExposedReference<TimelineManager> tilelineManager;
    public SpawnControlBehavior.SpawnObject objectToSpawn;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<SpawnControlBehavior>.Create(graph);

        var spawnControlBehaviour = playable.GetBehaviour();
        spawnControlBehaviour.timelineManager = tilelineManager.Resolve(graph.GetResolver());
        spawnControlBehaviour.currentSpawnObject = objectToSpawn;

        return playable;
    }
}
