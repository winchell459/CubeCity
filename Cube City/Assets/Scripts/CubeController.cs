using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public float AngularForce = 100;
    public float TipForce = 30;
    private Rigidbody rb;
    public Transform pivot;
    private float halfWidth = 0.5f;
    Vector3 rayPoint;
    float startAngle;

    public Transform pypx, pymx, mypx, mymx, pypz, pymz, mypz, mymz, pxpz, pxmz, mxpz, mxmz;
    public Transform Right, Left, Top, Bottom, Front, Back;
    private Transform[] pivotTransforms;
    private Transform[] sideTransforms;

    Vector3 moving;
    public Transform forwardSide, backwardSide;
    // Start is called before the first frame update
    void Start()
    {
        sideTransforms = new Transform[]{ Front, Back, Right, Left, Top, Bottom};
        pivotTransforms = new Transform[] { pypx, pymx, mypx, mymx, pypz, pymz, mypz, mymz, pxpz, pxmz, mxpz, mxmz };
        rb = GetComponent<Rigidbody>();
        //pivot = new Vector3(transform.position.x + 0.5f, transform.position.y - 0.5f, transform.position.z);
        setPivot(Vector3.forward);
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxis("Horizontal") * AngularForce * Time.deltaTime;
        float v = Input.GetAxis("Vertical") * AngularForce * Time.deltaTime;

        if (v > 0 && checkSides()) setPivot(Vector3.forward);
        else if (v < 0 && checkSides()) setPivot(-Vector3.forward);
        else if (h > 0 && checkSides()) setPivot(Vector3.right);
        else if (h < 0 && checkSides()) setPivot(-Vector3.right);

        if (moving == Vector3.forward || moving == -Vector3.forward)
            transform.RotateAround(pivot.position, Vector3.right, v);
            
        else if (moving == Vector3.right || moving == -Vector3.right)
            transform.RotateAround(pivot.position, Vector3.forward, -h);
        //Debug.Log(transform.eulerAngles.x + " " + transform.eulerAngles.y + " " + transform.eulerAngles.z);
        //Debug.Log(transform.localPosition.x);
    }

    bool checkSides()
    {
        return !(sideRaycast(backwardSide) > 0.01f && sideRaycast(forwardSide) > 0.01f);
    }
    float sideRaycast(Transform sideT)
    {
        RaycastHit hit;
        if (Physics.Raycast(sideT.position, sideT.forward, out hit, 10f))
        {
            Debug.Log(sideT.name + " " + hit.distance);
            return hit.distance;
        }

        return Mathf.Infinity;
    }

    void setPivot(Vector3 movingDirection){
        //if (moving == movingDirection) return;
        moving = movingDirection;
        Vector3 position = transform.position;
        if(movingDirection == Vector3.right){
            //pivot = new Vector3(position.x + halfWidth, position.y - halfWidth, position.z);
            setPivotTransform(new Vector3(position.x + halfWidth, position.y - halfWidth, position.z));
            //rayPoint = new Vector3(position.x - halfWidth, position.y - halfWidth, position.z);
            setSides();
            startAngle = transform.eulerAngles.x;
        }else if(movingDirection == - Vector3.right){
            //pivot = new Vector3(position.x - halfWidth, position.y - halfWidth, position.z);
            setPivotTransform(new Vector3(position.x - halfWidth, position.y - halfWidth, position.z));
            //rayPoint = new Vector3(position.x + halfWidth, position.y - halfWidth, position.z);
            setSides();
            startAngle = transform.eulerAngles.x;
        }else if (movingDirection == -Vector3.forward){
            //pivot = new Vector3(position.x , position.y - halfWidth, position.z - halfWidth);
            setPivotTransform(new Vector3(position.x, position.y - halfWidth, position.z - halfWidth));
            //rayPoint = new Vector3(position.x , position.y - halfWidth, position.z + halfWidth);
            setSides();
            startAngle = transform.eulerAngles.z;
        }else if (movingDirection == Vector3.forward){
            //pivot = new Vector3(position.x , position.y - halfWidth, position.z + halfWidth);
            setPivotTransform(new Vector3(position.x, position.y - halfWidth, position.z + halfWidth));
            //rayPoint = new Vector3(position.x, position.y - halfWidth, position.z - halfWidth);
            setSides();
            startAngle = transform.eulerAngles.z;
        }
        else
        {
            Debug.Log("pivot not set");
        }
    }
    void setPivotTransform(Vector3 pivotLoc)
    {
        foreach(Transform pivotT in pivotTransforms)
        {
            if(Vector3.Distance(pivotT.position, pivotLoc) < 0.1)
            {
                pivot = pivotT;
            }
        }
    }

    void setSides()
    {
        Vector3 pos = transform.position + moving*halfWidth;
        //Debug.Log(pos);
        foreach(Transform sideT in sideTransforms)
        {
            //Debug.Log(sideT.position);
            if (Vector3.Distance(sideT.position, pos) < 0.1)
            {
                forwardSide = sideT;
                setBackSide();
                break;
            }
        }
        Debug.Log("setSides error");

    }

    void setBackSide()
    {
        if (pivot == mypz)
        {
            if (forwardSide == Front) backwardSide = Bottom;
            else backwardSide = Front;
        }
        if(pivot == pypz)
        {
            if (forwardSide == Front) backwardSide = Top;
            else backwardSide = Front;
        }
        if (pivot == pymz)
        {
            if (forwardSide == Back) backwardSide = Top;
            else backwardSide = Back;
        }
        if (pivot == mymz)
        {
            if (forwardSide == Back) backwardSide = Bottom;
            else backwardSide = Back;
        }
        if (pivot == pypx)
        {
            if (forwardSide == Right) backwardSide = Top;
            else backwardSide = Right;
        }
        if (pivot == pymx)
        {
            if (forwardSide == Left) backwardSide = Top;
            else backwardSide = Left;
        }
        if (pivot == mypx)
        {
            if (forwardSide == Right) backwardSide = Bottom;
            else backwardSide = Right;
        }
        if (pivot == mymx)
        {
            if (forwardSide == Left) backwardSide = Bottom;
            else backwardSide = Left;
        }
        if (pivot == pxpz)
        {
            if (forwardSide == Right) backwardSide = Front;
            else backwardSide = Right;
        }
        if (pivot == mxpz)
        {
            if (forwardSide == Left) backwardSide = Front;
            else backwardSide = Left;
        }
        if (pivot == pxmz)
        {
            if (forwardSide == Right) backwardSide = Bottom;
            else backwardSide = Right;
        }
        if (pivot == mxmz)
        {
            if (forwardSide == Left) backwardSide = Bottom;
            else backwardSide = Left;
        }

    }
}
