using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject character;               // �÷��̾� ������Ʈ
    public Camera mainCamera;                  // ���� ī�޶�
    public Vector3 offset;                     // ī�޶��� �÷��̾���� �Ÿ� ������
    private bool shouldFollow = false;         // ī�޶� ����;� �ϴ��� ����
    private bool hasFollowed = false;          // ī�޶� �̹� ����Դ��� ���θ� ����

    public GameObject obstaclesGroup;          // ������ ������Ʈ �׷�
    public GameObject[] targetPositions;       // ���������� ��ǥ ��ġ ������Ʈ �迭
    private int currentStageIndex = 0;         // ���� �������� �ε���

    void Start()
    {
        if (targetPositions.Length > 0)
        {
            // �ʱ� ī�޶� ��ġ ���� (ù ��° ��ǥ ��ġ��)
            offset = mainCamera.transform.position - character.transform.position;
        }
        else
        {
            Debug.LogError("targetPositions �迭�� ����ֽ��ϴ�. ��ǥ ��ġ�� �������ּ���.");
        }
    }

    private void FixedUpdate()
    {
        if (shouldFollow && !hasFollowed && currentStageIndex < targetPositions.Length)
        {
            // ���� ���������� ��ǥ ��ġ�� ī�޶� �̵�
            Vector3 targetPos = targetPositions[currentStageIndex].transform.position;
            mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPos, 5 * Time.deltaTime);

            // ī�޶� ��ǥ ��ġ�� ���� �����ߴ��� Ȯ��
            if (Vector3.Distance(mainCamera.transform.position, targetPos) < 0.1f)
            {
                GameManager.Instance.ResetTurn();
                hasFollowed = true;
                shouldFollow = false;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == character && !hasFollowed)
        {
            // �÷��̾ Ʈ���� ������ ���� ī�޶� ���� ��ǥ ��ġ�� �̵��ϰ� ����
            shouldFollow = true;

            // ���������� ��ֹ� ����
            Destroy(obstaclesGroup);
        }
    }

    public void CompleteStage()
    {
        // �������� �Ϸ� ó��
        if (currentStageIndex < targetPositions.Length - 1)
        {
            currentStageIndex++;  // ���� ���������� �ε��� ����
            hasFollowed = false;  // ���� ������������ ī�޶� �̵��� �ٽ� Ȱ��ȭ
        }
        else
        {
            Debug.Log("��� ���������� �Ϸ�Ǿ����ϴ�.");
        }
    }
}
