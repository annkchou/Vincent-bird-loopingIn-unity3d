using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveForward : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(Deactivate());

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.forward * 5.0f * Time.deltaTime);
    }
    IEnumerator Deactivate()
    {
        yield return new WaitForSeconds(3.0f);
        gameObject.SetActive(false);
    }
}
