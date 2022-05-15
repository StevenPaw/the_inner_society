using UnityEngine;

namespace InnerSociety
{
    public class InfoDisplay : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        private bool isVisible;
        
        public void ToggleDisplay()
        {
            isVisible = !isVisible;
            animator.SetBool("isVisible", isVisible);
        }
    }
}