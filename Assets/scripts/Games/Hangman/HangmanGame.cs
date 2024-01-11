using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;
using System.Text;

public class HangmanGame : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI wordText, letters;
    [SerializeField] Transform hangman, player, game, winUI;
    [SerializeField] GameObject sign, mainUI;

    string[] words;
    string word, guess, lettersLeft;
    string alphabet = "A B C D E F G H I J K L M N O P Q R S T U V W X Y Z";
    int life = 0;
    // Start is called before the first frame update
    void Start()
    {
        Refresh();
    }

    private void OnEnable()
    {
        Refresh();
    }

    void OnGUI()
    {

        Event e = Event.current;

        if (e.type == EventType.KeyDown &&
            e.keyCode.ToString().Length == 1 &&
            char.IsLetter(e.keyCode.ToString()[0]))
        {
            checkLetter(e.keyCode.ToString().ToUpper());
        }

    }

    void Refresh()
    {
        getWords();
        changeWord();
    }

    void getWords()
    {
        TextAsset mytxtData = (TextAsset)Resources.Load("HangmanWords");
        string all = mytxtData.text;
        words = all.Split('\n');
    }

    void changeWord()
    {
        word = words[Random.Range(0,words.Length)].ToUpper();
        guess = "";
        for (int i = 0; i<word.Length-1; i++)
        {
            if (i > 0)
            {
                guess += " ";
            }
            guess += "_";
        }
        wordText.text = guess;
        lettersLeft = alphabet;
        letters.text = lettersLeft;
        life = 0;

        for (int i=0; i<hangman.childCount; i++)
        {
            hangman.GetChild(i).gameObject.SetActive(false);
        }
    }

    void checkLetter(string c)
    {
        if (word.Contains(c) && lettersLeft.Contains(c))
        {
            fillLetter(c);
        }else if ((!word.Contains(c)) && lettersLeft.Contains(c))
        {
            removeLetter(c);
            loseLife();
        }
    }

    void fillLetter(string c)
    {
        StringBuilder sb = new StringBuilder(guess);
        for(int i=0; i< word.Length; i++)
        {
            char ch = word[i];
            if (ch.ToString().ToUpper().Equals(c))
            {
                sb[i * 2] = ch;
            }
        }
        guess = sb.ToString();
        wordText.text = guess;
        removeLetter(c);

        if (!guess.Contains("_"))
        {
            win();
        }
    }

    void removeLetter(string c)
    {
        StringBuilder sb = new StringBuilder(lettersLeft);
        for (int i = 0; i < lettersLeft.Length; i++)
        {
            char ch = lettersLeft[i];
            if (ch.ToString().ToUpper().Equals(c))
            {
                sb[i] = ' ';
            }
        }
        lettersLeft = sb.ToString();
        letters.text = lettersLeft;
    }

    void loseLife()
    {
        hangman.GetChild(life).gameObject.SetActive(true);
        life++;
        if (life >= hangman.childCount)
        {
            lose(true);
        }
    }

    void lose(bool loss)
    {
        player.transform.Find("Student").transform.Find("Main Camera").gameObject.SetActive(true);
        player.transform.Find("UICanvas").transform.Find("Main").gameObject.SetActive(true);
        player.transform.Find("UICanvas").transform.Find("Hangman").gameObject.SetActive(false);
        PlayerController controller = player.GetComponent<PlayerController>();
        controller.SetActive(true);
        PlayerInteracter interacter = player.GetComponent<PlayerInteracter>();
        interacter.enabled = true;
        game.Find("Camera").gameObject.SetActive(false);
        game.GetComponent<HangmanInteractable>().activateCollider();
        if (loss)
        {
            winUI.gameObject.SetActive(true);
            winUI.GetComponent<GameOutcome>().lose();
        }
        sign.SetActive(true);
        mainUI.SetActive(true);
    }

    void win()
    {
        GetComponent<PuzzelAppear>().appear();
        lose(false);
    }
}
