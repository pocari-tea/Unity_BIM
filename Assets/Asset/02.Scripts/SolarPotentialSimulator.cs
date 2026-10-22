using UnityEngine;

public class SolarPotentialSimulator : MonoBehaviour
{
    public Transform buildingGroup; 
    public Material gradeAMaterial; 
    public Material gradeBMaterial; 
    public Material gradeCMaterial; 

    // 시뮬레이션용 상수 정의
    private float areaPerKW = 10f;          // 1kW당 필요 면적 (m²)
    private float dailySunHours = 3.6f;     // 일평균 발전 시간 (시간)
    private float daysPerMonth = 30f;       // 한 달 일수
    private float electricityRate = 200f;   // 1kWh당 전기요금 (원)

    void Start()
    {
        AnalyzeBuildings();
    }

    void AnalyzeBuildings()
    {
        foreach (Transform building in buildingGroup)
        {
            Renderer renderer = building.GetComponent<Renderer>();
            if (renderer == null) continue;

            // 1. 지붕 면적 계산
            float sizeX = renderer.bounds.size.x;
            float sizeZ = renderer.bounds.size.z;
            float roofArea = sizeX * sizeZ; 

            // 2. 발전량 및 절감액 수학 계산
            float estimatedCapacityKW = roofArea / areaPerKW; // 예상 설치 용량 (kW)
            float monthlyGenerationKWh = estimatedCapacityKW * dailySunHours * daysPerMonth; // 월 발전량 (kWh)
            float monthlySavingsWon = monthlyGenerationKWh * electricityRate; // 월 절감액 (원)

            // 3. 등급 판별 및 시각화 (면적 60m² 이상이면 6kW 이상 설치 가능)
            string grade = "";
            if (roofArea >= 60f) 
            {
                renderer.material = gradeAMaterial;
                grade = "A등급";
            }
            else if (roofArea >= 30f) 
            {
                renderer.material = gradeBMaterial;
                grade = "B등급";
            }
            else 
            {
                renderer.material = gradeCMaterial;
                grade = "C등급 (설치협소)";
            }

            // 결과 확인용 로그 출력 (소수점 첫째 자리까지만 표시: "F1")
            Debug.Log($"[건물 분석] {grade} / 면적: {roofArea:F1}m² / 설치가능용량: {estimatedCapacityKW:F1}kW / 월 예상발전량: {monthlyGenerationKWh:F1}kWh / 월 예상절감액: {monthlySavingsWon:N0}원");
            
            // TODO: 이 계산된 데이터들을 건물 오브젝트에 저장해두면, 나중에 마우스 클릭 시 UI에 띄울 수 있습니다.
        }
    }
}