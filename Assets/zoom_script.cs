using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class zoom_script : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer close;
    public bool open;

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

    private SpriteRenderer spriteRenderer;
    public BoxCollider2D boxCollider2D;

    public position_script posScript;

    void Start()
    {
        open = false;
        close = transform.GetChild(1).GetComponent<SpriteRenderer>();
        close.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        posScript = GetComponent<position_script>();

        ksx = new Keyframe[2];
        ksy = new Keyframe[2];
        ksz = new Keyframe[2];

        startPos = transform.localPosition;

        spriteRenderer.sortingOrder = 0 + this.GetComponent<position_script>().n;

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
        Debug.Log(GetComponent<position_script>().sibling_List);
        for (int i = 0; i < this.GetComponent<position_script>().sibling_List.Count(); i++)
        {
            Debug.Log(GetComponent<position_script>().sibling_List[i]);
            if (GetComponent<position_script>().sibling_List[i].GetComponent<zoom_script>().open == true)
            {
                boxCollider2D.enabled = false;
                Debug.Log($"Hello my name is {gameObject.name} and my position script is: {GetComponent<position_script>()}");
                spriteRenderer.enabled = false;
            }
            else
            {
                boxCollider2D.enabled = true;
                spriteRenderer.enabled = true;
            }
        }
        for (int i = 0; i<this.GetComponent<position_script>().child_List.Count(); i++)
        {
            if (this.GetComponent<position_script>().child_List[i].GetComponent<zoom_script>().open == true)
            {
                boxCollider2D.enabled = false;
                spriteRenderer.enabled = false;
                for (int j = 0; j < this.transform.childCount; j++) if (this.transform.GetChild(j).tag == "close") this.transform.GetChild(j).GetComponent<BoxCollider2D>().enabled = false;
            }
            else
            {
                boxCollider2D.enabled = true;
                spriteRenderer.enabled = true;
                for (int j = 0; j < this.transform.childCount; j++) if (this.transform.GetChild(j).tag == "close") this.transform.GetChild(j).GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        if (this.GetComponent<position_script>().n == 1) boxCollider2D.enabled = true;
        else if (this.transform.parent.transform.GetComponent<zoom_script>() && this.transform.parent.transform.GetComponent<zoom_script>().open == true)
        {
            boxCollider2D.enabled = true;
            spriteRenderer.sortingOrder = 2 + this.GetComponent<position_script>().n;
        }
        else
        {
            boxCollider2D.enabled = false;
            spriteRenderer.sortingOrder = 0 + this.GetComponent<position_script>().n;
        }

    }   

    public void userToggle()
    {
        

        if (open == false)
        {
            spriteRenderer.sortingOrder = 2+this.GetComponent<position_script>().n;

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
        if (open == true)
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
            spriteRenderer.sortingOrder = 0 + this.GetComponent<position_script>().n;
        }
    }
}
