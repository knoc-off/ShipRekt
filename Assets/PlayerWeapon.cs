﻿using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public Transform firePoint;
    public GameObject bulletPrefab;
    public Camera camCam;
    public Rigidbody2D rigbdy;
    public float force = 20f;
    private Vector2 pz;
    private float angle;
    private float frames;

    private bool shot = false;
    void Update()
    {

        if (Input.GetButtonDown("Fire1"))
        {
            shot = false;
            frames = Time.fixedTime+(float).01;
            print("rot start1");
            pz = camCam.ScreenToWorldPoint(Input.mousePosition);
            Vector2 lookDir = pz - rigbdy.position;
            angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
            rigbdy.rotation = angle;
        }
        if (Time.fixedTime > frames && !shot)
        {
            shot = true;
            shoot();
        }

    }

    void shoot()
    {

        print("shoot");
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(firePoint.up * force, ForceMode2D.Impulse);
    }
}