using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InputNavigator : MonoBehaviour
{
    EventSystem system;

    void Start()
    {
        system = EventSystem.current;// EventSystemManager.currentSystem;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (system.currentSelectedGameObject == null)
            {
                var selectables = GetComponentsInChildren<Selectable>();
                if (selectables != null && selectables.Length > 0)
                {
                    system.SetSelectedGameObject(selectables[0].gameObject);
                }
                else
                {
                    return;
                }
            }

            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next == null)
            {
                var selectables = GetComponentsInChildren<Selectable>();
                if (selectables != null && selectables.Length > 0)
                {
                    next = selectables[0];
                }
                else
                {
                    return;
                }
            }

            // Navigate
            InputField inputfield = next.GetComponent<InputField>();
            if (inputfield != null)
                inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

            system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
        }
    }
}
