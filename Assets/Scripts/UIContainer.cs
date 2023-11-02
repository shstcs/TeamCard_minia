using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIContainer : MonoBehaviour
{
    public Text resultTxt;       //      
    public Text matTxt;       //      
    public Text timebonusText;       //      
    public Text matpenaltyText;       //      
    public Text totalScoreText;       //      
    public Text bestScoreText;       //      
    public GameObject endPanel;       //      

    public static UIContainer I;

    private void Awake() {
        I = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValue(int mat, int timebonus, int penalty, int total, int best) {
        int cardsLeft = GameObject.Find("Cards").transform.childCount;
        if (cardsLeft <= 2) {
            resultTxt.text = "Success";
            resultTxt.color = new Color(0x2B / 255f, 0xB3 / 255f, 0xC9 / 255f);
        } else {
            resultTxt.text = "Fail";
            resultTxt.color = new Color(0xEB / 255f, 0x46 / 255f, 0x4A / 255f);
        }
        matTxt.text = mat.ToString();      
        timebonusText.text = timebonus.ToString();
        matpenaltyText.text = penalty.ToString();
        totalScoreText.text = total.ToString();
        bestScoreText.text = best.ToString();
    }
}
