using DG.Tweening;
using farmingsim.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal : MonoBehaviour
{
    [SerializeField] private string SceneToTeleportTo;
    [SerializeField] private Vector2 pathDirection;
    [SerializeField] private Vector2 targetPosition;
    private Rigidbody2D playerControllerRB;
        
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GameTags.PLAYER))
        {
            playerControllerRB = other.GetComponent<Rigidbody2D>();
            Vector2 targetDirection = (Vector2)other.gameObject.transform.position + (pathDirection * 3);
            other.gameObject.transform.DOMove(targetDirection, 1f)
                .OnComplete(ChangeScene);
            playerControllerRB.GetComponentInParent<PlayerController>().FadeToBlack(1f);
        }
    }

    private void ChangeScene()
    {
        playerControllerRB.isKinematic = true;
        playerControllerRB.GetComponent<Collider2D>().enabled = false;
        playerControllerRB.transform.localPosition = targetPosition;

        playerControllerRB.GetComponent<Collider2D>().enabled = true;
        playerControllerRB.isKinematic = false;
        
        SceneManager.LoadScene(SceneToTeleportTo);
    }
}
