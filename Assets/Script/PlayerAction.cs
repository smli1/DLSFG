using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAction : MonoBehaviour {

    #region Variables

    private Animator m_animator;
    public float speed = 10;
    private bool movementEnable = true;
    private Vector3 lastDir = Vector3.zero;
    private bool isPickedUp = false;
    //private float force = 50f;
    private GameObject pickedObj = null;

    public Transform plantingPoint;
    public Transform ploughedGround;
    private Tool m_tool = Tool.Shovel;
    #endregion

    #region Methods

    void Start() {
        m_animator = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().receiveShadows = true;
        GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;

    }


    void Update() {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        m_animator.SetInteger("vertical", (int)(h * 5f));
        m_animator.SetInteger("horizontal", (int)(v * 5f));

        if (movementEnable) { 

            if (h != 0 || v != 0)
            {
                m_animator.SetBool("isStay", false);
            }
            else if (h == 0 && v == 0)
            {
                m_animator.SetBool("isStay", true);
            }

            transform.position += transform.forward * Time.deltaTime * v * speed;
            transform.position += transform.right * Time.deltaTime * h * speed;  
            
        }
        SetDirection(m_animator.GetCurrentAnimatorStateInfo(0));

        if (m_animator.GetBool("isStay") && isPickedUp == false) {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position + getDirection() * 0.8f - new Vector3(0, 1.0f, 0), 0.6f);
                Stack<Collider> filter = new Stack<Collider>();
                foreach (Collider c in colliders)
                {
                    if (c.gameObject.tag != "pickable") continue;
                    filter.Push(c);
                    Debug.Log(c.name + " : " + Vector3.Distance(gameObject.transform.position, c.gameObject.transform.position));
                }

                colliders = filter.ToArray();

                if (colliders.Length != 0)
                {
                    movementEnable = false;
                    StartCoroutine(waitToEnable(0.75f));
                    isPickedUp = true;
                    m_animator.SetBool("isPickedUp", isPickedUp);
                    //Debug.Log(closestCollider(colliders).name);
                    pickedObj = ClosestCollider(colliders).gameObject;
                    pickedObj.GetComponent<Rigidbody>().isKinematic = true;
                    pickedObj.GetComponent<Collider>().isTrigger = true;
                    pickedObj.transform.parent = gameObject.transform.GetChild(0);
                    pickedObj.transform.localPosition = Vector3.zero;
                    //gameObject.transform.GetChild(0);gameObject.transform.GetChild(0).gameObject.set
                }
            }

            //Code for player actions when using current tools
            if (Input.GetKeyDown(KeyCode.R)) {
                switch (m_tool) {
                    case Tool.Shovel:
                        //Ploughing the ground ready to plant
                        Instantiate(ploughedGround, plantingPoint.position, plantingPoint.rotation);
                        break;

                    case Tool.Dibber:
                        //Search infront for ground to plant seed
                        break;

                    case Tool.WateringCan:
                        //Search infront for seed to water
                        break;
                }
            }
        }
        else if (isPickedUp == true)
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                movementEnable = false;
                StartCoroutine(waitToEnable(0.75f));
                isPickedUp = false;
                m_animator.SetBool("isPickedUp", isPickedUp);
                ReleasePickedUpObj();
            }
        }
    }

    IEnumerator waitToEnable(float time)
    {
        yield return new WaitForSeconds(time);
        movementEnable = true;
    }

    void ReleasePickedUpObj()
    {
        if (pickedObj == null) return;

        gameObject.transform.GetChild(0).localPosition = Vector3.zero;
        pickedObj.GetComponent<Rigidbody>().isKinematic = false;
        pickedObj.GetComponent<Collider>().isTrigger = false;
        pickedObj.transform.parent = null;
        pickedObj.transform.position += getDirection();
        pickedObj = null;
    }

    void SetDirection(AnimatorStateInfo animStateInfo)
    {

        if (animStateInfo.IsName("Idle_Font"))
        {
            lastDir = -transform.forward;
        }
        else if (animStateInfo.IsName("Idle_Left"))
        {
            lastDir = -transform.right;
        }
        else if (animStateInfo.IsName("Idle_Right"))
        {
            lastDir = transform.right;
        }
        else if (animStateInfo.IsName("Idle_Back"))
        {
            lastDir = transform.forward;
        }

    }

    Vector3 getDirection()
    {
        return lastDir;
    }

    Collider ClosestCollider(Collider[] colliders)
    {
        if (colliders.Length == 0) return null;
        if (colliders.Length == 1) return colliders[0];

        Collider closestCol = colliders[0];

        foreach (Collider c in colliders)
        {
            if (Vector3.Distance(gameObject.transform.position,closestCol.transform.position) > Vector3.Distance(gameObject.transform.position,c.transform.position))
            {
                closestCol = c;
            }
        }

        return closestCol;
    }

    #endregion
}
