using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject token1, token2, token3;
    private int[,] GameMatrix; //0 not chosen, 1 player, 2 enemy
    private int[] startPos = new int[2];
    private int[] objectivePos = new int[2];
    private List<Node> openSet;
    private List<Node> closedSet;
    class Node //se que seria recomendable hacer esto en otro .cs pero no tengo apenas ganas de vivir como para molestarme (estoy cansado jefe)
    {
        int[] position;
        float cost;
        bool walkable;
        float heuristica;
        private int[] objectivePos;

        public Node(int[] position, float cost, bool walkable, int[] objectivePos)
        {
            this.position = position;
            this.cost = cost;
            this.walkable = walkable;
            this.cost = 0.2f;          
            heuristica = Calculator.CheckDistanceToObj(position, objectivePos);
        }
        //public int calcHeuristica(Node node1, Node node2) 
        //{
        //    int x = (node1.position[1] - node2.position[1]);
        //    int y = (node1.position[0] - node2.position[0]);
        //    return x + y;
        //}
        public Node[] checkAround() 
        {
            for (int[] i = position; position[0] < position[0] + 1; i[0] = i[0]);
            for (int[] i = position; position[0] < position[0] + 1; i[0] = i[0]);
            for (int[] i = position; position[0] < position[0] + 1; i[0] = i[0]);
            for (int[] i = position; position[0] < position[0] + 1; i[0] = i[0]);
            return //Lista nodos, meterla a lista abierta, comprobar valor mas pequeño y meter a la cerrada;
        }
    }
    private void Awake()
    {
        GameMatrix = new int[Calculator.length, Calculator.length];

        for (int i = 0; i < Calculator.length; i++) //fila
            for (int j = 0; j < Calculator.length; j++) //columna
                GameMatrix[i, j] = 0;

        //randomitzar pos final i inicial;
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        startPos[0] = rand1;
        startPos[1] = rand2;
        SetObjectivePoint(startPos);

        GameMatrix[startPos[0], startPos[1]] = 1;
        GameMatrix[objectivePos[0], objectivePos[1]] = 2;

        InstantiateToken(token1, startPos);
        InstantiateToken(token2, objectivePos);
        ShowMatrix();
    }
    private void InstantiateToken(GameObject token, int[] position)
    {
        Instantiate(token, Calculator.GetPositionFromMatrix(position),
            Quaternion.identity);
    }
    private void SetObjectivePoint(int[] startPos) 
    {
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        if (rand1 != startPos[0] || rand2 != startPos[1])
        {
            objectivePos[0] = rand1;
            objectivePos[1] = rand2;
        }
    }

    private void ShowMatrix() //fa un debug log de la matriu
    {
        string matrix = "";
        for (int i = 0; i < Calculator.length; i++)
        {
            for (int j = 0; j < Calculator.length; j++)
            {
                matrix += GameMatrix[i, j] + " ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }
    //EL VOSTRE EXERCICI COMENÇA AQUI
    private void Update()
    {
        if(!EvaluateWin())
        {
            openSet.Add(new Node(startPos, 0, false, objectivePos));
            while(openSet.Count > 0) 
            {
                
            }
        }
    }
    private bool EvaluateWin()
    {
        return false;
    }

    
}
