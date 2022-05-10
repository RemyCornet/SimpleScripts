using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disable : MonoBehaviour
{
    public float Time;
    void OnEnable()
    {
        StartCoroutine(Disa(Time));
    }
    IEnumerator Disa(float time)
    {
        yield return new WaitForSeconds(time);       
        this.gameObject.SetActive(false);
        yield return true;
    }

}
