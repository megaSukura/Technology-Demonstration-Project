using UnityEngine;

static class ChildFinder
{
    public static void FindChild(GameObject parent, string childName, out GameObject child)
    {
        child = null;
        if(parent.transform.childCount == 0) return;
        foreach (Transform childTransform in parent.transform)
        {
            if (childTransform.name == childName)
            {
                child = childTransform.gameObject;
                return;
            }
            FindChild(childTransform.gameObject, childName, out child);
        }

    }
}