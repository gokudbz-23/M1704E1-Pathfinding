using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject token1, token2, token3;
    private int[,] GameMatrix; //0 not chosen, 1 player, 2 enemy
    private int[] startPos = new int[2];
    static private int[] objectivePos = new int[2];
    private List<Node> openSet;
    private List<Node> closedSet;
    private List<Node> path; // Lista para almacenar el camino óptimo

    class Node
    {
        public int[] position;
        public float cost;
        public float heuristic;
        public Node parent;

        public Node(int[] position, float cost, Node parent, int[] objectivePos)
        {
            this.position = position;
            this.cost = cost;
            this.parent = parent;
            this.heuristic = Calculator.CheckDistanceToObj(position, objectivePos);
        }

        public Node[] CheckAround(int[,] GameMatrix, int length)
        {
            List<Node> neighbors = new List<Node>();

            // Definir las posiciones vecinas
            int[][] positions = new int[][]
            {
                new int[] { position[0] + 1, position[1] }, // Arriba
                new int[] { position[0] - 1, position[1] }, // Abajo
                new int[] { position[0], position[1] + 1 }, // Derecha
                new int[] { position[0], position[1] - 1 }  // Izquierda
            };

            foreach (int[] pos in positions)
            {
                // Comprobar si la posición está dentro de los límites del tablero
                if (pos[0] >= 0 && pos[0] < length && pos[1] >= 0 && pos[1] < length)
                {
                    // Crear un nuevo nodo vecino con un costo arbitrario de 1
                    Node neighbor = new Node(pos, 1, this, objectivePos);
                    neighbors.Add(neighbor);
                }
            }

            return neighbors.ToArray();
        }
    }

    private void Awake()
    {
        GameMatrix = new int[Calculator.length, Calculator.length];
        openSet = new List<Node>();
        closedSet = new List<Node>();
        path = new List<Node>(); // Inicializar la lista de camino óptimo

        for (int i = 0; i < Calculator.length; i++) //fila
        {
            for (int j = 0; j < Calculator.length; j++) //columna
            {
                GameMatrix[i, j] = 0;
            }
        }

        //randomizar posición final e inicial;
        var rand1 = Random.Range(0, Calculator.length);
        var rand2 = Random.Range(0, Calculator.length);
        startPos[0] = rand1;
        startPos[1] = rand2;
        SetObjectivePoint(startPos);

        GameMatrix[startPos[0], startPos[1]] = 1;
        GameMatrix[objectivePos[0], objectivePos[1]] = 2;

        InstantiateToken(token1, startPos);
        InstantiateToken(token2, objectivePos);
        //ShowMatrix();
    }

    private void InstantiateToken(GameObject token, int[] position)
    {
        Instantiate(token, Calculator.GetPositionFromMatrix(position), Quaternion.identity);
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

    //private void ShowMatrix() //debug log de la matriz
    //{
    //    string matrix = "";
    //    for (int i = 0; i < Calculator.length; i++)
    //    {
    //        for (int j = 0; j < Calculator.length; j++)
    //        {
    //            matrix += GameMatrix[i, j] + " ";
    //        }
    //        matrix += "\n";
    //    }
    //    Debug.Log(matrix);
    //}

    private void Update()
    {
        if (!EvaluateWin())
        {
            openSet.Add(new Node(startPos, 0, null, objectivePos)); // Agregamos el nodo inicial a la lista abierta
            while (openSet.Count > 0)
            {
                // Encontrar el nodo con menor costo + heurística en la lista abierta
                Node currentNode = openSet[0];
                for (int i = 1; i < openSet.Count; i++)
                {
                    if ((openSet[i].cost + openSet[i].heuristic) < (currentNode.cost + currentNode.heuristic))
                    {
                        currentNode = openSet[i];
                    }
                }

                // Mover el nodo actual de la lista abierta a la lista cerrada
                openSet.Remove(currentNode);
                closedSet.Add(currentNode);

                // Si el nodo actual es el objetivo, finalizar
                if (currentNode.position[0] == objectivePos[0] && currentNode.position[1] == objectivePos[1])
                {
                    // Reconstruir el camino óptimo
                    Node currentPathNode = currentNode;
                    while (currentPathNode != null)
                    {
                        path.Add(currentPathNode);
                        currentPathNode = currentPathNode.parent;
                    }
                    path.Reverse(); // Invertir la lista para obtener el camino desde el inicio hasta el objetivo

                    // Dibujar el camino en la escena
                    DrawPath();
                    break; // Finalizar el bucle
                }

                // Expandir los nodos vecinos del nodo actual
                Node[] neighbors = currentNode.CheckAround(GameMatrix, Calculator.length);
                foreach (Node neighbor in neighbors)
                {
                    if (neighbor == null || closedSet.Contains(neighbor))
                        continue;

                    float newCost = currentNode.cost + neighbor.cost;
                    if (!openSet.Contains(neighbor) || newCost < neighbor.cost)
                    {
                        neighbor.cost = newCost;
                        neighbor.parent = currentNode;

                        if (!openSet.Contains(neighbor))
                            openSet.Add(neighbor);
                    }
                }
            }
        }
    }

    private bool EvaluateWin()
    {
        // Si la lista de camino óptimo contiene al menos dos nodos (inicio y objetivo), detener el algoritmo
        return path.Count >= 2;
    }

    private void DrawPath()
    {
        // Dibuja el camino recorrido
        for (int i = 0; i < path.Count - 1; i++)
        {
            // Instanciar un marcador en la posición del nodo
            GameObject pathMarker = Instantiate(token3, Calculator.GetPositionFromMatrix(path[i].position), Quaternion.identity);
            GameObject finalMarker = Instantiate(token3, Calculator.GetPositionFromMatrix(path[path.Count - 1].position), Quaternion.identity);
        }
    }
}
