using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessMonster : MonoBehaviour
{
    [SerializeField] List<Potion> potionOptions;
    [SerializeField] float minThrowRadius;
    [SerializeField] float maxThrowRadius;
    [SerializeField] float throwInterval;
    [SerializeField] float throwSpeed;

    float throwTimer;
    List<Potion> thrownPotions;

    // Start is called before the first frame update
    void Start()
    {
        thrownPotions = new List<Potion>();
        throwTimer = 0.0f;
        //ThrowPotion();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (throwTimer >= throwInterval)
        {
            ThrowPotion();
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
        float throwAngle = Random.Range(0f, 2*Mathf.PI);

        Potion spawnedPotion = Instantiate<Potion>(potionOptions[indexOfPotion]);
        thrownPotions.Add(spawnedPotion);
        spawnedPotion.gameObject.transform.position = gameObject.transform.position;
        spawnedPotion.Throw(new Vector3(gameObject.transform.position.x + throwLength*Mathf.Cos(throwAngle), gameObject.transform.position.y + throwLength * Mathf.Sin(throwAngle), 0), throwSpeed);
    }

    
    void OnDestroy()
    {
        for (int i = 0; i < thrownPotions.Count; i++)
        {
            if (thrownPotions[i] != null)
            {
                Destroy(thrownPotions[i].gameObject);
            }
        }
    }
    
}
