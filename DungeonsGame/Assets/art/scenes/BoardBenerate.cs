using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardBenerate : MonoBehaviour
{
    enum tiles
    {
        Dirt,
        Floor,
        Wall,
        Door,
    }
    enum Directions
    {
        North, East, South, West,
    }

    public GameObject[] Type;
    tiles[,] _grid;
    GameObject _holder;
    int _roomCount;
    Directions dir;
    Vector2 doorPos;
    Vector2 roomPos;
    int roomWidth;
    int roomHeight;
    // Use this for initialization
    void Start()
    {
        _grid = new tiles[80, 80];
        _holder = new GameObject("Holder");
        createFirstRoom();
        while (_roomCount < 30)
            createFeature();


    }

    void createFirstRoom()
    {
        createRoom(40, 40, 8, 8);
    }

    void createFeature()
    {
        do
        {
            selectPoint();
        } while (!inState());
    }

    private bool inState()
    {
        Vector2 Pos = doorPos;
        int fw = UnityEngine.Random.Range(6, 12);
        int fh = UnityEngine.Random.Range(6, 12);
        if (dir == Directions.North || dir == Directions.South)
        {
            if (Pos.x - fw / 2 - 1 < 1 || Pos.x + fw / 2 + 1 > 80 - 1)
                return false;
            Pos.x -= Mathf.CeilToInt(fw * 0.5f);
            if (dir == Directions.North)
            {
                Pos.y += 1;
                if (Pos.y + fh > 80 - 1)
                    return false;
            }
            else
            {
                Pos.y -= fh + 1;
                if (Pos.y < 1)
                    return false;
            }
        }
        else
        {
            if (Pos.y - fh / 2 - 1 < 1 || Pos.y + fh / 2 + 1 > 80 - 1)
                return false;
            Pos.y -= Mathf.CeilToInt(fh * 0.5f);
            if (dir == Directions.East)
            {
                Pos.x += 1;
                if (Pos.x + fw > 80 - 1)
                    return false;
            }
            else
            {
                Pos.x -= fw + 1;
                if (Pos.x < 1)
                    return false;
            }
        }
        roomPos = Pos;
        roomWidth = fw;
        roomHeight = fh;
        return true;
    }
    //选择点
    void selectPoint()
    {
        bool inWall = true;
        int x, y;
        tiles tt, tb, tl, tr;
        do
        {
            x = UnityEngine.Random.Range(2, 78);
            y = UnityEngine.Random.Range(2, 78);

            if (_grid[x,y] == tiles.Wall)
            {
                tt = _grid[x,y + 1];
                tb = _grid[x,y - 1];
                tl = _grid[x - 1,y];
                tr = _grid[x + 1,y];
                if (tt == tiles.Dirt && (tl == tiles.Wall && tr == tiles.Wall))
                {
                    dir = Directions.North;
                    inWall = false;
                }//North
                else if (tb == tiles.Dirt && (tl == tiles.Wall && tr == tiles.Wall))
                {
                    dir = Directions.South;
                    inWall = false;
                }//South
                else if (tl == tiles.Dirt && (tt == tiles.Wall && tb == tiles.Wall))
                {
                    dir = Directions.West;
                    inWall = false;
                }//West
                else if (tr == tiles.Dirt && (tt == tiles.Wall && tb == tiles.Wall))
                {
                    dir = Directions.East;
                    inWall = false;
                }//East
            }
        } while (inWall);
        doorPos = new Vector2(x, y);
    }
    //创建房间
    private bool createRoom(int s, int e, int w, int h)
    {
        w += s;
        h += e;
        //check Area
        if (checkArea(s, e, w, h) && (s != w && e != h))
        {
            for (int i = s; i <= w; i++)
            {
                for (int j = e; j <= h; j++)
                {
                    if (i == s || i == w || j == e || j == h)
                        _grid[i, j] = tiles.Wall;
                    else
                        _grid[i, j] = tiles.Floor;
                }
            }
            return true;
        }
        return false;
    }
    //检查区域是否为空
    bool checkArea(int s, int e, int w, int h)
    {
        for (int i = s; i < w; i++)
        {
            for (int j = e; j < h; j++)
            {
                if (_grid[i, j] != tiles.Dirt)
                    return false;
            }
        }
        return true;
    }
    //初始化Tiles
    void InstantiateTiles()
    {
        for (int i = 0; i < 80; i++)
        {
            for (int j = 0; j < 80; j++)
            {
                switch (_grid[i, j])
                {
                    case tiles.Dirt:
                        InstantiateFromArray(Type[0], i, j);
                        break;
                    case tiles.Floor:
                        InstantiateFromArray(Type[1], i, j);
                        break;
                    case tiles.Wall:
                        InstantiateFromArray(Type[2], i, j);
                        break;
                    case tiles.Door:
                        InstantiateFromArray(Type[3], i, j);
                        break;
                    default:
                        throw new Exception("InstantiateTiles fail");
                }
            }
        }
    }
    //创建游戏对象
    private void InstantiateFromArray(GameObject prefab, int xCoord, int yCoord)
    {
        float x = (float)xCoord * 0.1f;
        float y = (float)yCoord * 0.1f;
        prefab.GetComponent<Transform>().localScale = new Vector2(0.1f, 0.1f);
        GameObject tileInstance = Instantiate(prefab, new Vector2(x, y), Quaternion.identity) as GameObject;
        tileInstance.transform.SetParent(_holder.transform);
    }

}