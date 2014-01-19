using UnityEngine;
using System.Collections;

public class scriptAniSprite : MonoBehaviour
{
    public int columnSize = 0;
    public int rowSize = 0;
    public int colFrameStart = 0;
    public int rowFrameStart = 0;
    public int totalFrames = 0;
    public int framesPerSecond = 0;

    public void AniSprite(int columnSize, int rowSize, int colFrameStart, int rowFrameStart, int totalFrames, int framesPerSecond)
    {
        int index = (int)(Time.time * framesPerSecond);
        index = index % totalFrames;

        float fcolumnSize = columnSize;
        float frowSize = rowSize;
        Vector2 size = new Vector2(1.0F / fcolumnSize, 1.0F / frowSize);

        int u = index % columnSize;
        int v = index / columnSize;

        Vector2 offset = new Vector2((u + colFrameStart) * size.x, (1 - size.y) - (v + rowFrameStart) * size.y);
        renderer.material.mainTextureOffset = offset;
        renderer.material.mainTextureScale = size;

        //renderer.material.SetTextureOffset("_BumpMap", offset);
        //renderer.material.SetTextureScale("_BumpMap", size);
    }

    public void AniSprite(int columnSize, int rowSize, int colFrameStart, int rowFrameStart, int totalFrames, int framesPerSecond, float aniStart)
    {
        int index = (int)((Time.time - aniStart) * framesPerSecond);
        index = index % totalFrames;

        float fcolumnSize = columnSize;
        float frowSize = rowSize;
        Vector2 size = new Vector2(1.0F / fcolumnSize, 1.0F / frowSize);

        int u = index % columnSize;
        int v = index / columnSize;

        Vector2 offset = new Vector2((u + colFrameStart) * size.x, (1 - size.y) - (v + rowFrameStart) * size.y);
        renderer.material.mainTextureOffset = offset;
        renderer.material.mainTextureScale = size;

        //renderer.material.SetTextureOffset("_BumpMap", offset);
        //renderer.material.SetTextureScale("_BumpMap", size);
    }
}
