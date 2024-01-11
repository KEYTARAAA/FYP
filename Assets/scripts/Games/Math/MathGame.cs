using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MathGame : MonoBehaviour
{
    enum SIGN {PLUS, MINUS, MULTIPLY, DIVIDE };
    List<SIGN> signs = new List<SIGN>() { SIGN.PLUS, SIGN.MINUS, SIGN.MULTIPLY, SIGN.DIVIDE };
    SIGN sign;
    int a, b, c;
    string d;
    bool refresh, change;

    [SerializeField] TextMeshProUGUI text;
    [SerializeField] TextMeshProUGUI score;
    // Start is called before the first frame update
    void Start()
    {
        refresh = true;
        change = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (checkAnswer())
            {
                refresh = true;
                GetComponent<MathManager>().setCurrent(GetComponent<MathManager>().getCurrent() + 1);
            }
            else
            {
                text.color = Color.red;
            }
        }
        if (refresh)
        {
            Refresh();
        }
        if (change)
        {
            Change();
        }

        if (isNumber())
        {
            d += KeyToString();
            change = true;
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            if (d.Length >0) 
            {
                d = d.Remove(d.Length - 1, 1);
                change = true;
            }
        }
    }

    void getSign()
    {
        sign = signs[Random.Range(0, signs.Count)];
    }

    bool checkAnswer()
    {
        if (!d.Equals(""))
            {
            if (int.Parse(d) == c)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void Refresh()
    {
        refresh = false;
        getSign();
        a = Random.Range(0,11);
        b = Random.Range(0,11);

        if (sign == SIGN.PLUS)
        {
            c = a + b;
        }
        else if (sign == SIGN.MINUS)
        {
            if (b>a)
            {
                int temp = a;
                a = b;
                b = temp;
            }
            c = a - b;
        }
        else if (sign == SIGN.MULTIPLY)
        {
            c = a * b;
        }
        else if (sign == SIGN.DIVIDE)
        {
            if (b==0)
            {
                b = Random.Range(1, 11);
            }
            c = a;
            a = b * c;
        }

        d = "";
        change = true;
        text.color = Color.black;
    }

    void Change()
    {
        text.text = a.ToString() + " " + signToString() + " " + b.ToString() + " = " + d;
        change = false;
    }

    string signToString()
    {
        switch (sign)
        {
            default: return "";
            case (SIGN.PLUS): return "+";
            case (SIGN.MINUS): return "-";
            case (SIGN.MULTIPLY): return "x";
            case (SIGN.DIVIDE): return "/";
        }
    }
    bool isNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || 
            Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Alpha6) || 
            Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Alpha9) || 
            Input.GetKeyDown(KeyCode.Alpha0) ||
            Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Keypad3) ||
            Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Keypad6) ||
            Input.GetKeyDown(KeyCode.Keypad7) || Input.GetKeyDown(KeyCode.Keypad8) || Input.GetKeyDown(KeyCode.Keypad9) ||
            Input.GetKeyDown(KeyCode.Keypad0))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    string KeyToString()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
        {
            return "1";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
        {
            return "2";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
        {
            return "3";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
        {
            return "4";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
        {
            return "5";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
        {
            return "6";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
        {
            return "7";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
        {
            return "8";
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
        {
            return "9";
        }
        else
        {
            return "0";
        }
    }


}
