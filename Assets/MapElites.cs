using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapElites : MonoBehaviour
{
    /*
     * Mutate some tracks from the population
     * Map the tracks with the best fitness
     */

    public Tracks[] map = new Tracks[9];
    public Tracks[] orgMap = new Tracks[9];
    public Tracks track;
    Vector3 startingPoint = new Vector3(0,0,0);
    public List<Vector3> Direction = new List<Vector3>();
    public List<Tracks> population = new List<Tracks>();
    public Tracks[] originalArray;
    Color color = Color.red;
    public int mutations = 1000;
    int xMax = 10;
    int yMax = 10;
    int xMin = -10;
    int yMin = -10;
    int pointSpace = 1;
    int dirSize = 8;
    int curveMin = 50;
    int curveMax = 100;

    private void Start()
    {
                        
        // Create an array/list of directions
        Direction = GetDirections(dirSize);

        //To check the directions in the list/array
        /* for( int i = 0; i <= Direction.Count -1; i++)
          {
              Debug.Log(Direction[i]);
          }*/

        // Make 100 random tracks and place in map
       for (int i = 0; i <= 100; i++)
        {
            //Create random racetracks
            track = new Tracks(TrackPoints(startingPoint));

            //Place random tracks in population list
            population.Add(track);
        }

        population.Sort((x, y) => x.fitness.CompareTo(y.fitness));

        originalArray = population.ToArray();

        population.RemoveRange(0, 30);

        
      /*    //see how many tracks is mapped
          Debug.Log(map.Count);
        */
        MutateTracks();
    }

    
    //create vectors of directions
    private List<Vector3> GetDirections(int numberOfDirections)
    {
       //sharpness of directions is based on the number of directions
        List<Vector3> Directions = new List<Vector3>();
        float angleStep = ((ushort)(360.0 / numberOfDirections));

        //create vectors and add to list
        for (int i = 0; i < numberOfDirections; i++)
        {
            float theta = i * angleStep;
            float newX = Mathf.Cos(theta * Mathf.Deg2Rad);
            float newY = Mathf.Sin(theta * Mathf.Deg2Rad);
            Directions.Add(new Vector3(newX, newY, 0));
        }
        return Directions;
    }

    private List<Vector3> TrackPoints(Vector3 startPoint)
    {
        //create a collection of the directions
        List<Vector3> trackVectors = new List<Vector3>();
        List<int> trackDir = new List<int>();
        

        //Startpoint
        //tra.Add(startPoint);

        //save point as origin for drawing line
        Vector3 point = startPoint;
        Vector3 prevDir = new Vector3(0, 0, 0);

        //select a random direction
        Vector3 direction = Direction[Random.Range(0, Direction.Count)];
        
        
        // set endpoint for drawing
        Vector3 newPoint = startPoint + direction * pointSpace;
        

        //Check if the new point is inside the determined space
        while (newPoint.x <= xMax && newPoint.x >= xMin && newPoint.y <= yMax && newPoint.y >= yMin)
        {
            //check if the new point is not the starting point
            if (newPoint.x != startingPoint.x && newPoint.y != startingPoint.y)
            {
                //if not add the point to the draw list and the array of directions
                trackVectors.Add(direction);
                
                trackDir.Add(Direction.IndexOf(direction));
                

                //draw line and set endpoint as new beginpoint for next line
                //Debug.DrawLine(point, newPoint, color, 300f);
                prevDir = direction;

                //Select new random direction
                direction = Direction[Random.Range(0, Direction.Count)];

                //check that the new direction is not going back the previous direction
                if (direction == prevDir*-1)
                {
                    //select a new direction and set new points for track 
                    direction = Direction[Random.Range(0, Direction.Count)];
                    point = newPoint;
                    newPoint = newPoint + direction * pointSpace;
                    
                }
                else
                {
                    //set point for track
                    point = newPoint;
                    newPoint = newPoint + direction * pointSpace;
                   
                }
                
            }
             
            else
            {
               break;
            }
        }
        //end the track and close it
        newPoint = startPoint;
        direction = newPoint - point;
        trackVectors.Add(direction);
        trackDir.Add(Mathf.Abs(Direction.IndexOf(direction)));
        //Debug.DrawLine(point, newPoint, color, 300f);

        //print size and list of directions
       /* Debug.Log(trackDir.Count);
        
        for (int i = 0; i < trackDir.Count; i++)
        {
            Debug.Log(trackDir[i]);
        }*/

        //return list of directions
        return trackVectors;   
    }
 
    private void MutateTracks()
    {
        for (int i = 0; i < mutations; i++) {
            Tracks[] popArr = population.ToArray();

            for (int j = 0; j < 30; j++) {
                int randomInt = Random.Range(0, popArr.Length);

                List<Vector3> newDirections = popArr[randomInt].trackDirections;

                newDirections.RemoveAt(newDirections.Count - 1);

                randomInt = Random.Range(0, newDirections.Count);

                newDirections[randomInt] = Direction[Random.Range(0, Direction.Count)];

                Vector3 point = new Vector3(0,0,0);
                Vector3 newPoint;

                for (int d = 0; d < newDirections.Count; d++)
                {

                    newPoint = point + newDirections[d] * pointSpace;
                    point = newPoint;
                }
                newPoint = new Vector3(0, 0, 0);
                Vector3 newD = newPoint - point;

                newDirections.Add(newD);

                Tracks mutatedTrack = new Tracks(newDirections);

                population.Add(mutatedTrack);
            }
            PopSort();
            if(i != mutations - 1) {
                KillPop();
            }
        }
        DrawMaps();
    }

    public void PopSort() {
        population.Sort((x, y) => x.fitness.CompareTo(y.fitness));
    }

    public void KillPop() {
        population.RemoveRange(0, 30);
    }

    public void DrawMaps() {
       // Tracks[] maps = population.ToArray();
        foreach(Tracks t in population) {
            MapTrack(t);
        }

         //Draw tracks in scene
        DrawTrack(map[0], new Vector3(0, 0, 0));
        DrawTrack(map[1], new Vector3(0, 30, 0));
        DrawTrack(map[2], new Vector3(0, 60, 0));
        DrawTrack(map[3], new Vector3(30, 0, 0));
        DrawTrack(map[4], new Vector3(30, 30, 0));
        DrawTrack(map[5], new Vector3(30, 60, 0));
        DrawTrack(map[6], new Vector3(60, 0, 0));
        DrawTrack(map[7], new Vector3(60, 30, 0));
        DrawTrack(map[8], new Vector3(60, 60, 0));


        foreach (Tracks n in originalArray) {
            OrgMapTrack(n);
        }

        //Draw tracks in scene
        DrawTrack(orgMap[0], new Vector3(90, 0, 0));
        DrawTrack(orgMap[1], new Vector3(90, 30, 0));
        DrawTrack(orgMap[2], new Vector3(90, 60, 0));
        DrawTrack(orgMap[3], new Vector3(120, 0, 0));
        DrawTrack(orgMap[4], new Vector3(120, 30, 0));
        DrawTrack(orgMap[5], new Vector3(120, 60, 0));
        DrawTrack(orgMap[6], new Vector3(120, 0, 0));
        DrawTrack(orgMap[7], new Vector3(150, 30, 0));
        DrawTrack(orgMap[8], new Vector3(150, 60, 0));
    }

    private void OrgMapTrack(Tracks track) {
        float length = track.length;
        int curves = track.curves;
        int fitness = track.fitness;

        //place track in map space according to length and curves
        if (length > 0 && length < 50) {
            if (curves > 0 && curves < curveMin) {
                if (orgMap[0].fitness < track.fitness) {
                    orgMap[0] = track;
                } else if (orgMap[0] == null) {
                    orgMap[0] = track;
                }

            } else if (curves >= curveMin && curves < curveMax) {
                if (orgMap[3].fitness < track.fitness) {
                    orgMap[3] = track;
                } else if (orgMap[3] == null) {
                    orgMap[3] = track;
                }
            } else if (curves >= curveMax) {
                if (orgMap[6].fitness < track.fitness) {
                    orgMap[6] = track;
                } else if (orgMap[6] == null) {
                    orgMap[6] = track;
                }

            }

        } else if (length >= 50 && length < 100) {
            if (curves > 0 && curves < curveMin) {
                if (orgMap[1].fitness < track.fitness) {
                    orgMap[1] = track;
                } else if (orgMap[1] == null) {
                    orgMap[1] = track;
                }
            } else if (curves >= curveMin && curves < curveMax) {
                if (orgMap[4].fitness < track.fitness) {
                    orgMap[4] = track;
                } else if (orgMap[4] == null) {
                    orgMap[4] = track;
                }
            } else if (curves >= curveMax) {
                if (orgMap[7].fitness < track.fitness) {
                    orgMap[7] = track;
                } else if (orgMap[7] == null) {
                    orgMap[7] = track;
                }
            }
        } else if (length >= 100) {
            if (curves > 0 && curves < curveMin) {
                if (orgMap[2].fitness < track.fitness) {
                    orgMap[2] = track;
                } else if (orgMap[2] == null) {
                    orgMap[2] = track;
                }
            } else if (curves >= curveMin && curves < curveMax) {
                if (orgMap[5].fitness < track.fitness) {
                    orgMap[5] = track;
                } else if (orgMap[5] == null) {
                    orgMap[5] = track;
                }
            } else if (curves >= curveMax) {
                if (orgMap[8].fitness < track.fitness) {
                    orgMap[8] = track;
                } else if (orgMap[8] == null) {
                    orgMap[8] = track;
                }
            }

        }


        //Check if track is added to map
        // Debug.Log(map.Count);
    }


    private void MapTrack(Tracks track)
    {
        float length = track.length;
        int curves=track.curves;
        int fitness = track.fitness;

        //place track in map space according to length and curves
        if (length > 0 && length < 50)
        {
            if(curves > 0 && curves < curveMin)
            {
                if (map[0].fitness < track.fitness)
                {
                   map[0] = track;
                }
                else if (map[0] == null)
                {
                    map[0] = track;
                }
                               
            }
            else if (curves >= curveMin && curves < curveMax){
                if (map[3].fitness < track.fitness)
                {
                    map[3] = track;
                }
                else if (map[3] == null)
                {
                    map[3] = track;
                }
            }
            else if(curves >= curveMax)
            {
                if (map[6].fitness < track.fitness)
                {
                    map[6] = track;
                }
                else if (map[6] == null)
                {
                    map[6] = track;
                }

            }
                    
        }
        else if (length >= 50 && length < 100)
        {
            if (curves > 0 && curves < curveMin)
            {
                if (map[1].fitness < track.fitness)
                {
                    map[1] = track;
                }
                else if (map[1] == null)
                {
                    map[1] = track;
                }
            }
            else if(curves >= curveMin && curves < curveMax)
            {
                if (map[4].fitness < track.fitness)
                {
                    map[4] = track;
                }
                else if (map[4] == null)
                {
                    map[4] = track;
                }
            }
            else if (curves >= curveMax)
            {
                if (map[7].fitness < track.fitness)
                {
                    map[7] = track;
                }
                else if (map[7] == null)
                {
                    map[7] = track;
                }
            }
        }
        else if (length >= 100)
        {
            if (curves > 0 && curves < curveMin)
            {
                if (map[2].fitness < track.fitness)
                {
                    map[2] = track;
                }
                else if (map[2] == null)
                {
                    map[2] = track;
                }
            }
            else if (curves >= curveMin && curves < curveMax)
            {
                if (map[5].fitness < track.fitness)
                {
                    map[5] = track;
                }
                else if (map[5] == null)
                {
                    map[5] = track;
                }
            }
            else if (curves >= curveMax)
            {
                if (map[8].fitness < track.fitness)
                {
                    map[8] = track;
                }
                else if (map[8] == null)
                {
                    map[8] = track;
                }
            }
          
        }
       

        //Check if track is added to map
       // Debug.Log(map.Count);
    }

    public Tracks[] DeletePart(int position, params Tracks[] map)
    {
        Tracks[] result = new Tracks[map.Length - 1];
        int z = 0;

        for (int i = 0; i < map.Length; i++)
        {
            if (position - 1 != i)
            {
                result[z] = map[i];
                z++;
            }
        }
        return result;
    }

    public List<Vector3> Shuffle(List<Vector3> values)
    {
        
        for (int i = 2; i > 0; i--)
        {
            int k = Random.Range(0, i);

            Vector3 value = values[k];
         
            values[k] = values[i];
            values[i] = value;
        }

        return values;
    }

    private void DrawTrack(Tracks track, Vector3 startpoint)
    {
        

        Vector3 point = startpoint;
        Vector3 newPoint;

        for(int d = 0; d < track.length; d++)
        {
           
            newPoint = point + track.trackDirections[d] * pointSpace;
            Debug.DrawLine(point, newPoint, color, 300f);
            point = newPoint;
        }
        Vector3 lineStart = new Vector3(75, -10, 0);
        Vector3 lineEnd = new Vector3(75, 100, 0);
        Debug.DrawLine(lineStart, lineEnd, color, 300f);



        //Vector3 newPoint = startPoint + direction * pointSpace;
       // Debug.DrawLine(point, newPoint, color, 300f);
    }



   
}
