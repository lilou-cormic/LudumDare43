using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Text SavedText;
    public Text SacrificedText;

    public Text BestText;

    private void Update()
    {
        SavedText.text = $"Saved: {GameManager.Instance.PlayerSafe.ToString()}";
        SacrificedText.text = $"Sacrificed: {GameManager.Instance.PlayerDead.ToString()}";

        BestText.text = $"Best: {GameManager.HighScore.ToString()}";
    }
}
