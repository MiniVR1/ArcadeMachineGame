using System;
using System.Diagnostics;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private float direction = 1.0f; 
    [SerializeField] private float moveSpeed = 5.0f;
    public enum MovePlatformType { Vertical, Horizontal };
    public MovePlatformType movePlatformType;
    private Vector3 position;

    public GameObject Player; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move(){
        switch(movePlatformType)
        {
            case MovePlatformType.Horizontal:
                position.x += direction * moveSpeed * Time.deltaTime;
                transform.position = position;
                break;
            case MovePlatformType.Vertical:
                position.y += direction * moveSpeed * Time.deltaTime;
                transform.position = position;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.CompareTag("DirectionChange")){
            direction = -direction;
        }
    }

    private void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.CompareTag("Player")){
            Player.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D other){
        if(other.gameObject.CompareTag("Player")){
            Player.transform.SetParent(null);
        }
    }
}
