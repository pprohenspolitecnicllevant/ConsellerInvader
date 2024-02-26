using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSpawner : MonoBehaviour
{
    [SerializeField] private GameObject invaderPrefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnInvader());
    }

    private IEnumerator SpawnInvader()
    {
        while (true)
        {
            GameObject invader = Instantiate(invaderPrefab, transform.position, Quaternion.identity);
            Destroy(invader, 30f);
            yield return new WaitForSeconds(3);
        }
    }
}
