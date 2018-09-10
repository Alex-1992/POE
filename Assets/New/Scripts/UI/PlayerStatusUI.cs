using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatusUI : MonoBehaviour {

    //Inspector
    public Slider PlayerHPSlider;
    public Slider PlayerMPSlider;
    public Slider PlayerEXPSlider;
    public Text PlayerLevelText;
	
	void Update () {
        PlayerHPSlider.value = POEStatics.PlayerHPController.GetCurrentHP() / POEStatics.PlayerPOEStatus.S["MaxHP"].Get();
        PlayerMPSlider.value = POEStatics.PlayerHPController.GetCurrentMP() / POEStatics.PlayerPOEStatus.S["MaxMP"].Get();
        PlayerEXPSlider.value = (POEStatics.PlayerLevelController.GetCurrentExp() - POEStatics.PlayerLevelController.GetExpToThisLevel()) / (POEStatics.PlayerLevelController.GetExpToNextLevel() - POEStatics.PlayerLevelController.GetExpToThisLevel());
        PlayerLevelText.text = POEStatics.PlayerLevelController.GetCurrenLevel().ToString();
    }
}