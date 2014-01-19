using UnityEngine;
using System.Collections;

public class TimeToolComplete : MonoBehaviour {

    public GameObject aniFont1;
    public GameObject aniFont2;
    public GameObject aniFont3;

    public float playTime = 0;
    public float continueTimeDown = 0;
    public float countDownDelay = 0;
    public float countDownAmount = 0;

    public bool playTimeEnabled = true;
    public bool countDownEnabled = true;

    void AniSprite(GameObject spriteObject, int columnSize, int rowSize, int colFrameStart, int rowFrameStart, int totalFrames, string type)
    {
        int index = (int)playTime;

        int font1 = index % 10;
        int font2 = ((index - font1) / 10) % 10;
        int font3 = ((index - font1) / 100) % 10;

        if (type == "font1") index = font1;
        if (type == "font2") index = font2;
        if (type == "font3") index = font3;

        Vector2 size = new Vector2(1.0F / columnSize, 1.0F / rowSize);

        int u = index % columnSize;
        int v = index / columnSize;

        Vector2 offset = new Vector2((u + colFrameStart) * size.x, (1 - size.y) - (v + rowFrameStart) * size.y);

        spriteObject.renderer.material.mainTextureOffset = offset;
        spriteObject.renderer.material.mainTextureScale = size;
    }

    void Update()
    {
        if (aniFont1 != null) AniSprite(aniFont1, 10, 1, 0, 0, 10, "font1");
        if (aniFont1 != null) AniSprite(aniFont2, 10, 1, 0, 0, 10, "font2");
        if (aniFont1 != null) AniSprite(aniFont3, 10, 1, 0, 0, 10, "font3");

        if (playTimeEnabled && countDownEnabled)
        {
            playTime = countDownDelay - Time.time + countDownAmount + continueTimeDown;
        }
        if (playTime <= 0)
        {
            playTimeEnabled = false;
            countDownEnabled = false;
        }
    }


    //void Update()
    //{
    //    AniSprite(aniFont1, 10, 1, 0, 0, 10, "font1");
    //    AniSprite(aniFont2, 10, 1, 0, 0, 10, "font2");
    //    AniSprite(aniFont3, 10, 1, 0, 0, 10, "font3");
    //    AniSprite(aniFont4, 10, 1, 0, 0, 10, "font4");

    //    playTime = Time.time * gameSpeed;
    //    seconds = (int)playTime % 60;
    //    fractions = (playTime * 10) % 10;
    //    minutes = playTime / 60;
    //    hours = (playTime / 3600) % 24;
    //    days = (playTime / 86400) % 365;


    //    if (timeActive && !countDownEnabled)
    //    {
    //        playTime = Time.time - fromLoadTime + addToTimeAmount;
    //    }
    //    if (!timeActive && countDownEnabled)
    //    {
    //        playTime = countDownDelay + fromLoadTime - Time.time;
    //    }
    //    // Start Time
    //    if (Input.GetKeyDown("1"))      //press to activate add to timer single amount
    //    {
    //        startTime = Time.time - Time.time;
    //        addToTimeAmount = 0;
    //        continueTime = 0;
    //        timeActive = true;
    //        countDownEnabled = false;
    //    }
    //    fromStartTime = Time.time - startTime;
    //    // Time from Load
    //    if (Input.GetKeyDown("2"))
    //    {
    //        fromLoadTime = Time.timeSinceLevelLoad;

    //    }
    //    // Stop Play Time
    //    if (Input.GetKeyDown("3"))
    //    {
    //        if (timeActive)
    //        {
    //            stopTime = Time.time;
    //            timeActive = false;
    //        }
    //    }
    //    // Stop game time (pause)
    //    if (Input.GetKeyDown("4"))
    //    {
    //        if (Time.timeScale == 0)
    //        {
    //            Time.timeScale = 1;
    //        }
    //        else
    //        {
    //            Time.timeScale = 0;
    //        }
    //    }
    //    // Continue Time
    //    if (Input.GetKeyDown("5"))
    //    {
    //        continueTime = Time.time - playTime;
    //        timeActive = true;
    //    }
    //    // Stop time again.....No idea
    //    if (Input.GetKeyDown("6"))
    //    {
    //        playTime = 0;
    //        stopTime = 0;
    //        timeActive = false;
    //    }
    //    // Being Count Down Timer
    //    if (Input.GetKeyDown("7"))
    //    {
    //        countDownDelay = countDownAmount;
    //        fromLoadTime = Time.timeSinceLevelLoad;
    //        timeActive = false;
    //        countDownEnabled = true;
    //    }
    //    if (playTime < 0)
    //    {
    //        addToTimeAmount = 0;
    //        fromLoadTime = Time.timeSinceLevelLoad;
    //        timeActive = true;
    //        countDownEnabled = false;
    //    }
    //    // Delay Amount / rate (single/once)
    //    if (Input.GetKeyDown("8"))
    //    {
    //        addToTimeAmount = timeAmount;
    //    }
    //    // Add time Amount (repeatedly/each button press)
    //    if (Input.GetKeyDown("9"))
    //    {
    //        addToTimeAmount += timeAmount;
    //    }

    //    if (playTime > delayTime)
    //    {
    //        delayTime = Time.time + delayedAmount;
    //    }
    //}
    //void OnGUI()
    //{
    //    GUILayout.Label(" Play Time " + playTime.ToString("f2"));
    //    GUILayout.Label(" Minutes  " + minutes.ToString("f0"));
    //    GUILayout.Label(" Seconds  " + seconds.ToString("f0"));
    //    GUILayout.Label(" Fractions " + fractions.ToString("f3"));
    //    GUILayout.Label(" Actual Time " + actualTime.ToString("f0"));
    //    GUILayout.Label(" Start Time " + startTime.ToString("f2"));
    //    GUILayout.Label(" From Start Time " + fromStartTime.ToString("f2"));
    //    GUILayout.Label(" From Load time " + fromLoadTime.ToString("f2"));
    //    GUILayout.Label(" Stop Time  " + stopTime.ToString("f0"));
    //    GUILayout.Label(" Pause Game Time  " + pauseGameTime.ToString("f0"));
    //    GUILayout.Label(" Delay Time  " + delayTime.ToString("f0"));

    //    GUI.skin = marioGui;
    //    GUI.Label(new Rect(Screen.width / 2, 10, 1000, 100), "" + playTime.ToString("f1"));
    //}

}
