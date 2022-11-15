using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float bowSpeed = 2f;

    Health target = null;
    private GameObject instigator = null;
    float damage = 0;

    private void Start()
    {
        transform.LookAt(GetAimLocation());
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null) return;
        bowWay();
    }

    public void SetTarget(Health target , float damage , GameObject instigator) // Gönderilecek olan mermi için  hedef olarak belirlendiği method.
    {
        this.target = target;
        this.damage = damage;
        this.instigator = instigator;
    }

    public void bowWay()
    {
        //transform.LookAt(GetAimLocation());
        transform.Translate(Vector3.forward * bowSpeed * Time.deltaTime);
    }

    private Vector3 GetAimLocation()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.transform.position;
        }
        return target.transform.position + Vector3.up * targetCapsule.height / 2; /* Capsule Collider yükseliğini ikiye bölerek posizyonuna eklediğimizde
                                                                                      Okun targetın neresine geleceğini hesaplıyoruz. */
    }

    private void OnTriggerEnter(Collider other) //Oklar hedefe vardığında hedefe hasar vuruyor ve sonra yok oluyorlar. Kontrolünü sağladığımız method.
    {
        if(other.GetComponent<Health>() != target) return;
        if(target.IsDead()) return;
        target.TakeDamage(instigator,damage);
        Destroy(gameObject);
    }
}
