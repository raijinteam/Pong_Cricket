using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SelctedAbiltyAnimation : MonoBehaviour {

    [SerializeField] private List<Sprite> all_Sprite;
    [SerializeField] private Image img_Selcted;
    [SerializeField] private float flt_DelayOfTwoSpriteAnimation;

    private int maxCount = 20;

    
    public void ActivtedAnimation(int selctedSprite) {
        this.gameObject.SetActive(true);
        all_Sprite.Clear();
        for (int i = 0; i < AbilityManager.Instance.GetTotalAbilityCount(); i++) {

            all_Sprite.Add(AbilityManager.Instance.GetAbilityIcon(i));
        }
        StartCoroutine(SetRandomSprite(selctedSprite));
    }

    private IEnumerator SetRandomSprite(int index) {
        for (int i = 0; i < maxCount; i++) {

            img_Selcted.sprite = all_Sprite[Random.Range(0, all_Sprite.Count)];
            yield return new WaitForSeconds(flt_DelayOfTwoSpriteAnimation);
        }
        yield return new WaitForSeconds(flt_DelayOfTwoSpriteAnimation);
        img_Selcted.sprite = all_Sprite[index];

      

        UIManager.Instance.ui_Abilities.CompltedSelectedPanel(index);
        
    }
}
