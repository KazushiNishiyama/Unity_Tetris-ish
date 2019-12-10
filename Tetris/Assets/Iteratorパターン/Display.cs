using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : Aggregate
{

    private GameObject[] LED;
    private int last = 0;

    public Display(int maxsize)
    {
        LED = new GameObject[maxsize];
    }

    public GameObject GetLEDAt(int index)
    {
        return LED[index];
    }

    public void AppendLED(GameObject led)
    {
        this.LED[last] = led;
        last++;
    }

    public int GetLength()
    {
        return last;
    }

    public Iterator iterator()
    {
        return new LEDIterator(this);
    }
}
