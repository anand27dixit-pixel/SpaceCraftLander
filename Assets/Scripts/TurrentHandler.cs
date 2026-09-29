using System;
using System.Collections.Generic;
using UnityEngine;

public class TurrentHandler : MonoBehaviour
{
    private const float SHOOT_INTERVAL=4f;
    [SerializeField] private Transform turrentTop;

    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float maxAngle=30f;
    [SerializeField] private float speed=1f;

    private List<Bullet> bullets=new List<Bullet>();

    private float nextShootTime=0f;

    void Start()
    {
        if(bullets!=null)
        bullets.Clear();
    }


    // Update is called once per frame
    void Update()
    {
        if(Time.time > nextShootTime)
        {
            ShootBullets();
            nextShootTime = Time.time + SHOOT_INTERVAL;
        }
        RotateTurrentTop();
    }


    private void RotateTurrentTop()
    {
        float angle = Mathf.Sin(Time.time * speed) * maxAngle;
        turrentTop.localRotation = Quaternion.Euler(0,0,angle);
    }

    private void ShootBullets()
    {
        GameObject bulletObject = Instantiate(bulletPrefabs,firePoint.position,Quaternion.identity);
        Bullet bullet =  bulletObject.GetComponent<Bullet>();
        Vector2 directionToShoot = firePoint.up;
        bullet.ShootBulletInDirection(directionToShoot);
        bullets.Add(bullet);
    }

}
