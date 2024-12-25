// в общем для каждого PickupActive нужен свой скрипт. Если не переписывать Activate(),
// то значит шмотка просто увеличивает статы

public abstract class PickupActive : Pickup {
    private bool _isCharged = true;

    public virtual void Activate(Player player) {
        statModifierSO.ApplyModifierEffect(player);
        // player.updateCalculatedStats();
    }

    public bool isReadyToUse() {
        return _isCharged;
    }

    public void SetIsCharged(bool newValue) {
        _isCharged = newValue;
    }
}