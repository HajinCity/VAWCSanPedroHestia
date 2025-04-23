using System;
using System.Runtime.InteropServices;

public static class AgoraInterop
{
    [DllImport("agora_rtc_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr createAgoraRtcEngine();

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void OnJoinChannelSuccess(string channel, uint uid, int elapsed);

    [DllImport("agora_rtc_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int initialize(IntPtr engine, string appId);

    [DllImport("agora_rtc_sdk.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int joinChannel(IntPtr engine, string token, string channelName, string optionalInfo, uint uid);
}
