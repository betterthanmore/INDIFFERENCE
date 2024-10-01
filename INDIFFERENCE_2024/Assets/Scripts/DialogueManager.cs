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
    private List<Dialogue> dialogues = new List<Dialogue>();  // ��ȭ ����Ʈ
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
            playerController.canMove = false;  // �÷��̾� �̵� ��Ȱ��ȭ
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
    // CSV ���Ͽ��� ���� ĳ���� �̸� �ҷ����� �Լ�
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

            // ������ ���� Ȯ���� �Ŀ� ������ �������� �Լ�
            List<DialogueChoice> choices = new List<DialogueChoice>();
            if (!string.IsNullOrEmpty(values[2]))  // ������1
            {
                choices.Add(new DialogueChoice(values[2], int.Parse(values[3])));
            }
            if (!string.IsNullOrEmpty(values[4]))  // ������2
            {
                choices.Add(new DialogueChoice(values[4], int.Parse(values[5])));
            }

            // �������� �ִ� ���, choices ����Ʈ�� ������ Dialogue �߰�
            dialogues.Add(new Dialogue(characterName, dialogueText, choices.Count > 0 ? choices : null));
        }
        reader.Close();
    }
    // ��ȭ ���� �ε�
    public void LoadDialogueFile(string fileName)
    {
        string filePath = "Assets/Dialogues/" + fileName + ".csv";
        LoadDialogueFromCSV(filePath);
        StartDialogue();
    }

    // ��ȭ ����
    void StartDialogue()
    {
        if (dialogues.Count > 0)
        {
            currentDialogueIndex = 0;
            typingCoroutine = StartCoroutine(TypeDialogue(dialogues[currentDialogueIndex]));
        }
    }
    // ���� ���� �Ѿ��
    void NextDialogue()
    {
        if (currentDialogueIndex < dialogues.Count - 1)
        {
            currentDialogueIndex++;
            if (typingCoroutine != null) StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeDialogue(dialogues[currentDialogueIndex]));
        }
    }
    // ��ȭ ������ �� ���ھ� ����ϴ� �ڷ�ƾ
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
        // ��ȭ�� ������ �������� �ִ��� Ȯ���ϰ� ���
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

    // ������ ǥ��
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

    // ������ ���� �� ó��
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

    // ��ȭ ���� �� ȣ��
    void EndDialogue()
    {
        isDialogueActive = false;
        dialogueUI.SetActive(false);
        playerController.canMove = true;  // �÷��̾� �̵� Ȱ��ȭ

    }
}

// ĳ���� �̸��� ��縦 ���� Ŭ����
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

// ������ �����͸� ���� Ŭ����
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