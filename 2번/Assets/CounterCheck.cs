using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterCheck : MonoBehaviour
{
    private int Division = 10;
    private int Count;

    private void Update()
    {
        Count = Counter.Counter_Instance.Count;
        if(CountChecker(Count))
        {
            Debug.Log(Count);
            Debug.Log("자릿수 변화");
        }
    }

    public bool CountChecker(int Count)
    {
        if(Count / Division != 0)
        {
            Division *= 10;
            return true;
        }
        else
        {
            return false;
        }
    }
}
