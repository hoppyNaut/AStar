using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格子类型
/// </summary>
public enum MyGridType
{
    //正常
    Normal,
    //障碍物
    Obstacle,
    //起点
    Start,
    //终点
    End,
}

public class MyGrid : IComparable
{
    //格子坐标
    public int x;
    public int y;
    //格子代价
    public int f;
    public int g;
    public int h;
    //格子类型
    public MyGridType gridType;
    //父格子节点
    public MyGrid parent;

    public MyGrid(int x,int y)
    {
        this.x = x;
        this.y = y;
    }

    public int CompareTo(object obj)
    {
        MyGrid myGrid = (MyGrid)obj;
        if(this.f > myGrid.f)
        {
            return 1;
        }
        if(this.f < myGrid.f)
        {
            return -1;
        }
        return 0;
    }
}
