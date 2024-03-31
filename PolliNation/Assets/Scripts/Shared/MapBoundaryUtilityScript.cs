using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapBoundaryUtilityScript 
{     
    /// <summary>
    ///  Finds the minimum distance between the origin of a map and the boundaries 
    ///  tagged with <c>boundaryTag</c>
    /// </summary>
    /// <param name="boundaryTag">The tag on the map boundary objects</param>
    /// <return>Minimum distance from origin to a boundary.</return>   
   public static float FindMinBoundaryDistance(string boundaryTag) {

        //get all of the boundary walls with tag
        GameObject[] boundaries = GameObject.FindGameObjectsWithTag(boundaryTag);
        float minDistanceToCenter = float.MaxValue;

        foreach (GameObject boundary in boundaries) 
        {
                // get position of wall
                UnityEngine.Vector3 wallPos = boundary.transform.position;
                // get distance of boundary from center
                float boundaryDistance = UnityEngine.Vector3.Distance(new Vector3(0,0,0), boundary.transform.position);
                if (boundaryDistance < minDistanceToCenter) 
                {
                    minDistanceToCenter = boundaryDistance;
                }
        }
        return minDistanceToCenter;
    }
}
