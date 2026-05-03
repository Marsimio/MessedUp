using UnityEngine;

public class ChoiceDelete : MonoBehaviour, IInteractable
{ 
    [SerializeField] FileAction fileAction;


    public void Interact()
    {
        fileAction.Execute();
        if (Act2Manager.Instance != null && Act2Manager.Instance.currentStep != 2)
        {
            Act2Manager.Instance.NextStep();
        }
    }

    public string GetInteractText()
    {
        throw new System.NotImplementedException();
    }
}
