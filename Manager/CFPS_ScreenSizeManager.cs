using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFPS_ScreenSizeManager : MonoBehaviour {

    private void Awake()
    {
        // 프레임 고정
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 50;

        // 화면 비율 고정, 화면 꺼짐 X
        //Screen.SetResolution(Screen.width, Screen.height, true);
        Screen.SetResolution(720, 1280, true);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // 사용된 모든 쉐이더를 미리 한번 웜업해준다. 로딩시 렉 최소화
        Shader.WarmupAllShaders();

        // 터치 입력을 오직 하나만 받는다.
        Input.multiTouchEnabled = false;
    }
}
