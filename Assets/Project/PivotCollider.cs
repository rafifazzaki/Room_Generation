using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PivotCollider : MonoBehaviour
{
    GameObject collidedObj;
    
    public enum ColliderChecker
    {
        Pivot,
        Dimension
    }

    public ColliderChecker colliderChecker;
    private void OnCollisionEnter(Collision collision)
    {
        if(colliderChecker == ColliderChecker.Pivot)
        {
            Invoke("PivotCallInstancer", 0.01f);
            
        }
        else if(colliderChecker == ColliderChecker.Dimension && collision != null)
        {
            collidedObj = collision.gameObject;
            Invoke("DimensionCallInstancer", 0.01f);

        }
        
    }

    void PivotCallInstancer()
    {
        Instancer.Instance.CheckIfPivotIsColliding(this.gameObject.transform);
        Destroy(this.GetComponent<Collider>());
        Destroy(this.GetComponent<Rigidbody>());
        Destroy(this);
        //Destroy(gameObject);
    }

    void DimensionCallInstancer()
    {
        
        Transform t;
        if (collidedObj.name == "Dimension")
        {
            Instancer.Instance.CheckIfDimensionIsColliding(this.gameObject.transform, true, this.transform.parent.GetComponentInParent<Section>().GetIsNotSymmetric());
        }
        else
        {
            t = collidedObj.transform;
            Instancer.Instance.CheckIfDimensionIsColliding(t, false, this.transform.parent.GetComponentInParent<Section>().GetIsNotSymmetric());
            
        }

        Destroy(this.GetComponent<Collider>());
        Destroy(this.GetComponent<Rigidbody>());
        Destroy(this);
    }

}
