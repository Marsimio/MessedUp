using UnityEngine;
using UnityEngine.UIElements;

public class KeypadInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private Keypad keypad;
    public void Interact()
    {
        keypad.OpenKeypad();
        SIMain.Window.OpenBrowser("https://youtu.be/sh5jLtkKgPw");
    }

    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }
}
