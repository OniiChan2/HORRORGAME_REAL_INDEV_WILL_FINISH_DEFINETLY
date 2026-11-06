using UnityEditor;


[CustomEditor(typeof(Interactable),true)]
public class InteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //calls for interactable target 
        Interactable interactable = (Interactable)target;
        if (target.GetType() == typeof(EventOnlyInteractable))
        {

            interactable.promptMsg = EditorGUILayout.TextField("Prompt Message", interactable.promptMsg);
            EditorGUILayout.HelpBox("Event Only Interactable can ONLY use UnityEvents", MessageType.Info);

            if(interactable.GetComponent<InteractionEvents>() == null) {
                interactable.useEvents = true;
                interactable.gameObject.AddComponent<InteractionEvents>();

            }

        }
        else
        {
            base.OnInspectorGUI();
            if (interactable.useEvents)
            {
                //Checks if the gameObject uses events, if it does create an events list 
                if (interactable.GetComponent<InteractionEvents>() == null)
                {
                    interactable.gameObject.AddComponent<InteractionEvents>();
                }
            }
            else
            {
                //Checks if the gameObject does not use events and removes all event list components 
                if (interactable.GetComponent<InteractionEvents>() != null)
                {
                    DestroyImmediate(interactable.GetComponent<InteractionEvents>());
                }
            }
        }
    }
}
