using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyReference : MonoBehaviour
{
    //不同类型格子的颜色材质
    public Material startMat;
    public Material endMat;
    public Material obstacleMat;
    //格子坐标
    public int x;
    public int y;

    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "Start")
        {
            GetComponent<MeshRenderer>().material = startMat;
            AStar._instance.myGrids[x, y].gridType = MyGridType.Start;
            AStar._instance.openList.Add(AStar._instance.myGrids[x, y]);
            AStar._instance.startX = x;
            AStar._instance.startY = y;
        }
        else if(other.name == "End")
        {
            GetComponent<MeshRenderer>().material = endMat;
            AStar._instance.myGrids[x, y].gridType = MyGridType.End;
            AStar._instance.targetX = x;
            AStar._instance.targetY = y;
        }
        else if(other.name == "Obstacle")
        {
            GetComponent<MeshRenderer>().material = obstacleMat;
            AStar._instance.myGrids[x, y].gridType = MyGridType.Obstacle;
        }
    }
}
