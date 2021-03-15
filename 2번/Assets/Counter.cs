using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Counter : MonoBehaviour
{
    public static Counter Counter_Instance;
    public int Count;

    private void Awake()
    {
        if (Counter_Instance != null)
        {
            Debug.Log("싱글톤 중복");
        }
        else
        {
            Counter_Instance = this;
        }
    }

    public void Update()
    {
        AddCount();
    }

    public int AddCount()
    {
        int RandomNumber = Random.Range(0, 10);
        Count += RandomNumber;
        return Count;
    }
}
