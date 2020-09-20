using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public GameObject[] zombieType;
    //나오는 좀비의 종류
    public Vector3 spawnValues;
    //좀비 생성위치
    public int spawnCount;
    //좀비가 한 웨이브에 몇 명씩 나오는가?
    public float spawnWait;
    //좀비가 몇 초 간격으로 생성되는가?
    public float startWait;
    //게임이 시작했을 때 좀비가 몇 초 지나고 나오는가?
    public float waveWait;
    //다음 웨이브로 넘어갈 때 몇 초가 걸리는가?
    private AudioSource audioSource;
    //오디오소스 변수

    private int score;
    //점수 계산
    private bool gameOver;
    //게임오버 상태인지 묻는 변수
    public Text scoreText;
    //점수 UI
    private int yourScore;
    //네가 획득한 점수
    public GameObject gameOverGUI;
    //게임오버 화면
    
    public Text yourScoreLabel;
    //네가 획득한 점수 표시
    
    private void Start()
    {
        gameOver = false;
        //게임오버 상태가 아님
        score = 0;
        //점수는 0점으로 초기화
        yourScore = 0;
        //네가 획득한 점수도 초기화
        
        audioSource = GetComponent<AudioSource>();
        //오디오 재생

        StartCoroutine(SpawnWaves());
        //좀비 소환 명령
        UpdateScore();
        //점수 업데이트
    }
    //게임 시작 시

    IEnumerator SpawnWaves()
    {
        if(gameOver != true)
        {
            yield return new WaitForSeconds(startWait);
            while (true)
            {
                for (int i = 0; i < spawnCount; i++)
                {
                    Vector3 spawnPosition = new Vector3(
                        UnityEngine.Random.Range(-spawnValues.x, spawnValues.x),
                        spawnValues.y,
                        spawnValues.z);
                    //소행성 위치 설정, x축은 무작위

                    Quaternion spawnRotation = Quaternion.identity;
                    //방향값 계산
                    int rnd = UnityEngine.Random.Range(0, zombieType.Length);
                    //인덱스 번호가 0부터 zombieType의 최대 인덱스 값까지 출력
                    Instantiate(zombieType[rnd], spawnPosition, spawnRotation);
                    //좀비 소환
                    yield return new WaitForSeconds(spawnWait);
                    //spawnWait가 지나고 밑의 명령 실행
                    spawnWait *= 0.9f;
                    //좀비 생성 간격 계산

                    if (gameOver == true)
                    { 
                        break;
                    }
                    //게임오버 될 시 게임 중단
                }
                yield return new WaitForSeconds(waveWait);
                //모두 다 출력시키고 지연
            }
            //중괄호 안의 내용을 무한 반복
        }
        //게임오버 상태가 아닐 시
    }
    //게임 시작 시 자동 반복

    public void GameOver()
    {
        if(gameOver != true)
        {
            audioSource.Stop();
            //오디오 재생 중단
            gameOver = true;
            //게임오버 상태임
            yourScore = score;
            //너의 점수는 게임에서 획득한 점수다
            yourScoreLabel.text = string.Format("Your Score : {0}", yourScore);
            //점수 UI 표시
            gameOverGUI.SetActive(true);
            //게임오버 창을 띄움
        }
        //게임오버 상태일 시
    }

    public void GoMain()
    {
        SceneManager.LoadScene("_Title");
        //타이틀 화면으로 이동
    }
    //게임오버 창 닫기 버튼을 누를 시

    public void AddScore(int scoreValue)
    {
        score += scoreValue;
        //잡은 좀비가 주는 점수를 더함
        UpdateScore();
        //점수 UI 실행
    }
    //점수를 추가 획득

    public void UpdateScore()
    {
        scoreText.text = "Score : " + score; 
        //점수 UI 표시
    }
    //점수 추가를 표시
}
