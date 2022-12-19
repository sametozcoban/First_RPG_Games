using System;
using System.Collections;
using System.Collections.Generic;
using RPG.Attributes;
using RPG.Core;
using UnityEngine;
using UnityEngine.Events;

public class Projectile : MonoBehaviour
{
    [SerializeField] bool isHoming = true;
    [SerializeField] GameObject hitEffect = null;
    [SerializeField]  float maxLifeTime = 10f;
    [SerializeField]  float bowSpeed = 2f;
    [SerializeField] UnityEvent onHit;
    [SerializeField] GameObject[] destroyOnHit = null;
    [SerializeField] float lifeAfterImpact = 2;

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
        if (isHoming && !target.IsDead())
        {
            transform.LookAt(GetAimLocation());
        }
        transform.Translate(Vector3.forward * bowSpeed * Time.deltaTime);
    }

    public void SetTarget(Health target , float damage , GameObject instigator) // Gönderilecek olan mermi için  hedef olarak belirlendiği method.
    {
        this.target = target;
        this.damage = damage;
        this.instigator = instigator;
        Destroy(gameObject, maxLifeTime);
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
        bowSpeed = 0;

        onHit.Invoke();

        if (hitEffect != null)
        {
            Instantiate(hitEffect, GetAimLocation(), transform.rotation);
        }
        foreach (GameObject toDestroy in destroyOnHit)
        {
            Destroy(toDestroy);
        }
        Destroy(gameObject);
    }
}
