using UnityEngine;
using extOSC;   // extOSC 네임스페이스 꼭 추가!

public class OSCPreSetup : MonoBehaviour
{
    [Header("OSC Receiver")]
    public OSCReceiver receiver;   // 인스펙터에서 연결할 예정

    [Header("Debug Target (Optional)")]
    public Transform debugCube;    // 테스트용 큐브 (없어도 동작은 함)

    private void Start()
    {
        // 혹시 인스펙터에서 안 넣었으면, 같은 오브젝트에서 자동으로 찾아보기
        if (receiver == null)
        {
            receiver = GetComponent<OSCReceiver>();
        }

        if (receiver == null)
        {
            Debug.LogError("[OSC] OSCReceiver가 설정되지 않았습니다!");
            return;
        }

        // "/unity/test" 주소로 오는 메시지 바인딩
        receiver.Bind("/unity/test", OnTestMessage);
        Debug.Log("[OSC] /unity/test 바인딩 완료. 포트: " + receiver.LocalPort);
    }

    // TouchDesigner에서 "/unity/test 1" 같은 메시지를 보내면 여기로 옴
    private void OnTestMessage(OSCMessage message)
    {
        // 값 꺼내기 (첫 번째 값)
        float value = 0f;
        if (message.Values.Count > 0 && message.Values[0].Type == OSCValueType.Float)
        {
            value = message.Values[0].FloatValue;
        }

        Debug.Log("[OSC] /unity/test 수신! 값: " + value);

        // 디버그용: 큐브가 있으면 크기를 랜덤으로 살짝 변경
        if (debugCube != null)
        {
            float scale = Random.Range(0.5f, 1.5f);
            debugCube.localScale = Vector3.one * scale;
        }
    }
}
