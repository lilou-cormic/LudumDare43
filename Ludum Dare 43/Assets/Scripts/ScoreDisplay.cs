using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreDisplay : MonoBehaviour
{
    public Text SavedText;
    public Text SacrificedText;

    private void Update()
    {
        SavedText.text = $"Saved: {GameManager.Instance.PlayerSafe.ToString()}";
        SacrificedText.text = $"Sacrificed: {GameManager.Instance.PlayerDead.ToString()}";
    }
}
