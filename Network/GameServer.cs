using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameServer : MonoBehaviour {

	//public enum GAMESERVER_EVENTS{CONNECTED, DISCONNECTED};
	
	public string m_ServerIP = "127.0.0.1";
	public int m_ServerPort = 9933;
	bool m_Connected = false;
	
	// Use this for initialization
	void Start () {
		OnStart();
		Connect();
	}
	
	// Update is called once per frame
	void Update () {
		OnUpdate();
	}

	void OnApplicationQuit()
	{
		if(m_Connected)
		{
			Disconnect();
		}
	}

	public void Reconnect()
	{
		Disconnect();
		Connect();
	}
	
	abstract protected void OnStart();

	abstract protected void OnUpdate();

	abstract protected void Connect( );
	abstract protected void Disconnect();
	//abstract protected void AddEventListener(GAMESERVER_EVENTS _event);

}
