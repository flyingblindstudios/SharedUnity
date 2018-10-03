using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
using Sfs2X;
using Sfs2X.Core;*/
//using Sfs2X.Request;

public class SmartfoxGameServer /*: GameServer*/ {

/*	SmartFox sfs;
	
	protected override void OnStart()
	{
		sfs = new SmartFox();
		sfs.ThreadSafeMode = true;
		sfs.AddEventListener(SFSEvent.CONNECTION,OnConnection);
	
	}
	
	protected override void OnUpdate()
	{
		sfs.ProcessEvents();
	}

	protected override void Connect()
	{
		sfs.Connect(m_ServerIP, m_ServerPort);
	}

	protected override void Disconnect()
	{
		if(sfs.IsConnected)
		{
			sfs.Disconnect();
		}
	}

	void OnConnection(BaseEvent _event)
	{
		if((bool)_event.Params["success"])
		{
			Debug.Log("[SmartFoxServer] Connection established!");
		}

	}*/
}
