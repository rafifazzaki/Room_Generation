using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region Documentation
/*
 * --------------------------------------------------------------------------- Setup ---------------------------------------------------------------------------
 * Dimension and Pivot is only collided with their own Layer
 * Corridor/ Object that IsNotSymmetric is true have "Dimension" Collider don't have PivotCollider class attached
 * 
 * Dimension Parent (that holds collidable dimension) should be inactive so that it's not checking before other process such as CheckForRotate function is done
 * 
 * ------------------------------------------------------------------------- Functions -------------------------------------------------------------------------
 * void InstantiateRandomSection(), void InstantiateKeySection(), void InstantiateEndSection()
 * is Instantiating Section but respective to the list of GameObject.
 * 
 * void InitialKeySection()
 * is Initiating First Instantiation from list of keySection
 * 
 * 
 * Vector3 CalculatePositionRelativeToAvailableSpace(Transform AvailablePosition, Transform t, string sectionType, out string axisToRemove)
 * is Returning new position (whether rotated or not) for the instantiated object.
 * parameters : Transform AvailablePosition: position to instantiate, 
 *              Transform t: shift according to the scale of to be instantiated object
 *              string sectionType: get SectionType for determine whether the gameObject must be rotated or not
 *              out string axisToRemove: returning axis pivot that was opposite name of AvailablePosition
 * 
 * 
 * void CheckForRotate(Transform InstantiatedObj, Transform pivotObj, string sectionType, string axis)
 * is for fixing rotation and position of instantiated object
 * Parameters : Transform InstantiatedObj: get instantiated object reference,
 *              Transform pivotObj: get availablePosition as a reference of right position,
 *              string sectionType: get sectionType to determine how instantiated object supposed to be fixed,
 *              string axis: get opposite axis to determine how instantiated object supposed to be fixed
 *              
 * void AddSpace(GameObject obj, GameObject availableObject)
 * is for removing availableSpace that has been used and adding a new appropriate availableSpace
 * Parameters : GameObject obj: Instantiated object for getting availablePosition from object Section,
 *              GameObject availableObject: get availablePosition that supposed to be removed
 * 
 * IEnumerator Generate()
 * is to start the generation process
 * 
 * 
 * void EnableSectionDimension(GameObject obj)
 * is for enabling dimensions gameObject so the collider can start working by calling CheckIfDimensionIsColliding method
 * 
 * ----------------------------------------------------------------- Called from other Objects -----------------------------------------------------------------
 * 
 * 
 * public void CheckIfDimensionIsColliding(Transform collidingPivot, bool isEndSection, bool isNotSymmetric)
 * is called when an object collided with object with name "Dimension" (including itself) for removing possible spot from availableSpace 
 * Parameters : Transform collidingPivot: if it was detecting "Dimension" object then the object is itself, but if it's not then it's the collided object,
 *              bool isEndSection: is the object is perpendicular to other object so it needed to be closed or not,
 *              bool isNotSymmetric: is the object that colliding has a possibility to be rotated or not
 * 
 * 
 * Transform EndIntersection(Transform InstantiatedObj, Transform Collided, bool isNotSymmetric)
 * is returning position for closing section that was perpendicular to other object
 * Parameters : Transform InstantiatedObj: instantiated object reference,
 *              Transform Collided: to get the collided name of the object,
 *              bool isNotSymmetric: to determine whether it is object that can be rotated
 *              
 *              
 *              
 * public void CheckIfPivotIsColliding(Transform removePivot)
 * is called when an object pivot is collided with each other if it is then remove it from availableSpace
 * Parameters : Transform removePivot: pivot to be removed
 * 
 * 
 * public bool GetIsDoneGenerate()
 * get is generate is done
 * 
 * -------------------------------------------------------------------- What can be done --------------------------------------------------------------------
 * 
 * Changing from passing string axis to passing the actual object so string checking is not needed
 * GetIsDoneGenerate() can be extended to be event/delegate
 * maybe there is a bug where when it doesn't have more availableSpace it still looping
 * 
 * */
#endregion

public class Instancer : MonoBehaviour
{
    #region singleton
    private static Instancer _instance;

    public static Instancer Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public GameObject[] section;
    public List<GameObject> keySection;
    public GameObject[] EndSection;
    public GameObject miniEndSection;
    public int MaxRandomSection = 3;
    public bool isSectionRandom = false;

    [SerializeField]
    private List<Transform> availableSpace;

    //List<Transform> pivot;

    [SerializeField]
    List<Transform> rotatedObject;

    List<Transform> placeholderPivotArray;
    Transform placeholderObj;

