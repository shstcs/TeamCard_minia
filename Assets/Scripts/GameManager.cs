using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

public class GameManager : MonoBehaviour
{
    public Text TimeText;       //Ÿ�̸�
    public GameObject Card;     
    float time = 0;             //�帥 �ð�
    public static GameManager I;
    public GameObject firstCard;        //�׸� �񱳸� ���� ī�� ����
    public GameObject secondCard;
    public GameObject endpanel;
    public AudioSource audioSource;
    public AudioClip match;
    public Text scoreText;
    int matchTimes = 0;

    private void Awake()
    {
        I = this;
    }
    void Start()
    {
        Time.timeScale = 1.0f;
        int[] rtans = {0,0,1,1,2,2,3,3,4,4,5,5,6,6,7,7};    
        rtans = rtans.OrderBy(item => Guid.NewGuid()).ToArray();    //�������� ����
        for (int i = 0; i < 4; i++)         //4x4�� ī�带 ��ġ�ϴ� �ݺ���
        {
            for(int j = 0; j < 4; j++)
            {
                GameObject newCard = Instantiate(Card);
                newCard.transform.parent = GameObject.Find("Cards").transform;  //card�� cards�Ʒ��� ��ġ

                float x = i * 1.4f - 2.1f;
                float y = j * 1.4f - 3.0f;
                newCard.transform.position = new Vector3(x, y, 0);

                string rtanName = "rtan" + rtans[i*4+j].ToString();
                newCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite 
                = Resources.Load<Sprite>(rtanName);     //���� �̸��� �°� �ٿ��ֱ�
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        TimeText.text = time.ToString("N2");
        if(time > 30.0f)        //30�� �� ���ӿ���
        {
            gameOver();
        }

    }

    public void isMatched()     //ī�� �׸� ��ġ�ϴ��� Ȯ���ϴ� �Լ�
    {
        matchTimes++;
        //ī�� ������Ʈ���� �׸� �̹��� �̸��� ���ͼ� ����.
        string firstImage = firstCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name; 
        string secondImage = secondCard.transform.Find("front").GetComponent<SpriteRenderer>().sprite.name;
        if (firstImage == secondImage)      //��ġ�ϸ�
        {
            audioSource.PlayOneShot(match);

            firstCard.GetComponent<Card>().destroyCard();   //ī�� ����
            secondCard.GetComponent<Card>().destroyCard();
        }
        else                                //����ġ�ϸ�
        {
            firstCard.GetComponent <Card>().closeCard();    //ī�� �ٽ� ������
            secondCard.GetComponent<Card>().closeCard();
        }
        firstCard = null;
        secondCard = null;

        //Cards�� �ڽ� �� Ȯ��
        int cardsLeft = GameObject.Find("Cards").transform.childCount;
        if (cardsLeft <= 2)     //ī�� �� ������� ���ӿ���. ���� ���°ͱ��� �ؼ� 2�� �ּ�.
        {
            gameOver();
        }
    }
    public void gameOver()  //���ӿ��� �Լ�. �ð� ���߰� ���� ����.
    {
        endpanel.SetActive(true);
        Time.timeScale = 0;
        scoreText.text = "��Ī Ƚ�� : " + matchTimes + "ȸ";
    }
}
