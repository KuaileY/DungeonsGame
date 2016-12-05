using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entitas;
using UnityEngine;


public static class AstarExtension
{
    static List<Node> map;
    static List<Vector2> path = new List<Vector2>();
    const int map_size = 32;
    const int map_size1 = 31;
    const int D1 = 5;

    private static int mapVersion = 0;
    private static int nodeVersion = 0;
    private static int xx;
    private static int yy;

    public static void PathFind(Vector2 startPos, Vector2 endPos, Pools pools, Action<List<Vector2>> action)
    {
        endPos.print();
        Debug.Log("cat");
        setMap(startPos,endPos,pools);
        search(startPos - new Vector2(xx, yy), endPos - new Vector2(xx, yy), action);

    }
    static void setMap(Vector2 startPos,Vector2 endPos, Pools pools)
    {
        mapVersion = 1;
        int l, x, y, c;
        Node[] links;
        map = null ?? new List<Node>();
        map.Clear();
        path.Clear();

        xx = (int) startPos.x - map_size/2;
        yy = (int) startPos.y - map_size/2;

        if (xx < 1)
            xx = 1;
        if ((xx + map_size / 2) > Res.columns)
            xx = Res.columns - map_size;
        if (yy < 1)
            yy = 1;
        if ((yy + map_size / 2) > Res.rows)
            yy = Res.rows - map_size;

        for (int i = 0; i < map_size*map_size; i++)
        {
            var n = new Node();
            x = i & map_size1;
            y = i >> D1;
            n.x = x;
            n.y = y;
            n.links = new Node[4];
            var gx = n.x + xx ;
            var gy = n.y + yy - 1;

            if (gx < 0 || gx > Res.columns - 1)
            {
                gx.print();
                throw new Exception("gx is wrong!");
            }
            if (gy < 0 || gy > Res.rows - 1)
            {
                gy.print();
                throw new Exception("gy is wrong!");
            }

            if (pools.board.dungeonItemsCache.grid[gx, gy] != null)
                n.block = mapVersion;

            map.Add(n);
        }

        l = map.Count;
        for (int i = 0; i < l; i++)
        {
            var n = map[i];
            links = n.links; 
            x = n.x;
            y = n.y;
            c = 0;
            if ((((i & map_size) >> D1) ^ (i & 1)) == 1)
            {
                if (y > 0 && map[(y - 1) << D1 | x].block != mapVersion)
                {
                    links[c] = (map[(y - 1) << D1 | x]);
                    c++;
                }
                if (y < map_size1 && map[(y + 1) << D1 | x].block != mapVersion)
                {
                    links[c] = (map[(y + 1) << D1 | x]);
                    c++;
                }
                if (x < map_size1 && map[y << D1 | (x + 1)].block != mapVersion)
                {
                    links[c] = (map[y << D1 | (x + 1)]);
                    c++;
                }
                if (x > 0 && map[y << D1 | (x - 1)].block != mapVersion)
                {
                    links[c] = (map[y << D1 | (x - 1)]);
                    c++;
                }
            }
            else
            {
                if (x < map_size1 && map[y << D1 | (x + 1)].block != mapVersion)
                {
                    links[c] = (map[y << D1 | (x + 1)]);
                    c++;
                }
                if (x > 0 && map[y << D1 | (x - 1)].block != mapVersion)
                {
                    links[c] = (map[y << D1 | (x - 1)]);
                    c++;
                }
                if (y > 0 && map[(y - 1) << D1 | x].block != mapVersion)
                {
                    links[c] = (map[(y - 1) << D1 | x]);
                    c++;
                }
                if (y < map_size1 && map[(y + 1) << D1 | x].block != mapVersion)
                {
                    links[c] = (map[(y + 1) << D1 | x]);
                    c++;
                }
            }
            n.linksLenght = c;
        }


    }

    static void search(Vector2 startPos, Vector2 endPos, Action<List<Vector2>> action)
    {
        Node startNode = map[((int) startPos.y ) << D1 | (int) startPos.x];
        Node endNode = map[((int)endPos.y ) << D1 | (int)endPos.x];

        if (startNode.block == mapVersion )
            throw new Exception("start pos is obstacle");
        var ss = string.Empty;
        for (int i = 0; i < map.Count ; i++)
        {
            if (i%map_size == 0)
                ss += "\n";
            if (map[i].block == mapVersion)
                ss += "#";
            else
                ss += " ";
        }
        ss.print();
        endPos.print();

        if(endNode.block == mapVersion)
            throw new Exception("end pos is obstacle");

        int l, f;
        Node t;
        var openBase = Mathf.Abs(endPos.x - startPos.x) + Mathf.Abs(endPos.y - startPos.y);
        Node[] open = new Node[2];
        Node current, test;
        Node[] links;

        open[0] = startNode;
        startNode.pre = startNode.next = null;
        startNode.version = ++nodeVersion;
        startNode.nowCost = 0;

        while (true)
        {
            current = open[0];
            open[0] = current.next;
            if (open[0] != null)
                open[0].pre = null;
            if (current == endNode)
            {
                Debug.Log("OK");
                buildPath(startNode, current);
                var outList = new List<Vector2>();
                foreach (var pos in path )
                    outList.Add(pos + new Vector2(xx, yy));

                action.Invoke(outList);
                return;
            }
            links = current.links;
            l = current.linksLenght;
            for (int i = 0; i < l; i++)
            {
                test = links[i];
                f = current.nowCost + 1;
                if (test.version != nodeVersion)
                {
                    test.version = nodeVersion;
                    test.parent = current;
                    test.nowCost = f;
                    var d = Mathf.Abs(endPos.x - test.x) + Mathf.Abs(endPos.y  - test.y);
                    test.dist = (int)d;
                    f += test.dist;
                    test.mayCost = f;
                    f = (f - (int)openBase) >> 1;
                    test.pre = null;
                    test.next = open[f];
                    if (open[f] != null)
                        open[f].pre = test;
                    open[f] = test;
                }
                else if (test.nowCost > f)
                {
                    if (test.pre != null)
                        test.pre.next = test.next;
                    if (test.next != null)
                        test.next.pre = test.pre;
                    if (open[1] == test)
                        open[1] = test.next;
                    test.parent = current;
                    test.nowCost = f;
                    test.mayCost = f + test.dist;
                    test.pre = null;
                    test.next = open[0];
                    if (open[0] != null)
                        open[0].pre = test;
                    open[0] = test;
                }
            }
            if (open[0] == null)
            {
                if (open[1] == null)
                    break;
                t = open[0];
                open[0] = open[1];
                open[1] = t;
                openBase += 2;
            }
        }
        return;
    }

    static void buildPath(Node startNode, Node endNode)
    {
        path.Add(new Vector2(endNode.x,endNode.y));
        while (endNode != startNode)
        {
            endNode = endNode.parent;
            path.Add(new Vector2(endNode.x,endNode.y));
        }
    }

    public class Node
    {
        public int block;
        public int version;
        public Node[] links;
        public int linksLenght;
        public Node parent;
        public int nowCost;
        public int mayCost;
        public int dist;
        public int x;
        public int y;
        public Node next;
        public Node pre;
        public GameObject go;
    }
}

