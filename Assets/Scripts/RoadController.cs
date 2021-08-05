using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadController : MonoBehaviour
{
    public GameObject road0;
    public GameObject endRoad1;
    public GameObject straightRoad2;
    public GameObject cornerRoad2;
    public GameObject teeRoad3;
    public GameObject allRoad4;

    [SerializeField]
    private int connections = 0;
    [SerializeField]
    private bool roadUp, roadDown, roadLeft, roadRight;

    public bool canCheckEdges = true;

    private void Start()
    {
        CheckEdges(true);
    }

    private void FixedUpdate()
    {
        if (Random.Range(0f, 1f) > 0.9f && canCheckEdges)
        //if (canCheckEdges)
        {
            CheckEdges(false);
        }
    }

    private void CheckEdges(bool callUpdates = false)
    {
        connections = 0;
        roadUp = false;
        roadDown = false;
        roadLeft = false;
        roadRight = false;

        int layerMask = 1 << 7;



        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out RaycastHit hitUp, 0.5f, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitUp.distance, Color.green);
            //Debug.Log("Hit Up");
            roadUp = true;
            connections++;

            if (callUpdates)
            {
                RoadController roadController = hitUp.collider.gameObject.GetComponent<RoadController>();
                roadController.canCheckEdges = true;
            }
        }
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1, Color.red);
        //}



        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.back), out RaycastHit hitDown, 0.5f, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * hitDown.distance, Color.green);
            //Debug.Log("Hit Down");
            roadDown = true;
            connections++;

            if (callUpdates)
            {
                RoadController roadController = hitDown.collider.gameObject.GetComponent<RoadController>();
                roadController.canCheckEdges = true;
            }
        }
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.back) * 1, Color.red);
        //}



        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out RaycastHit hitLeft, 0.5f, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hitLeft.distance, Color.green);
            //Debug.Log("Hit Left");
            roadLeft = true;
            connections++;

            if (callUpdates)
            {
                RoadController roadController = hitLeft.collider.gameObject.GetComponent<RoadController>();
                roadController.canCheckEdges = true;
            }
        }
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * 1, Color.red);
        //}



        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out RaycastHit hitRight, 0.5f, layerMask))
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitRight.distance, Color.green);
            //Debug.Log("Hit Right");
            roadRight = true;
            connections++;

            if (callUpdates)
            {
                RoadController roadController = hitRight.collider.gameObject.GetComponent<RoadController>();
                roadController.canCheckEdges = true;
            }
        }
        //else
        //{
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * 1, Color.red);
        //}



        if (connections != 0)
        {
            UpdateRoad();
        }

        canCheckEdges = false;
    }

    private void UpdateRoad()
    {
        HideAll();

        if (connections == 1)
        {
            if (roadUp)
            {
                endRoad1.SetActive(true);
                endRoad1.transform.rotation = Quaternion.Euler(0, 90f, 0);
            }
            else if (roadDown)
            {
                endRoad1.SetActive(true);
                endRoad1.transform.rotation = Quaternion.Euler(0, -90f, 0);
            }
            else if (roadLeft)
            {
                endRoad1.SetActive(true);
                endRoad1.transform.rotation = Quaternion.Euler(0, 0f, 0);
            }
            else if (roadRight)
            {
                endRoad1.SetActive(true);
                endRoad1.transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
        }
        else if (connections == 2)
        {
            if (roadUp && roadDown)
            {
                straightRoad2.transform.rotation = Quaternion.Euler(0, 90f, 0);
                straightRoad2.SetActive(true);
            }
            else if (roadLeft && roadRight)
            {
                straightRoad2.transform.rotation = Quaternion.Euler(0, 0f, 0);
                straightRoad2.SetActive(true);
            }
            else if (roadUp && roadLeft)
            {
                cornerRoad2.transform.rotation = Quaternion.Euler(0, 270f, 0);
                cornerRoad2.SetActive(true);
            }
            else if (roadUp && roadRight)
            {
                cornerRoad2.transform.rotation = Quaternion.Euler(0, 0f, 0);
                cornerRoad2.SetActive(true);
            }
            else if (roadDown && roadLeft)
            {
                cornerRoad2.transform.rotation = Quaternion.Euler(0, -180f, 0);
                cornerRoad2.SetActive(true);
            }
            else if (roadDown && roadRight)
            {
                cornerRoad2.transform.rotation = Quaternion.Euler(0, 90f, 0);
                cornerRoad2.SetActive(true);
            }
        }
        else if (connections == 3)
        {
            if (roadDown && roadLeft && roadUp)
            {
                teeRoad3.transform.rotation = Quaternion.Euler(0, -90f, 0);
                teeRoad3.SetActive(true);
            }
            else if (roadLeft && roadUp && roadRight)
            {
                teeRoad3.transform.rotation = Quaternion.Euler(0, 0f, 0);
                teeRoad3.SetActive(true);
            }
            else if (roadUp && roadRight && roadDown)
            {
                teeRoad3.transform.rotation = Quaternion.Euler(0, 90f, 0);
                teeRoad3.SetActive(true);
            }
            else if (roadRight && roadDown && roadLeft)
            {
                teeRoad3.transform.rotation = Quaternion.Euler(0, 180f, 0);
                teeRoad3.SetActive(true);
            }
        }
        else if (connections == 4)
        {
            if (roadUp && roadDown && roadLeft && roadRight)
            {
                allRoad4.transform.rotation = Quaternion.Euler(0, 0f, 0);
                allRoad4.SetActive(true);
            }
        }
        else
        {
            road0.transform.rotation = Quaternion.Euler(0, 0f, 0);
            road0.SetActive(true);
        }
    }

    private void HideAll()
    {
        road0.SetActive(false);
        endRoad1.SetActive(false);
        straightRoad2.SetActive(false);
        cornerRoad2.SetActive(false);
        teeRoad3.SetActive(false);
        allRoad4.SetActive(false);
    }
}
