using UnityEngine;

public class Glitches : MonoBehaviour
{
    public bool glitching = true;
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

    public void ChangeState(int zone)
    {
        if (glitching)
        {
            glitching = false;
        }
        else
        {
            glitching = true;
        }
        Debug.Log("I changed and I was from zone " + zone);
    }

    public void EnableGlitch()
    {
        glitching = true;
    }

    public void DisableGlitch()
    {
        glitching = false;
    }
}
