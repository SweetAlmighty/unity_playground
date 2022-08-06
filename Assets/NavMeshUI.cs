using UnityEngine;
using UnityEngine.UIElements;

public class NavMeshUI : MonoBehaviour
{
    /// <summary>
    /// Reference to the <see cref="UIDocument"/> component used for this scene.
    /// </summary>
    [SerializeField]
    [Tooltip("Reference to the UIDocument component used for this scene.")]
    private UIDocument uiDocument;

    /// <summary>
    /// Unity Event function that subscribes scene-loading callbacks to UI buttons when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        if (uiDocument.rootVisualElement.Q<Button>() is { } button)
            button.RegisterCallback<ClickEvent>(_ => UnityEngine.SceneManagement.SceneManager.LoadScene(0));
    }
}
