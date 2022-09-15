using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoom_script : MonoBehaviour
{
    public Animator anim;
    public GameObject currentObj;

    void Start()
    {
        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (hit.collider && Input.GetMouseButtonDown(0)&&hit.collider.gameObject==this.gameObject)
        {
            userToggle();
        }
    }

    public void userToggle()
    {
        anim.SetTrigger("Active");
    }
}
