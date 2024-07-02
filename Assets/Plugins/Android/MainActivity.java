package com.unity3d.player;
 
import android.os.Bundle;
import com.unity3d.player.UnityPlayerActivity;
import android.content.Intent;
import android.content.IntentFilter;
 
import android.util.Log;
 
import com.spotify.android.appremote.api.ConnectionParams;
import com.spotify.android.appremote.api.Connector;
import com.spotify.android.appremote.api.SpotifyAppRemote;
 
import com.spotify.protocol.client.Subscription;
import com.spotify.protocol.types.PlayerState;
import com.spotify.protocol.types.Track;
 
public class MainActivity extends UnityPlayerActivity 
{
    public static MainActivity instance;
    public static String currentSongText = "";
    public static String currentArtistText = "";
    public static String songCurrentText = "";
    public static String songTotalText = "";
    public static long currentTrackDuration;
    public static long currentPlaybackPosition; 

    private static final String CLIENT_ID = "a19a5e7ac7414ac891ea56a38ee270a6"; // Use your own Client Id
    private static final String REDIRECT_URI = "http://localhost:5000/callback"; // Also match this on the app settings page
    private SpotifyAppRemote mSpotifyAppRemote;
 
    @Override
    protected void onCreate(Bundle savedInstanceState) 
    {
        super.onCreate(savedInstanceState);
        instance = this;
    }
    
    public void PlaySong(String trackId) 
    {
        mSpotifyAppRemote.getPlayerApi().play("spotify:track:" + trackId);
    }

    public void Pause() 
    {
        mSpotifyAppRemote.getPlayerApi().pause();
    }

    public void Resume() 
    {
        mSpotifyAppRemote.getPlayerApi().resume();
    }

    public void SkipNext() 
    {
        mSpotifyAppRemote.getPlayerApi().skipNext();
    }

    public void SkipPrevious() 
    {
        mSpotifyAppRemote.getPlayerApi().skipPrevious();
    }

    public void PlayPlaylist() 
    {
        mSpotifyAppRemote.getPlayerApi().play("spotify:playlist:1yERzbi3nMiw28c1bQQfWo");
        mSpotifyAppRemote.getPlayerApi().setRepeat(2);
    }
 
    public void setShuffle(boolean enabled) 
    {
        mSpotifyAppRemote.getPlayerApi().setShuffle(enabled);
    }

    public void toggleShuffle() 
    {
        mSpotifyAppRemote.getPlayerApi().toggleShuffle();
    }

    public void setRepeat(int repeatMode) 
    {
        mSpotifyAppRemote.getPlayerApi().setRepeat(repeatMode);
    }

    public void toggleRepeat() 
    {
        mSpotifyAppRemote.getPlayerApi().toggleRepeat();
    }

    public void getPlayerState()
    {
        mSpotifyAppRemote.getPlayerApi().getPlayerState().setResultCallback(playerState -> 
        {
            final Track track = playerState.track;
            if (track != null) 
            {
                currentTrackDuration = track.duration;
                currentPlaybackPosition = playerState.playbackPosition;
                String formattedTrackDuration = String.format("%d:%02d", 
                    currentTrackDuration / 60000, (currentTrackDuration % 60000) / 1000);
                String formattedPlaybackPosition = String.format("%d:%02d", 
                    currentPlaybackPosition / 60000, (currentPlaybackPosition % 60000) / 1000);
                currentSongText = track.name;
                currentArtistText = track.artist.name; 
                songCurrentText = formattedPlaybackPosition; 
                songTotalText = formattedTrackDuration;
            }
        });
    }
    @Override
    protected void onStart() 
    {
        super.onStart();
        ConnectionParams connectionParams =
                new ConnectionParams.Builder(CLIENT_ID)
                        .setRedirectUri(REDIRECT_URI)
                        .showAuthView(true)
                        .build();
 
        SpotifyAppRemote.connect(this, connectionParams, new Connector.ConnectionListener() 
        {
            public void onConnected(SpotifyAppRemote spotifyAppRemote) 
            {
                mSpotifyAppRemote = spotifyAppRemote;
            }
 
            public void onFailure(Throwable throwable) {
                Log.e("MyActivity", throwable.getMessage(), throwable);
            }
        });
    }
    @Override
    protected void onStop() 
    {
        super.onStop();
        SpotifyAppRemote.disconnect(mSpotifyAppRemote);
    }
}