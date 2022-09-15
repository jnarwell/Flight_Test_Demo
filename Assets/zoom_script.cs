using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class zoom_script : MonoBehaviour
{
    public Animator anim;
    public Transform close;
    public bool open;
    public AnimationClip animclip;
    public List<Vector3> positions;

    public AnimationCurve animcurvx;
    public AnimationCurve animcurvy;
    public AnimationCurve animcurvz;

    public Keyframe[] ksx;
    public Keyframe[] ksy;
    public Keyframe[] ksz;

    void Start()
    {
        open = false;
        close = transform.GetChild(1);
        close.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        anim = GetComponent<Animator>();

        ksx = new Keyframe[2];
        ksy = new Keyframe[2];
        ksz = new Keyframe[2];

    }

    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (open==false&&hit.collider && Input.GetMouseButtonDown(0)&&hit.collider.gameObject==this.gameObject)
        {

            userToggle();
            close.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            open = true;
        }
        if (open == true && hit.collider && Input.GetMouseButtonDown(0) && hit.collider.gameObject == close.gameObject)
        {
            userToggle();
            close.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            open = false;
        }

    }   

    public void userToggle()
    {
        positions.Clear();
        positions.Add(transform.localPosition);
        positions.Add(new Vector3(2.0165f, -0.4062f, 0.13107f));

        for (int i = 0; i < positions.Count(); i++)
        {
            ksx[i] = new Keyframe(i, positions[i].x);
            ksy[i] = new Keyframe(i, positions[i].y);
            ksz[i] = new Keyframe(i, positions[i].z);
        }

        for (int i = 0; i < 2; i++)
        {
            animcurvx.AddKey(ksx[i]);
            animcurvy.AddKey(ksy[i]);
            animcurvz.AddKey(ksz[i]);
        }

        animclip.SetCurve("", typeof(Transform), "m_LocalPosition.x", animcurvx);
        animclip.SetCurve("", typeof(Transform), "m_LocalPosition.y", animcurvy);
        animclip.SetCurve("", typeof(Transform), "m_LocalPosition.z", animcurvz);

        if (open == false) anim.SetTrigger("Open");
        if (open == true) anim.SetTrigger("Close");
    }
}
