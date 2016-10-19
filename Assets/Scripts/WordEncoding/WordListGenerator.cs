using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public class WordListGenerator : MonoBehaviour {

    public string fileName = "RAM_wordpool";
    private string[] words;
    private static string[] finalList=new string[300];
    public static bool canSelectWords = false;

    List<string> tempWords=new List<string>();
    List<string> selectableWords = new List<string>();
    
	public WordEncodingLogTrack wordEncodingLogTrack;
    //SINGLETON
    private static WordListGenerator _instance;

    public static WordListGenerator Instance
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        if (_instance != null)
        {
            Debug.Log("Instance already exists!");
            return;
        }
        _instance = this;

        QualitySettings.vSyncCount = 1;
    }

        void Start () {

	}

    public string SelectWords()
    {
        int selectedWordIndex = Random.Range(0, selectableWords.Count);
        string selectedWord= selectableWords[selectedWordIndex];
        selectableWords.RemoveAt(selectedWordIndex);
        return selectedWord;
    } 

    public IEnumerator GenerateWordList()
    {
        yield return StartCoroutine("ReadWordpoolFile");
        yield return StartCoroutine("ShuffleWords");
        canSelectWords = true;
       // BillboardText.InitiateWord();
        //yield return StartCoroutine("CreateSelectableWordList");
        yield return null;
        
    }

    IEnumerator ReadWordpoolFile()
    {
        words = System.IO.File.ReadAllLines(fileName);       
       // PrintAllWords();
        
        yield return null;
    }

    List<string> AddWordsToList(string[] wordArray)
    {
        List<string> temp=new List<string>();
        for(int i=0;i<wordArray.Length;i++)
        {
            temp.Add(wordArray[i]); 
        }
        UnityEngine.Debug.Log("The word array length is: " + wordArray.Length);
        return temp;
    }

    IEnumerator ShuffleWords()
    {
        
        tempWords=AddWordsToList(words);
        UnityEngine.Debug.Log("The temp words count is " + tempWords.Count);
        while (tempWords.Count > 0)
        {
         //   UnityEngine.Debug.Log("in here");
           // UnityEngine.Debug.Log("the finalList length is: " + finalList.Length);
            int randWord = Random.Range(0, tempWords.Count);
            selectableWords.Add(tempWords[randWord]);
            tempWords.RemoveAt(randWord);
        }
        yield return 0;
    }
/*
    IEnumerator CreateSelectableWordList()
    {
        for(int i=0;i<finalList.Length;i++)
        {
            selectableWords.Add(finalList[i]);
        }

        yield return null;
    }
	*/
    void PrintAllWords()
    {
        if (words != null)
        {
            for (int i = 0; i < words.Length; i++)
            {
                UnityEngine.Debug.Log(words[i]);
            }
        }
    }
	// Update is called once per frame
	void Update () {
	
	}
}
