using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueBox : MonoBehaviour
{
    [System.Serializable]
    public class tuts
    {
        public string scene;
        public bool prefs;
    }

    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float speed;

    public tuts[] dialogues;

    public bool title = false;
    public bool everyTime = false;

    int index;

    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        
        // PlayerPrefs.SetInt("MapTut", 0);
        // PlayerPrefs.SetInt("TroopTut", 0);
        // PlayerPrefs.SetInt("PowerupTut", 0);
        // PlayerPrefs.SetInt("BattleTut", 0);
        // PlayerPrefs.SetInt("LivesTut", 0);
        
        if (everyTime || GetPrefs()) StartDialogue();
        else if (title) Invoke("StartDialogue", 1f);
        else gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (title) return;
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            } else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        } else gameObject.SetActive(false);
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComponent.text += c;
            yield return new WaitForSeconds(speed);
        }
    }

    bool GetPrefs()
    {
        foreach (tuts t in dialogues)
        {
            string pref = t.scene + "Tut";
            if (t.prefs && PlayerPrefs.GetInt(pref)==0)
            {
                PlayerPrefs.SetInt(pref, 1);
                return true;
            }
        }
        return false;
    }
}
