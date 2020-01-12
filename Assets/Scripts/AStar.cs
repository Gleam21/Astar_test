using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStar
{
    private static SortingQueue<Node> closedQueue, openQueue;

    public static ArrayList FindPath(Node startNode, Node endNode)
    {
        int findCount = 0;

        // 탐색을 위한 Node를 담을 Queue 설정
        openQueue = new SortingQueue<Node>();
        openQueue.Enqueue(startNode);

        startNode.gScore = 0f;
        startNode.hScore = GetPostionScore(startNode, endNode);

        // 탐색이 끝난 Node를 담을 Queue 설정
        closedQueue = new SortingQueue<Node>();

        Node node = null;

        while (openQueue.Count != 0)
        {
            node = openQueue.Dequeue();

            // 목적지를 찾았다면
            if (node == endNode)
            {
                Debug.Log("Find: " + findCount);
                
                return GetReverseResult(node);
            }

            // Node를 기준으로 갈수있는 주변 길 찾기
            ArrayList availableNodes = GameController.Instance.GetAvailableNodes(node);

            foreach (Node availableNode in availableNodes)
            {
                if (!closedQueue.Contains(availableNode))
                {
                    if (openQueue.Contains(availableNode))
                    {
                        float score = GetPostionScore(node, availableNode);
                        float newGScore = node.gScore + score;

                        if (availableNode.gScore > newGScore)
                        {
                            availableNode.gScore = newGScore;
                            availableNode.parent = node;
                        }
                    }
                    else
                    {
                        float score = GetPostionScore(node, availableNode);

                        float newGScore = node.gScore + score;
                        float newHScore = GetPostionScore(availableNode, endNode);

                        availableNode.gScore = newGScore;
                        availableNode.hScore = newHScore;
                        availableNode.parent = node;

                        openQueue.Enqueue(availableNode);
                        findCount++;
                    }
                }
            }
            closedQueue.Enqueue(node);
        }

        if (node == endNode)
        {
            Debug.Log("Find: " + findCount);
            return GetReverseResult(node);
        }

        return null;
    }

    private static ArrayList GetReverseResult(Node node)
    {
        ArrayList resultArrayList = new ArrayList();
        while (node != null)
        {
            resultArrayList.Add(node);
            node = node.parent;
        }
        resultArrayList.Reverse();

        return resultArrayList;
    }

    private static float GetPostionScore(Node currentNode, Node endNode)
    {
        Vector3 resultValue = currentNode.position - endNode.position;
        return resultValue.magnitude;
    }
}
