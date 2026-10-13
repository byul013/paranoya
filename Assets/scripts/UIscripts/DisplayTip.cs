using TMPro;
using UnityEngine;

public class DisplayTip : MonoBehaviour
{
    [SerializeField] private GameObject tipPanel;
    [SerializeField] private TMP_Text tipText;

    private void Update()
    {
        if (CurrentGameState.Instance == null) return;

        switch (CurrentGameState.Instance.state)
        {
            case GameState.GettingCalled:
                tipPanel.SetActive(true);
                tipText.text = "Tip: Press \"E\" to answer the phone.";
                Debug.Log("game state is getting called");
                break;

            case GameState.AnsweredThePhone:
                tipPanel.SetActive(false);
                Debug.Log("Game state set to answered the phone");
                break;

            default:
                tipPanel.SetActive(false);
                Debug.Log("tip panel set active");
                break;
        }
    }
}