using Common.systems.UI.PagesSystem;
using System;
using UnityEngine;

public class SettingImpViewModel
{
    private PagesContainer _pages;

    public PagesContainer Pages
    {
        get => _pages;
        set { _pages = value; }
            
    }

    internal void OpenAudioPage()
    {
        _pages.OpenPageByName("Audio");
    }

    internal void OpenControlsPage()
    {
        _pages.OpenPageByName("Controls");
    }

    internal void OpenGraphicPage()
    {
        _pages.OpenPageByName("Graphics");
    }
}
