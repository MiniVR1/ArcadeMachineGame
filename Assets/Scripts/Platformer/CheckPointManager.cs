using UnityEngine;
using System.Collections.Generic;

public class CheckPointManager : MonoBehaviour
{

    public GameObject Player;
    ArcadePlayer arcadePlayer;
    CheckPoint checkpoint;
    List<CheckPoint> checkpoints = new List<CheckPoint>();
    List<CheckPoint> activeCheckpoints = new List<CheckPoint>();

    void Start()
    {
        checkpoints.AddRange(gameObject.GetComponentsInChildren<CheckPoint>());
        arcadePlayer = (ArcadePlayer)Player.GetComponent(typeof(ArcadePlayer));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CheckForActive(CheckPoint targetCheckpoint)
    {
        foreach (CheckPoint checkpoint in checkpoints)
        {
            if (activeCheckpoints.Contains(targetCheckpoint))
            {
                return;
            }
            else
            {
                if (activeCheckpoints.Count != 0)
                {
                    foreach (CheckPoint activeCheckpoint in activeCheckpoints)
                    {
                        activeCheckpoint.Deactivate();
                    }
                    activeCheckpoints.Clear();
                }
                activeCheckpoints.Add(targetCheckpoint);
                targetCheckpoint.Activate();
                SetRespawn(targetCheckpoint);
            }
        }
    }

    public void SetRespawn(CheckPoint checkPoint)
    {
        arcadePlayer.SetRespawn(checkPoint.transform.position);
    }
}
