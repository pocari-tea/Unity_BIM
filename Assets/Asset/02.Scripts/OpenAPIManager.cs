using System.Collections;
using UnityEngine;
using UnityEngine.Networking; // UnityWebRequest를 사용하기 위해 필수

public class OpenAPIManager : MonoBehaviour
{
    // API 요청 URL (공공데이터포털 등에서 발급받은 URL)
    // 보안을 위해 서비스 키(API Key)는 따로 관리하는 것이 좋습니다.
    [Header("API Settings")]
    // 공공데이터포털에서 발급받은인증키
    public string serviceKey = "인증키";

    void Start()
    {
        StartCoroutine(GetSolarData());
    }

    IEnumerator GetSolarData()
    {
        // 디코딩 키의 특수문자가 웹 통신 중 깨지지 않도록 유니티가 안전하게 포장(EscapeURL)하게 만듭니다.
        string safeKey = UnityWebRequest.EscapeURL(serviceKey.Trim());
    
        // URL 끝에 &serviceKey=safeKey 형태로 쿼리를 붙여줍니다.
        string requestUrl = $"https://api.odcloud.kr/api/15039430/v1/uddi:e82484dd-5ddb-43af-867c-fe76cbcc9464_201912161319?page=1&perPage=1000&serviceKey={safeKey}";
    
        Debug.Log("요청 URL: " + requestUrl);

        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            // request.SetRequestHeader 코드는 사용하지 않습니다.
        
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("API 호출 실패: " + request.error);
                Debug.LogError("서버 응답 내용: " + request.downloadHandler.text); 
            }
            else
            {
                Debug.Log("데이터 수신 성공: " + request.downloadHandler.text);
                ParseJsonData(request.downloadHandler.text);
            }
        }
    }

    // 파싱을 처리하는 함수
    void ParseJsonData(string jsonText)
    {
        // JsonUtility를 사용하여 JSON 텍스트를 파싱
        SolarDataResponse responseData = JsonUtility.FromJson<SolarDataResponse>(jsonText);

        // 변경된 부분: items 대신 data 배열을 확인합니다.
        if (responseData != null && responseData.data != null)
        {
            Debug.Log($"총 {responseData.data.Count}개의 담양군 태양광 시설 데이터를 파싱했습니다.");

            // data 리스트를 순회하며 정보 출력
            foreach (SolarFacilityItem item in responseData.data)
            {
                Debug.Log($"[{item.연번}] 시설명: {item.태양광발전시설명}, " +
                          $"주소: {item.소재지도로명주소}, " +
                          $"용량: {item.설비용량}, " +
                          $"위치(위도/경도): {item.위도}, {item.경도}");
                
                // TODO: 이 정보를 바탕으로 3D 마커 생성 로직을 연결할 수 있습니다.
            }
        }
        else
        {
            Debug.LogWarning("JSON 파싱에 실패했거나 data 배열이 비어있습니다. API 응답 텍스트를 확인하세요.");
        }
    }
}