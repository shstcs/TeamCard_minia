using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Unity.VisualScripting;
using Unity.Mathematics;

public class GameManager : MonoBehaviour
{
    public Text TimeText;       //      
    public Text matText;        //    
    public Image failImage;
    public GameObject Card;     
    float time = 60f;             // 제한시간 60초 설정     
    public static GameManager I;
    public GameObject firstCard;        //                          
    public GameObject secondCard;
    public GameObject endpanel;
    public Text scoreText;
    public AudioSource audioSource;
    public AudioClip match;
    public AudioClip wrong;
    int matchTimes = 0;
    int maxAttempts = 10;      // 10번까지는 점수에 영향을 미치지 않음
    int penaltyPerAttempt = 1; // 11번 이상부터 시도 횟수당 -1점

    public bool IsGameStart { get; private set; }
    private List<GameObject> cards;
    private string[] names = { "김태형", "성연호", "박준형", "이정석", "김동현" };
    private void Awake()
    {
        I = this;
    }
    void Start()
    {
        Time.timeScale = 1.0f;
        IsGameStart = false;
        cards = new List<GameObject>();

        if (PlayerPrefs.GetInt("mode") == 0)
        {
            int[] rtans = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7 };
            rtans = rtans.OrderBy(item => Guid.NewGuid()).ToArray();
            for (int i = 0; i < 4; i++)         //4x4                         
            {
                for (int j = 0; j < 4; j++)
                {
                    GameObject newCard = Instantiate(Card);
                    newCard.transform.parent = GameObject.Find("Cards").transform;
                    newCard.GetComponent<Card>().Number = rtans[i * 4 + j];

                    float x = i * 1.4f - 2.1f;
                    float y = j * 1.4f - 3.0f;
                    newCard.transform.position = new Vector3(x, y, 0);

                    int count = rtans[i * 4 + j] + 1;
                    string fileName = "Cards/";
                    fileName += "Card" + count.ToString("D2");
                    newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<Sprite>(fileName);
                    cards.Add(newCard);
                }
            }
        }

        else if (PlayerPrefs.GetInt("mode") == 1)
        {
            time -= 20f;
            GameObject.Find("Main Camera").GetComponent<Camera>().backgroundColor = Color.black;
            int[] members = { 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10, 10, 11, 11, 12, 12, 13, 13, 14, 14};
            members = members.OrderBy(item => Guid.NewGuid()).ToArray();
            for (int i = 0; i <5; i++)
            {
                for (int j = 0; j < 6; j++)
                {
                    GameObject newCard = Instantiate(Card);
                    newCard.transform.parent = GameObject.Find("Cards").transform;
                    newCard.GetComponent<Card>().Number = members[i * 6 + j];

                    float x = i * 1.0f - 2.1f;
                    float y = j * 1.0f - 3.0f;
                    newCard.transform.position = new Vector3(x, y, 0);
                    newCard.transform.localScale = new Vector3(0.8f, 0.8f, 1);

                    int count = members[i * 6 + j] + 1;
                    string fileName = "Cards/";
                    fileName += "Card" + count.ToString("D2");
                    newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite
                    = Resources.Load<Sprite>(fileName);
                    cards.Add(newCard);
                }
            }
        }

        StartCoroutine(StartCardRotate());
        //StartCoroutine(StartCardRotate2());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGameStart) {
            time -= Time.deltaTime;
            TimeText.text = time.ToString("N2");
        }

        if (time < 0f)                 
        {
            gameOver();
        }
        if(time < 15.0f)    //15초 남았을 때 타이머 색깔 변경
        {
            TimeText.text = "<color=red>" + (string)TimeText.text + "</color>";
           
        }

    }

    public void isMatched()                                    
    {
        matchTimes++;
                                                      
        string firstImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name; 
        string secondImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstImage != secondImage)
        {
            audioSource.PlayOneShot(wrong);

            time -= 2.0f;       //카드 불일치시 시간 2초씩 감소
            if (time < 0f)   //시간이 음수로 가지 않게 설정
            {
                time = 0f;
            }
        }

        if (firstImage == secondImage)            
        {
            audioSource.PlayOneShot(match);

            firstCard.GetComponent<Card>().destroyCard();        
            secondCard.GetComponent<Card>().destroyCard();
            StartCoroutine(MatTextActive(firstImage == secondImage, names[firstCard.GetComponent<Card>().Number%5]));
            time += 3.0f;       //카드 일치시 시간 3초씩 증가
           

        } else                                      
        {
            firstCard.GetComponent<Card>().closeCard();                  
            secondCard.GetComponent<Card>().closeCard();
            StartCoroutine(MatTextActive(firstImage == secondImage));

         
        }
        firstCard = null;
        secondCard = null;

        //Cards               
        int cardsLeft = GameObject.Find("Cards").transform.childCount;
        if (cardsLeft <= 2)     
        {
            gameOver();
        }
    }

    public void gameOver()  
    {
        matText.gameObject.SetActive(false);
        failImage.gameObject.SetActive(false);
        
        // 남은 시간 계산
        float remainingTime = Mathf.Max(0, time);

        // 시도 횟수에 따른 점수 계산
        int attemptsScore = 0;
        if (matchTimes > maxAttempts)
        {
            attemptsScore = (matchTimes - maxAttempts) * (-penaltyPerAttempt);
        }

        // 남은 시간 보너스 내림하여 정수로 계산
        int timeBonus = Mathf.FloorToInt(remainingTime * 10);

        // 전체 점수 계산
        int totalScore = timeBonus + attemptsScore;

        // 점수를 scoreText에 표시
        scoreText.text = "매칭 횟수: " + matchTimes + "회\n" +
                         "남은 시간 보너스: " + timeBonus + "점\n" +
                         "<color=red>시도 횟수 패널티: " + attemptsScore + "점</color>\n" +
                         "<size=100>총 점수: " + totalScore + "점</size>";
        endpanel.SetActive(true);
        Time.timeScale = 0;
    }
    private int CalculateScore()
    {
        // 남은 시간에 따른 점수 계산
        float remainingTime = time;
        int timeScore = Mathf.RoundToInt(remainingTime * 10); // 1초당 +10점

        // 매칭 시도 횟수에 따른 점수 계산
        int attemptsScore = Mathf.Max(0, matchTimes - maxAttempts) * (-penaltyPerAttempt);

        // 전체 점수 계산
        int totalScore = timeScore + attemptsScore;

        return totalScore;
    }

    private IEnumerator MatTextActive(bool isMat, string text ="") {
        if (isMat) {
            matText.text = text;
            matText.gameObject.SetActive(true);
        } else {
            failImage.gameObject.SetActive(true);
        }
        
        yield return new WaitForSeconds(0.5f);
        matText.gameObject.SetActive(false);
        failImage.gameObject.SetActive(false);
    }

    private IEnumerator StartCardRotate() {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.4f);
        for (int i = 0; i < cards.Count; ++i) {
            yield return StartCoroutine(cards[i].GetComponent<Card>().CoRoteateFace(true));
            cards[i].GetComponent<Card>().RotateCard(false);
        }

        IsGameStart = true;
    }

    private IEnumerator StartCardRotate2() {
        for (int i = 0; i < cards.Count; ++i) {
            cards[i].GetComponent<Card>().RotateCard(true);
        }

        yield return new WaitForSeconds(3.0f);

        for (int i = 0; i < cards.Count; ++i) {
            cards[i].GetComponent<Card>().RotateCard(false);
        }

        IsGameStart = true;
    }
}
