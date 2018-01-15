using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public float radiusOverlap = 2f;

    private Inventory inventory;

    public Image alert;
    public Text alertText;
    public AudioClip diggingSound, wateringSound, footstepLeft,footstepRight;
    private AudioSource audioSource;
    bool nextFootstep = true;
    bool footstepLR = true;
    #endregion

    #region Methods

    void Start() {
        m_animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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
                if (nextFootstep)
                {
                    nextFootstep = false;
                    if (footstepLR)
                    {
                        audioSource.PlayOneShot(footstepLeft,0.6f);
                        footstepLR = !footstepLR;
                    }
                    else
                    {
                        audioSource.PlayOneShot(footstepRight, 0.3f);
                        footstepLR = !footstepLR;
                    }
                    StartCoroutine(WaitForFootstep(0.3f));
                }
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
            if (Input.GetKeyDown(KeyCode.F))
            {
                Collider[] colliders = FindNearbyColliders("pickable", 0.6f);

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
                        if (FindNearbyColliders("pickable", radiusOverlap).Length != 0)
                        {
                            Alert("Something block me...");
                            break;
                        }
                        //Must in stay action before use Shovel and not Ploughing
                        if (m_animator.GetBool("isStay") && !m_animator.GetBool("isPloughing")) {
                            
                            if (FindNearbyColliders("PloughedGround", radiusOverlap).Length == 0 && FindNearbyColliders("Flowering", radiusOverlap).Length == 0) {
                                //Ploughing the ground ready to plant
                                if (canPlant) {
                                    //Shovel Animation Start and player movement disable
                                    movementEnable = false;
                                    m_animator.SetBool("isPloughing", true);
                                    StartCoroutine(WaitForPloughingAnim(0.85f));
                                    audioSource.PlayOneShot(diggingSound,1f);
                                } else {
                                    Alert("I should only dig in the field...");
                                }
                            } else {
                                Alert("I have already dug here...");
                            }
                        }
                        break;

                    case Tool.Dibber:
                        int[] index = inventory.SearchBag("seed");
                        Debug.Log(index[0] + "," + index[1]);
                        if (index[0] != -1) {

                            //Search infront for ground to plant seed
                            Collider[] colliders = FindNearbyColliders("PloughedGround", 0.6f);

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
                            } else {
                                Alert("There is no where to plant the seed...");
                            }
                        }

                        break;

                    case Tool.WateringCan:
                        //Search infront for seed to water
                        Collider[] colliders1 = FindNearbyColliders("Flowering", 0.6f);

                        Collider[] colliders2 = FindNearbyColliders("PloughedGround", 0.6f);

                        List<Collider> collider = new List<Collider>();

                        collider.AddRange(colliders2);
                        collider.AddRange(colliders1);

                        Collider[] collider3 = collider.ToArray();
                        if (collider3.Length != 0) {
                            GameObject nearbyGround = ClosestCollider(collider3).gameObject;
                            if (!nearbyGround.transform.GetChild(0).gameObject.activeSelf) {
                                audioSource.PlayOneShot(wateringSound, 1f);
                                movementEnable = false;
                                m_animator.SetBool("isWatering", true);
                                StartCoroutine(WaitForWateringAnim(0.9f, nearbyGround));
                            }
                        } else {
                            Alert("There is nothing to water...");
                        }
                        break;
                }
            }
        }
        else if (isPickedUp)
        {
            if (Input.GetKeyDown(KeyCode.F))
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

    IEnumerator WaitForFootstep(float time)
    {
        yield return new WaitForSeconds(time);
        nextFootstep = true;
    }

    IEnumerator WaitForPloughingAnim(float time)
    {
        yield return new WaitForSeconds(time);

        movementEnable = true;
        m_animator.SetBool("isPloughing", false);

        if (FindNearbyColliders("Harvest", radiusOverlap).Length == 0) {
            //Create ploughedGround
            PloughGround();
            
        }else {
            FindAndHarvest();
        }
    }

    IEnumerator WaitForWateringAnim(float time, GameObject water)
    {
        yield return new WaitForSeconds(time);

        movementEnable = true;
        m_animator.SetBool("isWatering", false);


        water.GetComponent<Renderer>().material.SetColor("_Color", Color.gray);

        water.transform.GetChild(0).gameObject.SetActive(true);
        
    }

    void Alert(string msg, float duration = 3)
    {
        StopCoroutine(TurnAlertOff(duration));
        alert.CrossFadeAlpha(1, 0, true);
        alertText.CrossFadeAlpha(1, 0, true);
        alert.gameObject.SetActive(true);
        alertText.text = msg;
        StartCoroutine(TurnAlertOff(duration));
    }

    IEnumerator TurnAlertOff(float duration) {
        yield return new WaitForSeconds(duration - 1);
        alert.GetComponent<Image>().CrossFadeAlpha(0,1,false);
        alertText.CrossFadeAlpha(0, 1, false);
        yield return new WaitForSeconds(1f);
        alert.gameObject.SetActive(false);
    }

    public void PloughGround() {
        GameObject temp = Instantiate(ploughedGround, ploughingPoint.position + GetDirection() * 1.2f, ploughingPoint.rotation).gameObject;

        //ploughedGround Fade Effect
        Color tempColor = temp.GetComponent<SpriteRenderer>().material.color;
        tempColor.a = 0;
        temp.GetComponent<SpriteRenderer>().material.color = tempColor;
        while (tempColor.a < 1) {
            tempColor.a += 0.2f;
            temp.GetComponent<SpriteRenderer>().material.color = tempColor;
        }
    }

    public void FindAndHarvest() {
        Collider[] colliders = FindNearbyColliders("Harvest", 0.6f);

        if (colliders.Length != 0) {
            GameObject nearbyGround = ClosestCollider(colliders).gameObject;
            GameObject nearbyPlant = nearbyGround.transform.GetChild(1).gameObject;

            inventory.AddItem(nearbyPlant.GetComponent<PlantBehaviour>().plant.GetName() + "/flower");


            Destroy(nearbyPlant);
            nearbyGround.transform.tag = "PloughedGround";
            nearbyGround.GetComponent<Renderer>().material.SetColor("_Color", Color.white);
            nearbyGround.transform.GetChild(0).gameObject.SetActive(false);
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

    public Collider[] FindNearbyColliders(string tag, float radius) {
        Collider[] colliders = Physics.OverlapSphere(transform.position + GetDirection() * 0.8f - new Vector3(0, 1.0f, 0), radius);
        Stack<Collider> filter = new Stack<Collider>();
        foreach (Collider c in colliders) {
            if (c.gameObject.tag != tag) continue;
            filter.Push(c);
            //Debug.Log(c.name + " : " + Vector3.Distance(gameObject.transform.position, c.gameObject.transform.position));
        }

        colliders = filter.ToArray();

        return colliders;
    }

    public void SetCanPlant(bool trueorfalse) {
        canPlant = trueorfalse;
    }

    #endregion
}
