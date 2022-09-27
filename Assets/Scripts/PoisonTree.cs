using UnityEngine;
using UnityEngine.UI;

public class PoisonTree : MonoBehaviour
{
    public Text LogCollsiionEnter;
    public Text LogCollisionStay;
    public Text LogCollisionExit;

    private void OnCollisionEnter(Collision collision)
    {
        LogCollsiionEnter.text = "On Collision Enter: " + collision.collider.name;
    }

    private void OnCollisionStay(Collision collision)
    {
        LogCollisionStay.text = "On Collision stay: " + collision.collider.name;
    }

    private void OnCollisionExit(Collision collision)
    {
        LogCollisionExit.text = "On Collision exit: " + collision.collider.name;
    }
}