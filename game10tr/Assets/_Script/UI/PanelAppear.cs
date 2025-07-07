using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanelType
{
    Socialize,
    Work,
    War,
    FreeStyle,
    IdealsChose
}
public class PanelAppear : MonoBehaviour
{
    public GameObject socializePanel;
    public GameObject WorkPanel;
    public GameObject WarPanel;
    public GameObject FreeStyle;
    public GameObject IdealsChosePanel;
    private Dictionary<PanelType, GameObject> panels;
    private void Start()
    {
        // Hiển thị SocializePanel mặc định khi bắt đầu
        ShowPanel(PanelType.IdealsChose);
    }
    private void Awake()
    {
        panels = new Dictionary<PanelType, GameObject>
        {
            { PanelType.Socialize, socializePanel },
            { PanelType.Work, WorkPanel },
            { PanelType.War, WarPanel },
            { PanelType.FreeStyle, FreeStyle },
            { PanelType.IdealsChose, IdealsChosePanel }
        };
    }
    public void ShowPanel(PanelType typeToShow)
    {
        foreach (var panel in panels)
        {
            panel.Value.SetActive(panel.Key == typeToShow);
        }
    }
    public void clickSocialPanel()
    {
        ShowPanel(PanelType.Socialize);
    }
    public void clickWorkPanel()
    {
        ShowPanel(PanelType.Work);
    }
    public void clickWarPanel()
    {
        ShowPanel(PanelType.War);
    }
    public void clickFreeStylePanel()
    {
        ShowPanel(PanelType.FreeStyle);
    }
    public void clickIdealsChosePanel()
    {
        ShowPanel(PanelType.IdealsChose); // Mặc định hiển thị Socialize khi mở IdealsChosePanel
    }
}

