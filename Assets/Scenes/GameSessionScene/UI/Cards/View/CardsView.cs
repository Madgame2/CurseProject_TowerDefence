using Common.systems.UI.View;
using UnityEngine;
using UnityEngine.UI;

public class CardsView : ViewBase<CardsViewModel>
{
    [SerializeField] private Image _grossCanonCard;
    [SerializeField] private Image _teslaTowerCard;
    [SerializeField] private Image _warriorCampCard;



    protected override void OnViewModelAssigned()
    {
        
    }

    public override void Cleanup()
    {
        
    }
}
