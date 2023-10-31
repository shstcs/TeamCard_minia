using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Animator cardAnim;
    public AudioClip flip;
    public AudioSource audioSource;
    public Color flippedColor;             // ������ ������ ����
    private bool isFlipped = false;        // ī�尡 ������ ���� ����

    public int Number;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openCard()
    {
        audioSource.PlayOneShot(flip);
        if (GameManager.I.firstCard == null)
        {
            GameManager.I.firstCard = gameObject;
        }
        else
        {
            GameManager.I.secondCard = gameObject;
            GameManager.I.isMatched();
        }
        
        transform.Find("front").gameObject.SetActive(true);
        transform.Find("back").gameObject.SetActive(false);
        if (!isFlipped)
        {
            // ī�尡 �� �� �̻� ������ ��쿡�� ���� ����
            isFlipped = true;
            ChangeCardColor(Color.gray);
        }
        cardAnim.SetBool("isOpen", true);
    }
    public void destroyCard()
    {
        Invoke("destroyCardInvoke", 0.5f);
    }

    void destroyCardInvoke()
    {
        Destroy(gameObject);
    }

    public void closeCard()
    {
        Invoke("closeCardInvoke", 0.5f);
    }

    void closeCardInvoke()
    {
        cardAnim.SetBool("isOpen", false);
        transform.Find("back").gameObject.SetActive(true);
        transform.Find("front").gameObject.SetActive(false);
    }
    private void ChangeCardColor(Color newColor)
    {
        // ī���� �޸� ��������Ʈ�� ���� ����
        SpriteRenderer backSprite = transform.Find("back").GetComponent<SpriteRenderer>();
        backSprite.color = newColor;

    }
}
