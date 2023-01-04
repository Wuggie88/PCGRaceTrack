using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tracks 
{
    /* Correct NumberofCurves()
     * Correct Fitness(), change how to calculate fitness, conflict between factors?
     */

    Color color = Color.red;
    public List<Vector3> trackDirections;
    public int length;
    public int curves;
    public int fitness;
    

    public Tracks(List<Vector3> ListOfDirection)
    {
        this.trackDirections = ListOfDirection;
        this.length = trackDirections.Count;
        this.curves = NumberOfCurves(trackDirections);
        this.fitness = Fitness(trackDirections);
    }

    private int NumberOfCurves(List<Vector3> track)
     {
         
         for (int i = 0; i < track.Count; i++)
         {
            for (int x = 1; x < track.Count; x++)
            {
                //calculate number of curves based on neightbouring directions being distinct
                if (track[i] != track[x])
                {
                    curves++;
                }
            }
         }

         return curves;
     }

    private int Fitness(List<Vector3> trackList)
    {
        // speed (how many times do we have the same direction in a row) Look at two spaces in the arrow at a time and move the span
        int trackSpeed = 0;
        int trackChallenge = 0;
        int trackDiverse = 0;
       

        //look through the list of directions
        for (int i = 0; i < trackList.Count; i++)
        {
            for (int d = 1; d < trackList.Count; d++)
            {
                //calculate speedpoints if direction is uniform
                if (trackList[i] == trackList[d])
                {
                    trackSpeed++;
                }

                //calculate challangepoints based on steepness of curves

                //int curveSteep = Mathf.Abs(trackList[i] - trackList[x]);
                if (Mathf.Sqrt(Mathf.Pow(Mathf.Abs(trackList[i].x - trackList[d].x), 2) + Mathf.Pow(Mathf.Abs(trackList[i].y - trackList[d].y), 2)) < 2)
                {
                    trackChallenge++;
                }

                for(int r = 0; r > trackList.Count; r++)
                {
                    //calculate diversitypoints if the three following directions are diverse
                    if (trackList[i] != trackList[d] && trackList[d] != trackList[r] && trackList[r] != trackList[r])
                    {
                        trackDiverse++;
                    }
                }
            }
        }

        //Sum up points from these factores
        int fitness = trackSpeed + trackChallenge + trackDiverse;
        return fitness;

    }
}
