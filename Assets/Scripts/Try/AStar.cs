using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar : MonoBehaviour
{
    /// <summary>
    /// 单例
    /// </summary>
    public static AStar _instance;

    public GameObject reference;
    //格子数组
    public MyGrid[,] myGrids;
    //格子对应的对象数组
    public GameObject[,] myObjs;
    //开启列表
    public ArrayList openList;
    //关闭列表
    public ArrayList closeList;
    //目标点坐标
    public int targetX;
    public int targetY;
    //起始点坐标
    public int startX;
    public int startY;

    //格子行列数
    private int row;
    private int colomn;
    //结果栈
    private Stack<Vector2> parentStack;
    //基础物体
    private Transform plane;
    private Transform start;
    private Transform end;
    private Transform obstacle;

    private void Awake()
    {
        _instance = this;
        plane = GameObject.Find("Plane").transform;
        start = GameObject.Find("Start").transform;
        end = GameObject.Find("End").transform;
        obstacle = GameObject.Find("Obstacle").transform;
        openList = new ArrayList();
        closeList = new ArrayList();
        parentStack = new Stack<Vector2>();
    }

    /// <summary>
    /// 初始化操作
    /// </summary>
    private void Init()
    {
        int x = (int)(plane.localScale.x * 20);
        int y = (int)(plane.localScale.y * 20);
        row = x;
        colomn = y;
        myGrids = new MyGrid[x, y];
        myObjs = new GameObject[x, y];

        //生成起始坐标
        Vector3 startPos = new Vector3(plane.localScale.x * -5, 0, plane.localScale.z * -5);
        for(int i = 0; i < x; i++) {
            for(int j = 0; j < y; j++)
            {
                myGrids[i, j] = new MyGrid(i, j);
                GameObject item = GameObject.Instantiate(reference, new Vector3(i * 0.5f, 0, j * 0.5f) + startPos, Quaternion.identity);
                myObjs[i, j] = item;
                item.transform.GetChild(0).GetComponent<MyReference>().x = i;
                item.transform.GetChild(0).GetComponent<MyReference>().y = j;
            }
        }
    }

    /// <summary>
    /// A* 算法 
    /// </summary>
    /// <returns></returns>
    IEnumerator Count()
    {
        //延时等待前面操作完成 
        yield return new WaitForSeconds(0.1f);
        //添加起始点
        openList.Add(myGrids[startX, startY]);
        //声明当前格子变量，并赋初值
        MyGrid currentGrid = openList[0] as MyGrid;
        //循环遍历路径最小的点
        while(openList.Count > 0 && currentGrid.gridType != MyGridType.End)
        {
            currentGrid = openList[0] as MyGrid;
            //将当前格子从开启列表中移除
            openList.Remove(currentGrid);
            //加入封闭列表
            closeList.Add(currentGrid);
            //如果当前点为目标点
            if (currentGrid.gridType == MyGridType.End)
            {
                Debug.Log("Find");
                //生成结果
                GenerateResult(currentGrid);
            }
            //遍历四周格子
            for(int i = -1; i <= 1; i++)
            {
                for(int j = -1; j <= 1; j++)
                {
                    if (i != 0 || j != 0)
                    {
                        int tempX = currentGrid.x + i;
                        int tempY = currentGrid.y + j;
                        //未超出所有格子范围且不是障碍物且不是重复格子
                        if (tempX >= 0 && tempY >= 0 && tempX < row && tempY < colomn && myGrids[tempX, tempY].gridType != MyGridType.Obstacle && !closeList.Contains(myGrids[tempX, tempY]))
                        {
                            //计算g值
                            int g = currentGrid.g + (int)(Mathf.Sqrt(Mathf.Abs(i) + Mathf.Abs(j)) * 10);
                            //与原g值比较
                            if (myGrids[tempX,tempY].g == 0 || myGrids[tempX,tempY].g > g)
                            {
                                myGrids[tempX, tempY].g = g;
                                myGrids[tempX, tempY].parent = currentGrid;
                            }

                            //使用Manhattan法计算h值
                            myGrids[tempX,tempY].h = (int)((Mathf.Abs(targetX - tempX) + Mathf.Abs(targetY - tempY)) * 10);
                            //计算f值
                            myGrids[tempX, tempY].f = myGrids[tempX, tempY].g + myGrids[tempX, tempY].h;

                            if(!openList.Contains(myGrids[tempX,tempY]))
                            {
                                openList.Add(myGrids[tempX, tempY]);
                            }
                            openList.Sort();
                        }
                    }
                }
            }
            if(openList.Count  == 0)
            {
                Debug.Log("Can Not Find");
            }

        }
    }

    /// <summary>
    /// 生成结果
    /// </summary>
    /// <param name="currentGrid"></param>
    private void GenerateResult(MyGrid currentGrid)
    {
        //如果当前格子没有父格子
        if(currentGrid.parent != null)
        {
            parentStack.Push(new Vector2(currentGrid.x, currentGrid.y));
            GenerateResult(currentGrid.parent);
        }
    }

    /// <summary>
    /// 显示结果
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(.3f);
        while(parentStack.Count > 0)
        {
            Vector2 currentGridPos = parentStack.Pop();
            yield return new WaitForSeconds(.3f);
            myObjs[(int)currentGridPos.x, (int)currentGridPos.y].transform.GetChild(0).GetComponent<MeshRenderer>().material.color = Color.red;
        }
    }

    private void Start()
    {
        Init();
        StartCoroutine(Count());
        StartCoroutine(ShowResult());
    }
}
