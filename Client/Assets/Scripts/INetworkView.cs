﻿using UnityEngine;
using System.Collections;

public interface INetworkView {

	void SetNativeNetworkView(NetworkView nativeNetworkView);

	void RPC(string name, RPCMode mode, params object[] args);

	GameObject gameObject();

	NetworkViewID getNetworkViewID();

	INetworkView Find(NetworkViewID viewID);
}