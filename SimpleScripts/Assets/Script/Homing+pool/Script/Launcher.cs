using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Launcher : MonoBehaviour
{
    public Transform Cannon;
    public GameObject Target;
    public bool launch;
    [Range(1, 6)]
    public float Time = 1.0f;
    public KeyCode OnOff = KeyCode.R;
    public Text Key;
    public GameObject On;
    public GameObject Off;
    Coroutine routine;
    private void Start()
    {
        Key.text = OnOff.ToString();
    }
    private void Update()
    {
        if (Input.GetKeyDown(OnOff) && launch == false)
        {
            On.SetActive(true);
            Off.SetActive(false);
            launch = true;
            routine = StartCoroutine(Launch(Time));
        }
        else if (Input.GetKeyDown(OnOff) && launch == true)
        {
            Off.SetActive(true);
            On.SetActive(false);
            launch = false;
            StopCoroutine(routine);
        }
    }
    IEnumerator Launch(float Time)
    {
            while (launch == true)
            {
                yield return new WaitForSeconds(Time);
                PoolManager.instance.SpawItem("Mis", Cannon, Target);
            }
    }
}
