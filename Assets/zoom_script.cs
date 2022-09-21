using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class zoom_script : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer close;
    public bool open;
    public bool check;

    public AnimationClip animopen;
    public AnimationClip animclose;

    public List<Vector3> positionsOpen;
    public List<Vector3> positionsClose;

    public AnimationCurve animcurvx;
    public AnimationCurve animcurvy;
    public AnimationCurve animcurvz;

    public AnimationCurve animcurvxClose;
    public AnimationCurve animcurvyClose;
    public AnimationCurve animcurvzClose;

    private Keyframe[] ksx;
    private Keyframe[] ksy;
    private Keyframe[] ksz;

    private Vector3 startPos;

    private SpriteRenderer spriteRend;
    public BoxCollider2D boxColl2D;

    public position_script posScript;

    void Start()
    {
        open = false;
        check = false;
        for (int j = 0; j < transform.childCount; j++) if (transform.GetChild(j).tag == "close") close = transform.GetChild(j).GetComponent<SpriteRenderer>();
        close.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        boxColl2D = GetComponent<BoxCollider2D>();
        posScript = GetComponent<position_script>();

        ksx = new Keyframe[2];
        ksy = new Keyframe[2];
        ksz = new Keyframe[2];

        startPos = transform.localPosition;

        spriteRend.sortingOrder = 0 + posScript.n;

        //InvokeRepeating("CheckFamily", 0f, 1.0f);

    }

    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (open==false&&hit.collider && Input.GetMouseButtonDown(0)&&hit.collider.gameObject==gameObject)
        {
            userToggle();
            close.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            open = true;
            boxColl2D.enabled = false;
        }
        if (open == true && hit.collider && Input.GetMouseButtonDown(0) && hit.collider.gameObject.tag == "close")
        {
            userToggle();
            close.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            open = false;
            boxColl2D.enabled = true;
        }
        //else if (hit.collider && Input.GetMouseButtonDown(0)) Debug.Log(hit.collider.gameObject.ToString());
        CheckFamily();
    }


    public void userToggle()
    {
        

        if (open == false)
        {
            spriteRend.sortingOrder = 1 + posScript.n;
            SetPositionAnimationOpen();
        }
        if (open == true)
        {
            SetPositionAnimationClose();
            spriteRend.sortingOrder = 0 + posScript.n;
        }
    }

    public void SetPositionAnimationOpen()
    {
        SetPositionAnimationClose();

        positionsOpen.Clear();
        positionsOpen.Add(startPos);
        positionsOpen.Add(new Vector3(2.0165f, -0.4062f, 0.13107f));

        for (int i = 0; i < positionsOpen.Count(); i++)
        {
            ksx[i] = new Keyframe(i, positionsOpen[i].x);
            ksy[i] = new Keyframe(i, positionsOpen[i].y);
            ksz[i] = new Keyframe(i, positionsOpen[i].z);
        }

        for (int i = 0; i < 2; i++)
        {
            animcurvx.AddKey(ksx[i]);
            animcurvy.AddKey(ksy[i]);
            animcurvz.AddKey(ksz[i]);
        }

        animopen.SetCurve("", typeof(Transform), "m_LocalPosition.x", animcurvx);
        animopen.SetCurve("", typeof(Transform), "m_LocalPosition.y", animcurvy);
        animopen.SetCurve("", typeof(Transform), "m_LocalPosition.z", animcurvz);

        anim.Rebind();
        anim.Update(0f);
        anim.Play("zoom_Out", 0, 1000000000f);
        anim.SetTrigger("Open");
    }
    public void SetPositionAnimationClose()
    {
        positionsClose.Clear();
        positionsClose.Add(new Vector3(2.0165f, -0.4062f, 0.13107f));
        positionsClose.Add(startPos);

        for (int i = 0; i < positionsClose.Count(); i++)
        {
            ksx[i] = new Keyframe(i, positionsClose[i].x);
            ksy[i] = new Keyframe(i, positionsClose[i].y);
            ksz[i] = new Keyframe(i, positionsClose[i].z);
        }

        for (int i = 0; i < 2; i++)
        {
            animcurvxClose.AddKey(ksx[i]);
            animcurvyClose.AddKey(ksy[i]);
            animcurvzClose.AddKey(ksz[i]);
        }

        animclose.SetCurve("", typeof(Transform), "m_LocalPosition.x", animcurvxClose);
        animclose.SetCurve("", typeof(Transform), "m_LocalPosition.y", animcurvyClose);
        animclose.SetCurve("", typeof(Transform), "m_LocalPosition.z", animcurvzClose);

        anim.Rebind();
        anim.Update(0f);
        anim.Play("zoom_In", 0, 1000000000f);
        anim.SetTrigger("Close");
    }

    public void SetPositionScaleClose()
    {
        positionsClose.Clear();
        positionsClose.Add(new Vector3(2.0165f, -0.4062f, 0.13107f));
        positionsClose.Add(startScale);

        for (int i = 0; i < scaleClose.Count(); i++)
        {
            ksx[i] = new Keyframe(i, scaleClose[i].x);
            ksy[i] = new Keyframe(i, scaleClose[i].y);
            ksz[i] = new Keyframe(i, scaleClose[i].z);
        }

        for (int i = 0; i < 2; i++)
        {
            scale_xClose.AddKey(ksx[i]);
            scale_yClose.AddKey(ksy[i]);
            scale_zClose.AddKey(ksz[i]);
        }

        animclose.SetCurve("", typeof(Transform), "m_LocalScale.x", scale_xClose);
        animclose.SetCurve("", typeof(Transform), "m_LocalScale.y", scale_yClose);
        animclose.SetCurve("", typeof(Transform), "m_LocalScale.z", scale_zClose);

        anim.Rebind();
        anim.Update(0f);
        anim.Play("zoom_In", 0, 1000000000f);
        anim.SetTrigger("Close");
    }



    public void CheckFamily()
    {
        //Debug.Log(posScript.sibling_List.Count());
        for (int i = 0; i < posScript.sibling_List.Count(); i++) if (posScript.sibling_List[i].GetComponent<zoom_script>().open==true) 
            {
                //Debug.Log("colin");
                if (spriteRend.enabled == true) spriteRend.enabled = false;
                if (boxColl2D.enabled == true) boxColl2D.enabled = false;
                for (int j = 0; j<posScript.child_List.Count(); j++)
                {
                    posScript.child_List[j].GetComponent<zoom_script>().spriteRend.enabled = false;
                    posScript.child_List[j].GetComponent<zoom_script>().boxColl2D.enabled = false;
                }
            }
        else if (open!=true)
            {
                if (spriteRend.enabled == false) spriteRend.enabled = true;
                if (boxColl2D.enabled == false) boxColl2D.enabled = true;
            }
        if (open)
        {
            for (int j = 0; j < posScript.child_List.Count(); j++)
            {
                posScript.child_List[j].GetComponent<zoom_script>().spriteRend.enabled = true;
                posScript.child_List[j].GetComponent<zoom_script>().boxColl2D.enabled = true;
                posScript.child_List[j].GetComponent<zoom_script>().spriteRend.sortingOrder = 1 + posScript.child_List[j].GetComponent<position_script>().n;
            }
        }
    }
}
