using UnityEngine;

public class Glitches : MonoBehaviour
{
    public bool glitching;
    GameObject self;
    Rigidbody2D body;
    Collider2D collider;
    MeshRenderer mesh;

    public Material material1;
    public Material material2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        collider = GetComponent<BoxCollider2D>();
        mesh = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (glitching)
        {
            collider.enabled = false;
            mesh.material = material2;
        }
        else
        {
            collider.enabled = true;
            mesh.material = material1;
        }
    }

    public void EnableGlitch(int zone)
    {
        glitching = true;
        Debug.Log("I changed and I was from zone " + zone);
    }

    public void DisableGlitch()
    {
        glitching = false;
    }
}
