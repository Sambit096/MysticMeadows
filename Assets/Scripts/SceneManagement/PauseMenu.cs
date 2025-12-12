using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{
public class PauseMenu : MonoBehaviour
{
    [SerializeField] private SavingWrapper savingWrapper;

    public void OnSaveButton()
    {
        savingWrapper.Save();
    }
}
}
