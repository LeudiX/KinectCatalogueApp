using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderMenuAnim : MonoBehaviour
{
    public GameObject PanelMenu;

    public void ShowHideMenu()
    {
         
        if(PanelMenu != null)
        {
            Animator animator = PanelMenu.GetComponent<Animator>();
            if(animator != null)
            {
                bool isOpen= animator.GetBool("show");
                animator.SetBool("show", !isOpen); 
                                           
            }
          
           
        }
    }
    public void CloseMenu(){

        if(PanelMenu != null)
        {
            Animator animator = PanelMenu.GetComponent<Animator>();
            animator.SetBool("show",false);
        }
    }


     public void OpenOptionsPanel()
    {  
        if(PanelMenu != null)
        {
            Animator animator = PanelMenu.GetComponent<Animator>();
        
             animator.SetBool("isOpen", true);                                  
                         
        }
    }
     public void CloseOptionsPanel()
    {  
        if(PanelMenu != null)
        {
            Animator animator = PanelMenu.GetComponent<Animator>();
        
             animator.SetBool("isOpen", false);                                  
                         
        }
    }



     public void OpenInfoPanel(){

            Animator anim = PanelMenu.GetComponent<Animator>();         
            anim.SetBool("show", true);
                                                                                
    }

    public void CloseInfoPanel(){

            Animator anim = PanelMenu.GetComponent<Animator>();
            anim.SetBool("show",false);                                                                    
    }


}
