using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Node : IComparable
{
    public float FScore
    {
        get
        {
            return hScore + gScore;
        }
    }

    public int index;

    public float hScore;            // Node에서 목적지까지 직선 거리를 점수로 표현한 것
    public float gScore;            // 출발지에서 현재 Node까지의 거리를 점수로 표현한 것
    public Node parent;             // 자신을 찾아준 부모 Node

    public Vector3 position;        // 위치값
    public bool isObstacle;         // 장애물인지 아닌지 여부

    public Node(Vector3 position, int index)
    {
        this.hScore = 0f;
        this.gScore = 0f;
        this.isObstacle = false;
        this.parent = null;
        this.position = position;
        this.index = index;
    }

    public int CompareTo(object obj)
    {
        Node node = (Node)obj;

        if (this.FScore < node.FScore)
        {
            return -1;
        }
        else if (this.FScore > node.FScore)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }

    public override string ToString()
    {
        return "[ " + this.index + "]";
    }
}