    bool isDoneGenerate = false;

    private void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.J))
        {
            string sym = section[0].GetComponent<Section>().GetSectionType();
            Vector3 t = CalculatePositionRelativeToAvailableSpace(availableSpace[0], section[0].transform, sym, out string axis);
            GameObject newSection = Instantiate(section[0], t, Quaternion.identity);

            CheckForRotate(newSection.transform, availableSpace[0].transform, sym, axis);

            AddSpace(newSection, availableSpace[0].gameObject);
            newSection.GetComponent<Section>().EnableDimension();
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            string sym = section[1].GetComponent<Section>().GetSectionType();
            Vector3 t = CalculatePositionRelativeToAvailableSpace(availableSpace[0], section[1].transform, sym, out string axis);
            GameObject newSection = Instantiate(section[1], t, Quaternion.identity);

            CheckForRotate(newSection.transform, availableSpace[0].transform, sym, axis);

            AddSpace(newSection, availableSpace[0].gameObject);
            newSection.GetComponent<Section>().EnableDimension();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            string sym = section[2].GetComponent<Section>().GetSectionType();
            Vector3 t = CalculatePositionRelativeToAvailableSpace(availableSpace[0], section[2].transform, sym, out string axis);
            GameObject newSection = Instantiate(section[2], t, Quaternion.identity);

            CheckForRotate(newSection.transform, availableSpace[0].transform, sym, axis);

            AddSpace(newSection, availableSpace[0].gameObject);
            newSection.GetComponent<Section>().EnableDimension();
        }
        */ 
    }
    void Start()
    {
        StartCoroutine("Generate");
    }

    void EnableSectionDimension(GameObject obj)
    {
        obj.GetComponent<Section>().EnableDimension();
    }
    void InstantiateKeySection()
    {
        string sym = keySection[0].GetComponent<Section>().GetSectionType();
        Vector3 t = CalculatePositionRelativeToAvailableSpace(availableSpace[0], keySection[0].transform, sym, out string axis);
        GameObject newSection = Instantiate(keySection[0], t, Quaternion.identity);

        CheckForRotate(newSection.transform, availableSpace[0].transform, sym, axis);


        AddSpace(newSection, availableSpace[0].gameObject);
        keySection.Remove(keySection[0]);
        newSection.GetComponent<Section>().EnableDimension();
    }
    void InstantiateRandomSection()
    {

        int rand = Random.Range(0, section.Length);

        string sym = section[rand].GetComponent<Section>().GetSectionType();
        Vector3 t = CalculatePositionRelativeToAvailableSpace(availableSpace[0], section[rand].transform, sym, out string axis);
        GameObject newSection = Instantiate(section[rand], t, Quaternion.identity);

        CheckForRotate(newSection.transform, availableSpace[0].transform, sym, axis);

        AddSpace(newSection, availableSpace[0].gameObject);
        newSection.GetComponent<Section>().EnableDimension();


    }
    void InstantiateEndSection()
    {

        int rand = Random.Range(0, EndSection.Length);

        string sym = EndSection[rand].GetComponent<Section>().GetSectionType();
        Vector3 t = CalculatePositionRelativeToAvailableSpace(availableSpace[0], EndSection[rand].transform, sym, out string axis);
        GameObject newSection = Instantiate(EndSection[rand], t, Quaternion.identity);

        CheckForRotate(newSection.transform, availableSpace[0].transform, sym, axis);


        availableSpace.Remove(availableSpace[0]);
        newSection.GetComponent<Section>().EnableDimension();
    }
    void InitialKeySection()
    {
        GameObject initial = Instantiate(keySection[0], Vector3.zero, Quaternion.identity);
        placeholderPivotArray = initial.GetComponent<Section>().GetPivot();
        availableSpace.AddRange(placeholderPivotArray);

        placeholderPivotArray.Clear();
        placeholderPivotArray.TrimExcess();
    }
    Vector3 CalculatePositionRelativeToAvailableSpace(Transform AvailablePosition, Transform t, string sectionType, out string axisToRemove)
    {
        Vector3 position = Vector3.zero;
        axisToRemove = "";

        switch (AvailablePosition.name)
        {
            case "Pivot_L":
                if (sectionType == "isNotSymmetric")
                {
                    transform.Rotate(0f, 90f, 0f);
                }
                position = new Vector3(AvailablePosition.position.x - t.localScale.x, AvailablePosition.position.y, AvailablePosition.transform.position.z);
                axisToRemove = "Pivot_R";
                break;
            case "Pivot_R":
                if (sectionType == "isNotSymmetric")
                {
                    transform.Rotate(0f, 90f, 0f);
                }
                position = new Vector3(AvailablePosition.position.x + t.localScale.x, AvailablePosition.position.y, AvailablePosition.transform.position.z);
                axisToRemove = "Pivot_L";
                break;
            case "Pivot_U":
                position = new Vector3(AvailablePosition.position.x, AvailablePosition.position.y, AvailablePosition.transform.position.z + t.localScale.x);

                if (sectionType != "isNotSymmetric" && rotatedObject.Find((x) => x == AvailablePosition.parent.parent))
                {
                    position = new Vector3(AvailablePosition.position.x + t.localScale.x, AvailablePosition.position.y, AvailablePosition.transform.position.z);
                }
                axisToRemove = "Pivot_D";
                break;
            case "Pivot_D":
                position = new Vector3(AvailablePosition.position.x, AvailablePosition.position.y, AvailablePosition.transform.position.z - t.localScale.x);

                if (sectionType != "isNotSymmetric" && rotatedObject.Find((x) => x == AvailablePosition.parent.parent))
                {
                    position = new Vector3(AvailablePosition.position.x - t.localScale.x, AvailablePosition.position.y, AvailablePosition.transform.position.z);
                }
                axisToRemove = "Pivot_U";
                break;
            default:

                break;
        }

        return position;
    }
    void CheckForRotate(Transform InstantiatedObj, Transform pivotObj, string sectionType, string axis)
    {
        float localScaleX = pivotObj.parent.parent.transform.localScale.x;
        float localScaleZ = pivotObj.parent.parent.transform.localScale.z;

        if (sectionType == "isNotSymmetric")
        {
            if (axis == "Pivot_R" || axis == "Pivot_L")
            {
                InstantiatedObj.transform.Rotate(0f, 90f, 0f);
                rotatedObject.Add(InstantiatedObj);
            }
            else if (rotatedObject.Find((x) => x == pivotObj.parent.parent))
            {
                InstantiatedObj.transform.Rotate(0f, 90f, 0f);
                rotatedObject.Add(InstantiatedObj);
                //U is right D is Left because its always rotated 90 degree
                switch (axis)
                {
                    case "Pivot_U":
                        InstantiatedObj.transform.position = new Vector3(InstantiatedObj.position.x - localScaleX, InstantiatedObj.position.y, InstantiatedObj.transform.position.z + localScaleZ);
                        break;
                    case "Pivot_D":
                        
                        InstantiatedObj.transform.position = new Vector3(InstantiatedObj.position.x + localScaleX, InstantiatedObj.position.y, InstantiatedObj.transform.position.z - localScaleZ);
                        break;
                    default:

                        break;
                }
            }
        }
        else if (sectionType == "isAffectedAxis")
        {
            if (axis == "Pivot_U")
            {
                InstantiatedObj.transform.Rotate(0f, 180, 0f);
            }
            else if (axis == "Pivot_R")
            {
                InstantiatedObj.transform.Rotate(0f, -90f, 0f);
            }
            else if (axis == "Pivot_L")
            {
                InstantiatedObj.transform.Rotate(0f, 90f, 0f);
            }

            if (rotatedObject.Find((x) => x == pivotObj.parent.parent))
            {
                InstantiatedObj.transform.Rotate(0f, 90f, 0f);
            }
        }
    }
    void AddSpace(GameObject obj, GameObject availableObject)
    {
        availableSpace.Remove(availableSpace[0]);

        placeholderPivotArray = obj.GetComponent<Section>().GetPivot();
        placeholderPivotArray.Remove(placeholderPivotArray.Find((x) => x == availableObject));

        availableSpace.AddRange(placeholderPivotArray);

        placeholderPivotArray.Clear();
        placeholderPivotArray.TrimExcess();
    }
    IEnumerator Generate()
    {
        isDoneGenerate = false;
        
        InitialKeySection();
        yield return new WaitForSeconds(0.1f);

        int key = keySection.Count;

        while (key > 0)
        {
            if (key <= 0)
            {
                break;
            }
            else if (availableSpace.Count <= 0)
            {
                break;
            }
            else
            {
                key--;
            }

            InstantiateKeySection();
            yield return new WaitForSeconds(0.1f);

            #region Random Section
            int j = MaxRandomSection;
            if (isSectionRandom)
            {
                j = Random.Range(1, MaxRandomSection);
            }
            else if (MaxRandomSection <= 0)
            {
                j = 3;
            }
            #endregion
            
            while (j > 0)
            {
                if (j <= 0)
                {
                    //yield return new WaitForSeconds(1f);
                    break;
                }
                else if (availableSpace.Count <= 0)
                {
                    //yield return new WaitForSeconds(1f);
                    break;
                }
                else
                {
                    j--;
                }

                InstantiateRandomSection();
                yield return new WaitForSeconds(0.1f);

            }

            
        }


        int a = availableSpace.Count;
        while (a > 0)
        {
            if (a <= 0)
            {
                break;
            }
            else if (availableSpace.Count <= 0)
            {
                break;
            }
            else
            {
                a--;
            }

            InstantiateEndSection();
            yield return new WaitForSeconds(0.1f);
            
        }

        isDoneGenerate = true;
        Debug.Log("Done");
        yield return null;
    }

    Transform EndIntersection(Transform InstantiatedObj, Transform Collided, bool isNotSymmetric)
    {
        float ObjScaleX = InstantiatedObj.localScale.x;
        float ObjScaleZ = InstantiatedObj.localScale.z;

        Transform parent = InstantiatedObj.parent.parent.parent.transform;

        if (InstantiatedObj.parent.parent.parent.GetComponent<Section>().GetSectionType() == "isAffectedAxis")
        {
            Destroy(InstantiatedObj.gameObject);
        }
        else if (isNotSymmetric && rotatedObject.Find((x) => x == InstantiatedObj.parent.parent.parent))
        {
            Debug.Log("in");
            
            switch (Collided.name)
            {
                case "Dimension_U":
                    InstantiatedObj.position = new Vector3(InstantiatedObj.position.x + parent.localScale.x, InstantiatedObj.position.y, InstantiatedObj.position.z);
                    break;
                case "Dimension_D":
                    InstantiatedObj.position = new Vector3(InstantiatedObj.position.x - parent.localScale.x, InstantiatedObj.position.y, InstantiatedObj.position.z);
                    break;
            }
        }
        else
        {
            switch (Collided.name)
            {
                case "Dimension_L":
                    InstantiatedObj.transform.Rotate(0f, 90f, 0f);
                    InstantiatedObj.position = new Vector3(InstantiatedObj.position.x - parent.localScale.x, InstantiatedObj.position.y, InstantiatedObj.position.z);
                    break;
                case "Dimension_R":
                    InstantiatedObj.transform.Rotate(0f, 90f, 0f);
                    InstantiatedObj.position = new Vector3(InstantiatedObj.position.x + parent.localScale.x, InstantiatedObj.position.y, InstantiatedObj.position.z);
                    break;
                case "Dimension_U":
                    
                    InstantiatedObj.position = new Vector3(InstantiatedObj.position.x, InstantiatedObj.position.y, InstantiatedObj.position.z + parent.localScale.z);
                    break;
                case "Dimension_D":
                    InstantiatedObj.position = new Vector3(InstantiatedObj.position.x, InstantiatedObj.position.y, InstantiatedObj.position.z - parent.localScale.z);
                    break;
                default:

                    break;
            }
        }
        
        return this.transform;
    }
    public void CheckIfDimensionIsColliding(Transform collidingPivot, bool isEndSection, bool isNotSymmetric)
    {
        
        if (isEndSection)
        {
            Vector3 t = CalculatePositionRelativeToAvailableSpace(collidingPivot, miniEndSection.transform, "", out string axis);
            Vector3 r = new Vector3(collidingPivot.position.x, collidingPivot.position.y, collidingPivot.position.z);
            GameObject newSection = Instantiate(miniEndSection, collidingPivot);
            EndIntersection(newSection.transform, collidingPivot, isNotSymmetric);
        }
        

        switch (collidingPivot.name)
        {
            case "Dimension_L":
                placeholderObj = collidingPivot.transform.parent.parent.Find("Pivot/Pivot_L");
                break;
            case "Dimension_R":
                placeholderObj = collidingPivot.transform.parent.parent.Find("Pivot/Pivot_R");
                break;
            case "Dimension_U":
                placeholderObj = collidingPivot.transform.parent.parent.Find("Pivot/Pivot_U");
                break;
            case "Dimension_D":
                placeholderObj = collidingPivot.transform.parent.parent.Find("Pivot/Pivot_D");
                break;
            default:

                break;
        }
        availableSpace.Remove(placeholderObj);

    }
    public void CheckIfPivotIsColliding(Transform removePivot)
    {
        availableSpace.Remove(removePivot);
    }
    
    public bool GetIsDoneGenerate()
    {
        return isDoneGenerate;
    }
}
