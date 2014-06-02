using System;
using System.Collections.Generic;
using UnityEngine;

class BumpMatcher
{
	const float BUMP_BACKLOG_TIME = 1.0f;
	const float MAX_BUMP_TIME = 0.1f;

	const float VERTICAL_BUMP_THRESHOLD   = 0.3f;

	LinkedList<Bump> bumpHistory = new LinkedList<Bump>();

	public delegate void BumpMatchHandler(Bump bump1, Bump bump2);
	public event BumpMatchHandler OnBumpMatch;

	public void Add(Bump newBump)
	{
		Debug.Log ("Player " + newBump.Sender + " sent bump at " + newBump.Time + " with force " + newBump.Force);
		LinkedList<Bump> matches = findMatches(newBump);

		// If no matches were found, add the bump to the bump history so it can be matched to future incoming bumps.
		if (matches.Count == 0) {
			bumpHistory.AddFirst (newBump);
		}

		// If exactly one match was found, assume the match is correct. Remove the matched bump from the history to 
		// prevent re-matching.
		else if (matches.Count == 1) {
			if (OnBumpMatch != null) {
				OnBumpMatch(matches.First.Value, newBump);
			}
		}

		// If more than one match was found, discard all matches.
		foreach (Bump match in matches) {
			bumpHistory.Remove (match);
		}
	}

	private LinkedList<Bump> findMatches(Bump bump)
	{
		LinkedList<Bump> matches = new LinkedList<Bump>();

		// Iterate through previous bumps
		LinkedListNode<Bump> node = bumpHistory.First;
		while (node != null)
		{
			var next = node.Next;
			var oldBump = node.Value;
			
			// Remove all entries older than the maximum age in the bump backlog.
			if (oldBump.Time < Network.time - BUMP_BACKLOG_TIME)
			{
				bumpHistory.Remove(oldBump);
			}
			
			// Remove all old entries from this sender.
			else if (oldBump.Sender == bump.Sender) {
				bumpHistory.Remove(oldBump);
			}
			
			// Check if two bumps are within the maximum time in between bumps.
			else if (Math.Abs(oldBump.Time - bump.Time) <= MAX_BUMP_TIME)
			{
				//if (Math.Abs (oldBump.Force - bump.Force) <= VERTICAL_BUMP_THRESHOLD) {
					Debug.Log("Possible bump detected between players " + bump.Sender + " and " + oldBump.Sender);
					matches.AddFirst(oldBump);
				//}
			}
			node = next;
		}

		return matches;
	}
}
