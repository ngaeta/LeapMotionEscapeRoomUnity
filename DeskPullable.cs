using UnityEngine;
using System.Collections;

public class DeskPullable : Pullable {

	private bool m_TotalClosed=true;
	private bool m_TotalOpened = false;
	private Vector3 m_CurrentPosition;
	private Vector3 m_InitialPosition;

	public Vector3 MaxOpened;


	void OnStart() {
		m_InitialPosition = transform.parent.localPosition;
		m_CurrentPosition = m_InitialPosition;
	}

	public override bool PullOut(float forceToPull) {

		if (forceToPull < 0 && !m_TotalClosed) {
			
			m_TotalOpened = false;
			if (m_CurrentPosition.z < m_InitialPosition.z) {
				m_CurrentPosition = m_InitialPosition;  
				m_TotalClosed = true;
				return false;
			}
			Pull (forceToPull);	
			return true;
		}
		else if(forceToPull > 0 && !m_TotalOpened) {
			
			m_TotalClosed = false;
			if (m_CurrentPosition.z > MaxOpened.z) {
				m_CurrentPosition = MaxOpened;
				m_TotalOpened = true;
				return false;
			} 
			Pull (forceToPull);	
			return true;
		}

		return false;
	}

	private void Pull(float forceToPull) {
		Vector3 newPosition = new Vector3 (m_CurrentPosition.x, m_CurrentPosition.y, m_CurrentPosition.z + forceToPull);
		transform.parent.localPosition = newPosition;				
		m_CurrentPosition = newPosition;
	}
}
