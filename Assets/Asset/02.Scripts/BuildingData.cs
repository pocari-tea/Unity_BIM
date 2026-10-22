using UnityEngine;

public class BuildingData : MonoBehaviour
{
    // 계산된 결과를 저장할 변수들
    public string address = "수북면";
    public string grade = "등급 미정";
    public float roofArea = 0f;
    public float capacityKW = 0f;
    public float monthlyKWh = 0f;
    public float monthlySavings = 0f;

    // 이 건물의 데이터 계산하기
    public void CalculateSolarPotential(float area, Material gradeA, Material gradeB, Material gradeC)
    {
        roofArea = area;
        
        // 1kW 설치에 10m² 필요하다고 가정
        capacityKW = roofArea / 10f; 
        // 하루 3.6시간 발전 * 30일
        monthlyKWh = capacityKW * 3.6f * 30f; 
        // 1kWh당 200원 절약
        monthlySavings = monthlyKWh * 200f; 

        Renderer renderer = GetComponent<Renderer>();
        if (renderer == null) return;

        // 면적에 따른 등급 및 색상 부여
        if (roofArea >= 60f) 
        {
            grade = "A등급 (설치최적)";
            renderer.material = gradeA;
        }
        else if (roofArea >= 30f) 
        {
            grade = "B등급 (보통)";
            renderer.material = gradeB;
        }
        else 
        {
            grade = "C등급 (설치부적합)";
            renderer.material = gradeC;
        }
    }
}