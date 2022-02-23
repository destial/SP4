using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DrawProjection
{
    public static int points = 50;

    public static float timeBetweenPoints = 0.1f;
    // Update is called once per frame
    public static void DrawLine(in LineRenderer lineRender, in PlayerThrowing playerThrow, in Camera camera, in Transform transform, in LayerMask collide)
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

            if (Physics.OverlapSphere(newPos, 1, collide).Length > 0)
            {
                lineRender.positionCount = pointPos.Count;
                break;
            }
        }
        lineRender.SetPositions(pointPos.ToArray());
    }
}
