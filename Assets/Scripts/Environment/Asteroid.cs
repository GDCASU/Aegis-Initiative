using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : EnvironmentHealth
{
    Rigidbody rb;
    public Vector3 origin;
    private BoxCollider bc;
    public Vector2 velocityRange;
    public Vector2 returnForceRange;
    public int damage;
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
        returnForceRange = new Vector2(0,1);
        rb = GetComponent<Rigidbody>();
        bc = transform.parent.GetComponent<BoxCollider>();
        velocityRange = transform.parent.GetComponent<AsteroidHandler>().velocityRange;
        rb.AddForce(Random.Range(-velocityRange.x, velocityRange.y),
            Random.Range(-velocityRange.x, velocityRange.y),
            Random.Range(-velocityRange.x, velocityRange.y), ForceMode.Acceleration);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) Time.timeScale = Time.timeScale==1?0: 1;

        if (Mathf.Abs(transform.localPosition.x) > bc.size.x * .5f 
            || Mathf.Abs(transform.localPosition.y) > bc.size.y * .5f 
            || Mathf.Abs(transform.localPosition.z) > bc.size.z * .5f)
        {
            rb.AddForce((transform.parent.position + bc.center - transform.position)*Random.Range(returnForceRange.x,returnForceRange.y), ForceMode.Acceleration);
        }
    }
}
