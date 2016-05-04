using UnityEngine;
using System.Collections;

public class c_LounchSwipe : MonoBehaviour
{
    private static c_LounchSwipe instance;
    public static c_LounchSwipe singletone
    {
        get
        {
            return instance;
        }
    }


    public string action;
    public GameObject Chukko;
    public Transform ChukkoLaunchPosition;
    public float ShootAngle = 180;

    private GameObject trail;
    
    public bool isLauched = false;


    public void Start()
    {
        instance = this;
    }

    // Subscribe to events void OnEnable(){
    void OnEnable()
    {
        EasyTouch.On_SwipeStart += On_SwipeStart;
        EasyTouch.On_Swipe += On_Swipe;
        EasyTouch.On_SwipeEnd += On_SwipeEnd;
    }

    void OnDisable()
    {
        UnsubscribeEvent();

    }

    void OnDestroy()
    {
        UnsubscribeEvent();
    }

    void UnsubscribeEvent()
    {
        EasyTouch.On_SwipeStart -= On_SwipeStart;
        EasyTouch.On_Swipe -= On_Swipe;
        EasyTouch.On_SwipeEnd -= On_SwipeEnd;
    }

    private Vector2 swipeStartPosition;
    private Vector2 swipeEndPosition;

    private void On_SwipeStart(Gesture gesture)
    {

        // Only for the first finger
        if (gesture.fingerIndex == 0 && trail == null)
        {

            // the world coordinate from touch for z=5
            Vector3 position = gesture.GetTouchToWordlPoint(2);
            trail = Instantiate(Resources.Load("Trail"), position, Quaternion.identity) as GameObject;
            swipeStartPosition = gesture.position;
        }
    }

    // During the swipe
    private void On_Swipe(Gesture gesture)
    {

        if (trail != null)
        {

            // the world coordinate from touch for z=5
            Vector3 position = gesture.GetTouchToWordlPoint(2);
            trail.transform.position = position;



        }
    }

    // At the swipe end 
    private void On_SwipeEnd(Gesture gesture)
    {

        if (trail != null)
        {
            Destroy(trail);

            swipeEndPosition = gesture.position;

            // Get the swipe angle
            float angles = gesture.GetSwipeOrDragAngle();
            float length = gesture.swipeLength;
            float swipeTime = gesture.actionTime;
            float ForceStreight = length / swipeTime;
            Debug.Log("Last swipe : " + gesture.swipe.ToString() + " /  vector : " + gesture.swipeVector.normalized + " / angle : " + angles.ToString("f2") + " / " + gesture.deltaPosition.x.ToString("f5") + ", Vector Length: " + length.ToString() + " Event time: " + swipeTime.ToString() + " Force: " + ForceStreight.ToString());
            float launchAngle = new float();
            if (angles > 86.0f)
                launchAngle = -(angles * 0.0002f);
            else if (angles < 94.0f)
                launchAngle = (angles * 0.0004f);
            else if (angles <= 94.0f && angles >= 86.0f)
                launchAngle = 0;





            Vector3 dir = ChukkoLaunchPosition.transform.TransformDirection(Vector3.forward);
            
            if(!isLauched)
                LaunchChukko(ChukkoLaunchPosition.position, new Quaternion(ChukkoLaunchPosition.rotation.x, launchAngle, ShootAngle, ChukkoLaunchPosition.rotation.w), dir, ForceStreight, action);


        }

    }

    public void LaunchChukko(Vector3 position, Quaternion rotation, Vector3 ForceDirection, float force, string Action)
    {
        GameObject inst_Chukko = Instantiate(Chukko) as GameObject;
        inst_Chukko.transform.position = position;

        if (!inst_Chukko.GetComponent<Rigidbody>())
            inst_Chukko.AddComponent<Rigidbody>();

        inst_Chukko.GetComponent<Rigidbody>().mass = 0.3f;
        inst_Chukko.GetComponent<Rigidbody>().AddRelativeForce(ForceDirection * (force / 1.4f), ForceMode.Force);
        inst_Chukko.GetComponent<Rigidbody>().detectCollisions = true;
        inst_Chukko.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        //inst_Chukko.GetComponent<Rigidbody>().isKinematic = true;
        inst_Chukko.AddComponent<CollisionListener>();
        inst_Chukko.GetComponent<CollisionListener>().rotateAngle = Random.Range(10f, 50f);
        inst_Chukko.GetComponent<CollisionListener>().action = Action;
        c_StepByStepLogic.singletone.CurrentChukko = inst_Chukko;
        Debug.Log("Chukko launched! Rotation: (" + rotation.x + "; " + rotation.y + "; " + rotation.z + "; " + rotation.w + "), Rotating Angle: " + inst_Chukko.GetComponent<CollisionListener>().rotateAngle);
    }

}
