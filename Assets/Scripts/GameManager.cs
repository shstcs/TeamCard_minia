using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public Text TimeText;       //타이머
    public Text matText;        //매칭
    public Image failImage;
    public GameObject Card;     
    float time = 0;             //흐른 시간
    public static GameManager I;
    public GameObject firstCard;        //그림 비교를 위한 카드 두장
    public GameObject secondCard;
    public GameObject endpanel;
    public Text scoreText;
    public AudioSource audioSource;
    public AudioClip match;
    int matchTimes = 0;

    private string[] names = { "김태형", "성연호", "박준형", "이정석", "김동현" };
    private void Awake()
    {
        I = this;
    }
    void Start()
    {
        Time.timeScale = 1.0f;
        int[] rtans = {0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,7};    
        rtans = rtans.OrderBy(item => Guid.NewGuid()).ToArray();    //랜덤으로 정렬
        for (int i = 0; i < 4; i++)         //4x4로 카드를 배치하는 반복문
        {
            for(int j = 0; j < 4; j++)
            {
                GameObject newCard = Instantiate(Card);
                newCard.transform.parent = GameObject.Find("Cards").transform;  //card를 cards아래에 배치
                newCard.GetComponent<Card>().Number = rtans[i * 4 + j];

                float x = i * 1.4f - 2.1f;
                float y = j * 1.4f - 3.0f;
                newCard.transform.position = new Vector3(x, y, 0);

                int count = rtans[i * 4 + j] + 1;
                string fileName = "Cards/";
                fileName += "Card" + count.ToString("D2");
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite 
                = Resources.Load<Sprite>(fileName);     //사진 이름에 맞게 붙여넣기
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        TimeText.text = time.ToString("N2");
        if(time > 30.0f)        //30초 후 게임오버
        {
            gameOver();
        }
        if(time > 15.0f)    //15초 후 타이머 색깔 변경
        {
            TimeText.text = "<color=red>" + (string)TimeText.text + "</color>";
           
        }

    }

    public void isMatched()     //카드 그림 일치하는지 확인하는 함수
    {
        matchTimes++;
        //카드 오브젝트에서 그림 이미지 이름만 빼와서 저장.
        string firstImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name; 
        string secondImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        if (firstImage == secondImage)      //일치하면
        {
            audioSource.PlayOneShot(match);

            firstCard.GetComponent<Card>().destroyCard();   //카드 제거
            secondCard.GetComponent<Card>().destroyCard();
            StartCoroutine(MatTextActive(firstImage == secondImage, names[firstCard.GetComponent<Card>().Number%5]));

        } else                                //불일치하면
        {
            firstCard.GetComponent<Card>().closeCard();    //카드 다시 뒤집기
            secondCard.GetComponent<Card>().closeCard();
            StartCoroutine(MatTextActive(firstImage == secondImage));
        }
        firstCard = null;
        secondCard = null;

        //Cards의 자식 수 확인
        int cardsLeft = GameObject.Find("Cards").transform.childCount;
        if (cardsLeft <= 2)     //카드 다 사라지면 게임오버. 현재 보는것까지 해서 2가 최소.
        {
            gameOver();
        }
    }
    public void gameOver()  //게임오버 함수. 시간 멈추고 글자 띄우기.
    {
        matText.gameObject.SetActive(false);
        failImage.gameObject.SetActive(false);
        scoreText.text = "매칭 횟수 : " + matchTimes + "회";
        endpanel.SetActive(true);
        Time.timeScale = 0;
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
}
