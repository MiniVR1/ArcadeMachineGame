using UnityEngine;

public class Sledgehammer : Item
{
    protected override void Start()
    {
        base.Start();

        /*
          Hammer orientation is head on bench. Flip right-side up.
         */
        uprightRotation = originRotation * Quaternion.Euler(0f, 0f, 180f);
    }
}
