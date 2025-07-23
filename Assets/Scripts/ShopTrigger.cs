using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    public ShopManager shopManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered shop zone.");
            shopManager.OpenShop();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player exited shop zone.");
            shopManager.CloseShop();
        }
    }
}
