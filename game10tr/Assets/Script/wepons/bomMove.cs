// BomMove.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomMove : MonoBehaviour
{
    [SerializeField] Transform bomSpawnPoint; // Vị trí ném bom
    [SerializeField] float maxDistance = 3f; // Bán kính tối đa mà bom có thể di chuyển
    [SerializeField] float speed = 10f; // Tốc độ di chuyển của bom
    [SerializeField] GameObject prefabExBom; //nem prefab bom nổ
    [SerializeField] float invokeTime = 0.5f; // Thời gian để bom nổ sau khi di chuyển đến vị trí mục tiêu
    private Vector3 targetPosition; // Vị trí mục tiêu mà bom sẽ di chuyển đến
    private Vector3 mouseWorldPos;
    public bool isThrowing = false; // Biến để kiểm soát trạng thái di chuyển của bom
    private bool isTouching = false; // Biến để kiểm soát trạng thái chạm vào mục tiêu
    public transitions transitions; // Biến để truy cập vào script transitions
    private GameObject currentBom; // Biến để lưu trữ bom hiện tại
    private Vector3 takeTargetPosition()
    {
        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f; // Đặt z về 0 để tránh vấn đề với chiều cao
        return mouseWorldPos;
    }
    
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && transitions.isHoldingBom && !isThrowing)
        {
            takeTargetPosition();
            Vector3 direction = mouseWorldPos - bomSpawnPoint.position;
            if (direction.magnitude > maxDistance)
            {
                direction = direction.normalized * maxDistance;
            }
            targetPosition = bomSpawnPoint.position + direction;

            
            currentBom = Instantiate(prefabExBom, bomSpawnPoint.position, Quaternion.identity);
            isThrowing = true; // Bom bắt đầu bay
        }

        if (isThrowing && currentBom != null)
        {
            transitions.isHoldingBom = false;
            currentBom.transform.position = Vector3.MoveTowards(currentBom.transform.position, targetPosition, speed * Time.deltaTime);
            
            if ((currentBom.transform.position - targetPosition).sqrMagnitude < 0.01f)
            {
                Debug.Log("Bom da den dich");
                isThrowing = false; // Kết thúc ném
                Invoke(nameof(disapear), invokeTime);
            }
        }
    }

    private void disapear()
    {
        if(currentBom != null)

            Destroy(currentBom); // Hủy bom sau khi nổ
    }
}
