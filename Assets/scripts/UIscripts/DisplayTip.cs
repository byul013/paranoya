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
                break;

            case GameState.AnsweredThePhone:
                tipPanel.SetActive(false);
                break;

            default:
                tipPanel.SetActive(false);
                break;
        }
    }
}