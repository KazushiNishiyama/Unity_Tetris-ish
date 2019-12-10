using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iterator
{
    bool hasNext();
    Object next();
}