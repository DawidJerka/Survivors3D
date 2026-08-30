using TMPro;
using UnityEngine;

public class GameTimerUI : MonoBehaviour
{
    [SerializeField] private GameDirector gameDirector;
    [SerializeField] private TMP_Text timerText;

    private int lastDisplayedSecond = -1;

    private void Update()
    {
        if (gameDirector == null || timerText == null)
            return;

        int totalSeconds =
            Mathf.FloorToInt(gameDirector.ElapsedTime);

        if (totalSeconds == lastDisplayedSecond)
            return;

        lastDisplayedSecond = totalSeconds;

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        timerText.text =
            $"{minutes:00}:{seconds:00}";
    }
}