using UnityEngine;
using System.Collections;

public class FlowerPickable : Pickable {

	private bool m_InTrigger = false;

	public override void OnLeave (float forceToLeave)
	{
		if (!m_InTrigger)
			base.OnLeave (forceToLeave);	
	}

	public bool InTrigger {
		set {
			m_InTrigger = value;
		}
	}
		
}
