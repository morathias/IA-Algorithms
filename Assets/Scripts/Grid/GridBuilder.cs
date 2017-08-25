using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour 
{
    public GameObject wallTile, waterTile, mudTile;

    public Grid grid;

    GameObject[,] _obstacleTiles;

    void Start() 
    {
        Debug.Log(grid.getWidth());
        _obstacleTiles = new GameObject[grid.getWidth(), grid.getHeight()];
    }

	void Update () 
    {
        editWalls();
        editTerrain();
	}

    Vector3 mouseToWorld() 
    {
         RaycastHit hit;
         if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
         {
             Debug.Log("hit");
             return hit.point;
         }

         return new Vector3(-1,-1,-1);
    }

    public void updateObstaclesGrid() {
        _obstacleTiles = new GameObject[grid.getWidth(), grid.getHeight()];
    }

    void editWalls() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
             Vector3 pos = new Vector3(mouseToWorld().x, 0, mouseToWorld().z);
            
             pos.x = (int)pos.x;
             pos.z = (int)pos.z;
             Debug.Log(pos);
             if (pos.x <= grid.getWidth() && pos.z <= grid.getHeight() && pos.x >= 0 && pos.z >= 0)
             {
                 if (!grid.isWall((int)pos.x, (int)pos.z))
                 {
                     Debug.Log("creating wall");
                     _obstacleTiles[(int)pos.x, (int)pos.z] = Instantiate(wallTile, pos, Quaternion.identity, gameObject.transform);
                     grid.makeWall((int)pos.x, (int)pos.z, true);
                 }

                 else
                 {
                     Debug.Log("destroying wall");
                     Destroy(_obstacleTiles[(int)pos.x, (int)pos.z]);
                     grid.makeWall((int)pos.x, (int)pos.z, false);
                 }
             }
        }
    }

    void editTerrain() {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pos = new Vector3(mouseToWorld().x, 0, mouseToWorld().z);

            pos.x = (int)pos.x;
            pos.z = (int)pos.z;
            Debug.Log(pos);
            if (pos.x <= grid.getWidth() && pos.z <= grid.getHeight() && pos.x >= 0 && pos.z >= 0)
            {
                if (!grid.isWall((int)pos.x, (int)pos.z))
                {
                    Debug.Log("creating waterTile");
                    switch (grid.getNodeScore((int)pos.x, (int)pos.z))
                    {
                        case 1:
                            _obstacleTiles[(int)pos.x, (int)pos.z] = Instantiate(waterTile, pos, Quaternion.identity, gameObject.transform);
                            grid.setNodeScore((int)pos.x, (int)pos.z, 3);
                            break;

                        case 3:
                            Destroy(_obstacleTiles[(int)pos.x, (int)pos.z]);
                            _obstacleTiles[(int)pos.x, (int)pos.z] = Instantiate(mudTile, pos, Quaternion.identity, gameObject.transform);
                            grid.setNodeScore((int)pos.x, (int)pos.z, 6);
                            break;

                        default:
                            Destroy(_obstacleTiles[(int)pos.x, (int)pos.z]);
                            grid.setNodeScore((int)pos.x, (int)pos.z, 1);
                            break;
                    }
                }

                else
                {
                    Debug.Log("destroying wall");
                    Destroy(_obstacleTiles[(int)pos.x, (int)pos.z]);
                    grid.makeWall((int)pos.x, (int)pos.z, false);
                }
            }
        }
    }
}
