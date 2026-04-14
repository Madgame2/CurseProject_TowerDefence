using Common.systems.MainThread;
using Common.systems.UI.View;
using ModestTree;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class JoinToLobbyView : ViewBase<JoinToLobbyViewModel>
{
    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _JoinButton;
    [SerializeField] private TMP_InputField _inputCode;
    [SerializeField] private Transform _contentRoot;

    [SerializeField] private GameObject _template;

    private Dictionary<string, GameObject> lobbyes = new Dictionary<string, GameObject>();

    [Inject] private readonly MainThreadDispatcher _threadDispatcher;

    protected override void OnViewModelAssigned()
    {
        _closeButton.onClick.AddListener(ViewModel.onClose);
        _inputCode.onValueChanged.AddListener(FormatInput);

        ViewModel.InitArray += InitLobbyList;
        ViewModel.LobbyRemover += LobbyremovetHandle;
        ViewModel.LobbyCreated += LobbyAdeddHandle;
        ViewModel.LobbyUpdated += LobbyUpdateHandler;
    }

    private void LobbyUpdateHandler(string lobbyId, Scenes.Lobby.Entities.Lobby lobby)
    {
        GameObject lobbyObject = lobbyes.GetValueOrDefault(lobbyId);
        if (lobbyObject == null) return;

        _threadDispatcher.Run(() =>
        {
            if (lobbyObject.TryGetComponent<LobbyShowElem>(out LobbyShowElem lse))
            {
                lse.Init(lobby);
            }
        });
    }

    private void LobbyremovetHandle(string obj)
    {
        GameObject lobbyObject = lobbyes.GetValueOrDefault(obj);
        if (lobbyObject == null) return;

        _threadDispatcher.Run(() =>
        {
            if (lobbyObject.TryGetComponent<LobbyShowElem>(out LobbyShowElem lse))
            {
                lse.clearUp();
            }

            Destroy(lobbyObject);
        });
    }



    private void LobbyAdeddHandle(Scenes.Lobby.Entities.Lobby newLobby)
    {
        if (lobbyes.ContainsKey(newLobby.Id)) return;
        
        

        _threadDispatcher.Run(() =>
        {
            GameObject listElem = Instantiate(_template, _contentRoot, false);

            if (listElem.TryGetComponent<LobbyShowElem>(out LobbyShowElem lse))
            {
                lse.Init(newLobby);
            }

            lobbyes.Add(newLobby.Id, listElem);
        });
    }

    public void InitLobbyList(Scenes.Lobby.Entities.Lobby[] lobbies)
    {
        if (lobbies == null) return;

        foreach(var lob in lobbies)
        {
            GameObject listElem = Instantiate(_template, _contentRoot, false);
            if(listElem.TryGetComponent<LobbyShowElem>(out LobbyShowElem lse))
            {
                lse.Init(lob);
            }

            lobbyes.Add(lob.Id, listElem);
        }
    }

    public override async void Cleanup()
    {
        _closeButton.onClick.RemoveAllListeners();
        _inputCode.onValueChanged.RemoveAllListeners();

        ViewModel.InitArray -= InitLobbyList;
        ViewModel.LobbyRemover -= LobbyremovetHandle;
        ViewModel.LobbyCreated -= LobbyAdeddHandle;
        ViewModel.LobbyUpdated -= LobbyUpdateHandler;

        await ViewModel.ClearAll();
    }


    private void FormatInput(string input)
    {
        // убираем всё кроме букв/цифр
        string cleaned = "";

        foreach (char c in input.ToUpper())
        {
            if (char.IsLetterOrDigit(c))
                cleaned += c;
        }

        // ограничиваем 8 символов
        if (cleaned.Length > 8)
            cleaned = cleaned.Substring(0, 8);

        // вставляем дефис после 4 символа
        if (cleaned.Length > 4)
            cleaned = cleaned.Insert(4, "-");

        // чтобы не было зацикливания события
        _inputCode.onValueChanged.RemoveListener(FormatInput);
        _inputCode.text = cleaned;
        _inputCode.onValueChanged.AddListener(FormatInput);

        // ставим курсор в конец
        _inputCode.caretPosition = cleaned.Length;
    }
}
