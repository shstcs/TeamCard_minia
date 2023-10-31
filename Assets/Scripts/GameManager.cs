using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    public Text TimeText;       //??????
    public Text matText;        //????
    public Image failImage;
    public GameObject Card;     
    float time = 0;             //???? ????
    public static GameManager I;
    public GameObject firstCard;        //???? ?????? ???? ???? ????
    public GameObject secondCard;
    public GameObject endpanel;
    public Text scoreText;
    public AudioSource audioSource;
    public AudioClip match;
    public AudioClip wrong;
    int matchTimes = 0;

    private string[] names = { "±èÅÂÇü", "¼º¿¬È£", "¹ÚÁØÇü", "ÀÌÁ¤¼®", "±èµ¿Çö" };
    private void Awake()
    {
        I = this;
    }
    void Start()
    {
        Time.timeScale = 1.0f;
        int[] rtans = {0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,7};    
        rtans = rtans.OrderBy(item => Guid.NewGuid()).ToArray();    //???????? ????
        for (int i = 0; i < 4; i++)         //4x4?? ?????? ???????? ??????
        {
            for(int j = 0; j < 4; j++)
            {
                GameObject newCard = Instantiate(Card);
                newCard.transform.parent = GameObject.Find("Cards").transform;  //card?? cards?????? ????
                newCard.GetComponent<Card>().Number = rtans[i * 4 + j];

                float x = i * 1.4f - 2.1f;
                float y = j * 1.4f - 3.0f;
                newCard.transform.position = new Vector3(x, y, 0);

                int count = rtans[i * 4 + j] + 1;
                string fileName = "Cards/";
                fileName += "Card" + count.ToString("D2");
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite 
                = Resources.Load<Sprite>(fileName);     //???? ?????? ???? ????????
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        TimeText.text = time.ToString("N2");
        if(time > 30.0f)        //30?? ?? ????????
        {
            gameOver();
        }

    }

    public void isMatched()     //???? ???? ?????????? ???????? ????
    {
        matchTimes++;
        //???? ???????????? ???? ?????? ?????? ?????? ????.
        string firstImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name; 
        string secondImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;

        if (firstImage != secondImage)
        {
            audioSource.PlayOneShot(wrong);
        }

        if (firstImage == secondImage)      //????????
        {
            audioSource.PlayOneShot(match);

            firstCard.GetComponent<Card>().destroyCard();   //???? ????
            secondCard.GetComponent<Card>().destroyCard();
            StartCoroutine(MatTextActive(firstImage == secondImage, names[firstCard.GetComponent<Card>().Number%5]));

        } else                                //??????????
        {
            firstCard.GetComponent<Card>().closeCard();    //???? ???? ??????
            secondCard.GetComponent<Card>().closeCard();
            StartCoroutine(MatTextActive(firstImage == secondImage));
        }
        firstCard = null;
        secondCard = null;

        //Cards?? ???? ?? ????
        int cardsLeft = GameObject.Find("Cards").transform.childCount;
        if (cardsLeft <= 2)     //???? ?? ???????? ????????. ???? ?????????? ???? 2?? ????.
        {
            gameOver();
        }
    }
    public void gameOver()  //???????? ????. ???? ?????? ???? ??????.
    {
        matText.gameObject.SetActive(false);
        failImage.gameObject.SetActive(false);
        scoreText.text = "¸ÅÄª È½¼ö : " + matchTimes + "È¸";
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
