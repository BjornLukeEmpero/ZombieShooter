using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    public float lifetime;
    //정해진 시간

    private void Start()
    {
        Destroy(gameObject, lifetime); 
        //정해진 시간이 지나고 gameObject를 없앤다
    }
    //게임이 시작할 시
}
