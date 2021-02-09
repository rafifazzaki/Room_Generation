using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Section : MonoBehaviour
{
    public List<Transform> Pivot;

    public bool isNotSymmetric = false;

    public GameObject Dimension;

    public enum SectionType
    {
        Default,
        isNotSymmetric,
        isAffectedAxis
    }

    public SectionType sectionType;

    private bool isRotated = false;

    public List<Transform> GetPivot()
    {
        return Pivot;
    }

    public string GetSectionType()
    {
        return sectionType.ToString();
    }

    public void EnableDimension()
    {
        Dimension.SetActive(true);
    }

    public bool GetIsNotSymmetric()
    {
        return isNotSymmetric;
    }
}
