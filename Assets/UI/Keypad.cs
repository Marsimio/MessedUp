using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Keypad : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private UIDocument uiDocument;
    private VisualElement root;
    private Label display;

    private string currentInput = "";
    private DateTime now;

    private void Awake()
    {
        uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        display = root.Q<Label>("display-text");

        Debug.Log($"Display found: {display != null}");

        SetupRow(root.Q<VisualElement>("keypad-row1"), 1, 2, 3);
        SetupRow(root.Q<VisualElement>("keypad-row2"), 4, 5, 6);
        SetupRow(root.Q<VisualElement>("keypad-row3"), 7, 8, 9);

        VisualElement row4 = root.Q<VisualElement>("keypad-row4");

        Button clearButton = row4.ElementAt(0) as Button;
        Button zeroButton = row4.ElementAt(1) as Button;
        Button enterButton = row4.ElementAt(2) as Button;

        clearButton.text = "C";
        zeroButton.text = "0";
        enterButton.text = "Enter";

        clearButton.clicked += Clear;
        zeroButton.clicked += () => AddDigit(0);
        enterButton.clicked += Submit;

        root.style.display = DisplayStyle.None;
    }

    public void OpenKeypad()
    {
        currentInput = "";
        display.text = "";

        root.style.display = DisplayStyle.Flex;

        playerController.SetUIInputMode(true);
    }

    public void CloseKeypad()
    {
        playerController.SetUIInputMode(false);

        root.style.display = DisplayStyle.None;
    }

    private void SetupRow(
        VisualElement row,
        int first,
        int second,
        int third
    )
    {
        Button button1 = row.ElementAt(0) as Button;
        Button button2 = row.ElementAt(1) as Button;
        Button button3 = row.ElementAt(2) as Button;

        button1.text = first.ToString();
        button2.text = second.ToString();
        button3.text = third.ToString();

        button1.clicked += () => AddDigit(first);
        button2.clicked += () => AddDigit(second);
        button3.clicked += () => AddDigit(third);
    }

    private void AddDigit(int digit)
    {
        if (currentInput.Length >= 4)
            return;

        currentInput += digit.ToString();
        display.text = currentInput;
    }

    private void Clear()
    {
        currentInput = "";
        display.text = "";
    }

    private void Submit()
    {
        now = SIMain.PC.GetSystemTime();

        string correctCode = now.ToString("HHmm");

        if (currentInput == correctCode)
        {
            Debug.Log("Correct code!");
            Act2Manager.Instance.NextStep();
        }
        else
        {
            Debug.Log($"Incorrect code. Entered: {currentInput}");
        }

        Clear();
        CloseKeypad();
    }
}