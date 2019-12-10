using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LEDIterator : Iterator
{
    private Display display;
    private int index;

    public LEDIterator(Display display)
    {
        this.display = display;
        index = 0;
    }

    public bool hasNext()
    {
        return this.display.GetLength() > index;
    }

    public Object next()
    {
        GameObject led = display.GetLEDAt(index);
        index++;
        return led;
    }
}
