using Common.systems.UI.View;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HostPlayView : ViewBase<HostPlayViewModel>
{
    [SerializeField] private Button _backButton;
    [SerializeField] private Button _generateRandomSeed;
    [SerializeField] private Button _playButton;

    [SerializeField] private TMP_InputField _seedInput;
    [SerializeField] private TMP_Dropdown _difficultyDropdownBox;

    private MatchDifficulty[] values;
    protected override void OnViewModelAssigned()
    {
        _backButton.onClick.AddListener(ViewModel.OnBack);
        _generateRandomSeed.onClick.AddListener(generateRandomSeedHandler);
        _playButton.onClick.AddListener(PlayButtonHandler);

        values = (MatchDifficulty[])Enum.GetValues(typeof(MatchDifficulty));

        _difficultyDropdownBox.ClearOptions();
        _difficultyDropdownBox.AddOptions(values.Select(v => v.ToString()).ToList());


        generateRandomSeedHandler();
    }

    public override void Cleanup()
    {
        _backButton.onClick.RemoveAllListeners();
        _generateRandomSeed.onClick.RemoveAllListeners();
        _playButton.onClick.RemoveAllListeners();
    }

    private void generateRandomSeedHandler()
    {
        _seedInput.text = ViewModel.Generate16DigitNumber();
    }

    private void PlayButtonHandler()
    {
        MatchMakingRequestDTO dto = new MatchMakingRequestDTO();
        dto.Seed = long.Parse(_seedInput.text);
        dto.matchDifficulty = values[_difficultyDropdownBox.value].ToString();

        ViewModel.StartSearch(dto);
    }
}
