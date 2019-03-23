using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessMonster : MonoBehaviour
{
    [SerializeField] List<Potion> potionOptions;
    [SerializeField] float minThrowRadius;
    [SerializeField] float maxThrowRadius;
    [SerializeField] float throwInterval;

    float throwTimer;

    // Start is called before the first frame update
    void Start()
    {
        throwTimer = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (throwTimer >= throwInterval)
        {
            //ThrowPotion();
            throwTimer = 0.0f;
        }
        else
        {
            throwTimer += Time.deltaTime;
        }
    }

    void ThrowPotion()
    {
        int indexOfPotion = Random.Range(0, potionOptions.Count);
        float throwLength = Random.Range(minThrowRadius, maxThrowRadius);
        float throwAngle = Random.Range(0f, 360f);


    }
}
