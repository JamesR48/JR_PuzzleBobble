using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PB_PlayerScore : MonoBehaviour
{
    [SerializeField]
    private PB_IntVariable_SO _playerScore;
    [SerializeField]
    private TextMeshProUGUI _scoreText = null;

    void Start()
    {
        ResetScoreSprites();
    }

    private void ResetScoreSprites()
    {
        string[] scoreSpritesText = new string[6] { "<sprite=64>", "<sprite=64>", "<sprite=64>", "<sprite=64>", "<sprite=64>", "<sprite=64>" };
        if (_scoreText != null)
        {
            string finalText = "";
            for (int idx = 0; idx < 6; idx++)
            {
                finalText += scoreSpritesText[idx];
            }
            _scoreText.text = finalText;
        }

        if (_playerScore != null)
        {
            _playerScore.valueChangeEvent += UpdatePlayerScoreText;
        }
    }

    public void UpdatePlayerScoreText()
    {
        if (_playerScore != null && _scoreText != null) 
        {
            int score = _playerScore.Value;
            int scoreTextLength = 6;
            string[] scoreSpritesText = new string[6] { "<sprite=64>", "<sprite=64>", "<sprite=64>", "<sprite=64>", "<sprite=64>", "<sprite=64>" };
            int textIndex = scoreTextLength - 1;

            while (score > 0)
            {
                int digit = score % 10; // Extract the last digit
                score /= 10; // Remove the last digit from the score

                // assign the image number to the number place
                scoreSpritesText[textIndex] = "<sprite=" + (digit + 64) + ">";
                textIndex--;
            }

            if (_scoreText != null)
            {
                string finalText = "";
                for (int idx = 0; idx < scoreTextLength; idx++)
                {
                    finalText += scoreSpritesText[idx];
                }
                _scoreText.text = finalText;
            }
        }
    }
}
