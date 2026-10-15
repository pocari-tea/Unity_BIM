using System.Collections.Generic;
using UnityEngine;

// 전체 JSON 응답을 받는 최상위 클래스
[System.Serializable]
public class SolarDataResponse
{
    public int page;          // 현재 페이지 번호
    public int perPage;       // 페이지당 데이터 수
    public int totalCount;    // 전체 데이터 수
    public int currentCount;  // 현재 응답된 데이터 수
    public int matchCount;    // 조건 매칭 데이터 수
    
    // 핵심! 실제 태양광 시설 목록이 들어있는 배열 (명세서의 'data' 배열)
    public List<SolarFacilityItem> data; 
}

// 'data' 배열 안의 개별 태양광 시설 정보를 담는 클래스
[System.Serializable]
public class SolarFacilityItem
{
    // 주의: JSON 응답의 한글 Key와 변수명이 정확히 일치해야 JsonUtility가 파싱할 수 있습니다.
    public int 연번;
    public string 태양광발전시설명;
    public string 소재지도로명주소;
    public string 소재지지번주소;
    public string 위도;
    public string 경도;
    public string 설비용량;
    public int 공급전압;
    public int 주파수;
    public int 설치연도;
    public string 데이터기준일자;
}