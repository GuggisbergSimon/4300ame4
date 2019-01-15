using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SpawnControlBehavior : PlayableBehaviour
{
    public TimelineManager timelineManager = null;
    
    public enum SpawnObject
    {
        NONE,
        ARROW,
        ELEPHANT,
        HORSE
    }

    public SpawnObject currentSpawnObject;
    

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        switch (currentSpawnObject)
        {
            case SpawnObject.ARROW:
            {
                timelineManager.SpawnArrowSliding();
                break;
            }
            case SpawnObject.ELEPHANT:
            {
                //timelineManager.SpawnElephant();
                break;
            }
            case SpawnObject.HORSE:
            {
                //timelineManager.SpawnHorse();
                break;
            }
        }
    }
}
