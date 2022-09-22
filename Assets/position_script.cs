using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class position_script: MonoBehaviour
{
    public List<Transform> parent_List;
    public List<Transform> child_List;
    public List<Transform> sibling_List;

    public zoom_script zomScript;

    bool trigger;
    public int n;
    public int x;

    private void Start()
    {
        zomScript = GetComponent<zoom_script>();
        if (FindParents()!=null) n = FindParents().Count();
        //else n = 0;
        x = transform.GetSiblingIndex();
        for (int i = 0; i <x; i++)
        {
            if (FindParents() != null&&transform.parent.GetChild(i).tag != "display") x--;
        }

        FindSiblings();
        FindChildren();
    }

    public List<Transform> FindChildren()
    {
        child_List.Clear();
        int i = 0;
        trigger = true;
        if (transform.GetChild(i).tag == "display" && transform.GetChild(i))
        {
            for (i=0;i<transform.childCount;i++) if (transform.GetChild(i) && transform.GetChild(i).tag == "display") child_List.Add(transform.GetChild(i));

            return child_List;
        }
        else return null;
    }

    public List<Transform> FindParents()
    {
        parent_List.Clear();
        if (transform.parent)
        {
            parent_List.Add(transform.parent);
            trigger = true;
            while (trigger == true)
            {
                if (parent_List.Last().parent == null) trigger = false;
                else if (transform.parent.tag == "display") parent_List.Add(parent_List.Last().parent);
            }

            return parent_List;
        }
        else return null;
    }

    public List<Transform> FindSiblings()
    {
        sibling_List.Clear();
        if (FindParents() != null)
        {
            for (int i = 0; i < transform.parent.childCount; i++) if (transform.parent.GetChild(i).tag == "display") sibling_List.Add(transform.parent.GetChild(i));
            for (int j = 0; j < sibling_List.Count(); j++) if (sibling_List[j] == transform) sibling_List.Remove(transform);
            return sibling_List;
        }
        else return null;
    }
}
