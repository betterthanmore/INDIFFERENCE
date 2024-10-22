using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class DialogueManager : MonoBehaviour, IInteractable
{
    public Text characterNameText;  
    public Text dialogueText;
    public GameObject dialogueUI;
    public GameObject choiceUI;  
    public List<Button> choiceButtons;  

    public float dialogueSpeed = 0.05f; 
    private List<Dialogue> dialogues = new List<Dialogue>();  // 대화 리스트
    private int currentDialogueIndex = 0;  
    private bool isTyping = false;  
    private bool isDialogueFast = false;  
    private float autoNextTime = 10f;  
    private Coroutine typingCoroutine;

    public bool isDialogueActive = false;

    private PlayerController playerController;

    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }
    public void Interact()
    {
        if (!isDialogueActive)
        {
            dialogueUI.SetActive(true);
            StartDialogue();
            playerController.canMove = false;  // 플레이어 이동 비활성화
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && !isTyping)
        {
            NextDialogue();
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isDialogueFast = true;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isDialogueFast = false;
        }
    }
    // CSV 파일에서 대사와 캐릭터 이름 불러오는 함수
    void LoadDialogueFromCSV(string filePath)
    {
        StreamReader reader = new StreamReader(filePath);
        dialogues.Clear();
        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',');

            string characterName = values[0];
            string dialogueText = values[1];

            // 선택지 유무 확인한 후에 있으면 가져오는 함수
            List<DialogueChoice> choices = new List<DialogueChoice>();
            if (!string.IsNullOrEmpty(values[2]))  // 선택지1
            {
                choices.Add(new DialogueChoice(values[2], int.Parse(values[3])));
            }
            if (!string.IsNullOrEmpty(values[4]))  // 선택지2
            {
                choices.Add(new DialogueChoice(values[4], int.Parse(values[5])));
            }

            // 선택지가 있는 경우, choices 리스트를 포함한 Dialogue 추가
            dialogues.Add(new Dialogue(characterName, dialogueText, choices.Count > 0 ? choices : null));
        }
        reader.Close();
    }
    // 대화 파일 로드
    public void LoadDialogueFile(string fileName)
    {
        string filePath = "Assets/Dialogues/" + fileName + ".csv";
        LoadDialogueFromCSV(filePath);
        StartDialogue();
    }

    // 대화 시작
    void StartDialogue()
    {
        if (dialogues.Count > 0)
        {
            currentDialogueIndex = 0;
            typingCoroutine = StartCoroutine(TypeDialogue(dialogues[currentDialogueIndex]));
        }
    }
    // 다음 대사로 넘어가기
    void NextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            currentDialogueIndex++;
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeDialogue(dialogues[currentDialogueIndex]));
        }
    }
    // 대화 내용을 한 글자씩 출력하는 코루틴
    IEnumerator TypeDialogue(Dialogue dialogue)
    {
        isTyping = true;
        characterNameText.text = dialogue.characterName;
        dialogueText.text = ""; 

        foreach (char letter in dialogue.dialogueText.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(isDialogueFast ? dialogueSpeed / 2 : dialogueSpeed);
        }

        isTyping = false;
        // 대화가 끝나면 선택지가 있는지 확인하고 출력
        if (dialogue.choices != null && dialogue.choices.Count > 0)
        {
            ShowChoices(dialogue.choices);
        }
        else
        {
            yield return new WaitForSeconds(autoNextTime);
            NextDialogue();
        }
    }

    // 선택지 표시
    void ShowChoices(List<DialogueChoice> choices)
    {
        choiceUI.SetActive(true);

        for (int i = 0; i < choices.Count; i++)
        {
            choiceButtons[i].gameObject.SetActive(true);
            choiceButtons[i].GetComponentInChildren<Text>().text = choices[i].choiceText;
            int nextIndex = choices[i].nextDialogueIndex;
            choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(nextIndex));
        }
    }

    // 선택지 선택 시 처리
    void OnChoiceSelected(int nextIndex)
    {
        choiceUI.SetActive(false);
        foreach (Button button in choiceButtons)
        {
            button.gameObject.SetActive(false); 
            button.onClick.RemoveAllListeners();
        }

        currentDialogueIndex = nextIndex;
        StartDialogue();
    }

    // 대화 종료 시 호출
    void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false);
        playerController.canMove = true;  // 플레이어 이동 활성화

    }
}

// 캐릭터 이름과 대사를 담을 클래스
public class Dialogue
{
    public string characterName;
    public string dialogueText;
    public List<DialogueChoice> choices;

    public Dialogue(string name, string dialogue, List<DialogueChoice> choices = null)
    {
        characterName = name;
        dialogueText = dialogue;
        this.choices = choices;
    }
}

// 선택지 데이터를 담을 클래스
public class DialogueChoice
{
    public string choiceText;
    public int nextDialogueIndex;

    public DialogueChoice(string text, int nextIndex)
    {
        choiceText = text;
        nextDialogueIndex = nextIndex;
    }
}