using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class position_script: MonoBehaviour
{
    public List<Transform> parent_List;
    public List<Transform> child_List;

    bool trigger;
    public int n;
    public int x;

    private void Start()
    {
        if (FindParents()!=null) n = FindParents().Count();
        else n = 0;
        x = transform.GetSiblingIndex();
        for (int i = 0; i <x; i++)
        {
            if (FindParents() != null&&transform.parent.GetChild(i).tag != "display") x--;
        }
    }

    public List<Transform> FindChildren()
    {
        child_List.Clear();
        int i = 0;
        trigger = true;
        if (this.transform.GetChild(i).tag == "display" && this.transform.GetChild(i) != null)
        {
            while (trigger == true)
            {
                if (this.transform.childCount==child_List.Count()) trigger = false;
                else if (this.transform.GetChild(i).tag == "display") child_List.Add(this.transform.GetChild(i));
                i++;

            }

            return child_List;
        }
        else return null;
    }

    public List<Transform> FindParents()
    {
        parent_List.Clear();
        if (this.transform.parent)
        {
            parent_List.Add(this.transform.parent);
            trigger = true;
            while (trigger == true)
            {
                if (parent_List.Last().parent == null) trigger = false;
                else if (this.transform.parent.tag == "display") parent_List.Add(parent_List.Last().parent);
            }

            return parent_List;
        }
        else return null;
    }
}
