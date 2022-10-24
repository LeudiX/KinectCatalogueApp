using System.Collections;
using System.Collections.Generic;
using SwipeMenu;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controla los diferentes menus que tengo asociados a cada área de contenidos del catálogo
/// </summary>
public class MenuCatalogController : MonoBehaviour
{
    public List<Menu> menus = new List<Menu>();

    public int currentMenuIndex = 0;
    public Menu currentMenu;
    

    /// <summary>
    /// Función que verifica los indices de los menus, para luego, si  son diferentes realizar el cambio
    /// </summary>
    public void newMenu(int newMenu)
    {
        if (newMenu !=currentMenuIndex)
        {
            StartCoroutine("MenuChange", newMenu);
        }
    }

    /// <summary>
    /// Función para realizar el cambio de menu
    /// </summary>
    public IEnumerator MenuChange(int newPage)
    {
        //Closing current Menu
        currentMenu.gameObject.SetActive(false);

        //Opening new Menu
        currentMenuIndex = newPage;
        currentMenu = menus[currentMenuIndex];
        currentMenu.gameObject.SetActive(true); 
        
        yield return 0;
    }
    
    
}
