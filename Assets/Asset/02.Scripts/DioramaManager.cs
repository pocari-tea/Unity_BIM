using UnityEngine;
using TMPro;

public class DioramaManager : MonoBehaviour
{
    [Header("건물 그룹 및 등급별 머티리얼")]
    public Transform buildingGroup;
    public Material greenMaterial;
    public Material yellowMaterial;
    public Material redMaterial;

    [Header("UI 팝업 설정")]
    public GameObject infoCanvas; // World Space UI 캔버스
    public TextMeshProUGUI txtAddress;
    public TextMeshProUGUI txtGrade;
    public TextMeshProUGUI txtCapacity;
    public TextMeshProUGUI txtGeneration;
    public TextMeshProUGUI txtSavings;

    void Start()
    {
        if (infoCanvas != null) infoCanvas.SetActive(false);
        AnalyzeAllBuildings();
    }

    void Update()
    {
        // 마우스 왼쪽 클릭 감지
        if (Input.GetMouseButtonDown(0))
        {
            DetectBuildingClick();
        }
    }

    // 1. 씬 내의 모든 건물을 검색하여 데이터를 계산하고 색칠합니다.
    void AnalyzeAllBuildings()
    {
        foreach (Transform t in buildingGroup)
        {
            Renderer r = t.GetComponent<Renderer>();
            if (r == null) continue;

            // 3D 컬라이더가 있어야 마우스 클릭이 감지됩니다. 없으면 추가합니다.
            if (t.GetComponent<Collider>() == null)
            {
                t.gameObject.AddComponent<BoxCollider>();
            }

            // 개별 건물 데이터 스크립트 부착
            BuildingData bData = t.GetComponent<BuildingData>();
            if (bData == null)
            {
                bData = t.gameObject.AddComponent<BuildingData>();
            }

            // 지붕 면적 계산 (Bounds.size.x * z)
            float area = r.bounds.size.x * r.bounds.size.z;
            bData.CalculateSolarPotential(area, greenMaterial, yellowMaterial, redMaterial);
        }
    }

    // 2. 마우스 클릭 위치에 건물이 있는지 감지합니다.
    void DetectBuildingClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            BuildingData clickedBuilding = hit.collider.GetComponent<BuildingData>();
            if (clickedBuilding != null)
            {
                ShowBuildingInfo(clickedBuilding);
            }
            else
            {
                // 빈 땅을 클릭하면 팝업을 닫습니다.
                if (infoCanvas != null) infoCanvas.SetActive(false);
            }
        }
    }

    // 3. 클릭한 건물의 정보를 UI 팝업에 연동하고 화면에 띄웁니다.
    void ShowBuildingInfo(BuildingData data)
    {
        if (infoCanvas == null) return;

        // UI 텍스트 갱신 (F1: 소수점 첫째자리, N0: 천 단위 콤마)
        txtAddress.text = $"건물 정보: {data.address}";
        txtGrade.text = $"태양광 설치 등급: {data.grade}";
        txtCapacity.text = $"예상 설치 용량: {data.capacityKW:F1} kW";
        txtGeneration.text = $"월 예상 발전량: {data.monthlyKWh:N0} kWh";
        txtSavings.text = $"월 예상 절감액: {data.monthlySavings:N0} 원";

        // 팝업의 위치를 클릭한 건물 살짝 위쪽으로 이동시킵니다.
        infoCanvas.transform.position = data.transform.position + Vector3.up * 5f;
        infoCanvas.SetActive(true);
    }
}