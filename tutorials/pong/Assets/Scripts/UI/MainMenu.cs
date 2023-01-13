using System.Collections;
using System.Collections.Generic;

using FishNet;

using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private Button hostButton;

    [SerializeField]
    private Button joinButton;

    // Start is called before the first frame update
    void Start()
    {
        hostButton.onClick.AddListener(() => {
            InstanceFinder.ServerManager.StartConnection();
            InstanceFinder.ClientManager.StartConnection();
        });

        joinButton.onClick.AddListener(() => {
            InstanceFinder.ClientManager.StartConnection();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
