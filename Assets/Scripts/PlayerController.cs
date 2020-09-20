using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
//오디오소스
public class PlayerController : MonoBehaviour
{
    public float speed;
    //속력
    public float xMin, xMax;
    //갈 수 있는 최소 최대 범위
    public float hp;
    //생명력
    public float maxHp = 10;
    //최대 생명력
    public GameObject[] shot;
    //발사되는 총알
    public Transform[] shotSpawn;
    //총알의 방향
    public float[] fireRate;
    //1발 쏜 후 차탄 발사에 걸리는 시간
    private float[] coolDown;
    //1회 발사 후 재장전 시간

    public AudioClip[] playerSound;
    //재생할 오디오
    public AudioClip[] damageSound;
    //피격 시 내는 소리
    private AudioSource audioSource;
    //스피커
    private Rigidbody2D rb;
    //물리 계산 변수 선언
    public Slider hpBar;
    //HP바
    public Text hpText;
    //HP를 문자로 표시

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //물리 계산
        hp = maxHp;
        //현재 HP는 최대 HP이다
        audioSource = GetComponent<AudioSource>();
        //오디오 재생
        coolDown = new float[] { 0.5f, 1f };
        //재장전 시간 값 할당
    }
    //게임이 시작할 시

    private void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        //좌우 방향키 사용
        Vector3 movement = new Vector3(moveHorizontal, -4.0f, 0.0f);
        //플레이어 배치
        rb.velocity = movement * speed;
        //이동속도 관할

        rb.position = new Vector3(
            Mathf.Clamp(rb.position.x, xMin, xMax),
            -4.0f,
            0.0f);
        //위치 재계산
    }

    private void Update()
    {
        if(hp > 0)
        {
            if (Input.GetKey(KeyCode.Z) && Time.time > coolDown[0])
            {
                coolDown[0] = Time.time + fireRate[0];
                //쿨다운 값은 시간에 차탄 발사에 걸리는 시간을 더한다
                Instantiate(shot[0], shotSpawn[0].position, shotSpawn[0].rotation);
                //돌격소총 총알 소환
                audioSource.PlayOneShot(playerSound[0], 1f);
                //발사 소리 재생
                Debug.Log(gameObject);
                //총알을 발사할 때마다 로그 값 출력
            }
            //Z키를 누르는 동안과 현재 시간이 재장전 시간보다 큰 값을 갖는 동안 돌격 소총 사격
            if (Input.GetKeyDown(KeyCode.X) && Time.time > coolDown[1])
            {
                coolDown[1] = Time.time + fireRate[1];
                Instantiate(shot[1], shotSpawn[1].position, shotSpawn[1].rotation);
                audioSource.PlayOneShot(playerSound[1], 1f);
            }
            //X키를 1회 누르고 현재 시간이 재장전 시간보다 큰 값을 갖는 동안 저격 소총 사격
        }
        //HP가 0보다 높을 때
    }
    //공격 관할

    public void Hurt(float crashDamage)
    {
        if (hp > 0)
        {
            hp -= crashDamage;
            //HP 감소
            hpBar.value = hp / maxHp;
            //HP바 UI 표시
            hpText.text = "HP : " + hp + " / " + maxHp;
            //HP바 글씨 표시
            audioSource.PlayOneShot(damageSound[0], 1f);
            //피격 시 소리 재생
        }
        //생명력이 0보다 높을 때
        
        if (hp <= 0)
        {
            speed = 0;
            //이동 불가능
            audioSource.PlayOneShot(damageSound[1], 1f);
            //사망 소리 재생
            Destroy(gameObject);
            //자기 자신을 삭제

            var gc = GameObject.FindWithTag("GameController").GetComponent<GameController>();
            //GameController에 접근
            gc.GameOver();
            //접근 이후 게임오버 명령 실행
        }
        //생명력이 0보다 낮거나 같을 때
    }
    //플레이어가 피격될 시 좀비의 충돌 피해값을 참조해 실행한다
}
