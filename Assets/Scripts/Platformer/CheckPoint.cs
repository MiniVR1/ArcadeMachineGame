using Unity.VisualScripting;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    public bool isActive;

    public Material activeMaterial;
    public Material deactiveMaterial;
    public SpriteRenderer flagSprite;

    MeshRenderer mesh;
    CheckPointManager manager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        manager = (CheckPointManager)transform.parent.GetComponent(typeof(CheckPointManager));
        mesh = GetComponent<MeshRenderer>();
        Debug.Log("My boss is the " + manager);
        this.mesh.material = deactiveMaterial;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            manager.CheckForActive(this);
            if(flagSprite != null)
            {
                flagSprite.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
                SoundManager.instance.PlayUISound(SoundManager.instance.passCheckpoint);
            }
        }
    }

    public bool ReturnActive()
    {
        return isActive;
    }

    public void Deactivate()
    {
        isActive = false;
        this.mesh.material = deactiveMaterial;
    }

    public void Activate()
    {
        isActive = true;
        this.mesh.material = activeMaterial;
    }
}
