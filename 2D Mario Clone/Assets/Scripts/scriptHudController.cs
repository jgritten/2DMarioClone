using UnityEngine;
using System.Collections;

public class scriptHudController : MonoBehaviour {

    // HUD CONTROLLER
    // Controls Gui Coin Counting on pickup and Player Lives

    public GameObject livesFont1;
    public GameObject livesFont2;
    public GameObject coinFont1;
    public GameObject coinFont2;

    int lives = 0;
    int coins = 0;

    void AniSprite(GameObject spriteObject, int columnSize, int rowSize, int colFrameStart, int rowFrameStart, int totalFrames, string type, int index)
    {
        int font1 = lives;
        int font2 = (lives / 10) % 10;
        int font3 = coins;
        int font4 = (coins / 10) % 10;

        if (type == "font1") index = font1;
        if (type == "font2") index = font2;
        if (type == "font3") index = font3;
        if (type == "font4") index = font4;

        Vector2 size = new Vector2(1.0F / columnSize, 1.0F / rowSize);

        int u = index % columnSize;
        int v = index / columnSize;

        Vector2 offset = new Vector2((u + colFrameStart) * size.x, (1 - size.y) - (v + rowFrameStart) * size.y);

        spriteObject.renderer.material.mainTextureOffset = offset;
        spriteObject.renderer.material.mainTextureScale = size;
    }

    void Update()
    {
        GameObject pProp = GameObject.FindGameObjectWithTag("player");
        lives = pProp.GetComponent<scriptPlayerProperties>().lives;
        coins = pProp.GetComponent<scriptPlayerProperties>().coins;

        if (livesFont1 != null) AniSprite(livesFont1, 10, 1, 0, 0, 10, "font1", lives);
        if (livesFont2 != null) AniSprite(livesFont2, 10, 1, 0, 0, 10, "font2", lives);
        if (coinFont1 != null) AniSprite(coinFont1, 10, 1, 0, 0, 10, "font3", coins);
        if (coinFont2 != null) AniSprite(coinFont2, 10, 1, 0, 0, 10, "font4", coins);
    }
}
