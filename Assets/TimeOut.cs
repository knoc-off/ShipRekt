using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeOut : MonoBehaviour
{

    public GameObject HitEffect;
    public GameObject thisBullet;
    private float time;


    // Start is called before the first frame update
    void Start()
    {
        time = Time.fixedTime+3;
    }

    // Update is called once per frame
    void Update()
    {
        if (time < Time.fixedTime)
            Destroy(thisBullet);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject effect = Instantiate(HitEffect, transform.position, Quaternion.identity);
        Destroy(effect, .5f);
        Destroy(gameObject);

    }



}
