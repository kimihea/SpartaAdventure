using UnityEngine;

public class JumpPad : MonoBehaviour
{
    public float jumpForce; 

    private void OnTriggerEnter(Collider other)
    {

        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // 위쪽 방향으로 힘을 가함
            Vector3 force = Vector3.up * jumpForce;
            rb.AddForce(force, ForceMode.Impulse);
        }
    }
}