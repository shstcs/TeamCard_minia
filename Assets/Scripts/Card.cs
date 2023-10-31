using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public Animator cardAnim;
    public AudioClip flip;
    public AudioSource audioSource;
    public Color flippedColor;             // 뒤집힌 상태의 색상
    private bool isFlipped = false;        // 카드가 뒤집힌 상태 여부

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
            // 카드가 한 번 이상 뒤집힌 경우에만 색상 변경
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
        // 카드의 뒷면 스프라이트의 색상 변경
        SpriteRenderer backSprite = transform.Find("back").GetComponent<SpriteRenderer>();
        backSprite.color = newColor;

    }
}
