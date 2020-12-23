using System;
namespace Events
{
    public struct PlayerPickedUpMedalEventInfo
    {
        public PlayerPickedUpMedalEventInfo(int pickedUpMedalsCount) => PickedUpMedalsCount = pickedUpMedalsCount;

        public int PickedUpMedalsCount { get; }
    }
}