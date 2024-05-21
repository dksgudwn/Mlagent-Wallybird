using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] TMP_Text scoreText;
    [SerializeField] Player player;

    private void Update()
    {
        scoreText.text = player.Count.ToString();
    }
}
