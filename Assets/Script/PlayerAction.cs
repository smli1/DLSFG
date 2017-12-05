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

    private bool canPlant = false;
    public Transform ploughingPoint;
    public Transform plantingPoint;
    public Transform ploughedGround;
    public Transform seed;

    private Inventory inventory;
    #endregion

    #region Methods

    void Start() {
        m_animator = GetComponent<Animator>();
        GetComponent<SpriteRenderer>().receiveShadows = true;
        GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;

        inventory = GetComponent<Inventory>();
    }

    void Update() {
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxis("Horizontal");

        if (movementEnable) {

            m_animator.SetInteger("vertical", (int)(h * 10f));
            m_animator.SetInteger("horizontal", (int)(v * 10f));

            if (h != 0 || v != 0)
            {
                m_animator.SetBool("isStay", false);
            }
            else if (h == 0 && v == 0 && IsAllInputKeyUp())
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
                Collider[] colliders = FindNearbyColliders("pickable");

                if (colliders.Length != 0)
                {
                    movementEnable = false;
                    StartCoroutine(WaitToEnableMovement(0.75f));
                    isPickedUp = true;
                    m_animator.SetBool("isPickedUp", isPickedUp);
                    pickedObj = ClosestCollider(colliders).gameObject;
                    pickedObj.GetComponent<Rigidbody>().isKinematic = true;
                    pickedObj.GetComponent<Collider>().isTrigger = true;
                    pickedObj.transform.parent = gameObject.transform.GetChild(0);
                    pickedObj.transform.localPosition = Vector3.zero;
                }
            }

            //Code for player actions when using current tools
            if (Input.GetKeyDown(KeyCode.R)) {
                switch (GetComponent<Inventory>().CurrentTool()) {
                    case Tool.Shovel:
                        //Must in stay action before use Shovel and not Ploughing
                        if (m_animator.GetBool("isStay") && !m_animator.GetBool("isPloughing")) {
                            //Shovel Animation Start and player movement disable
                            movementEnable = false;
                            m_animator.SetBool("isPloughing", true);
                            //Ploughing the ground ready to plant
                            if (canPlant)
                            {
                                StartCoroutine( WaitForPloughingAnim(0.85f, GetDirection()));
                                //Instantiate(ploughedGround, ploughingPoint.position+getDirection()* 1.2f, ploughingPoint.rotation);
                            }
                        }
                        break;

                    case Tool.Dibber:
                        int[] index = inventory.SearchBag("seed");
                        Debug.Log(index[0] + "," + index[1]);
                        if (index[0] != -1) {

                            //Search infront for ground to plant seed
                            Collider[] colliders = FindNearbyColliders("PloughedGround");

                            if (colliders.Length != 0) {
                                GameObject nearbyGround = ClosestCollider(colliders).gameObject;
                                //Planting the seed in the nearby ground.
                                Transform newSeed = Instantiate(seed, nearbyGround.transform.position, plantingPoint.rotation);
                                //Making the parent of the seed the ground its planted in
                                newSeed.SetParent(nearbyGround.transform);
                                //Changing the tag of the ground so another seed cannot be planted in it
                                nearbyGround.tag = "Flowering";

                                //Creating new plant object and adding it to the seeds plantbehaviour script
                                string plantName = inventory.GetPlantName(index);
                                Plant newPlant = new PlantBuilder(plantName).SetUniqueValues().SetCommonValues().Build();
                                newSeed.GetComponent<PlantBehaviour>().SetPlant(newPlant);

                                inventory.RemoveItem(index);

                                Debug.Log(newSeed.GetComponent<PlantBehaviour>().GetPlant().GetName());
                            }
                        }

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
                StartCoroutine(WaitToEnableMovement(0.75f));
                isPickedUp = false;
                m_animator.SetBool("isPickedUp", isPickedUp);
                ReleasePickedUpObj();
            }
        }
    }

    bool IsAllInputKeyUp()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            return false;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            return false;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            return false;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            return false;
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            return false;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            return false;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            return false;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            return false;
        }
        return true;
    }

    IEnumerator WaitForPloughingAnim(float time , Vector3 direction)
    {
        yield return new WaitForSeconds(time);

        //Create ploughedGround
        GameObject temp = Instantiate(ploughedGround, ploughingPoint.position + direction * 1.2f, ploughingPoint.rotation).gameObject;
  
        movementEnable = true;
        m_animator.SetBool("isPloughing", false);

        //ploughedGround Fade Effect
        Color tempColor = temp.GetComponent<SpriteRenderer>().material.color;
        tempColor.a = 0;
        temp.GetComponent<SpriteRenderer>().material.color = tempColor;
        while (tempColor.a < 1)
        {
            tempColor.a += 0.2f;
            temp.GetComponent<SpriteRenderer>().material.color = tempColor;
            yield return new WaitForSeconds(0.01f);
        }
        
    }

    IEnumerator WaitToEnableMovement(float time)
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
        pickedObj.transform.position += GetDirection();
        Debug.Log(lastDir);
        pickedObj = null;
    }

    void SetDirection(AnimatorStateInfo animStateInfo)
    {
        
        if (animStateInfo.IsName("Idle_Front") || animStateInfo.IsName("Walk_Front") || animStateInfo.IsName("Pick_Idle_Front") || animStateInfo.IsName("Pick_Walk_Front"))
        {
            lastDir = -transform.forward;
            m_animator.SetInteger("direction", 1);
        }
        else if (animStateInfo.IsName("Idle_Left") || animStateInfo.IsName("Walk_Left") || animStateInfo.IsName("Pick_Idle_Left") || animStateInfo.IsName("Pick_Walk_Left"))
        {
            lastDir = -transform.right;
            m_animator.SetInteger("direction", 3);
        }
        else if (animStateInfo.IsName("Idle_Right") || animStateInfo.IsName("Walk_Right") || animStateInfo.IsName("Pick_Idle_Right") || animStateInfo.IsName("Pick_Walk_Right"))
        {
            lastDir = transform.right;
            m_animator.SetInteger("direction", 4);
        }
        else if (animStateInfo.IsName("Idle_Back") || animStateInfo.IsName("Walk_Back") || animStateInfo.IsName("Pick_Idle_Back") || animStateInfo.IsName("Pick_Walk_Back"))
        {
            lastDir = transform.forward;
            m_animator.SetInteger("direction", 2);
        }

    }

    Vector3 GetDirection()
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

    public Collider[] FindNearbyColliders(string tag) {
        Collider[] colliders = Physics.OverlapSphere(transform.position + GetDirection() * 0.8f - new Vector3(0, 1.0f, 0), 0.6f);
        Stack<Collider> filter = new Stack<Collider>();
        foreach (Collider c in colliders) {
            if (c.gameObject.tag != tag) continue;
            filter.Push(c);
            Debug.Log(c.name + " : " + Vector3.Distance(gameObject.transform.position, c.gameObject.transform.position));
        }

        colliders = filter.ToArray();

        return colliders;
    }

    public void SetCanPlant(bool trueorfalse) {
        canPlant = trueorfalse;
    }

    #endregion
}
