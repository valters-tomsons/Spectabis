using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

//Discord Rich Presence Client (RPC)

namespace Spectabis_WPF.Domain
{
    public class DiscordRPC
    {
        private const string ClientID = "450325564956344320";
        private RPCLibrary.RichPresence RPC_RichPresence;
        private RPCLibrary.EventHandlers RPC_EventHandlers;

        public DiscordRPC()
        {
            Initialize();
        }

        ~DiscordRPC()
        {
            RPCLibrary.Shutdown();
        }

        private void Initialize()
        {
            RPC_EventHandlers = new RPCLibrary.EventHandlers();

            RPC_EventHandlers.readyCallback += ReadyCallback;
            RPC_EventHandlers.disconnectedCallback += DisconnectCallback;
            RPC_EventHandlers.errorCallback += ErrorCallback;

#warning "WARN:: Reimplement discord libraries"
            //RPCLibrary.Initialize(ClientID, ref RPC_EventHandlers, true, null);

            Console.WriteLine("Discord Rich Presence Initialized.");
        }

        private void ReadyCallback()
        {

        }

        private void DisconnectCallback(int errorCode, string message)
        {

        }

        private void ErrorCallback(int errorCode, string message)
        {

        }

        public void UpdatePresence(string Game)
        {
            return;
#warning "WARN:: Reimplement discord libraries"
            RPC_RichPresence.details = Game != "Menus" ? "Playstation 2 (PCSX2)" : null;
            RPC_RichPresence.state = Game;
            RPC_RichPresence.largeImageKey = "menus";
            RPCLibrary.UpdatePresence(ref RPC_RichPresence);
            Console.WriteLine("Discord Presence Updated");

        }
    }

    class RPCLibrary
    {
        private const string DLL = "lib\\discord-rpc-w32";

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ReadyCallback();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void DisconnectedCallback(int errorCode, string message);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void ErrorCallback(int errorCode, string message);

        public struct EventHandlers
        {
            public ReadyCallback readyCallback;
            public DisconnectedCallback disconnectedCallback;
            public ErrorCallback errorCallback;
        }

        [System.Serializable]
        public struct RichPresence
        {
            public string state; /* max 128 bytes */
            public string details; /* max 128 bytes */
            public long startTimestamp;
            public long endTimestamp;
            public string largeImageKey; /* max 32 bytes */
            public string largeImageText; /* max 128 bytes */
            public string smallImageKey; /* max 32 bytes */
            public string smallImageText; /* max 128 bytes */
            public string partyId; /* max 128 bytes */
            public int partySize;
            public int partyMax;
            public string matchSecret; /* max 128 bytes */
            public string joinSecret; /* max 128 bytes */
            public string spectateSecret; /* max 128 bytes */
            public bool instance;
        }

        [DllImport(DLL, EntryPoint = "Discord_Initialize", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Initialize(string applicationId, ref EventHandlers handlers, bool autoRegister, string optionalSteamId);

        [DllImport(DLL, EntryPoint = "Discord_UpdatePresence", CallingConvention = CallingConvention.Cdecl)]
        public static extern void UpdatePresence(ref RichPresence presence);

        [DllImport(DLL, EntryPoint = "Discord_RunCallbacks", CallingConvention = CallingConvention.Cdecl)]
        public static extern void RunCallbacks();

        [DllImport(DLL, EntryPoint = "Discord_Shutdown", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Shutdown();
    }
}
