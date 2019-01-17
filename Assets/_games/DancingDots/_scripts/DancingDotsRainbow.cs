using UnityEngine;

namespace Antura.Minigames.DancingDots
{
	public class DancingDotsRainbow : MonoBehaviour {

		public float rotateSpeed = 200;

		// Use this for initialization
		void Start () {

		}

		// Update is called once per frame
		void Update () {
//			transform.Rotate(Vector3.right * Time.deltaTime);
			transform.Rotate(0, 0, Time.deltaTime * rotateSpeed);

		}
	}
}