using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LEDController : MonoBehaviour
{
    [SerializeField]
    GameObject led;

    LED[,] display = new LED[20, 10];

    void Start()
    {
        CreateDisplay();
    }

    private void CreateDisplay()
    {
        float startY = -4.5f;
        float startX = -2;
        float diffY = 0.45f;
        for (int y = 0; y < 20; y++)
        {
            for (int x = 0; x < 10; x++)
            {
                //LED(sphere)を生成
                var p = Instantiate(led);
                p.transform.position = new Vector3(startX, startY, 0);
                startX += diffY;
                display[y, x] = p.GetComponent<LED>();
            }
            startX = -2;
            startY += 0.45f;
        }
    }

    public void Write(Color color,Vector2Int index)
    {
        display[index.y, index.x].SetState(color);
    }
}
