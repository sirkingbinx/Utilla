using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Utilla.Utils;

namespace Utilla.Behaviours
{
    internal class UtillaNetworkController : MonoBehaviourPunCallbacks
    {
        public static UtillaNetworkController Instance { get; private set; }

        private Events.RoomJoinedArgs lastRoom;

        public override void OnEnable()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
                return;
            }
            Instance = this;

            base.OnEnable(); // Tell Photon to register this object as a callback target, this will be important shortly

            if (NetworkSystem.Instance is NetworkSystem netSys && netSys is NetworkSystemPUN && PhotonNetwork.NetworkingClient is LoadBalancingClient client)

                // PICO WAS HERE

            {
                client.UpdateCallbackTargets();
                MatchMakingCallbacksContainer callbackContainer = client.MatchMakingCallbackTargets;

                int networkSystemIndex = -1;
                for (int i = 0; i < callbackContainer.Count; i++)
                {
                    IMatchmakingCallbacks individualCallback = callbackContainer[i];
                    if ((object)individualCallback is MonoBehaviour behaviour && behaviour.gameObject == netSys.gameObject)
                    {
                        networkSystemIndex = i;
                        break;
                    }
                }

                if (networkSystemIndex >= 0)
                {
                    int currentIndex = callbackContainer.IndexOf(this);
                    if (currentIndex >= 0)
                    {
                        callbackContainer.Remove(this);
                        if (currentIndex < networkSystemIndex) networkSystemIndex--;
                    }
                    callbackContainer.Insert(networkSystemIndex + 1, this);
                }
            }
        }

        public override void OnDisable()
        {
            base.OnDisable();

            if (Instance == this) Instance = null;
        }

        public override void OnJoinedRoom()
        {
            if (ApplicationQuittingState.IsQuitting) return;

            // trigger events

            NetworkSystem netSys = NetworkSystem.Instance;
            bool isPrivate = netSys.SessionIsPrivate;
            string gameMode = netSys.GameModeString;

            GameModeUtils.CurrentGamemode = GameModeUtils.FindGamemodeInString(gameMode);

            Events.RoomJoinedArgs args = new()
            {
                isPrivate = isPrivate,
                Gamemode = gameMode
            };
            lastRoom = args;

            Events.Instance.TriggerRoomJoin(args);

            //RoomUtils.ResetQueue();
        }

        public override void OnLeftRoom()
        {
            if (ApplicationQuittingState.IsQuitting) return;

            GameModeUtils.CurrentGamemode = null;

            if (lastRoom != null)
            {
                Events.Instance.TriggerRoomLeft(lastRoom);
                lastRoom = null;
            }
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (ApplicationQuittingState.IsQuitting || NetworkSystem.Instance == null || !NetworkSystem.Instance.InRoom || NetworkSystem.Instance.GameModeString is not string gameMode || gameMode == null) return;

            GameModeUtils.CurrentGamemode = GameModeUtils.FindGamemodeInString(gameMode);

            if (lastRoom == null)
            {
                lastRoom = new Events.RoomJoinedArgs
                {
                    isPrivate = NetworkSystem.Instance.SessionIsPrivate,
                    Gamemode = gameMode
                };
                Events.Instance.TriggerRoomJoin(lastRoom);
                return;
            }

            if (lastRoom.Gamemode != gameMode || lastRoom.isPrivate != NetworkSystem.Instance.SessionIsPrivate)
            {
                if (GamemodeManager.HasInstance)
                    GamemodeManager.Instance.OnRoomLeft(null, lastRoom);

                lastRoom.Gamemode = gameMode;
                lastRoom.isPrivate = NetworkSystem.Instance.SessionIsPrivate;

                if (GamemodeManager.HasInstance)
                    GamemodeManager.Instance.OnRoomJoin(null, lastRoom);
            }
        }
    }
}
