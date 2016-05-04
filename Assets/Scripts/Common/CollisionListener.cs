using UnityEngine;
using System.Collections;


public class CollisionListener : MonoBehaviour
{

    public float rotateAngle
    {
        get; set;
    }

    public string action
    {
        get; set;
    }

    public bool collided
    {
        get; set;
    }

    private bool collidedToGround;

    public void Update()
    {
        if (!collidedToGround)
        {
            transform.Rotate(Vector3.down, rotateAngle);
        }

        //Debug.Log("Velocity magnitude: " + transform.GetComponent<Rigidbody>().velocity.magnitude.ToString("f2"));

    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.transform.GetComponent<Rigidbody>())
        {
            Rigidbody rig = col.gameObject.GetComponent<Rigidbody>();
            Vector3 direction = transform.TransformDirection(Vector3.forward);
            col.transform.GetComponent<Rigidbody>().AddRelativeForce(direction * 2, ForceMode.Impulse);
        }
        collidedToGround = true;
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.up / 4, ForceMode.Impulse);
        if (col.transform.tag == "Alchik")
        {
            collided = true;
            int k = new int();
            foreach (GameObject ch in c_StepByStepLogic.singletone.allPlayers[c_StepByStepLogic.singletone.indexOfCurrentPlayer].CollidedObjects)
            {
                if (col.gameObject != ch)
                {
                    k++;
                }
            }

            if (k <= 0)
                c_StepByStepLogic.singletone.allPlayers[c_StepByStepLogic.singletone.indexOfCurrentPlayer].CollidedObjects.Add(col.gameObject);
        }

        c_LounchSwipe.singletone.isLauched = true;
        
    }

}
