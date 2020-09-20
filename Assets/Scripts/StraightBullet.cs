using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightBullet : MonoBehaviour
{
    public float speed;
    //탄속
    public float damage;
    //공격력
    public float validRange;
    //유효 사거리
    public GameObject explosion;
    //폭발 이펙트
    public Transform[] zombie;
    //찾고 있는 좀비

    private void Update()
    {
        validRange -= Time.deltaTime;
        //유효 사거리에 도달하는 과정
        if(validRange <= 0)
        {
            Destroy(gameObject);
            //총알이 사라진다
        }
        //총알이 최대 유효 사거리에 도달한 이후

        transform.Translate(Vector3.up * speed * Time.deltaTime);
        //탄속 계산
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            return;
        }
        //같은 총알과 충돌할 시 파괴되지 않는다
        
        if(collision.gameObject.tag == "GameController")
        {
            return;
        }
        //게임 컨트롤러와 충돌해도 파괴되지 않는다

        if(collision.gameObject.tag == "Zombie")
        {
            Destroy(gameObject);
            //자기 자신을 삭제
            var zombiemover = collision.gameObject.GetComponent<ZombieMover>();
            //ZombieMover 소스에 접근
            zombiemover.Hurt(damage);
            //좀비에게 피해를 줌
            Instantiate(explosion, collision.transform.position, collision.transform.rotation);
            //폭발 이펙트 생성
        }
        //좀비와 부딪치면
    }
    //콜라이더 접촉 시 출력
}
