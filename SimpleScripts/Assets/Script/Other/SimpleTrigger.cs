using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SimpleTrigger : MonoBehaviour
{
    public string Tag = "YourTag";
    public UnityEvent Enter = new UnityEvent();
    public UnityEvent Exit = new UnityEvent();
    
    void OnTriggerEnter(Collider target)
    {
        if (target.tag == Tag)
        {
            this.Enter.Invoke();
        }
    }
    void OnTriggerExit(Collider target)
    {
        if (target.tag == Tag)
        {
            this.Exit.Invoke();
        }
    }
}
