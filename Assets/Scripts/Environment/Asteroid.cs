using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    Rigidbody rb;
    public Vector3 origin;
    public int health;
    private BoxCollider bc;
    public Vector2 velocityRange;
    public Vector2 returnForceRange;
    private int damage;
    public void SetProperties(Vector2 range, Vector3 o, int h)
    {
        float scale = Random.Range(range.x, range.y);
        health = h;
        damage = (int)scale;
        transform.localScale = Vector3.one * scale;
        origin = o;
    }
    void Start()
    {
        returnForceRange = new Vector2(0,2);
        rb = GetComponent<Rigidbody>();
        bc = transform.parent.GetComponent<BoxCollider>();
        velocityRange = transform.parent.GetComponent<AsteroidHandler>().velocityRange;
        rb.AddForce(Random.Range(-velocityRange.x, velocityRange.y),
            Random.Range(-velocityRange.x, velocityRange.y),
            Random.Range(-velocityRange.x, velocityRange.y), ForceMode.Acceleration);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player") collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
        if (collision.gameObject.tag == "Bullet") TakeDamge(collision.gameObject.GetComponent<Bullet>().damage);
    }
    private void Update()
    {
        if (Mathf.Abs(transform.position.x - transform.parent.position.x) > bc.size.x * bc.gameObject.transform.localScale.x
            || Mathf.Abs(transform.position.y - transform.parent.position.y) > bc.size.y * bc.gameObject.transform.localScale.y
            || Mathf.Abs(transform.position.z - transform.parent.position.z) > bc.size.z * bc.gameObject.transform.localScale.z)
        {
            rb.AddForce((transform.parent.position - transform.position)*Random.Range(returnForceRange.x,returnForceRange.y), ForceMode.Acceleration);
        }
    }
    public void TakeDamge(int damage)=>health -= damage;
}
