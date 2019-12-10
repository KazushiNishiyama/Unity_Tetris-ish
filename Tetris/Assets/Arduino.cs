using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block
{
    public Vector2Int[] point = new Vector2Int[3];

    public Block(Vector2Int a,Vector2Int b,Vector2Int c)
    {
        point[0] = a;
        point[1] = b;
        point[2] = c;
    }
}

public class State
{
    public int x;
    public int y;
    public int type;
    public Block block;
}

public class Arduino : MonoBehaviour
{
    [SerializeField]
    LEDController display;

    //10*22+枠+画面上部４マス
    //壁 : 1
    //無 : 0
    private int[,] board = new int[25, 12];
    private int[,] displayBoard = new int[25, 12];

    private State current = new State();


    private Color[] CellColor = new Color[8]
    {
        Color.black,
        Color.cyan,
        new Color(243,152,0),//オレンジ
        Color.blue,
        Color.red,
        Color.green,
        Color.yellow,
        new Color(167,87,168),//紫
    };

    private enum Mino
    {
        None,
        Tetris,
        L,
        J,
        Z,
        S,
        Square,
        T,
    }

    Block[] block = new Block[8]
    {
        new Block(new Vector2Int(0,0), new Vector2Int(0,0), new Vector2Int(0,0)), //null
        new Block(new Vector2Int(0,-1), new Vector2Int(0,1), new Vector2Int(0,2)), //tetris
        new Block(new Vector2Int(0,-1), new Vector2Int(0,1), new Vector2Int(1,1)), //L
        new Block(new Vector2Int(0,-1), new Vector2Int(0,1), new Vector2Int(-1,1)), //J
        new Block(new Vector2Int(0,-1), new Vector2Int(1,0), new Vector2Int(1,1)), //Z
        new Block(new Vector2Int(0,-1), new Vector2Int(-1,0), new Vector2Int(-1,1)), //S
        new Block(new Vector2Int(0,1), new Vector2Int(1,0), new Vector2Int(1,1)), //Square
        new Block(new Vector2Int(0,-1), new Vector2Int(1,0), new Vector2Int(0,1)), //T
    };

    void Start()
    {
        for(int y = 0; y < 25; y++)
        {
            for(int x = 0; x < 12; x++)
            {
                if (x == 0 || x == 11 || y == 0)
                {
                    board[y, x] = -1;
                    displayBoard[y, x] = -1;
                }
                else
                {
                    board[y, x] = 0;
                    displayBoard[y, x] = 0;
                }
            }
        }
        WriteDisplay();

        current.x = 5;
        current.y = 21;

        StartCoroutine(Loop());
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (!CheckMoveable(Vector2Int.right)) return;
            Move(Vector2Int.right);
            WriteDisplay();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (!CheckMoveable(Vector2Int.left)) return;
            Move(Vector2Int.left);
            WriteDisplay();
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            Rotate();
            WriteDisplay();
        }
    }

    private void Rotate()
    {
        SetDisplayBoardState(0);
        for(int i = 0; i < 3; i++)
        {
            int buff = current.block.point[i].y;
            current.block.point[i].y = -current.block.point[i].x;
            current.block.point[i].x = buff;
        }
        SetDisplayBoardState(current.type);
    }

    private void Move(Vector2Int point)
    {
        SetDisplayBoardState(0);
        current.y += point.y;
        current.x += point.x;
        SetDisplayBoardState(current.type);
    }

    IEnumerator Loop()
    {
        current.type = Random.Range(1, 7);
        current.block = block[current.type];
        while (true)
        {
            yield return new WaitForSeconds(0.1f);

            if (CheckMoveable(Vector2Int.down)) Move(Vector2Int.down);
            else Next();
            WriteDisplay();
        }
    }

    private void SetDisplayBoardState(int s)
    {
        displayBoard[current.y, current.x] = s;
        for (int i = 0; i < 3; i++)
        {
            int pointY = current.y + current.block.point[i].y;
            int pointX = current.x + current.block.point[i].x;
            displayBoard[pointY, pointX] = s;
        }
    }

    private void SetBoardState(int s)
    {
        board[current.y, current.x] = s;
        for (int i = 0; i < 3; i++)
        {
            int pointY = current.y + current.block.point[i].y;
            int pointX = current.x + current.block.point[i].x;
            board[pointY, pointX] = s;
        }
    }

    private void Next()
    {
        SetBoardState(current.type);
        if (current.y == 21) GameOver();
        else
        {
            current.x = 5;
            current.y = 21;
            current.type = Random.Range(1, 7);
            current.block = block[current.type];
        }
        LineProcess();
    }

    private void LineProcess()
    {
        for (int y = 1; y < 21; y++)
        {
            for (int x = 1; x < 11; x++)
            {
                if (board[y, x] == 0) break;
                if (x == 10) RemoveLine(y);
            }
        }
        WriteDisplay();
    }

    private void RemoveLine(int line)
    {
        for(int x = 1; x < 11; x++)
        {
            board[line, x] = 0;
            displayBoard[line, x] = 0;
        }

        for(int y = line+1; y < 21; y++)
        {
            for(int x = 1; x < 11; x++)
            {
                board[y - 1, x] = board[y, x];
                displayBoard[y - 1, x] = displayBoard[y, x];
            }
        }
        LineProcess();
    }

    private void GameOver()
    {
        Debug.Log("GameOver");
    }

    private bool CheckMoveable(Vector2Int diff)
    {
        if (board[current.y+diff.y, current.x+diff.x] != 0) return false;
        for (int i = 0; i < 3; i++)
        {
            int pointY = current.y+diff.y + current.block.point[i].y;
            int pointX = current.x + diff.x + current.block.point[i].x;
            if (board[pointY, pointX] != 0) return false;
        }
        return true;
    }


    private void WriteDisplay()
    {
        for(int y = 1; y < 21; y++)
        {
            for(int x = 1; x < 11; x++)
            {
                display.Write(CellColor[displayBoard[y, x]], new Vector2Int(x-1, y-1));
            }
        }
    }
}
