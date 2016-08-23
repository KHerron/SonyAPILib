using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SonyAPILib
{

    #region IRCC
    /// <summary>
    /// IRCC:1 Service Description class
    /// </summary>

    [Serializable]
    public class Ircc
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string ServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string ServiceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string ControlUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string EventSubURL { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string ScpdUrl { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Last Change.
        /// </summary>
        public string LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Status
        /// </summary>
        public string CurrentStatus { get; set; }

    }
    #endregion

    #region AVTransport
    /// <summary>
    /// AVTransport:1 Service Description class
    /// </summary>
    [Serializable]
    public class AVTransport
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string ServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string ServiceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string ControlUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string EventSubUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string ScpdUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service Last Change variable
        /// </summary>
        public string LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Transport State.
        /// </summary>
        public string TransportState { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Transport Status.
        /// </summary>
        public string TransportStatus { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Playback Storage Medium.
        /// </summary>
        public string PlayBackStorageMedium { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Record Storage Medium.
        /// </summary>
        public string RecordStorageMedium { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Possible Playback Storage Medium.
        /// </summary>
        public string PossiblePlaybackStorageMedia { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Possible Record Storage Medium.
        /// </summary>
        public string PossibleRecordStorageMedia { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current PLay Maode.
        /// </summary>
        public string CurrentPlayMode { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Transport Play Speed.
        /// </summary>
        public int TransportPlaySpeed { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Record Medium Write Status.
        /// </summary>
        public string RecordMediumWriteStatus { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Record Quality Mode.
        /// </summary>
        public string CurrentRecordQualityMode { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Possible Record Quality Modes.
        /// </summary>
        public string PossibleRecordQualityModes { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Number of Tracks.
        /// </summary>
        public int NumberOfTracks { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Track.
        /// </summary>
        public int CurrentTrack { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Track Duration.
        /// </summary>
        public string CurrentTrackDuration { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Track Meta Data.
        /// </summary>
        public string CurrentTrackMetaData { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Track URI.
        /// </summary>
        public string CurrentTrackURI { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the AVTransport URI.
        /// </summary>
        public string AVTransportURI { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the AVTransport URI Meta Data.
        /// </summary>
        public string AVTransportURIMetaData { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Next AVTransport URI.
        /// </summary>
        public string NextAVTransportURI { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Next AVTransport URI Meta Data.
        /// </summary>
        public string NextAVTransportURIMetaData { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Relative Time Position.
        /// </summary>
        public string RelativeTimePosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Absolute Time Position.
        /// </summary>
        public string AbsoluteTimePosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Relative Counter Position.
        /// </summary>
        public string RelativeCounterPosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Absolute Counter POsition.
        /// </summary>
        public string AbsoluteCounterPosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Transport Actions.
        /// </summary>
        public string CurrentTransportActions { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Relative Byte Position.
        /// </summary>
        public string X_DLNA_RelativeBytePosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Absolute Byte Position.
        /// </summary>
        public string X_DLNA_AbsoluteBytePosition { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Track Size.
        /// </summary>
        public string X_DLNA_CurrentTrackSize { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Seek Mode.
        /// </summary>
        public string A_ARG_TYPE_SeekMode { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Seek Target.
        /// </summary>
        public string A_ARG_TYPE_SeekTarget { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Instance ID.
        /// </summary>
        public int A_ARG_TYPE_InstanceID { get; set; }
    }
    #endregion

    #region Rendering Control
    /// <summary>
    /// RenderingControl:1 Service Description class
    /// </summary>
    [Serializable]
    public class RenderingControl
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string ServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string ServiceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string ControlUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string EventSubUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string ScpdUrl { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Last Change.
        /// </summary>
        public string LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Preset Name List.
        /// </summary>
        public string PresetNameList { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Mute State.
        /// </summary>
        public Boolean MuteState { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Volume state.
        /// </summary>
        public int VolumeState { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Channel(Master).
        /// </summary>
        public string ChannelState = "Master";
        /// <summary>
        /// Gets or Sets the State Variable for the Instance ID (0).
        /// </summary>
        public int InstanceID = 0;
        /// <summary>
        /// Gets or Sets the State Variable for the Preset Name.
        /// </summary>
        public string PresetName { get; set; }
    }
    #endregion

    #region Connection Managger
    /// <summary>
    /// ConnectionManager:1 Service Description class
    /// </summary>
    [Serializable]
    public class ConnectionManager
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string ServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string ServiceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string ControlUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string EventSubUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string ScpdUrl { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Last Change.
        /// </summary>
        public string LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Connection Id.
        /// </summary>
        public int ConnectionID { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Connection Status.
        /// </summary>
        public string sv_ConnectionStatus { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Connection Manager.
        /// </summary>
        public string Manager { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Direction.
        /// </summary>
        public string Direction { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the RcsID.
        /// </summary>
        public string RcsID { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Transport ID.
        /// </summary>
        public int AVTransportID { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Peer Connection ID.
        /// </summary>
        public int PeerConnectionID { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Proticol Information.
        /// </summary>
        public string ProtocolInfo { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Proticol Sink.
        /// </summary>
        public string ProtocolSink { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Proticol Source.
        /// </summary>
        public string ProticolSource { get; set; }
    }
    #endregion

    #region Party
    /// <summary>
    /// Party:1 Service Description class
    /// </summary>
    [Serializable]
    public class Party
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string ServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string ServiceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string ControlUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string EventSubUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string ScpdUrl { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Last Change.
        /// </summary>
        public string LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Singer Capability.
        /// </summary>
        public int SingerCapability { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Transport Port.
        /// </summary>
        public int TransportPort { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Party State.
        /// </summary>
        public string PartyState { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Party Mode.
        /// </summary>
        public string PartyMode { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Party Song.
        /// </summary>
        public string PartySong { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Session ID.
        /// </summary>
        public int SessionID { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Number of Listeners.
        /// </summary>
        public int NumberOfListeners { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the ListenersList
        /// </summary>
        public string ListenersList { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the UUID.
        /// </summary>
        public string Uuid { get; set; }
    }
    #endregion

    #region Action List

    /// <summary>
    /// IRCC:1 Service Description class
    /// </summary>
    [Serializable]
    public class ActionList
    {
        /// <summary>
        /// Gets or Sets the Action List Registration Mode
        /// </summary>
        public int RegisterMode { get; set; }
        /// <summary>
        /// Gets or Sets the Registration URL
        /// </summary>
        public string RegisterUrl { get; set; }
        /// <summary>
        /// Gets or Sets the System Information URL
        /// </summary>
        public string SystemInformationUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Remote Commandlist URL
        /// </summary>
        public string RemoteCommandListUrl { get; set; }
        /// <summary>
        /// Gets or Sets the GetStatus URL
        /// </summary>
        public string StatusUrl { get; set; }
        /// <summary>
        /// Gets or Sets the GetText URL
        /// </summary>
        public string GetTextUrl { get; set; }
        /// <summary>
        /// Gets or Sets the SendText URL
        /// </summary>
        public string SendTextUrl { get; set; }
    }
    #endregion

    #region Device Service
    /// <summary>
    /// Basic Service Description class
    /// </summary>
     
    [Serializable]
    public class DeviceService
    {
        /// <summary>
        /// Gets or Sets the Service Type
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// Gets of Sets the Friendly Service Identifier
        /// </summary>
        public string ServiceIdentifier { get; set; }
        /// <summary>
        /// Gets or sets the Service ID
        /// </summary>
        public string ServiceID { get; set; }
        /// <summary>
        /// Gets or Sets the Service Control URL
        /// </summary>
        public string ControlUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service Event URL
        /// </summary>
        public string EventSubUrl { get; set; }
        /// <summary>
        /// Gets or Sets the Service SCPD URL
        /// </summary>
        public string ScpdUrl { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Last Change.
        /// </summary>
        public string LastChange { get; set; }
        /// <summary>
        /// Gets or Sets the State Variable for the Current Status
        /// </summary>
        public string CurrentStatus { get; set; }
    }
    #endregion

}
