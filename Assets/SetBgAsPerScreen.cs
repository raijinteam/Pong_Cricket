using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBgAsPerScreen : MonoBehaviour
{
    [SerializeField]private SpriteRenderer spriteRenderer;

    private void Start() {
        SetBg();
    }
    public void SetBg() {

        float worldScreenHeight = Camera.main.orthographicSize * 2;

        // world width is calculated by diving world height with screen heigh
        // then multiplying it with screen width
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // to scale the game object we divide the world screen width with the
        // size x of the sprite, and we divide the world screen height with the
        // size y of the sprite
        transform.localScale = new Vector3(
            worldScreenWidth / spriteRenderer.sprite.bounds.size.x,
            worldScreenHeight / spriteRenderer.sprite.bounds.size.y, 1);
    }
}
