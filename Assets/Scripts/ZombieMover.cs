using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieMover : MonoBehaviour
{
    public float speed;
    //좀비의 이동속도
    public float hp;
    //좀비의 생명력
    public float maxHp;
    //좀비의 최대 생명력
    public float crashDamage;
    //좀비와 충돌했을 때 피해
    public int scoreValue;
    //좀비를 잡을 때 주는 점수
    private Rigidbody2D rb;
    //물리 선언
    private AudioSource audioSource;
    //스피커 선언
    public GameObject explosion;
    //폭발 효과 선언
    public AudioClip[] damageSound;
    //피격 소리 선언
    public Transform player;
    //찾는 대상은 플레이어
    private GameController gameController;
    //GameConrtoller 참조 연결

    private void Start()
    {  
        //플레이어 오브젝트를 찾아 상태 저장
        rb = GetComponent<Rigidbody2D>();
        //물리 구현
        hp = maxHp;
        //현재 HP는 최대 HP로 설정
        audioSource = GetComponent<AudioSource>();
        //오디오 재생
        player = GameObject.FindWithTag("GameController").transform;
        //GameController 태그를 찾고 좀비를 플레이어의 방향으로 돌진

    }

    private void Update()
    {               
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        //좀비의 이동 속도
    }

    public void CrashPlayer()
    {
        var pc = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        //플레이어 태그 감지
        pc.Hurt(crashDamage);
        //pc의 Hurt에 접근해 충돌 피해를 줌
    }
    //좀비가 플레이어와 충돌할 시 피해를 줌

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Zombie")
        {
            return;
        }
        //같은 좀비와 부딪칠 때 파괴되지 않는다
        if (other.tag == "Boundary")
        {
            return;
        }
        //막아둔 벽에 닿았을 때 파괴되지 않는다
        if(other.tag == "GameController")
        {
            return;
        }
        //GameController에 접근해도 파괴되지 않는다
        
        if(other.tag == "ZombieKillzone")
        {
            Destroy(gameObject);
            //자기 자신을 삭제
        }
        //ZombieKillzone에 닿았을 시
        Instantiate(explosion, other.transform.position, other.transform.rotation);
        //폭발 이펙트 생성
        if (other.tag == "Player")
        {
            Instantiate(explosion, other.transform.position, other.transform.rotation);
            //폭발 효과 재생
            CrashPlayer();
            //CrashPlayer 실행
            Destroy(gameObject);
            //자기 자신을 삭제
        }
        //플레이어와 부딪치면 파괴되면서 피해를 준다     
    }
    //다른 개체와 충돌 시 결과

    public void Hurt(float damage)
    {
        if (hp > 0)
        {
            hp -= damage;
            //생명력 감소
            audioSource.PlayOneShot(damageSound[0], 1f);
            //피격 소리 재생
            Debug.Log(damageSound[0]);
            //피격 소리 재생 여부를 로그 값으로 출력
        }
        //HP가 0보가 높을 때
        if (hp <= 0)
        {
            speed = 0;
            //이동 불가능
            Instantiate(explosion, transform.position, transform.rotation);
            //폭발 이펙트 재생
            audioSource.PlayOneShot(damageSound[1], 1f);
            //사망 소리 재생
            Destroy(gameObject);
            //자기 자신을 삭제
            var gc = GameObject.FindWithTag("GameController").gameObject.GetComponent<GameController>();
            //GameController 태그를 찾아 GameController에 접근
            gc.AddScore(scoreValue);
            //점수 추가
        }
        //HP가 0보다 낮거나 같을 때
    }
    //좀비가 피해를 입을 시
}
