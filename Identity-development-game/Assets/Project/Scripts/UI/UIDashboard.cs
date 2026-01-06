using UnityEngine;

public class UIDashboard : MonoBehaviour
{
    [SerializeField] private GameObject ClassesPanel;
    [SerializeField] private GameObject SettingsPanel;
    [SerializeField] private GameObject StudentsPanel;
    [SerializeField] private GameObject MainPanel;
    public void OpenClassesPanel(){
        SettingsPanel.SetActive(false);
        StudentsPanel.SetActive(false);
        ClassesPanel.SetActive(true);
    }

    public void OpenSettingsPanel(){
        ClassesPanel.SetActive(false);
        StudentsPanel.SetActive(false);
        SettingsPanel.SetActive(true);
    }
    
    public void OpenStudentsPanel(){
        ClassesPanel.SetActive(false);
        SettingsPanel.SetActive(false);
        StudentsPanel.SetActive(true);
    }
    
}
