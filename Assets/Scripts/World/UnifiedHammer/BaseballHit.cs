using System.Collections;
using UnityEngine;

public class BaseballHit : HitBroadcaster
{
    private static BaseballHit instance;

    public static BaseballHit Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindAnyObjectByType<BaseballHit>();
            return instance;
        }
    }
}
