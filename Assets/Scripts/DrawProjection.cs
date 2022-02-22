using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawProjection : MonoBehaviour
{
    PlayerThrowing playerThrow;
    LineRenderer lineRender;

    public int points = 50;

    public float timeBetweenPoints = 0.1f;

    public Camera camera;

    public LayerMask CollidableLayers;

    // Start is called before the first frame update
    void Start()
    {
        playerThrow = GetComponent<PlayerThrowing>();
        lineRender = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        lineRender.positionCount = points;
        List<Vector3> pointPos = new List<Vector3>();
        Vector3 startPos = playerThrow.throwPoint.position;
        Vector3 direction = camera.transform.forward;
        RaycastHit hit;
        if(Physics.Raycast(camera.transform.position,camera.transform.forward, out hit, 500f))
        {
            direction = (hit.point - playerThrow.throwPoint.position).normalized;
        }
        Vector3 startVel = direction * playerThrow.throwForce + transform.up * playerThrow.throwUpwardForce;
        // Prediction throwing
        for(float t = 0; t< points; t+= timeBetweenPoints)
        {
            Vector3 newPos =  startPos + t * startVel;
            newPos.y = startPos.y + startVel.y * t + Physics.gravity.y *0.5f * t * t;
            pointPos.Add(newPos);

            if (Physics.OverlapSphere(newPos, 1, CollidableLayers).Length > 0)
            {
                lineRender.positionCount = pointPos.Count;
                break;
            }
        }
        lineRender.SetPositions(pointPos.ToArray());
    }
}
