using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingProjectile : MonoBehaviour
{
    public float movementSpeed;
    public LayerMask layerMaskCollision;
    bool Co = false;
    GameObject Target;
    public float destroytime;
    private void OnTriggerEnter(Collider other)
    {
        if ((layerMaskCollision.value & (1 << other.transform.gameObject.layer)) > 0)
        {
            Co = true;                                              
            PoolManager.instance.SpawItem("Exp", this.transform, this.gameObject);
            StartCoroutine(disable(destroytime));
        }
    }
    private void FixedUpdate()
    {
        if (!Co && Target != null)
        {
            Transform t = Target.transform;
            var step = movementSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, t.position, step);
        }
    }
    IEnumerator disable(float time)
    {
        WaitForSeconds wait = new WaitForSeconds(time);
        yield return wait;
        this.gameObject.SetActive(false);
    }
    public void assignetarget(GameObject Gam)
    {
        Target = Gam;
    }
    public void OnEnable()
    {
        Co = false;
    }
}
