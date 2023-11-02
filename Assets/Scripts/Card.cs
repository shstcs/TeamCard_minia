using System;
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
        if ((GameManager.I.firstCard != null &&
            GameManager.I.secondCard != null) ||
            !GameManager.I.IsGameStart)
            return;

        StartCoroutine(CoRoteateFace(true));

        if (GameManager.I.firstCard == null) {
            GameManager.I.firstCard = gameObject;
        } else if (GameManager.I.secondCard == null && 
            GameManager.I.firstCard != gameObject) {
            GameManager.I.secondCard = gameObject;
            StartCoroutine(CoWaitAction(GameManager.I.isMatched));
        }

        if (!isFlipped)
        {
            // 카드가 한 번 이상 뒤집힌 경우에만 색상 변경
            isFlipped = true;
            ChangeCardColor(Color.gray);
        }
        //transform.Find("front").gameObject.SetActive(true);
        //transform.Find("back").gameObject.SetActive(false);
        //cardAnim.SetBool("isOpen", true);
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
        StartCoroutine(CoRoteateFace(false));
        //Invoke("closeCardInvoke", 0.5f);
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

    public void RotateCard(bool faceUp) {
        StartCoroutine(CoRoteateFace(faceUp));
    }

    public IEnumerator CoRoteateFace(bool faceUp) {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.01f);
        audioSource.PlayOneShot(flip);

        for (float i = 0.0f; i <= 90.0f; i += 10.0f) {
            transform.rotation = Quaternion.Euler(0.0f, i, 0.0f);
            yield return waitForSeconds;
        }

        if (faceUp) {
            transform.Find("front").gameObject.SetActive(true);
            transform.Find("back").gameObject.SetActive(false);
        } else {
            transform.Find("back").gameObject.SetActive(true);
            transform.Find("front").gameObject.SetActive(false);
        }

        for (float i = 90.0f; i >= 0.0f; i -= 10.0f) {
            transform.rotation = Quaternion.Euler(0.0f, i, 0.0f);
            yield return waitForSeconds;
        }
    }

    private IEnumerator CoWaitAction(Action action) {
        yield return new WaitForSeconds(0.5f);
        action();
    }
}
