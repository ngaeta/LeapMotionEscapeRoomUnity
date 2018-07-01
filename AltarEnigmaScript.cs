using UnityEngine;
using System.Collections.Generic;

public class AltarEnigmaScript : MonoBehaviour {


	private IList<Transform> m_Candles;
	public Transform[] OrderCandleEnigma;
	Transform[] candlesOrderPlayer;
	public CageMove cageScript;


	// Use this for initialization
	void Start () {
		m_Candles = new List<Transform> ();
	}

	public void ControlCandles() {
		int i = 0;

		foreach (Transform candle in m_Candles) {
			if (!(candle.name.Equals (OrderCandleEnigma [i].name))) {
				Debug.Log ("Combinazione Errata");
				return;
			}

			i++;
		}

		cageScript.enabled = true;
	}

	public int GetCandleCountOn() {
		
		return m_Candles.Count;
	}

	public void AddCandle(Transform candle) {
		
		m_Candles.Add (candle);
	}

	public void RemoveCandle(Transform candle) {
		
		m_Candles.Remove (candle);
	}

	private void TurnOffCandles() {
		
		for(int i=0; i < m_Candles.Count; i++)
			candlesOrderPlayer[i].GetChild(2).GetComponent<CandleEnigma> ().FireOff ();
		
		m_Candles.Clear ();	
	}
}
