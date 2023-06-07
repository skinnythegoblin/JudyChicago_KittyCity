using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActivatedItem : MonoBehaviour
{
    enum CompletionAction { None, Disable, Deactivate, Destroy }

    [SerializeField] UnityEvent OnActivate;
    [SerializeField] CompletionAction _completionAction = CompletionAction.None;
    
    public virtual void Activate()
    {
        OnActivate?.Invoke();
    }

    public void ExecuteCompletionAction()
    {
        switch (_completionAction)
        {
            case CompletionAction.None:
                break;
            case CompletionAction.Disable:
                enabled = false;
                break;
            case CompletionAction.Deactivate:
                gameObject.SetActive(false);
                break;
            case CompletionAction.Destroy:
                Destroy(gameObject, 0.01f);
                break;
        }
    }
}