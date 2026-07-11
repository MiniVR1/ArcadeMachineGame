using System.Collections;
using UnityEngine;

public class HammerHit : HitBroadcaster
{
    private static HammerHit instance;

    public static HammerHit Instance
    {
        get
        {
            if (instance == null)
                instance = GameObject.FindAnyObjectByType<HammerHit>();
            return instance;
        }
    }
}
