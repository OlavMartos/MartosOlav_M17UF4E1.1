using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject token1, token2;
    private float distance = 1.1f;
    private int length = 3;
    private int[,] GameMatrix; //0 not chosen, 1 player, 2 enemy
    private Vector2[,] GameMatrixPos;
    private Vector3 mousePos;
    private void Awake()
    {
        GameMatrix = new int[length, length];
        GameMatrixPos = new Vector2[length, length];

        for (int i = 0; i < length; i++) //fila
            for (int j = 0; j < length; j++) //columna
                GameMatrix[i, j] = 0;

        float iniX = -1f;
        float iniY = -1f;
        for (int i = 0; i < length; i++) //fila
        {
            for (int j = 0; j < length; j++) //columna
            {
                GameMatrixPos[i, j] = new Vector2(iniX * 2f * distance, -iniY * 2f * distance);
                iniY++;
            }
            iniY = -1f;
            iniX++;
        }
        ShowMatrixPos();


    }

    private void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePos - Vector3.forward * -2, transform.forward);
        Debug.DrawLine(mousePos - Vector3.forward * -2, mousePos - Vector3.forward * +2);

        if (hit.collider != null)
        {
            if (Input.GetMouseButtonUp(0))
            {
                var pos = CalculatePosition();
                if (pos.Item2)
                {
                    Instantiate(token1, pos.Item1, Quaternion.identity);
                    // GameMatrix
                    EvaluateWin();
                    //EnemyResponse();
                    //EvaluateWin();
                }
            }
        }
        
    }
    /// <summary>
    /// Torna la posició de la fitxa que tries en funció de la posició del click
    /// </summary>
    /// <returns>
    /// Una tupla que contiene:
    /// - Un booleano que indica si la operación fue exitosa.
    /// - Un Vector3 que representa una posición en el espacio 3D.
    /// </returns>
    private (Vector3, bool) CalculatePosition() //3 x 3, es podria fer recursiu però no em dona la gana
    {
        Vector3 position = Vector3.zero;
        int[] booleanPos = new int[2];
        bool dev = false;
        int x = 0, y = 0;
        booleanPos[1] = 1; //columna
        if (mousePos.x > distance) //X
        {
            position.x = 2 * distance;
            booleanPos[1] = 2;
        }
        else if (mousePos.x < -distance)
        {
            position.x = -2 * distance;
            booleanPos[1] = 0;
        }

        booleanPos[0] = 1; //fila
        if (mousePos.y > distance) //Y
        {
            position.y = 2 * distance;
            booleanPos[0] = 0;
        }
        else if (mousePos.y < -distance)
        {
            position.y = -2 * distance;
            booleanPos[0] = 2;
        }
        if (GameMatrix[booleanPos[0], booleanPos[1]] == 0)
        {
            GameMatrix[booleanPos[0], booleanPos[1]] = 1;
            dev = true;
            ShowMatrix();
        }
        return (position, dev);
    }

    private bool CalculatePositionEnemy(int row, int column)
    {
        bool dev = false;
        if (GameMatrix[row, column] == 0)
        {
            GameMatrix[row, column] = 2;
            dev = true;
            ShowMatrix();
        }
        return dev;
    }
    private void ShowMatrix() //fa un debug log de la matriu
    {
        string matrix = "";
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                matrix += GameMatrix[i, j] + " ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }
    private void ShowMatrixPos() //fa un debug log de la matriu
    {
        string matrix = "";
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < length; j++)
            {
                matrix += GameMatrixPos[i, j] + " ";
            }
            matrix += "\n";
        }
        Debug.Log(matrix);
    }

    //EL VOSTRE EXERCICI COMENÇA AQUI
    private Vector3 CalculateEnemyPosition(int iChoice, int jChoice)
    {
        Vector3 position = Vector3.zero;

        return position;
    }
    private void EnemyResponse()
    {
        int[] booleanPos = new int[2];
        CalculateEnemyPosition(booleanPos[0], booleanPos[1]);
        ShowMatrix();
    }
    private int EvaluateWin()
    {
        int lose = 0; //0 game not ended, 1 wins player, 2 wins enemy
        int winPlayer = 0;
        int winAI = 0;
        int counter = 0;
        while (counter <3 && lose == 0)
        {
            for (int j = 0; j < length; j++)
            {
                winPlayer += GameMatrix[counter, j] == 1 ? 1 : 0;
                winAI += GameMatrix[counter, j] == 2 ? 1 : 0;
            }
            counter++;
            lose = winPlayer == 3 ? 1 : lose;
            lose = winAI == 3 ? 2 : lose;
            winPlayer = 0;
            winAI = 0;
        }
        counter = 0;
        while (counter <3 && lose == 0)
        {
            for (int j = 0; j < length; j++)
            {
                winPlayer += GameMatrix[j, counter] == 1 ? 1 : 0;
                winAI += GameMatrix[j, counter] == 2 ? 1 : 0;
            }
            counter++;
            lose = winPlayer == 3 ? 1 : lose;
            lose = winAI == 3 ? 2 : lose;
            winPlayer = 0;
            winAI = 0;
        }
        counter = 0;
        if (lose == 0)
        {
            for (int j = 0; j < length; j++)
            {
                winPlayer += GameMatrix[j, j] == 1 ? 1 : 0;
                winAI += GameMatrix[j, j] == 2 ? 1 : 0;
            }
            lose = winPlayer == 3 ? 1 : lose;
            lose = winAI == 3 ? 2 : lose;
            winPlayer = 0;
            winAI = 0;
        }
        if (lose == 0)
        {
            for (int j = 0; j < length; j++)
            {
                winPlayer += GameMatrix[2 - j, j] == 1 ? 1 : 0;
                winAI += GameMatrix[2 - j, j] == 2 ? 1 : 0;
            }
            lose = winPlayer == 3 ? 1 : lose;
            lose = winAI == 3 ? 2 : lose;
            counter = 0;
            winPlayer = 0;
            winAI = 0;
        }
        if (lose == 1)
            Debug.Log("Player wins");
        else if (lose == 2)
            Debug.Log("Enemy wins");
        return lose;
    }
    
}
