using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;
    public MenuIndexes entryMenu;
    [HideInInspector] public MenuIndexes currentlyActiveMenu;
    public Menu[] menus;
    public Hashtable menusTable;

    private void Awake()
    {
        if (!Instance)
        {
            Instance = this;
            menusTable = new Hashtable();
            RegisterAllMenus();
            
            foreach(var menu in menus)
            {
                menu.gameObject.SetActive(false);
            }

            TurnMenuOn(entryMenu);
           // DontDestroyOnLoad(gameObject);
        }
        /*else
        {
            Destroy(gameObject);
        }*/
    }

    public void TurnMenuOn(MenuIndexes menuIndex)
    {
        if (menuIndex == MenuIndexes.None) return;
        if (!MenuExists(menuIndex))
        {
            LogWarning("You are trying to turn a page on ["+menuIndex+"] that has not been registered");
            return;
        }

        Menu menu = GetMenu(menuIndex);
        menu.gameObject.SetActive(true);
        menu.Animate(true);

        currentlyActiveMenu = menuIndex;
    }

    public void TurnMenuOff(MenuIndexes off, MenuIndexes on = MenuIndexes.None, bool waitForExit = false)
    {
        if (off == MenuIndexes.None) return;
        if (!MenuExists(off))
        {
            LogWarning("You are trying to turn a page on ["+off+"] that has not been registered");
            return;
        }
        Menu offMenu = GetMenu(off);
        if (offMenu.gameObject.activeSelf)
            offMenu.Animate(false);

        currentlyActiveMenu = MenuIndexes.None;
        

        if (on == MenuIndexes.None) return;
        if (!MenuExists(on))
        {
            LogWarning("You are trying to turn a page on ["+on+"] that has not been registered");
            return;
        }
        if (waitForExit)
        {
            Menu onMenu = GetMenu(on);
            StopCoroutine("WaitForMenuExit");
            StartCoroutine(WaitForMenuExit(onMenu, offMenu));
        }
        else
        {
            TurnMenuOn(on);
        }

        currentlyActiveMenu = on;
    }

    public bool MenuIsOn(MenuIndexes menu)
    {
        if (!MenuExists(menu))
        {
            LogWarning("You are trying to detect if a menu in on ["+menu+"] but it has not been registered yet");
            return false;
        }

        return GetMenu(menu).isOn;
    }


    private IEnumerator WaitForMenuExit(Menu on, Menu off)
    {
        while (off.targetState != Menu.FLAG_NONE)
        {
            yield return null;
        }
        
        TurnMenuOn(on.index);
    }
    private bool MenuExists(MenuIndexes menuIndex)
    {
        return menusTable.ContainsKey(menuIndex);
    }
    private void RegisterAllMenus()
    {
        foreach (var menu in menus)
        {
            RegisterMenu(menu);
        }
    }
    private void RegisterMenu(Menu menu)
    {
        if (MenuExists(menu.index))
        {
            LogWarning("You are trying to register a page ["+menu.index+"] that has already been registered: " + menu.gameObject.name);
            return;
        }
        
        menusTable.Add(menu.index, menu);
        Log("Registered new page ["+menu.index+"]");
    }
    private Menu GetMenu(MenuIndexes menuIndex)
    {
        if (!MenuExists(menuIndex))
        {
            LogWarning("You are trying to get a page ["+menuIndex+"] that has not been registered");
            return null;
        }

        return (Menu)menusTable[menuIndex];
    }
    private void Log(string msg)
    {
        if (!MenuSettings.Get().debugSystem) return;
        Debug.Log("[Menu Controller]: " + msg);
    }
    private void LogWarning(string msg)
    {
        if (!MenuSettings.Get().debugSystem) return;
        Debug.LogWarning("[Menu Controller]: " + msg);
    }
}
