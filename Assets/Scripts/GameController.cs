using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    // Cell의 사이즈
    public float cellSize;
    public int numOfRows;
    public int numOfColumns;

    // 길 찾기 결과
    private ArrayList pathList;

    // 장애물
    private GameObject[] obstacles;

    // 게임의 시작 위치
    private Vector3 origin = new Vector3();

    // 시작과 끝 위치
    private Transform startTransform, endTransform;

    // Node 객체들
    private Node[,] nodes;

    // GameController 싱글톤 프로퍼티
    private static GameController instance = null;
    public static GameController Instance
    {
        get
        {
            if (!instance)
            {
                instance = FindObjectOfType(typeof(GameController)) as GameController;
                if (!instance)
                {
                    Debug.Log("GameController가 존재하지 않습니다.");
                }
            }
            return instance;
        }
    }

    private void Start()
    {
        // 장애물 정보 설정
        obstacles = GameObject.FindGameObjectsWithTag("Obstacle");

        // Nodes 정보를 설정
        InitNodes();

        startTransform = GameObject.FindGameObjectWithTag("Start").GetComponent<Transform>();
        endTransform = GameObject.FindGameObjectWithTag("End").GetComponent<Transform>();

        int nodeIndex, nodeRowIndex, nodeColumnIndex;
        nodeIndex = GetNodeIndex(startTransform.position);
        nodeRowIndex = GetRowIndex(nodeIndex);
        nodeColumnIndex = GetColumnIndex(nodeIndex);

        Node startNode = nodes[nodeRowIndex, nodeColumnIndex];

        nodeIndex = GetNodeIndex(endTransform.position);
        nodeRowIndex = GetRowIndex(nodeIndex);
        nodeColumnIndex = GetColumnIndex(nodeIndex);

        Node endNode = nodes[nodeRowIndex, nodeColumnIndex];

        // 길 찾기
        pathList = AStar.FindPath(startNode, endNode);

        Debug.Log(pathList);
    }

    private void InitNodes()
    {
        nodes = new Node[numOfRows, numOfColumns];

        // Node의 인덱스
        int index = 0;

        for (int i = 0; i < numOfRows; i++)
        {
            for (int j = 0; j < numOfColumns; j++)
            {
                Vector3 nodePosition = GetNodePosition(index);
                Node node = new Node(nodePosition, index);
                nodes[i, j] = node;
                index++;
            }
        }

        // 장애물 위치 설정
        if (obstacles != null && obstacles.Length > 0)
        {
            foreach (GameObject obstacle in obstacles)
            {
                int nodeIndex = GetNodeIndex(obstacle.transform.position);
                int columnIndex = GetColumnIndex(nodeIndex);
                int rowIndex = GetRowIndex(nodeIndex);

                nodes[rowIndex, columnIndex].isObstacle = true;
            }
        }
    }

    // 특정 좌표 기준으로 전/후/좌/우 방향에 장애물이 있는지 여부 확인
    // 없으면 이동 가능한 영역으로 판단해서 반환
    public ArrayList GetAvailableNodes(Node node)
    {
        ArrayList resultList = new ArrayList();
        Vector3 nodePosition = node.position;
        int nodeIndex = GetNodeIndex(nodePosition);
        int rowIndex = GetRowIndex(nodeIndex);
        int columnIndex = GetColumnIndex(nodeIndex);
        int nodeRowIndex;
        int nodeColumnIndex;

        // 위
        nodeRowIndex = rowIndex + 1;
        nodeColumnIndex = columnIndex;
        if (IsAvailableNode(nodeRowIndex, nodeColumnIndex))
        {
            resultList.Add(nodes[nodeRowIndex, nodeColumnIndex]);
        }

        // 아래
        nodeRowIndex = rowIndex - 1;
        nodeColumnIndex = columnIndex;
        if (IsAvailableNode(nodeRowIndex, nodeColumnIndex))
        {
            resultList.Add(nodes[nodeRowIndex, nodeColumnIndex]);
        }

        // 오른쪽
        nodeRowIndex = rowIndex;
        nodeColumnIndex = columnIndex + 1;
        if (IsAvailableNode(nodeRowIndex, nodeColumnIndex))
        {
            resultList.Add(nodes[nodeRowIndex, nodeColumnIndex]);
        }

        // 왼쪽
        nodeRowIndex = rowIndex;
        nodeColumnIndex = columnIndex - 1;
        if (IsAvailableNode(nodeRowIndex, nodeColumnIndex))
        {
            resultList.Add(nodes[nodeRowIndex, nodeColumnIndex]);
        }

        return resultList;
    }

    // 특정 Row, Column index의 Node가 유효한지 확인하는 함수
    private bool IsAvailableNode(int rowIndex, int columnIndex)
    {
        // 해당 row, column이 범위 밖이라면 false
        if (!IsAvailableIndex(rowIndex, columnIndex)) return false;

        // 해당 row, column이 장애물인지 확인
        Node node = nodes[rowIndex, columnIndex];

        if (!node.isObstacle) return true;
        return false;
    }

    private bool IsAvailablePosition(Vector3 position)
    {
        float availableWidht = numOfColumns * cellSize;
        float availableHeight = numOfRows * cellSize;

        if (position.x >= origin.x && position.x <= origin.x + availableWidht &&
            position.z >= origin.z && position.z <= origin.z + availableHeight)
        {
            return true;
        }
        return false;
    }

    // row와 column 인덱스가 유효한지 확인하는 함수
    private bool IsAvailableIndex(int rowIndex, int columnIndex)
    {
        if (rowIndex > -1 && columnIndex > -1 && 
            rowIndex < numOfRows && columnIndex < numOfColumns)
        {
            return true;
        }
        return false;
    }

    private Vector3 GetNodePosition(int index)
    {
        int rowIndex = GetRowIndex(index);
        int columnIndex = GetColumnIndex(index);

        float xPosition = (columnIndex * cellSize) + (cellSize / 2f);
        float zPosition = (rowIndex * cellSize) + (cellSize / 2f);

        return new Vector3(xPosition, 1f, zPosition);
    }

    private int GetNodeIndex(Vector3 position)
    {
        if (!IsAvailablePosition(position))
        {
            return -1;
        }

        int columnIndex = (int)(position.x / cellSize);
        int rowIndex = (int)(position.z / cellSize);

        return (rowIndex * numOfColumns + columnIndex);
    }

    private int GetRowIndex(int nodeIndex)
    {
        int rowIndex = nodeIndex / numOfColumns;
        return rowIndex;
    }

    private int GetColumnIndex(int nodeIndex)
    {
        int columnIndex = nodeIndex % numOfColumns;
        return columnIndex;
    }

    private void OnDrawGizmos()
    {
        if (pathList != null && pathList.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathList)
            {
                if (index < pathList.Count)
                {
                    Node nextNode = (Node)pathList[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.red);
                    index++;
                }
            }
        }
    }
}
