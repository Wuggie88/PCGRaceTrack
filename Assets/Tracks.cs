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
         
         for (int i = 0; i < track.Count -1 ; i++)
         {
            int compare = i + 1;

            if (track[i] != track[compare]) {
                curves++;
                Debug.Log("int i: " + i);
                Debug.Log("Compare: " + compare);
            }
         }
        Debug.Log("Curves: " + curves);
         return curves;
     }

    private int Fitness(List<Vector3> trackList)
    {
        // speed (how many times do we have the same direction in a row) Look at two spaces in the arrow at a time and move the span
        int trackSpeed = 0;
        int trackChallenge = 0;
        int trackDiverse = 0;
       

        //look through the list of directions
        for (int i = 0; i < trackList.Count - 2; i++)
        {
            int j = i + 1;
            int k = i + 2;

            if(trackList[i] == trackList[j]) {
                trackSpeed++;
            }

            if (Mathf.Sqrt(Mathf.Pow(Mathf.Abs(trackList[i].x - trackList[j].x), 2) + Mathf.Pow(Mathf.Abs(trackList[i].y - trackList[j].y), 2)) < 2) {
                trackChallenge++;
            }

            if (trackList[i] != trackList[j] && trackList[j] != trackList[k] && trackList[k] != trackList[k]) {
                trackDiverse++;
            }
        }

        //Sum up points from these factores
        int fitness = (trackSpeed + trackChallenge + trackDiverse)/length;
        return fitness;

    }
}
