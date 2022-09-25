using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class zoom_script : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer close;
    public bool open;
    public int orderShift;

    public AnimationClip anim_position_open;
    public AnimationClip anim_position_close;

    public AnimationClip anim_scale_open;
    public AnimationClip anim_scale_close;

    public List<Vector3> positionsOpen;
    public List<Vector3> positionsClose;

    public List<Vector3> scaleOpen;
    public List<Vector3> scaleClose;

    public AnimationCurve animcurvx;
    public AnimationCurve animcurvy;
    public AnimationCurve animcurvz;

    public AnimationCurve animcurvxClose;
    public AnimationCurve animcurvyClose;
    public AnimationCurve animcurvzClose;

    public AnimationCurve scale_xOpen;
    public AnimationCurve scale_yOpen;
    public AnimationCurve scale_zOpen;

    public AnimationCurve scale_xClose;
    public AnimationCurve scale_yClose;
    public AnimationCurve scale_zClose;

    private Keyframe[] ksx;
    private Keyframe[] ksy;
    private Keyframe[] ksz;

    private Vector3 startPos;
    private Vector3 startScale;

    private SpriteRenderer spriteRend;
    public BoxCollider2D boxColl2D;

    public position_script posScript;

    void Start()
    {
        open = false;
        for (int j = 0; j < transform.childCount; j++) if (transform.GetChild(j).tag == "close") close = transform.GetChild(j).GetComponent<SpriteRenderer>();
        close.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
        boxColl2D = GetComponent<BoxCollider2D>();
        posScript = GetComponent<position_script>();

        orderShift = 0;

        ksx = new Keyframe[2];
        ksy = new Keyframe[2];
        ksz = new Keyframe[2];

        startPos = transform.localPosition;
        startScale = transform.localScale;

        if (posScript.n == 1) boxColl2D.enabled = true;
        else boxColl2D.enabled = false;

        //InvokeRepeating("CheckFamily", 0f, 1.0f);

    }

    void Update()
    {

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
        if (open == false && hit.collider && Input.GetMouseButtonDown(0) && hit.collider.gameObject == gameObject)
        {
            orderShift++;
            userToggle();
            close.enabled = true;
            open = true;

            boxColl2D.enabled = false;
        }
        if (open == true && hit.collider && Input.GetMouseButtonDown(0) && hit.collider.gameObject.GetComponent<SpriteRenderer>() == close)
        {
            orderShift--;
            userToggle();
            close.enabled = false;
            open = false;
            boxColl2D.enabled = true;
            //transform.parent.GetComponent<zoom_script>().spriteRend.enabled = true;
            //transform.parent.GetComponent<zoom_script>().boxColl2D.enabled = true;
        }
        //if (transform.parent.parent) Debug.Log(posScript.parent_List[^2].transform.name);
        if (transform.parent.parent) spriteRend.sortingOrder = posScript.FindParents().Count() + posScript.parent_List[^2].GetComponent<zoom_script>().orderShift;
        else spriteRend.sortingOrder = posScript.FindParents().Count() + orderShift;
        //else if (hit.collider && Input.GetMouseButtonDown(0)) Debug.Log(hit.collider.gameObject.ToString());
        CheckFamily();
    }


    public void userToggle()
    {


        if (open == false)
        {
            //spriteRend.sortingOrder = 1 + posScript.n;
            SetScaleOpen();
            SetPositionOpen();

        }
        if (open == true)
        {
            SetScaleOpen();
            SetScaleClose();
            SetPositionOpen();
            SetPositionClose();

            //spriteRend.sortingOrder = 0 + posScript.n;
        }
    }

    public void SetPositionOpen()
    {
        SetPositionClose();

        positionsOpen.Clear();
        positionsOpen.Add(startPos);
        if (posScript.n == 1) positionsOpen.Add(new Vector3(2.0165f, -0.4062f, 0.13107f));
        else positionsOpen.Add(new Vector3(0f, 0f, 0f));

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

        anim_position_open.SetCurve("", typeof(Transform), "m_LocalPosition.x", animcurvx);
        anim_position_open.SetCurve("", typeof(Transform), "m_LocalPosition.y", animcurvy);
        anim_position_open.SetCurve("", typeof(Transform), "m_LocalPosition.z", animcurvz);

        anim.Rebind();
        anim.Update(0f);
        anim.Play("zoom_Out", 0, 1000000000f);
        anim.SetTrigger("Open");
    }
    public void SetPositionClose()
    {
        positionsClose.Clear();
        if (posScript.n == 1) positionsClose.Add(new Vector3(2.0165f, -0.4062f, 0.13107f));
        else positionsClose.Add(new Vector3(0f, 0f, 0f));
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

        anim_position_close.SetCurve("", typeof(Transform), "m_LocalPosition.x", animcurvxClose);
        anim_position_close.SetCurve("", typeof(Transform), "m_LocalPosition.y", animcurvyClose);
        anim_position_close.SetCurve("", typeof(Transform), "m_LocalPosition.z", animcurvzClose);

        anim.Rebind();
        anim.Update(0f);
        anim.Play("zoom_In", 0, 1000000000f);
        anim.SetTrigger("Close");
    }
    public void SetScaleOpen()
    {
        SetScaleClose();

        scaleOpen.Clear();
        scaleOpen.Add(startScale);
        if (posScript.n == 1) scaleOpen.Add(new Vector3(17.785f, 9.9971f, 9.2192f));
        else scaleOpen.Add(new Vector3(1, 1, 1f));

        for (int i = 0; i < scaleOpen.Count(); i++)
        {
            ksx[i] = new Keyframe(i, scaleOpen[i].x);
            ksy[i] = new Keyframe(i, scaleOpen[i].y);
            ksz[i] = new Keyframe(i, scaleOpen[i].z);
        }

        for (int i = 0; i < 2; i++)
        {
            scale_xOpen.AddKey(ksx[i]);
            scale_yOpen.AddKey(ksy[i]);
            scale_zOpen.AddKey(ksz[i]);
        }

        anim_scale_open.SetCurve("", typeof(Transform), "m_LocalScale.x", scale_xOpen);
        anim_scale_open.SetCurve("", typeof(Transform), "m_LocalScale.y", scale_yOpen);
        anim_scale_open.SetCurve("", typeof(Transform), "m_LocalScale.z", scale_zOpen);

        anim.Rebind();
        anim.Update(0f);
        //anim.Play("zoom_Out", 0, 1000000000f);
        //anim.SetTrigger("Open");
    }
    public void SetScaleClose()
    {
        scaleClose.Clear();
        if (posScript.n == 1) scaleClose.Add(new Vector3(17.785f, 9.9971f, 9.2192f));
        else scaleClose.Add(new Vector3(1, 1, 1f));
        scaleClose.Add(startScale);

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

        anim_scale_close.SetCurve("", typeof(Transform), "m_LocalScale.x", scale_xClose);
        anim_scale_close.SetCurve("", typeof(Transform), "m_LocalScale.y", scale_yClose);
        anim_scale_close.SetCurve("", typeof(Transform), "m_LocalScale.z", scale_zClose);

        anim.Rebind();
        anim.Update(0f);
        //anim.Play("zoom_In", 0, 1000000000f);
        //anim.SetTrigger("Close");
    }

    public void CheckFamily()
    {
        if (open) boxColl2D.enabled = false;
        else if (transform.parent.GetComponent<zoom_script>() && transform.parent.GetComponent<zoom_script>().open == false) boxColl2D.enabled = false;
        else if (transform.parent.GetComponent<zoom_script>() && transform.parent.GetComponent<zoom_script>().open) boxColl2D.enabled = true;

        if (transform.parent.GetComponent<zoom_script>() && transform.parent.GetComponent<zoom_script>().spriteRend.enabled == false) spriteRend.enabled = false;
        else if (transform.parent.GetComponent<zoom_script>() && transform.parent.GetComponent<zoom_script>().spriteRend.enabled == true) spriteRend.enabled = true;
        //Debug.Log(posScript.sibling_List.Count());
        for (int i = 0; i < posScript.sibling_List.Count(); i++) //sibling
        {
            if (posScript.sibling_List[i].GetComponent<zoom_script>().open == true)
            {
                //Debug.Log(gameObject+"colin");
                spriteRend.enabled = false;
                if (boxColl2D.enabled == true) boxColl2D.enabled = false;
                for (int j = 0; j < posScript.child_List.Count(); j++)
                {
                    posScript.child_List[j].GetComponent<zoom_script>().spriteRend.enabled = false;
                    posScript.child_List[j].GetComponent<zoom_script>().boxColl2D.enabled = false;
                }
            }
            else if (open != true && posScript.sibling_List[i].GetComponent<zoom_script>().open == false)
            {
                //Debug.Log(gameObject + "jamie");
                //spriteRend.enabled = true;
                boxColl2D.enabled = true;
                for (int j = 0; j < posScript.child_List.Count(); j++)
                {
                    //if (posScript.child_List[j].GetComponent<zoom_script>().open == false) posScript.child_List[j].GetComponent<zoom_script>().spriteRend.enabled = true;
                    //if (posScript.child_List[j].GetComponent<zoom_script>().open == false) posScript.child_List[j].GetComponent<zoom_script>().boxColl2D.enabled = true;
                    //if (posScript.child_List[j].GetComponent<zoom_script>().open == false) posScript.child_List[j].GetComponent<zoom_script>().spriteRend.sortingOrder = 1 + posScript.child_List[j].GetComponent<position_script>().n;
                }
            }
        }
        for (int i = 0; i < posScript.child_List.Count(); i++) //child
        {
            if (posScript.child_List[i].GetComponent<zoom_script>().open == true)
            {
                //spriteRend.enabled = false;
                //boxColl2D.enabled = false;
                close.gameObject.GetComponent<SpriteRenderer>().enabled = false;
                close.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            }
            else if (open)
            {
                Debug.Log(gameObject + "colin");
                close.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                close.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
    }
}
